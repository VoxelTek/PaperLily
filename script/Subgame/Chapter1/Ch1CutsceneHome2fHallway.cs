using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1CutsceneHome2fHallway : GameRoom
	{
		[GetNode("Room")]
		private GameRoom nRoom;

		[GetNode("Room/Main/Ruffle")]
		private SimpleAnimatedSprite nRuffle;

		[GetNode("Room/Main/FixHair")]
		private SimpleAnimatedSprite nFixHair;

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

		public void Ruffle()
		{
			Node2D lacie = GetMainLayer().GetNode<Node2D>("Lacie");
			Node2D node = GetMainLayer().GetNode<Node2D>("Hiro");
			lacie.Visible = false;
			node.Visible = false;
			nRuffle.Visible = true;
			nRuffle.Playing = true;
		}

		public void PostRuffle()
		{
			Node2D node = GetMainLayer().GetNode<Node2D>("Hiro");
			nFixHair.Visible = true;
			node.Visible = true;
			nRuffle.Visible = false;
		}

		public void FixHair()
		{
			nFixHair.Playing = true;
		}
	}
}
