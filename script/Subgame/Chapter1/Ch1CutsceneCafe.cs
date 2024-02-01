// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1CutsceneCafe
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1CutsceneCafe : GameRoom
  {
	[Export(PropertyHint.None, "")]
	private Texture texLacieDark;
	[LacieEngine.API.GetNode("Ch2_Cafe")]
	private GameRoom nRoom;
	[LacieEngine.API.GetNode("Ch2_Cafe/Main/Table4Chair2/LacieSit")]
	private Sprite nLacieSit;
	[LacieEngine.API.GetNode("Ch2_Cafe/Main/Table4Chair2")]
	private Node2D nLacieChair;
	[LacieEngine.API.GetNode("Ch2_Cafe/Main/ChrCafeCashierDrop")]
	private Sprite nCashierDrop;
	[LacieEngine.API.GetNode("Ch2_Cafe/Main/CashierIdle")]
	private Node2D nCashierIdle;
	[LacieEngine.API.GetNode("Ch2_Cafe/Main/Table3Chair1/NpcA")]
	private NPCStaticTurnable nNpcA;
	[LacieEngine.API.GetNode("Ch2_Cafe/Background/FurnitureBench/NpcA2")]
	private NPCStaticTurnable nNpcA2;
	[LacieEngine.API.GetNode("Ch2_Cafe/Main/Table3Chair2/NpcB")]
	private NPCStaticTurnable nNpcB;
	[LacieEngine.API.GetNode("Ch2_Cafe/Background/FurnitureBench3/NpcB2")]
	private NPCStaticTurnable nNpcB2;
	[LacieEngine.API.GetNode("Ch2_Cafe/Background/FurnitureBench4/NpcC")]
	private NPCStaticTurnable nNpcC;
	[LacieEngine.API.GetNode("Ch2_Cafe/Background/FurnitureBench2/NpcD")]
	private NPCStaticTurnable nNpcD;
	[LacieEngine.API.GetNode("Ch2_Cafe/Background/Table1Chair4/NpcE")]
	private NPCStaticTurnable nNpcE;
	[LacieEngine.API.GetNode("Ch2_Cafe/Background/Table1Chair4/NpcEIdle")]
	private SimpleAnimatedSprite nNpcEIdle;
	[LacieEngine.API.GetNode("Ch2_Cafe/Main/Table2/NpcG")]
	private NPCStaticTurnable nNpcG;

	public override void _UpdateRoom() => this.nRoom._UpdateRoom();

	public override Node2D GetMainLayer() => this.nRoom.GetMainLayer();

	public override void _BeforeFadeIn()
	{
	  this.nLacieSit.Visible = false;
	  this.nCashierDrop.Visible = false;
	  this.nNpcE.Visible = false;
	  this.nNpcA2.Visible = false;
	  this.nNpcB2.Visible = false;
	}

	public async void CashierDrop()
	{
	  Ch1CutsceneCafe baseNode = this;
	  NPCWalker cashier = baseNode.GetMainLayer().GetNode<NPCWalker>((NodePath) "Ch1_Cashier");
	  cashier.Visible = false;
	  baseNode.nCashierDrop.Visible = true;
	  baseNode.nCashierDrop.Frame = 0;
	  await baseNode.DelaySeconds(1.5);
	  baseNode.nCashierDrop.Frame = 1;
	  await baseNode.DelaySeconds(0.3);
	  baseNode.nCashierDrop.Frame = 2;
	  cashier.Turn(Direction.Down);
	  NPCWalker npcWalker = cashier;
	  npcWalker.Position = npcWalker.Position + new Vector2(-1f, -1f);
	  cashier.Visible = true;
	  Game.Animations.Play((LacieAnimation) new FadeOutAnimation((CanvasItem) baseNode.nCashierDrop, 1f));
	  cashier = (NPCWalker) null;
	}

	public void LacieSit()
	{
	  this.GetMainLayer().GetNode<Node2D>((NodePath) "Lacie").Visible = false;
	  this.nLacieSit.Visible = true;
	}

	public void GirlsABStandUp()
	{
	  this.nNpcA.Visible = false;
	  this.nNpcB.Visible = false;
	  this.nLacieSit.Frame = 1;
	}

	public void GirlsABSit()
	{
	  NPCWalker registeredNpC1 = Game.Room.RegisteredNPCs["ch1_npc_a"] as NPCWalker;
	  NPCWalker registeredNpC2 = Game.Room.RegisteredNPCs["ch1_npc_b"] as NPCWalker;
	  registeredNpC1.Visible = false;
	  registeredNpC2.Visible = false;
	  this.nNpcA2.Visible = true;
	  this.nNpcB2.Visible = true;
	}

	public void LacieGetUp()
	{
	  this.nLacieSit.Visible = false;
	  this.nLacieChair.Position += Vector2.Right * 20f;
	  NPCWalker registeredNpC = Game.Room.RegisteredNPCs["lacie"] as NPCWalker;
	  registeredNpC.OverrideTextureForState("stand", this.texLacieDark);
	  registeredNpC.OverrideTextureForState("walk", this.texLacieDark);
	  registeredNpC.Position = this.GetPoint("get_up");
	  registeredNpC.Turn((Direction) this.GetSpawnPoint("get_up").Direction);
	  registeredNpC.Visible = true;
	  registeredNpC.Teleport();
	}

	public void CashierIdle()
	{
	  Game.Room.RegisteredNPCs["ch1_cashier"].Visible = false;
	  this.nCashierIdle.Visible = true;
	}

	public void CashierTurn()
	{
	  this.nCashierIdle.Visible = false;
	  Game.Room.RegisteredNPCs["ch1_cashier"].Visible = true;
	  ((ITurnable) Game.Room.RegisteredNPCs["ch1_cashier"]).Turn(Direction.Down);
	}

	public void NpcATurnUp() => this.nNpcA.Turn(Direction.Up);

	public void NpcA2TurnDown() => this.nNpcA2.Turn(Direction.Down);

	public void NpcBTurnUp() => this.nNpcB.Turn(Direction.Up);

	public void NpcBTurnBack() => this.nNpcB.Turn(Direction.Right);

	public void NpcB2TurnDown() => this.nNpcB2.Turn(Direction.Down);

	public void NpcCTurnDown() => this.nNpcC.Turn(Direction.Down);

	public void NpcDTurnDown() => this.nNpcD.Turn(Direction.Down);

	public void NpcETurnDown()
	{
	  this.nNpcE.Visible = true;
	  this.nNpcEIdle.Visible = false;
	  this.nNpcE.Turn(Direction.Down);
	}

	public void NpcGTurnRight() => this.nNpcG.Turn(Direction.Right);

	public void NpcARestore() => this.nNpcA.TurnToDefault();

	public void NpcBRestore() => this.nNpcB.TurnToDefault();

	public void NpcCRestore() => this.nNpcC.TurnToDefault();

	public void NpcDRestore() => this.nNpcD.TurnToDefault();

	public void NpcERestore()
	{
	  this.nNpcE.Visible = false;
	  this.nNpcEIdle.Visible = true;
	}

	public void NpcGRestore() => this.nNpcG.TurnToDefault();
  }
}
