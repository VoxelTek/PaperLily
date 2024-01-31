// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerTextCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Translations;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerTextCommand : StoryPlayerCommand
  {
    private const string FONT_PREFIX = "dialogue_";
    [Inject]
    [NonSerialized]
    private ICharacterManager Characters;
    [Inject]
    [NonSerialized]
    private IVariableManager Variables;
    [Inject]
    [NonSerialized]
    private IItemManager Items;

    public string Text { get; set; }

    public string Who { get; set; }

    public string Mood { get; set; }

    public string Font { get; set; }

    public string Color { get; set; }

    public float? Time { get; set; }

    public StoryPlayerTextCommand.TextFormatOptions[] Formats { get; set; }

    public StoryPlayerTextCommand.TextSpeedOptions Speed { get; set; }

    public StoryPlayerTextCommand.TextPositionOptions Position { get; set; }

    public StoryPlayerTextCommand.TextBackgroundOptions Background { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      string text = this.Text;
      if (this.Formats != null)
      {
        foreach (int format in this.Formats)
        {
          switch (format)
          {
            case 1:
              text = "[center]" + text + "[/center]";
              break;
            case 2:
              text = "[shake rate=10 level=4]" + text + "[/shake]";
              break;
          }
        }
      }
      if (this.Color != null)
        text = "[color=" + this.Color + "]" + text + "[/color]";
      storyPlayer.Text.SetText(text);
      if (!this.Font.IsNullOrEmpty())
        storyPlayer.Text.SetFont(this.Font);
      switch (this.Speed)
      {
        case StoryPlayerTextCommand.TextSpeedOptions.Fast:
          storyPlayer.Text.SetSpeed(TextDisplayManager.TextSpeed.Fast);
          break;
        case StoryPlayerTextCommand.TextSpeedOptions.VeryFast:
          storyPlayer.Text.SetSpeed(TextDisplayManager.TextSpeed.VeryFast);
          break;
        case StoryPlayerTextCommand.TextSpeedOptions.Slow:
          storyPlayer.Text.SetSpeed(TextDisplayManager.TextSpeed.Slow);
          break;
        case StoryPlayerTextCommand.TextSpeedOptions.VerySlow:
          storyPlayer.Text.SetSpeed(TextDisplayManager.TextSpeed.VerySlow);
          break;
      }
      switch (this.Position)
      {
        case StoryPlayerTextCommand.TextPositionOptions.Bottom:
          storyPlayer.UI.SetFramePosition();
          break;
        case StoryPlayerTextCommand.TextPositionOptions.Center:
          storyPlayer.UI.SetFramePosition(UiDisplayManager.FramePosition.Center);
          break;
        default:
          storyPlayer.UI.SetFramePosition();
          break;
      }
      switch (this.Background)
      {
        case StoryPlayerTextCommand.TextBackgroundOptions.Normal:
          storyPlayer.UI.SetFrameType();
          break;
        case StoryPlayerTextCommand.TextBackgroundOptions.None:
          storyPlayer.UI.SetFrameType(UiDisplayManager.FrameType.NoBackground);
          break;
        case StoryPlayerTextCommand.TextBackgroundOptions.Blur:
          storyPlayer.UI.SetFrameType(UiDisplayManager.FrameType.Blur);
          break;
        default:
          storyPlayer.UI.SetFrameType();
          break;
      }
      storyPlayer.UI.ShowDialogueBox();
      storyPlayer.Characters.ShowCharacter(this.Who, this.Mood);
      storyPlayer.SetNextBlockContinue();
      float valueOrDefault = this.Time.GetValueOrDefault();
      if ((double) valueOrDefault > 0.0)
      {
        storyPlayer.NextAfterSeconds(valueOrDefault);
        storyPlayer.Text.IsTimedText = true;
      }
      storyPlayer.AdvanceDisabled = false;
    }

    public override void Load()
    {
      if (!this.Who.IsNullOrEmpty())
        this.Characters.LoadResourcesForCharacter(this.Who);
      if (this.Font.IsNullOrEmpty())
        return;
      Game.Memory.Cache("res://resources/font/" + this.Font + ".tres");
    }

    public override IList<string> GetDependencies()
    {
      List<string> dependencies = new List<string>();
      if (!this.Who.IsNullOrEmpty())
        dependencies.AddRange((IEnumerable<string>) this.Characters.GetDependencies(this.Who));
      if (!this.Font.IsNullOrEmpty())
        dependencies.Add("res://resources/font/" + this.Font + ".tres");
      if (!this.Text.IsNullOrEmpty() && TextDisplayManager.ImageTagRegex.IsMatch(this.Text))
      {
        foreach (Match match in TextDisplayManager.ImageTagRegex.Matches(this.Text))
          dependencies.Add("res://assets/img/ui/" + match.Groups[1].Value + ".png");
      }
      if (!this.Text.IsNullOrEmpty() && TextDisplayManager.ItemTagRegex.IsMatch(this.Text))
      {
        foreach (Match match in TextDisplayManager.ItemTagRegex.Matches(this.Text))
          dependencies.Add("res://assets/sprite/common/item/" + this.Items.Get(match.Groups[1].Value).Icon + ".png");
      }
      return (IList<string>) dependencies;
    }

    public override void FindCaptions(TranslatableMessages captions)
    {
      captions.Add(this.Text, this.TextContext());
    }

    public override void OverrideCaptions(ICaptionSet captions)
    {
      if (!captions.ContainsKey(this.Text))
        return;
      this.Text = captions.Get(this.Text, this.TextContext());
    }

    private string TextContext()
    {
      return "events." + this.Event.Id + ".dlg." + (this.Who.IsNullOrEmpty() ? "narrator" : this.Who);
    }

    public enum TextFormatOptions
    {
      None,
      Center,
      Shake,
    }

    public enum TextSpeedOptions
    {
      Normal,
      Fast,
      VeryFast,
      Slow,
      VerySlow,
    }

    public enum TextPositionOptions
    {
      Bottom,
      Center,
    }

    public enum TextBackgroundOptions
    {
      Normal,
      None,
      Blur,
    }
  }
}
