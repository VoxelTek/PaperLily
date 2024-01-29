using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1CutsceneMkLivingroom : GameRoom
	{
		[GetNode("Room")]
		private GameRoom nRoom;

		[GetNode("Room/Main/LacieSit")]
		private Node2D nLacieSit;

		[GetNode("Room/Main/SaiSit")]
		private Node2D nSaiSit;

		[GetNode("Room/Main/MkAnxious")]
		private SimpleAnimatedSprite nMkAnxious;

		[GetNode("Room/Main/MkConfused")]
		private Node2D nMkConfused;

		[GetNode("Room/Main/MkFireoff")]
		private SimpleAnimatedSprite nMkFireoff;

		[GetNode("Room/Main/SaiLessChill")]
		private Node2D nSaiLessChill;

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

		public void SaiSitDown()
		{
			Game.Room.RegisteredNPCs["sai"].Visible = false;
			nSaiSit.Visible = true;
		}

		public void LacieSitDown()
		{
			Game.Room.RegisteredNPCs["lacie"].Visible = false;
			nLacieSit.Visible = true;
		}

		public void BothStandUp()
		{
			nLacieSit.Visible = false;
			nSaiSit.Visible = false;
			Game.Room.RegisteredNPCs["lacie"].Visible = true;
			Game.Room.RegisteredNPCs["sai"].Visible = true;
		}

		public void CagesFall()
		{
			nAnimation.PlayFirstAnimation();
		}

		public void MkConfused()
		{
			Game.Room.RegisteredNPCs["mk"].Visible = false;
			nMkConfused.Visible = true;
		}

		public void HideMkConfused()
		{
			nMkConfused.Visible = false;
			Game.Room.RegisteredNPCs["mk"].Visible = true;
		}

		public void MkFireoff()
		{
			Game.Room.RegisteredNPCs["mk"].Visible = false;
			nMkFireoff.Visible = true;
			nMkFireoff.Playing = true;
		}

		public void HideMkFireoff()
		{
			nMkFireoff.Visible = false;
			Game.Room.RegisteredNPCs["mk"].Visible = true;
		}

		public void MkAnxious()
		{
			Game.Room.RegisteredNPCs["mk"].Visible = false;
			nMkAnxious.Visible = true;
			nMkAnxious.Playing = true;
		}

		public void MkStopAnim()
		{
			nMkAnxious.Playing = false;
			nMkAnxious.Frame = 1;
		}

		public void ShowLessChillSai()
		{
			Game.Room.RegisteredNPCs["sai"].Visible = false;
			nSaiLessChill.Visible = true;
		}
	}
}
