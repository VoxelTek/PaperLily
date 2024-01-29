using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1CutsceneFacilityB2fHallwayEast : GameRoomNested
	{
		[GetNode("Room/Background/Cutscene1")]
		private SimpleAnimatedSprite nCutscene1;

		[GetNode("Room/Background/Cutscene2")]
		private SimpleAnimatedSprite nCutscene2;

		[GetNode("Room/Background/Cutscene3")]
		private SimpleAnimatedSprite nCutscene3;

		[GetNode("Room/Background/Cutscene4")]
		private SimpleAnimatedSprite nCutscene4;

		[GetNode("Room/Background/SaiSmile")]
		private Node2D nSaiSmile;

		private static readonly Color SAI_TINT = new Color("#0a0a0a");

		public void CharactersInit()
		{
			Game.Room.RegisteredNPCs["sai"].Modulate = SAI_TINT;
			Game.Room.RegisteredNPCs["lacie"].Visible = false;
		}

		public void LacieStopRubbingButt()
		{
			nCutscene1.Playing = false;
			nCutscene1.Frame = 2;
		}

		public void LacieLookLeft()
		{
			nCutscene2.Visible = false;
			nCutscene1.Visible = true;
			nCutscene1.Frame = 3;
		}

		public void LacieLookLeftShade()
		{
			nCutscene1.Frame = 4;
		}

		public void SaiIlluminate()
		{
			Node2D nSai = Game.Room.RegisteredNPCs["sai"];
			Game.Animations.Play(new ModulateAnimation(nSai, 0.5f, Colors.Black, Colors.White));
		}

		public void SaiSmile()
		{
			Game.Room.RegisteredNPCs["sai"].Visible = false;
			nSaiSmile.Visible = true;
		}

		public void SaiDontSmile()
		{
			Game.Room.RegisteredNPCs["sai"].Visible = true;
			nSaiSmile.Visible = false;
		}

		public void LacieThrowSalt()
		{
			nCutscene1.Visible = false;
			nCutscene2.Visible = true;
			nCutscene2.Playing = true;
		}

		public void LacieRealizeItWasDumb()
		{
			nCutscene2.Playing = false;
			nCutscene2.Frame = 14;
			Game.Animations.Play(new FadeOutAnimation(nCutscene2, 0.5f));
			nCutscene3.Visible = true;
		}

		public void SaiReachOut()
		{
			Game.Room.RegisteredNPCs["sai"].Visible = false;
			nCutscene2.Visible = false;
			nCutscene3.Visible = true;
			nCutscene3.Playing = true;
		}

		public void LacieStandUpByHerself()
		{
			nCutscene3.Visible = false;
			nCutscene4.Visible = true;
			nCutscene4.Playing = true;
		}

		public void LacieAndSaiReset()
		{
			Node2D nSai = Game.Room.RegisteredNPCs["sai"];
			Node2D node2D = Game.Room.RegisteredNPCs["lacie"];
			nCutscene4.Visible = false;
			nSai.Visible = true;
			node2D.Visible = true;
		}
	}
}
