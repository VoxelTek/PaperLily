using System;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerLabelCommand : StoryPlayerCommand
	{
		public string Label { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			storyPlayer.IncreaseIndent();
			storyPlayer.SetNextBlockContinue();
			storyPlayer.Next();
		}
	}
}
