using Godot;
using LacieEngine.API;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1CutsceneMkB1fHallway : GameRoom
	{
		[GetNode("Room")]
		private GameRoom nRoom;

		public override Node2D GetMainLayer()
		{
			return nRoom.GetMainLayer();
		}

		public override Node FindNodeInRoom(string nodeName)
		{
			return nRoom.FindNodeInRoom(nodeName);
		}
	}
}
