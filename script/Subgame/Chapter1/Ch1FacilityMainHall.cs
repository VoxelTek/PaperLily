// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1FacilityMainHall
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1FacilityMainHall : Ch1FacilityRoom
  {
    private int layer = 1;
    [LacieEngine.API.GetNode("Other/BoundaryF1")]
    private StaticBody2D nBoundaryF1;
    [LacieEngine.API.GetNode("Other/BoundaryF2")]
    private StaticBody2D nBoundaryF2;
    [LacieEngine.API.GetNode("MainF1")]
    private Node2D nMainF1;
    [LacieEngine.API.GetNode("MainF2")]
    private Node2D nMainF2;
    [LacieEngine.API.GetNode("TilesWater")]
    private Node2D nTilesWater;
    [LacieEngine.API.GetNode("Background/WaterRipples")]
    private Node2D nWaterRipples1;
    [LacieEngine.API.GetNode("Background/WaterRipplesPillar")]
    private Node2D nWaterRipples2;
    [LacieEngine.API.GetNode("Other/trigger_f1_left")]
    private Area2D nTriggerF1a;
    [LacieEngine.API.GetNode("Other/trigger_f1_right")]
    private Area2D nTriggerF1b;
    [LacieEngine.API.GetNode("Other/trigger_f2_left")]
    private Area2D nTriggerF2a;
    [LacieEngine.API.GetNode("Other/trigger_f2_right")]
    private Area2D nTriggerF2b;
    [LacieEngine.API.GetNode("Other/EventsF1/move_hallway_tl")]
    private EventTrigger nMoveHallwayTl;
    [LacieEngine.API.GetNode("Other/EventsF1/move_hallway_tr")]
    private EventTrigger nMoveHallwayTr;
    [LacieEngine.API.GetNode("Other/EventsF1/move_hallway_bl")]
    private EventTrigger nMoveHallwayBl;
    [LacieEngine.API.GetNode("Other/EventsF1/move_hallway_br")]
    private EventTrigger nMoveHallwayBr;
    [LacieEngine.API.GetNode("Other/EventsF1/move_downstairs_left")]
    private EventTrigger nMoveDownstairsLeft;
    [LacieEngine.API.GetNode("Other/EventsF1/move_downstairs_right")]
    private EventTrigger nMoveDownstairsRight;
    [LacieEngine.API.GetNode("Other/EventsF1/misc_water")]
    private EventTrigger nWaterBlock;
    [LacieEngine.API.GetNode("Other/EventsF1/misc_gate")]
    private EventTrigger nGateEvt;
    [LacieEngine.API.GetNode("MainF1/chr_rune")]
    private Node2D nRuneSpr;
    [LacieEngine.API.GetNode("Other/EventsF1/chr_rune")]
    private EventTrigger nRuneEvt;
    [LacieEngine.API.GetNode("MainF1/FoliageTreeStump/Crow")]
    private Node2D nCrowSpr;
    [LacieEngine.API.GetNode("Background/Gate")]
    private Node2D nGate;
    private PVar varFloodEngaged = (PVar) "ch1.facility_flood_engaged";
    private PVar varGateOpen = (PVar) "ch1.facility_council_door_open";
    private PVar varRuneGone = (PVar) "ch1.facility_rune_gone";
    private PVar varSaveGone = (PVar) "ch1.facility_sai_attempting_leave";

    public override void _BeforeFadeIn()
    {
      int num1 = (int) this.nTriggerF1a.Connect("body_entered", (Object) this, "TriggerLayer1");
      int num2 = (int) this.nTriggerF1b.Connect("body_entered", (Object) this, "TriggerLayer1");
      int num3 = (int) this.nTriggerF2a.Connect("body_entered", (Object) this, "TriggerLayer2");
      int num4 = (int) this.nTriggerF2b.Connect("body_entered", (Object) this, "TriggerLayer2");
    }

    public override void _UpdateRoom()
    {
      base._UpdateRoom();
      bool flag = (bool) this.varFloodEngaged.Value;
      this.nWaterBlock.Enabled = !flag;
      this.nTilesWater.Visible = !flag;
      this.nWaterRipples1.Visible = !flag;
      this.nWaterRipples2.Visible = !flag;
      this.nGate.Visible = !(bool) this.varGateOpen.Value;
      this.nGateEvt.Enabled = this.nGate.Visible;
      this.nRuneSpr.Visible = !(bool) this.varRuneGone.Value;
      this.nRuneEvt.Enabled = !(bool) this.varRuneGone.Value;
      this.nCrowSpr.Visible = !(bool) this.varSaveGone.Value;
    }

    public override Node2D GetMainLayer() => this.layer != 1 ? this.nMainF2 : this.nMainF1;

    public void TriggerLayer1(Node body)
    {
      if (body != null && body.GetParent() == this.nMainF2)
        this.CallDeferred("ReparentCharacter", (object) body, (object) this.nMainF1);
      if (this.layer == 1)
        return;
      this.nBoundaryF1.CollisionLayer = 2U;
      this.nBoundaryF2.CollisionLayer = 0U;
      this.nMoveHallwayTl.Enabled = true;
      this.nMoveHallwayTr.Enabled = true;
      this.nMoveHallwayBl.Enabled = true;
      this.nMoveHallwayBr.Enabled = true;
      this.nMoveDownstairsLeft.Enabled = true;
      this.nMoveDownstairsRight.Enabled = true;
      this.layer = 1;
    }

    public void TriggerLayer2(Node body)
    {
      if (body != null && body.GetParent() == this.nMainF1)
        this.CallDeferred("ReparentCharacter", (object) body, (object) this.nMainF2);
      if (this.layer == 2)
        return;
      this.nBoundaryF1.CollisionLayer = 0U;
      this.nBoundaryF2.CollisionLayer = 2U;
      this.nMoveHallwayTl.Enabled = false;
      this.nMoveHallwayTr.Enabled = false;
      this.nMoveHallwayBl.Enabled = false;
      this.nMoveHallwayBr.Enabled = false;
      this.nMoveDownstairsLeft.Enabled = false;
      this.nMoveDownstairsRight.Enabled = false;
      this.layer = 2;
    }

    public override void ChangeLayer(int newLayer)
    {
      this.layer = 0;
      if (newLayer == 1)
        this.TriggerLayer1((Node) null);
      else
        this.TriggerLayer2((Node) null);
    }

    public void ReparentCharacter(Node node, Node to)
    {
      if (node is IPhysicsInterpolated physicsInterpolated)
        physicsInterpolated.Reparent(to);
      Game.Camera.TrackPlayer();
    }
  }
}
