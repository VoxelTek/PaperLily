using System.Collections.Generic;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	public class StoryPlayerUnparsedCommand : StoryPlayerCommand
	{
		public List<string> Tokens { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			Log.Error("Error: Attempted to execute without assigning parser! in event [", base.Event.Id, "] line ", base.Index + 1);
			storyPlayer.SetNextBlockContinue();
			storyPlayer.Next();
		}
	}
}
