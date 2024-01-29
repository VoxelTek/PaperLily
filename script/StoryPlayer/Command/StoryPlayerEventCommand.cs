using System;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerEventCommand : StoryPlayerCommand
	{
		public enum EventOperation
		{
			Start,
			End,
			HideUi,
			SkipOn,
			SkipOff,
			AutoOn,
			AutoOff,
			Call,
			Queue
		}

		public EventOperation Operation { get; set; }

		public string Value { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			switch (Operation)
			{
			case EventOperation.Start:
				storyPlayer.Event = true;
				break;
			case EventOperation.End:
				storyPlayer.Event = false;
				break;
			case EventOperation.HideUi:
				storyPlayer.HideAllUI();
				break;
			case EventOperation.SkipOn:
				storyPlayer.Text.SkipDisabled = false;
				break;
			case EventOperation.SkipOff:
				storyPlayer.Text.SkipDisabled = true;
				break;
			case EventOperation.AutoOn:
				storyPlayer.Text.AutoNext = true;
				break;
			case EventOperation.AutoOff:
				storyPlayer.Text.AutoNext = false;
				break;
			case EventOperation.Call:
				storyPlayer.SetNextBlockContinue();
				storyPlayer.CallEvent(Value);
				return;
			case EventOperation.Queue:
				storyPlayer.AddToCallQueue(Value);
				break;
			}
			storyPlayer.SetNextBlockContinue();
			storyPlayer.Next();
		}
	}
}
