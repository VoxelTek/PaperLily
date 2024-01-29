using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1CutsceneHome2fBedroomA : GameRoom
	{
		[GetNode("Room")]
		private GameRoom nRoom;

		[GetNode("Room/Background/furniture_bed/LacieLiedown")]
		private Sprite nLacieLiedown;

		[GetNode("Room/Main/furniture_table_lacie/MiscLetter")]
		private Node2D nGoldenLetter;

		[GetNode("Room/Main/LacieLetter")]
		private Node2D nLacieLetter;

		public override void _UpdateRoom()
		{
			nRoom._UpdateRoom();
		}

		public override Node2D GetMainLayer()
		{
			return nRoom.GetMainLayer();
		}

		public void LacieLieDown()
		{
			Game.Room.RegisteredNPCs["lacie"].Visible = false;
			nLacieLiedown.Visible = true;
		}

		public void LacieGetUp()
		{
			nLacieLiedown.Visible = false;
			Game.Room.RegisteredNPCs["lacie"].Visible = true;
		}

		public void PickUpLetter()
		{
			nGoldenLetter.Visible = false;
			nLacieLetter.Visible = true;
			Game.Room.RegisteredNPCs["lacie"].Visible = false;
		}

		public void PutAwayLetter()
		{
			nLacieLetter.Visible = false;
			Game.Room.RegisteredNPCs["lacie"].Visible = true;
		}

		public void LacieBedCloseEyes()
		{
			nLacieLiedown.Frame = 0;
		}

		public void LacieBedOpenEyes()
		{
			nLacieLiedown.Frame = 1;
		}
	}
}
