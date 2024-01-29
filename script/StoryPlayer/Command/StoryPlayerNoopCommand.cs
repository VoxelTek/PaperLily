using System;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerNoopCommand : StoryPlayerCommand
	{
		public override void Execute(StoryPlayer storyPlayer)
		{
			storyPlayer.SetNextBlockContinue();
			storyPlayer.Next();
		}
	}
}
