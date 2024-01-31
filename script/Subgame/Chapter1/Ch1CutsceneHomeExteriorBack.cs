// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1CutsceneHomeExteriorBack
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1CutsceneHomeExteriorBack : GameRoom
  {
    [LacieEngine.API.GetNode("Room")]
    private GameRoom nRoom;
    [LacieEngine.API.GetNode("Room/Background/furniture_bench/LacieSit")]
    private Sprite nLacieSit;
    [LacieEngine.API.GetNode("Room/Background/furniture_bench/HiroSit")]
    private Sprite nHiroSit;
    [LacieEngine.API.GetNode("Room/Background/furniture_bench/chr_hiro")]
    private NpcStaticTurnableVer2 nHiroNpc;
    private NPCWalker nLacie;

    public override void _UpdateRoom() => this.nRoom._UpdateRoom();

    public override Node2D GetMainLayer() => this.nRoom.GetMainLayer();

    public override Node FindNodeInRoom(string nodeName) => this.nRoom.FindNodeInRoom(nodeName);

    public override void _BeforeFadeIn()
    {
      this.nLacie = new NPCWalker("lacie", "right");
      this.nLacie.Position = this.GetPoint("lacie");
      this.nHiroNpc.Turn(Direction.Left);
      this.GetMainLayer().AddChild((Node) this.nLacie);
    }

    public async void LacieSit()
    {
      Ch1CutsceneHomeExteriorBack baseNode = this;
      await baseNode.DelaySeconds(0.3);
      baseNode.nHiroNpc.Turn(Direction.Down);
      await baseNode.DelaySeconds(0.3);
      baseNode.nHiroNpc.Turn(Direction.Right);
      await baseNode.DelaySeconds(0.3);
      baseNode.nLacie.Visible = false;
      baseNode.nLacieSit.Visible = true;
    }

    public void LacieStand()
    {
      this.nLacie.Visible = true;
      this.nLacieSit.Visible = false;
    }

    public void HiroRead()
    {
      this.nHiroNpc.Modulate = Godot.Colors.Transparent;
      this.nHiroSit.Visible = true;
    }

    public void HiroTurn() => this.nHiroSit.Frame = 1;

    public void HiroTurnBack() => this.nHiroSit.Frame = 0;

    public void HiroSmile() => this.nHiroSit.Frame = 2;

    public void LacieTurn() => this.nLacieSit.Frame = 1;

    public void LacieTurnBack() => this.nLacieSit.Frame = 0;
  }
}
