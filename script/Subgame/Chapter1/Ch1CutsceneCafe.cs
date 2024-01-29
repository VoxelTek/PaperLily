using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1CutsceneCafe : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Texture texLacieDark;

		[GetNode("Ch2_Cafe")]
		private GameRoom nRoom;

		[GetNode("Ch2_Cafe/Main/Table4Chair2/LacieSit")]
		private Sprite nLacieSit;

		[GetNode("Ch2_Cafe/Main/Table4Chair2")]
		private Node2D nLacieChair;

		[GetNode("Ch2_Cafe/Main/ChrCafeCashierDrop")]
		private Sprite nCashierDrop;

		[GetNode("Ch2_Cafe/Main/CashierIdle")]
		private Node2D nCashierIdle;

		[GetNode("Ch2_Cafe/Main/Table3Chair1/NpcA")]
		private NPCStaticTurnable nNpcA;

		[GetNode("Ch2_Cafe/Background/FurnitureBench/NpcA2")]
		private NPCStaticTurnable nNpcA2;

		[GetNode("Ch2_Cafe/Main/Table3Chair2/NpcB")]
		private NPCStaticTurnable nNpcB;

		[GetNode("Ch2_Cafe/Background/FurnitureBench3/NpcB2")]
		private NPCStaticTurnable nNpcB2;

		[GetNode("Ch2_Cafe/Background/FurnitureBench4/NpcC")]
		private NPCStaticTurnable nNpcC;

		[GetNode("Ch2_Cafe/Background/FurnitureBench2/NpcD")]
		private NPCStaticTurnable nNpcD;

		[GetNode("Ch2_Cafe/Background/Table1Chair4/NpcE")]
		private NPCStaticTurnable nNpcE;

		[GetNode("Ch2_Cafe/Background/Table1Chair4/NpcEIdle")]
		private SimpleAnimatedSprite nNpcEIdle;

		[GetNode("Ch2_Cafe/Main/Table2/NpcG")]
		private NPCStaticTurnable nNpcG;

		public override void _UpdateRoom()
		{
			nRoom._UpdateRoom();
		}

		public override Node2D GetMainLayer()
		{
			return nRoom.GetMainLayer();
		}

		public override void _BeforeFadeIn()
		{
			nLacieSit.Visible = false;
			nCashierDrop.Visible = false;
			nNpcE.Visible = false;
			nNpcA2.Visible = false;
			nNpcB2.Visible = false;
		}

		public async void CashierDrop()
		{
			NPCWalker cashier = GetMainLayer().GetNode<NPCWalker>("Ch1_Cashier");
			cashier.Visible = false;
			nCashierDrop.Visible = true;
			nCashierDrop.Frame = 0;
			await this.DelaySeconds(1.5);
			nCashierDrop.Frame = 1;
			await this.DelaySeconds(0.3);
			nCashierDrop.Frame = 2;
			cashier.Turn(Direction.Down);
			cashier.Position += new Vector2(-1f, -1f);
			cashier.Visible = true;
			Game.Animations.Play(new FadeOutAnimation(nCashierDrop, 1f));
		}

		public void LacieSit()
		{
			GetMainLayer().GetNode<Node2D>("Lacie").Visible = false;
			nLacieSit.Visible = true;
		}

		public void GirlsABStandUp()
		{
			nNpcA.Visible = false;
			nNpcB.Visible = false;
			nLacieSit.Frame = 1;
		}

		public void GirlsABSit()
		{
			NPCWalker npcA = Game.Room.RegisteredNPCs["ch1_npc_a"] as NPCWalker;
			NPCWalker obj = Game.Room.RegisteredNPCs["ch1_npc_b"] as NPCWalker;
			npcA.Visible = false;
			obj.Visible = false;
			nNpcA2.Visible = true;
			nNpcB2.Visible = true;
		}

		public void LacieGetUp()
		{
			nLacieSit.Visible = false;
			nLacieChair.Position += Vector2.Right * 20f;
			NPCWalker obj = Game.Room.RegisteredNPCs["lacie"] as NPCWalker;
			obj.OverrideTextureForState("stand", texLacieDark);
			obj.OverrideTextureForState("walk", texLacieDark);
			obj.Position = GetPoint("get_up");
			obj.Turn(GetSpawnPoint("get_up").Direction);
			obj.Visible = true;
			obj.Teleport();
		}

		public void CashierIdle()
		{
			Game.Room.RegisteredNPCs["ch1_cashier"].Visible = false;
			nCashierIdle.Visible = true;
		}

		public void CashierTurn()
		{
			nCashierIdle.Visible = false;
			Game.Room.RegisteredNPCs["ch1_cashier"].Visible = true;
			((ITurnable)Game.Room.RegisteredNPCs["ch1_cashier"]).Turn(Direction.Down);
		}

		public void NpcATurnUp()
		{
			nNpcA.Turn(Direction.Up);
		}

		public void NpcA2TurnDown()
		{
			nNpcA2.Turn(Direction.Down);
		}

		public void NpcBTurnUp()
		{
			nNpcB.Turn(Direction.Up);
		}

		public void NpcBTurnBack()
		{
			nNpcB.Turn(Direction.Right);
		}

		public void NpcB2TurnDown()
		{
			nNpcB2.Turn(Direction.Down);
		}

		public void NpcCTurnDown()
		{
			nNpcC.Turn(Direction.Down);
		}

		public void NpcDTurnDown()
		{
			nNpcD.Turn(Direction.Down);
		}

		public void NpcETurnDown()
		{
			nNpcE.Visible = true;
			nNpcEIdle.Visible = false;
			nNpcE.Turn(Direction.Down);
		}

		public void NpcGTurnRight()
		{
			nNpcG.Turn(Direction.Right);
		}

		public void NpcARestore()
		{
			nNpcA.TurnToDefault();
		}

		public void NpcBRestore()
		{
			nNpcB.TurnToDefault();
		}

		public void NpcCRestore()
		{
			nNpcC.TurnToDefault();
		}

		public void NpcDRestore()
		{
			nNpcD.TurnToDefault();
		}

		public void NpcERestore()
		{
			nNpcE.Visible = false;
			nNpcEIdle.Visible = true;
		}

		public void NpcGRestore()
		{
			nNpcG.TurnToDefault();
		}
	}
}
