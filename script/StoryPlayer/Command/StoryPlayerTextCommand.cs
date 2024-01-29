using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Translations;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerTextCommand : StoryPlayerCommand
	{
		public enum TextFormatOptions
		{
			None,
			Center,
			Shake
		}

		public enum TextSpeedOptions
		{
			Normal,
			Fast,
			VeryFast,
			Slow,
			VerySlow
		}

		public enum TextPositionOptions
		{
			Bottom,
			Center
		}

		public enum TextBackgroundOptions
		{
			Normal,
			None,
			Blur
		}

		private const string FONT_PREFIX = "dialogue_";

		[NonSerialized]
		[Inject]
		private ICharacterManager Characters;

		[NonSerialized]
		[Inject]
		private IVariableManager Variables;

		[NonSerialized]
		[Inject]
		private IItemManager Items;

		public string Text { get; set; }

		public string Who { get; set; }

		public string Mood { get; set; }

		public string Font { get; set; }

		public string Color { get; set; }

		public float? Time { get; set; }

		public TextFormatOptions[] Formats { get; set; }

		public TextSpeedOptions Speed { get; set; }

		public TextPositionOptions Position { get; set; }

		public TextBackgroundOptions Background { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			string text = Text;
			if (Formats != null)
			{
				TextFormatOptions[] formats = Formats;
				for (int i = 0; i < formats.Length; i++)
				{
					switch (formats[i])
					{
					case TextFormatOptions.Center:
						text = "[center]" + text + "[/center]";
						break;
					case TextFormatOptions.Shake:
						text = "[shake rate=10 level=4]" + text + "[/shake]";
						break;
					}
				}
			}
			if (Color != null)
			{
				text = "[color=" + Color + "]" + text + "[/color]";
			}
			storyPlayer.Text.SetText(text);
			if (!Font.IsNullOrEmpty())
			{
				storyPlayer.Text.SetFont(Font);
			}
			switch (Speed)
			{
			case TextSpeedOptions.VeryFast:
				storyPlayer.Text.SetSpeed(TextDisplayManager.TextSpeed.VeryFast);
				break;
			case TextSpeedOptions.Fast:
				storyPlayer.Text.SetSpeed(TextDisplayManager.TextSpeed.Fast);
				break;
			case TextSpeedOptions.Slow:
				storyPlayer.Text.SetSpeed(TextDisplayManager.TextSpeed.Slow);
				break;
			case TextSpeedOptions.VerySlow:
				storyPlayer.Text.SetSpeed(TextDisplayManager.TextSpeed.VerySlow);
				break;
			}
			switch (Position)
			{
			case TextPositionOptions.Bottom:
				storyPlayer.UI.SetFramePosition();
				break;
			case TextPositionOptions.Center:
				storyPlayer.UI.SetFramePosition(UiDisplayManager.FramePosition.Center);
				break;
			default:
				storyPlayer.UI.SetFramePosition();
				break;
			}
			switch (Background)
			{
			case TextBackgroundOptions.Normal:
				storyPlayer.UI.SetFrameType();
				break;
			case TextBackgroundOptions.None:
				storyPlayer.UI.SetFrameType(UiDisplayManager.FrameType.NoBackground);
				break;
			case TextBackgroundOptions.Blur:
				storyPlayer.UI.SetFrameType(UiDisplayManager.FrameType.Blur);
				break;
			default:
				storyPlayer.UI.SetFrameType();
				break;
			}
			storyPlayer.UI.ShowDialogueBox();
			storyPlayer.Characters.ShowCharacter(Who, Mood);
			storyPlayer.SetNextBlockContinue();
			float time = Time.GetValueOrDefault();
			if (time > 0f)
			{
				storyPlayer.NextAfterSeconds(time);
				storyPlayer.Text.IsTimedText = true;
			}
			storyPlayer.AdvanceDisabled = false;
		}

		public override void Load()
		{
			if (!Who.IsNullOrEmpty())
			{
				Characters.LoadResourcesForCharacter(Who);
			}
			if (!Font.IsNullOrEmpty())
			{
				Game.Memory.Cache("res://resources/font/" + Font + ".tres");
			}
		}

		public override IList<string> GetDependencies()
		{
			List<string> dependencies = new List<string>();
			if (!Who.IsNullOrEmpty())
			{
				dependencies.AddRange(Characters.GetDependencies(Who));
			}
			if (!Font.IsNullOrEmpty())
			{
				dependencies.Add("res://resources/font/" + Font + ".tres");
			}
			if (!Text.IsNullOrEmpty() && TextDisplayManager.ImageTagRegex.IsMatch(Text))
			{
				foreach (Match match2 in TextDisplayManager.ImageTagRegex.Matches(Text))
				{
					dependencies.Add("res://assets/img/ui/" + match2.Groups[1].Value + ".png");
				}
			}
			if (!Text.IsNullOrEmpty() && TextDisplayManager.ItemTagRegex.IsMatch(Text))
			{
				foreach (Match match in TextDisplayManager.ItemTagRegex.Matches(Text))
				{
					dependencies.Add("res://assets/sprite/common/item/" + Items.Get(match.Groups[1].Value).Icon + ".png");
				}
				return dependencies;
			}
			return dependencies;
		}

		public override void FindCaptions(TranslatableMessages captions)
		{
			captions.Add(Text, TextContext());
		}

		public override void OverrideCaptions(ICaptionSet captions)
		{
			if (captions.ContainsKey(Text))
			{
				Text = captions.Get(Text, TextContext());
			}
		}

		private string TextContext()
		{
			return "events." + base.Event.Id + ".dlg." + (Who.IsNullOrEmpty() ? "narrator" : Who);
		}
	}
}
