using System;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerChoiceCaseCommand : StoryPlayerCommand
	{
		public string Value { get; set; }

		public LogicStatement Condition { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			storyPlayer.SetNextBlockContinue();
			storyPlayer.Next();
		}
	}
}
