using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1CutsceneFacilityElevator : GameRoomNested
	{
		[GetNode("Room/Background/Flood1")]
		private Node2D nFlood1;

		[GetNode("Room/Background/Flood2")]
		private Node2D nFlood2;

		[GetNode("Room/Background/Flood3")]
		private Node2D nFlood3;

		[GetNode("Room/Background/Flood4")]
		private Node2D nFlood4;

		[GetNode("Room/Background/Flood5")]
		private Node2D nFlood5;

		[GetNode("Room/Background/Flood6")]
		private Node2D nFlood6;

		[GetNode("Lighting/dark")]
		private RoomLighting nLightDark;

		public override void _BeforeFadeIn()
		{
			((Ch1FacilityElevator)nRoom).Riding = true;
		}

		public void Flood0()
		{
			((Ch1FacilityElevator)nRoom).Riding = false;
			Game.Camera.Shake(0.3f, 10f);
		}

		public void PowerDown()
		{
			nLightDark.Apply();
		}

		public void Flood1()
		{
			nFlood1.Visible = true;
		}

		public void Flood2()
		{
			nFlood1.Visible = false;
			nFlood2.Visible = true;
		}

		public void Flood3()
		{
			nFlood2.Visible = false;
			nFlood3.Visible = true;
		}

		public void Flood4()
		{
			nFlood3.Visible = false;
			nFlood4.Visible = true;
		}

		public void Flood5()
		{
			nFlood4.Visible = false;
			nFlood5.Visible = true;
		}

		public void Flood6()
		{
			nFlood5.Visible = false;
			nFlood6.Visible = true;
		}
	}
}
