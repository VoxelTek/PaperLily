// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.TextDisplayManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;
using System.Text.RegularExpressions;

#nullable disable
namespace LacieEngine.StoryPlayer
{
    public class TextDisplayManager : Node
    {
        private static readonly bool EnableContinueIndicator = true;
        public const string DefaultFont = "default";
        private const int AutoNextTicks = 10;
        public readonly static Regex SpaceTagRegex = new Regex("\\${sp:(\\d+?)}");
        public readonly static Regex FullSpaceTagRegex = new Regex("\\${fsp:(\\d+?)}");
        public readonly static Regex BreakTagRegex = new Regex("\\${br:(\\d+?)}");
        public static readonly Regex VarTagRegex = new Regex("\\${var:(.+?)}");
        public static readonly Regex ImageTagRegex = new Regex("\\${img:(.+?)}");
        public static readonly Regex ItemTagRegex = new Regex("\\${item:(.+?)}");
        private readonly StoryPlayer storyPlayer;
        private float WaitTime;
        private float _accumulatedDelta;
        private string phrase;
        private string phraseRaw;
        private int numberCharacters;
        private int autoNextCounter = AutoNextTicks;

        public bool IsTextNode { get; private set; }

        public bool IsTimedText { get; set; }

        public bool IsShowingFullText {
            get {
                return storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters == -1 || storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters >= numberCharacters;
            }
        }

        public bool SkipDisabled { get; set; }

        public bool AutoNext { get; set; }

        private TextDisplayManager()
        {
        }

        public TextDisplayManager(StoryPlayer storyPlayer)
        {
            Name = nameof(TextDisplayManager);
            this.storyPlayer = storyPlayer;
            this.storyPlayer.nDialogueFrame.ContinueIndicator.Visible = false;
        }

        public void Reset()
        {
            IsTextNode = false;
            phrase = null;
            phraseRaw = null;
        }

        private void PartialCleanup() => storyPlayer.nDialogueFrame.ContinueIndicator.Hide();

        public void Cleanup()
        {
            IsTextNode = false;
            IsTimedText = false;
            storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters = -1;
            numberCharacters = 0;
            phraseRaw = null;
            autoNextCounter = AutoNextTicks;
        }

        public void UpdateDialogueText()
        {
            PartialCleanup();
            if (phraseRaw != null)
                numberCharacters = phraseRaw.Length;
            if (WaitTime > 0.0)
            {
                storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters = 0;
                _accumulatedDelta = 0.0f;
                SetProcess(true);
            }
            else
            {
                if (!storyPlayer.UI.Visible || !EnableContinueIndicator)
                    return;
                storyPlayer.nDialogueFrame.ContinueIndicator.Show();
            }
        }

        public void SetText(string text)
        {
            if (SpaceTagRegex.IsMatch(text))
            {
                foreach (Match match in SpaceTagRegex.Matches(text))
                {
                    var num = int.Parse(match.Groups[1].Value);
                    text = text.Replace(match.Groups[0].Value, new string(' ', num));
                }
            }
            if (FullSpaceTagRegex.IsMatch(text))
            {
                foreach (Match match1 in FullSpaceTagRegex.Matches(text))
                {
                    var num1 = int.Parse(match1.Groups[1].Value);
                    text = text.Replace(match1.Groups[0].Value, new string('\u3000', num1));
                }
            }
            if (BreakTagRegex.IsMatch(text))
            {
                foreach (Match match2 in BreakTagRegex.Matches(text))
                {
                    var num2 = int.Parse(match2.Groups[1].Value);
                    text = text.Replace(match2.Groups[0].Value, new string('\n', num2));
                }
            }
            if (VarTagRegex.IsMatch(text))
            {
                foreach (Match match in VarTagRegex.Matches(text))
                {
                    var variable = Game.Variables.GetVariable(match.Groups[1].Value);
                    text = !Game.Language.IsCaptionKey(variable) ? text.Replace(match.Groups[0].Value, variable) : text.Replace(match.Groups[0].Value, Game.Language.GetCaption(variable));
                }
            }
            if (ImageTagRegex.IsMatch(text))
            {
                foreach (Match match in ImageTagRegex.Matches(text))
                    text = text.Replace(match.Groups[0].Value, "[img=15]res://assets/img/ui/" + match.Groups[1].Value + ".png[/img]") + " ";
            }
            if (ItemTagRegex.IsMatch(text))
            {
                foreach (Match match in ItemTagRegex.Matches(text))
                    text = text.Replace(match.Groups[0].Value, "[font=res://resources/font/image_valign.tres][img=20]res://assets/sprite/common/item/" + Game.Items.Get(match.Groups[1].Value).Icon + ".png[/img][/font][color=aqua]" + Game.Items.Get(match.Groups[1].Value).Name + "[/color]") + " ";
            }
            IsTextNode = true;
            storyPlayer.nDialogueFrame.DialogueText.BbcodeText = text;
            phrase = text;
            phraseRaw = storyPlayer.nDialogueFrame.DialogueText.Text;
            SetSpeed(TextSpeed.Normal);
            SetFont("default");
        }

        public void SetFont(string font)
        {
            storyPlayer.nDialogueFrame.DialogueText.AddFontOverride("normal_font", GD.Load("res://resources/font/" + font + ".tres") as Font);
        }

        public void SetSpeed(TextSpeed speed)
        {
            WaitTime = (float)speed * (1f / 1000f);
        }

        public override void _Process(float delta)
        {
            if (!IsTextNode)
                SetProcess(false);
            else if (storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters < numberCharacters)
            {
                _accumulatedDelta += delta;
                if (_accumulatedDelta < (double)WaitTime)
                    return;
                int num = (int)(_accumulatedDelta / (double)WaitTime);
                _accumulatedDelta -= WaitTime * num;
                storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters += num;
                Game.Audio.PlayTextBeepSound();
                if (storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters != numberCharacters)
                    return;
                storyPlayer.AdvanceDisabled = false;
            }
            else if (AutoNext && autoNextCounter > 0)
                --autoNextCounter;
            else if (AutoNext)
                storyPlayer.Next();
            else
                ShowFullText();
        }

        public void AttemptShowFullText()
        {
            if (SkipDisabled)
                return;
            ShowFullText();
        }

        private void ShowFullText()
        {
            SetProcess(false);
            storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters = numberCharacters;
            Game.Audio.PlayTextBeepSound();
            if (storyPlayer.Choices.HasChoices)
                storyPlayer.Choices.Show();
            storyPlayer.AdvanceDisabled = false;
            storyPlayer.nDialogueFrame.ContinueIndicator.Show();
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
