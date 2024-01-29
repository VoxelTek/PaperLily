using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1CutsceneReleaseCaterpillar : GameRoom
	{
		[GetNode("Room")]
		private GameRoom nRoom;

		[GetNode("Animation")]
		private AnimationPlayer nAnimation;

		public override void _UpdateRoom()
		{
			nRoom._UpdateRoom();
		}

		public override Node2D GetMainLayer()
		{
			return nRoom.GetMainLayer();
		}

		public override Node FindNodeInRoom(string nodeName)
		{
			return nRoom.FindNodeInRoom(nodeName);
		}

		public void PlayAnimation()
		{
			nAnimation.PlayFirstAnimation();
		}
	}
}
