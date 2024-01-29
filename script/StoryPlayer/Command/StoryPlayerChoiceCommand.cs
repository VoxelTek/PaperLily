using System;
using System.Collections.Generic;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Translations;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerChoiceCommand : StoryPlayerCommand
	{
		[Serializable]
		public class Choice
		{
			public string ChoiceText;

			public int GotoLine;

			public LogicStatement Condition;
		}

		public string Text { get; set; }

		public List<Choice> Choices { get; set; } = new List<Choice>();


		public override void Execute(StoryPlayer storyPlayer)
		{
			List<Choice> visibleChoices = new List<Choice>();
			foreach (Choice choice2 in Choices)
			{
				if (choice2.Condition == null || choice2.Condition.Evaluate())
				{
					visibleChoices.Add(choice2);
				}
			}
			if (visibleChoices.IsEmpty())
			{
				Log.Trace("Choice block: no choices are valid.");
				storyPlayer.SetNextBlockContinue();
				storyPlayer.Next();
				return;
			}
			if (visibleChoices.Count == 1)
			{
				Log.Trace("Choice block: only one choice is valid.");
				storyPlayer.SetNextBlockToLine(visibleChoices[0].GotoLine);
				storyPlayer.Next();
				return;
			}
			storyPlayer.UI.SetFramePosition();
			storyPlayer.UI.SetFrameType();
			storyPlayer.Text.SetText(Text);
			storyPlayer.UI.ShowDialogueBox();
			storyPlayer.Characters.HideAllCharacters();
			storyPlayer.Characters.ShowCharacter(null);
			foreach (Choice choice in visibleChoices)
			{
				storyPlayer.Choices.AddChoice(choice.ChoiceText, choice.GotoLine);
			}
			storyPlayer.AdvanceDisabled = false;
		}

		public override void FindCaptions(TranslatableMessages captions)
		{
			captions.Add(Text, ChoiceTextContext());
			foreach (Choice choice in Choices)
			{
				captions.Add(choice.ChoiceText, ChoiceOptionContext());
			}
		}

		public override void OverrideCaptions(ICaptionSet captions)
		{
			string context = ChoiceTextContext();
			string choiceContext = ChoiceOptionContext();
			if (captions.ContainsKey(Text))
			{
				Text = captions.Get(Text, context);
			}
			foreach (Choice choice in Choices)
			{
				if (captions.ContainsKey(choice.ChoiceText))
				{
					choice.ChoiceText = captions.Get(choice.ChoiceText, choiceContext);
				}
			}
		}

		private string ChoiceTextContext()
		{
			return "events." + base.Event.Id + ".choice";
		}

		private string ChoiceOptionContext()
		{
			return "events." + base.Event.Id + ".choice." + Text;
		}
	}
}
