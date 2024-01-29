using System.Text.RegularExpressions;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	public class TextDisplayManager : Node
	{
		public enum TextSpeed
		{
			VerySlow = 120,
			Slow = 80,
			Normal = 22,
			Fast = 12,
			VeryFast = 8
		}

		private static readonly bool EnableContinueIndicator = true;

		public const string DefaultFont = "default";

		private const int AutoNextTicks = 10;

		public static readonly Regex VarTagRegex = new Regex("\\${var:(.+?)}");

		public static readonly Regex ImageTagRegex = new Regex("\\${img:(.+?)}");

		public static readonly Regex ItemTagRegex = new Regex("\\${item:(.+?)}");

		private StoryPlayer storyPlayer;

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
				if (storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters != -1)
				{
					return storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters >= numberCharacters;
				}
				return true;
			}
		}

		public bool SkipDisabled { get; set; }

		public bool AutoNext { get; set; }

		private TextDisplayManager()
		{
		}

		public TextDisplayManager(StoryPlayer storyPlayer)
		{
			base.Name = "TextDisplayManager";
			this.storyPlayer = storyPlayer;
			this.storyPlayer.nDialogueFrame.ContinueIndicator.Visible = false;
		}

		public void Reset()
		{
			IsTextNode = false;
			phrase = null;
			phraseRaw = null;
		}

		private void PartialCleanup()
		{
			storyPlayer.nDialogueFrame.ContinueIndicator.Hide();
		}

		public void Cleanup()
		{
			IsTextNode = false;
			IsTimedText = false;
			storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters = -1;
			numberCharacters = 0;
			phraseRaw = null;
			autoNextCounter = 10;
		}

		public void UpdateDialogueText()
		{
			PartialCleanup();
			if (phraseRaw != null)
			{
				numberCharacters = phraseRaw.Length;
			}
			if (WaitTime > 0f)
			{
				storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters = 0;
				_accumulatedDelta = 0f;
				SetProcess(enable: true);
			}
			else if (storyPlayer.UI.Visible && EnableContinueIndicator)
			{
				storyPlayer.nDialogueFrame.ContinueIndicator.Show();
			}
		}

		public void SetText(string text)
		{
			if (VarTagRegex.IsMatch(text))
			{
				foreach (Match match3 in VarTagRegex.Matches(text))
				{
					string varContent = Game.Variables.GetVariable(match3.Groups[1].Value);
					text = ((!Game.Language.IsCaptionKey(varContent)) ? text.Replace(match3.Groups[0].Value, varContent) : text.Replace(match3.Groups[0].Value, Game.Language.GetCaption(varContent)));
				}
			}
			if (ImageTagRegex.IsMatch(text))
			{
				foreach (Match match2 in ImageTagRegex.Matches(text))
				{
					text = text.Replace(match2.Groups[0].Value, "[img=15]res://assets/img/ui/" + match2.Groups[1].Value + ".png[/img]") + " ";
				}
			}
			if (ItemTagRegex.IsMatch(text))
			{
				foreach (Match match in ItemTagRegex.Matches(text))
				{
					text = text.Replace(match.Groups[0].Value, "[font=res://resources/font/image_valign.tres][img=20]res://assets/sprite/common/item/" + Game.Items.Get(match.Groups[1].Value).Icon + ".png[/img][/font][color=aqua]" + Game.Items.Get(match.Groups[1].Value).Name + "[/color]") + " ";
				}
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
			WaitTime = (float)speed * 0.001f;
		}

		public override void _Process(float delta)
		{
			if (!IsTextNode)
			{
				SetProcess(enable: false);
			}
			else if (storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters < numberCharacters)
			{
				_accumulatedDelta += delta;
				if (_accumulatedDelta >= WaitTime)
				{
					int letters = (int)(_accumulatedDelta / WaitTime);
					_accumulatedDelta -= WaitTime * (float)letters;
					storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters += letters;
					Game.Audio.PlayTextBeepSound();
					if (storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters == numberCharacters)
					{
						storyPlayer.AdvanceDisabled = false;
					}
				}
			}
			else if (AutoNext && autoNextCounter > 0)
			{
				autoNextCounter--;
			}
			else if (AutoNext)
			{
				storyPlayer.Next();
			}
			else
			{
				ShowFullText();
			}
		}

		public void AttemptShowFullText()
		{
			if (!SkipDisabled)
			{
				ShowFullText();
			}
		}

		private void ShowFullText()
		{
			SetProcess(enable: false);
			storyPlayer.nDialogueFrame.DialogueText.VisibleCharacters = numberCharacters;
			Game.Audio.PlayTextBeepSound();
			if (storyPlayer.Choices.HasChoices)
			{
				storyPlayer.Choices.Show();
			}
			storyPlayer.AdvanceDisabled = false;
			storyPlayer.nDialogueFrame.ContinueIndicator.Show();
		}
	}
}
