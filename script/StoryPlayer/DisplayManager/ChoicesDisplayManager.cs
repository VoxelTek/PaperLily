using System.Collections.Generic;
using Godot;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.StoryPlayer
{
	public class ChoicesDisplayManager
	{
		private Control nChoices;

		private Control nChoiceList;

		private StoryPlayer storyPlayer;

		private List<string> choiceLabels;

		private List<int> choiceJumps;

		private int currentChoice;

		public bool HasChoices => choiceJumps.Count > 0;

		public bool Visible => nChoices.Visible;

		public ChoicesDisplayManager(StoryPlayer storyPlayer, Control nChoices)
		{
			this.storyPlayer = storyPlayer;
			this.nChoices = nChoices;
			nChoices.Visible = false;
			choiceLabels = new List<string>();
			choiceJumps = new List<int>();
		}

		public void Show()
		{
			Frame frame = Game.Settings.UiProvider.MakeChoicesFrame();
			nChoiceList = UIUtil.CreateOptionList(choiceLabels);
			frame.AddToFrame(nChoiceList);
			nChoices.AddChild(frame);
			currentChoice = -1;
			nChoices.Visible = true;
		}

		public void ClearAndHide()
		{
			choiceLabels.Clear();
			choiceJumps.Clear();
			foreach (Node child in nChoices.GetChildren())
			{
				child.Delete();
			}
			nChoices.Visible = false;
		}

		public void AddChoice(string choiceText, int line)
		{
			choiceLabels.Add(choiceText);
			choiceJumps.Add(line);
		}

		public void HandleInput(InputEvent @event)
		{
			if (@event.IsActionPressed("input_up"))
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				if (--currentChoice <= -1)
				{
					currentChoice = choiceJumps.Count - 1;
				}
				UIUtil.HighlightItem(nChoiceList, currentChoice);
			}
			else if (@event.IsActionPressed("input_down"))
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				if (++currentChoice >= choiceJumps.Count)
				{
					currentChoice = 0;
				}
				UIUtil.HighlightItem(nChoiceList, currentChoice);
			}
			else if (@event.IsActionPressed("input_action") && currentChoice >= 0)
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
				int gotoLine = choiceJumps[currentChoice];
				storyPlayer.SetNextBlockToLine(gotoLine);
				ClearAndHide();
				storyPlayer.Next();
			}
		}
	}
}
