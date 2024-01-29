using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1CutsceneHomeExteriorBack : GameRoom
	{
		[GetNode("Room")]
		private GameRoom nRoom;

		[GetNode("Room/Background/furniture_bench/LacieSit")]
		private Sprite nLacieSit;

		[GetNode("Room/Background/furniture_bench/HiroSit")]
		private Sprite nHiroSit;

		[GetNode("Room/Background/furniture_bench/chr_hiro")]
		private NpcStaticTurnableVer2 nHiroNpc;

		private NPCWalker nLacie;

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

		public override void _BeforeFadeIn()
		{
			nLacie = new NPCWalker("lacie", "right");
			nLacie.Position = GetPoint("lacie");
			nHiroNpc.Turn(Direction.Left);
			GetMainLayer().AddChild(nLacie);
		}

		public async void LacieSit()
		{
			await this.DelaySeconds(0.3);
			nHiroNpc.Turn(Direction.Down);
			await this.DelaySeconds(0.3);
			nHiroNpc.Turn(Direction.Right);
			await this.DelaySeconds(0.3);
			nLacie.Visible = false;
			nLacieSit.Visible = true;
		}

		public void LacieStand()
		{
			nLacie.Visible = true;
			nLacieSit.Visible = false;
		}

		public void HiroRead()
		{
			nHiroNpc.Modulate = Colors.Transparent;
			nHiroSit.Visible = true;
		}

		public void HiroTurn()
		{
			nHiroSit.Frame = 1;
		}

		public void HiroTurnBack()
		{
			nHiroSit.Frame = 0;
		}

		public void HiroSmile()
		{
			nHiroSit.Frame = 2;
		}

		public void LacieTurn()
		{
			nLacieSit.Frame = 1;
		}

		public void LacieTurnBack()
		{
			nLacieSit.Frame = 0;
		}
	}
}
