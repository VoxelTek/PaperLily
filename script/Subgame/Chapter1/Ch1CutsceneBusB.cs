using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1CutsceneBusB : GameRoom
	{
		[GetNode("Ch1_Bus_B")]
		private GameRoom nRoom;

		[GetNode("Ch1_Bus_B/Background/seats_b5/lacie/Animation")]
		private AnimationPlayer nLacieScoochAnimation;

		public override Node FindNodeInRoom(string nodeName)
		{
			return nRoom.FindNodeInRoom(nodeName);
		}

		public void LacieScoochOut()
		{
			nLacieScoochAnimation.PlayFirstAnimation();
		}
	}
}
