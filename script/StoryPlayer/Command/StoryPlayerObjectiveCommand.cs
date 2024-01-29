using System;
using LacieEngine.API;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerObjectiveCommand : StoryPlayerCommand
	{
		public enum ObjectiveOperation
		{
			Add,
			Complete,
			Fail,
			Remove,
			Clear,
			Silence
		}

		[NonSerialized]
		[Inject]
		private IObjectiveManager Objectives;

		public ObjectiveOperation Operation { get; set; }

		public string Objective { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			switch (Operation)
			{
			case ObjectiveOperation.Add:
				Objectives.Add(Objective);
				break;
			case ObjectiveOperation.Complete:
				Objectives.Complete(Objective);
				break;
			case ObjectiveOperation.Fail:
				Objectives.Fail(Objective);
				break;
			case ObjectiveOperation.Remove:
				Objectives.Remove(Objective);
				break;
			case ObjectiveOperation.Clear:
				Objectives.ClearObjectives();
				break;
			case ObjectiveOperation.Silence:
				Objectives.SilenceNotifications();
				break;
			}
			storyPlayer.SetNextBlockContinue();
			storyPlayer.Next();
		}
	}
}
