using System;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerPauseCommand : StoryPlayerCommand
	{
		public enum PauseType
		{
			Pause,
			Wait
		}

		public PauseType Type { get; set; }

		public float Time { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			if (Type == PauseType.Wait)
			{
				storyPlayer.UI.HideDialogueBox();
				storyPlayer.nContinueIndicator2.Visible = true;
				storyPlayer.SetNextBlockContinue();
				storyPlayer.AdvanceDisabled = false;
			}
			else if (Type == PauseType.Pause)
			{
				storyPlayer.SetNextBlockContinue();
				storyPlayer.NextAfterSeconds(Time);
			}
		}
	}
}
