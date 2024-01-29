using System;
using System.Collections.Generic;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.StoryPlayer;

namespace LacieEngine.Modules.Tutorials
{
	[Serializable]
	public class StoryPlayerTutorialCommand : StoryPlayerCommand
	{
		[NonSerialized]
		[Inject]
		private ITutorialManager Tutorials;

		public string Value { get; set; }

		public string SaveEventLabel { get; set; }

		public string SaveLocation { get; set; }

		public string SaveImage { get; set; }

		public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
		{
			if (!Game.State.SeenTutorials.Contains(Value))
			{
				storyPlayer.Characters.HideAllCharacters();
				storyPlayer.UI.HideDialogueBox();
				Tutorials.ShowTutorial(Value);
				Tutorials.TutorialEnded += ResumeAfterTutorial;
				storyPlayer.SetNextBlockContinue();
			}
			else
			{
				storyPlayer.SetNextBlockContinue();
				storyPlayer.Next();
			}
		}

		public override void Load()
		{
			Tutorials.LoadResourcesForTutorial(Value);
		}

		public override IList<string> GetDependencies()
		{
			return Tutorials.GetDependencies(Value);
		}

		private void ResumeAfterTutorial()
		{
			Tutorials.TutorialEnded -= ResumeAfterTutorial;
			Game.InputProcessor = Inputs.Processor.StoryPlayer;
			Game.StoryPlayer.Next();
		}
	}
}
