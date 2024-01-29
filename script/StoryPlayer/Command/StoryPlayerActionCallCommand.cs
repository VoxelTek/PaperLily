using System;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerActionCallCommand : StoryPlayerCommand
	{
		[NonSerialized]
		[Inject]
		private IGameRoomManager Room;

		public string Value { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			if (Room.RegisteredRoomActions.ContainsKey(Value))
			{
				Room.RegisteredRoomActions[Value].Execute();
			}
			else
			{
				Log.Warn("Action ", Value, " not found in room: ", Room.CurrentRoom.Name);
			}
			storyPlayer.SetNextBlockContinue();
			storyPlayer.Next();
		}
	}
}
