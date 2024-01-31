// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.TextDisplayManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System.Text.RegularExpressions;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  public class TextDisplayManager : Node
  {
    private static readonly bool EnableContinueIndicator = true;
    public const string DefaultFont = "default";
    private const int AutoNextTicks = 10;
    public static readonly Regex VarTagRegex = new Regex("\\${var:(.+?)}");
    public static readonly Regex ImageTagRegex = new Regex("\\${img:(.+?)}");
    public static readonly Regex ItemTagRegex = new Regex("\\${item:(.+?)}");
    private LacieEngine.StoryPlayer.StoryPlayer storyPlayer;
    private float WaitTime;
    private float _accumulatedDelta;
    private string phrase;
    private string phraseRaw;
    private int numberCharacters;
    private int autoNextCounter = 10;

    public bool IsTextNode { get; private set; }

    public bool IsTimedText { get; set; }

    public bool IsShowingFullText
    {
      get
      {
        return this.storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters == -1 || this.storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters >= this.numberCharacters;
      }
    }

    public bool SkipDisabled { get; set; }

    public bool AutoNext { get; set; }

    private TextDisplayManager()
    {
    }

    public TextDisplayManager(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      this.Name = nameof (TextDisplayManager);
      this.storyPlayer = storyPlayer;
      this.storyPlayer.nDialogueFrame.ContinueIndicator.Visible = false;
    }

    public void Reset()
    {
      this.IsTextNode = false;
      this.phrase = (string) null;
      this.phraseRaw = (string) null;
    }

    private void PartialCleanup() => this.storyPlayer.nDialogueFrame.ContinueIndicator.Hide();

    public void Cleanup()
    {
      this.IsTextNode = false;
      this.IsTimedText = false;
      this.storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters = -1;
      this.numberCharacters = 0;
      this.phraseRaw = (string) null;
      this.autoNextCounter = 10;
    }

    public void UpdateDialogueText()
    {
      this.PartialCleanup();
      if (this.phraseRaw != null)
        this.numberCharacters = this.phraseRaw.Length;
      if ((double) this.WaitTime > 0.0)
      {
        this.storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters = 0;
        this._accumulatedDelta = 0.0f;
        this.SetProcess(true);
      }
      else
      {
        if (!this.storyPlayer.UI.Visible || !TextDisplayManager.EnableContinueIndicator)
          return;
        this.storyPlayer.nDialogueFrame.ContinueIndicator.Show();
      }
    }

    public void SetText(string text)
    {
      if (TextDisplayManager.VarTagRegex.IsMatch(text))
      {
        foreach (Match match in TextDisplayManager.VarTagRegex.Matches(text))
        {
          string variable = Game.Variables.GetVariable(match.Groups[1].Value);
          text = !Game.Language.IsCaptionKey(variable) ? text.Replace(match.Groups[0].Value, variable) : text.Replace(match.Groups[0].Value, Game.Language.GetCaption(variable));
        }
      }
      if (TextDisplayManager.ImageTagRegex.IsMatch(text))
      {
        foreach (Match match in TextDisplayManager.ImageTagRegex.Matches(text))
          text = text.Replace(match.Groups[0].Value, "[img=15]res://assets/img/ui/" + match.Groups[1].Value + ".png[/img]") + " ";
      }
      if (TextDisplayManager.ItemTagRegex.IsMatch(text))
      {
        foreach (Match match in TextDisplayManager.ItemTagRegex.Matches(text))
          text = text.Replace(match.Groups[0].Value, "[font=res://resources/font/image_valign.tres][img=20]res://assets/sprite/common/item/" + Game.Items.Get(match.Groups[1].Value).Icon + ".png[/img][/font][color=aqua]" + Game.Items.Get(match.Groups[1].Value).Name + "[/color]") + " ";
      }
      this.IsTextNode = true;
      this.storyPlayer.nDialogueFrame.DialogueText.BbcodeText = text;
      this.phrase = text;
      this.phraseRaw = this.storyPlayer.nDialogueFrame.DialogueText.Text;
      this.SetSpeed(TextDisplayManager.TextSpeed.Normal);
      this.SetFont("default");
    }

    public void SetFont(string font)
    {
      this.storyPlayer.nDialogueFrame.DialogueText.AddFontOverride("normal_font", GD.Load("res://resources/font/" + font + ".tres") as Font);
    }

    public void SetSpeed(TextDisplayManager.TextSpeed speed)
    {
      this.WaitTime = (float) speed * (1f / 1000f);
    }

    public override void _Process(float delta)
    {
      if (!this.IsTextNode)
        this.SetProcess(false);
      else if (this.storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters < this.numberCharacters)
      {
        this._accumulatedDelta += delta;
        if ((double) this._accumulatedDelta < (double) this.WaitTime)
          return;
        int num = (int) ((double) this._accumulatedDelta / (double) this.WaitTime);
        this._accumulatedDelta -= this.WaitTime * (float) num;
        this.storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters += num;
        Game.Audio.PlayTextBeepSound();
        if (this.storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters != this.numberCharacters)
          return;
        this.storyPlayer.AdvanceDisabled = false;
      }
      else if (this.AutoNext && this.autoNextCounter > 0)
        --this.autoNextCounter;
      else if (this.AutoNext)
        this.storyPlayer.Next();
      else
        this.ShowFullText();
    }

    public void AttemptShowFullText()
    {
      if (this.SkipDisabled)
        return;
      this.ShowFullText();
    }

    private void ShowFullText()
    {
      this.SetProcess(false);
      this.storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters = this.numberCharacters;
      Game.Audio.PlayTextBeepSound();
      if (this.storyPlayer.Choices.HasChoices)
        this.storyPlayer.Choices.Show();
      this.storyPlayer.AdvanceDisabled = false;
      this.storyPlayer.nDialogueFrame.ContinueIndicator.Show();
    }

    public enum TextSpeed
    {
      VeryFast = 8,
      Fast = 12, // 0x0000000C
      Normal = 22, // 0x00000016
      Slow = 80, // 0x00000050
      VerySlow = 120, // 0x00000078
    }
  }
}
