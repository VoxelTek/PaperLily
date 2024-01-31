// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1ForestPath2
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;
using System;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1ForestPath2 : GameRoom
  {
    [Inject]
    private IVariableManager Vars;
    [Inject]
    private IEventManager Events;
    [LacieEngine.API.GetNode("Main/signpost_blue_1")]
    private Sprite nBlueSign1;
    [LacieEngine.API.GetNode("Main/signpost_blue_2")]
    private Sprite nBlueSign2;
    [LacieEngine.API.GetNode("Main/signpost_blue_3")]
    private Sprite nBlueSign3;
    [LacieEngine.API.GetNode("Main/signpost_yellow_1")]
    private Sprite nYellowSign1;
    [LacieEngine.API.GetNode("Main/signpost_yellow_2")]
    private Sprite nYellowSign2;
    [LacieEngine.API.GetNode("Main/signpost_yellow_3")]
    private Sprite nYellowSign3;
    [LacieEngine.API.GetNode("Main/signpost_red_1")]
    private Sprite nRedSign1;
    [LacieEngine.API.GetNode("Main/signpost_red_2")]
    private Sprite nRedSign2;
    [LacieEngine.API.GetNode("TilesNorthBlock1")]
    private TileMap nTilesNorthBlock1;
    [LacieEngine.API.GetNode("TilesNorthBlock2")]
    private TileMap nTilesNorthBlock2;
    [LacieEngine.API.GetNode("Background/EntranceL")]
    private Node2D nEntranceL;
    [LacieEngine.API.GetNode("Background/EntranceR")]
    private Node2D nEntranceR;
    [LacieEngine.API.GetNode("Other/Boundary")]
    private StaticBody2D nPassageWalls;
    [LacieEngine.API.GetNode("Other/BoundaryInStairs")]
    private StaticBody2D nTunnelWalls;
    [LacieEngine.API.GetNode("Other/Events/move_station")]
    private EventTrigger nPassageEvent;
    [LacieEngine.API.GetNode("Main/StructStairs")]
    private Node2D nPassageStairs;
    [LacieEngine.API.GetNode("Main/StructStairsFence")]
    private Node2D nPassageFence;
    [LacieEngine.API.GetNode("Foreground/MiscStairsMask")]
    private Node2D nHidePlayerMask;
    [LacieEngine.API.GetNode("Other/Stairs")]
    private Stairs nStairs;
    [LacieEngine.API.GetNode("Other/ActivateStairsTrigger")]
    private Area2D nStairsTrigger;
    [LacieEngine.API.GetNode("Lighting/LightStationAccess")]
    private Node2D nLightStationAccess;
    private PVar varTryResetPuzzle = (PVar) "ch1.temp_forest_trigger_event_try_reset";
    private string _signToRotate;

    public override void _BeforeFadeIn()
    {
      this.Vars.SetInitialValue("ch1.forest_entrance_sign_blue_1", "left");
      this.Vars.SetInitialValue("ch1.forest_entrance_sign_blue_2", "left");
      this.Vars.SetInitialValue("ch1.forest_entrance_sign_blue_3", "left");
      this.Vars.SetInitialValue("ch1.forest_entrance_sign_yellow_1", "left");
      this.Vars.SetInitialValue("ch1.forest_entrance_sign_yellow_2", "left");
      this.Vars.SetInitialValue("ch1.forest_entrance_sign_yellow_3", "left");
      this.Vars.SetInitialValue("ch1.forest_entrance_sign_red_1", "left");
      this.Vars.SetInitialValue("ch1.forest_entrance_sign_red_2", "left");
      if (this.Vars.GetFlag("ch1.forest_entrance_gate_open") || this.Events.SeenEvent("Ch1_Forest_Entrance.event_try_reset") || 0 + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_1"), 1) + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_2"), 1) + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_3"), 1) + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_4"), 1) + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_5"), 1) + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_6"), 1) < 4)
        return;
      this.TriggerEventTryReset();
    }

    public override void _UpdateRoom()
    {
      this.UpdateSignDirection(this.nBlueSign1, "ch1.forest_entrance_sign_blue_1");
      this.UpdateSignDirection(this.nBlueSign2, "ch1.forest_entrance_sign_blue_2");
      this.UpdateSignDirection(this.nBlueSign3, "ch1.forest_entrance_sign_blue_3");
      this.UpdateSignDirection(this.nYellowSign1, "ch1.forest_entrance_sign_yellow_1");
      this.UpdateSignDirection(this.nYellowSign2, "ch1.forest_entrance_sign_yellow_2");
      this.UpdateSignDirection(this.nYellowSign3, "ch1.forest_entrance_sign_yellow_3");
      this.UpdateSignDirection(this.nRedSign1, "ch1.forest_entrance_sign_red_1");
      this.UpdateSignDirection(this.nRedSign2, "ch1.forest_entrance_sign_red_2");
      if (this.Vars.GetFlag("ch1.forest_entrance_south_opened_exit"))
      {
        this.nTilesNorthBlock1.Visible = false;
        this.nTilesNorthBlock1.CollisionLayer = 0U;
        this.nTilesNorthBlock2.Visible = false;
        this.nTilesNorthBlock2.CollisionLayer = 0U;
      }
      else
      {
        this.nTilesNorthBlock1.Visible = true;
        this.nTilesNorthBlock1.CollisionLayer = 2U;
        this.nTilesNorthBlock2.Visible = true;
        this.nTilesNorthBlock2.CollisionLayer = 2U;
      }
      this.DeactivateStairs((Node) null);
      if (this.Vars.GetFlag("ch1.forest_entrance_south_opened_station"))
      {
        this.nPassageWalls.CollisionLayer = 2U;
        this.nStairsTrigger.CollisionLayer = 1U;
        this.nPassageStairs.Visible = true;
        this.nPassageFence.Visible = true;
        this.nLightStationAccess.Visible = true;
      }
      else
      {
        this.nPassageWalls.CollisionLayer = 0U;
        this.nStairsTrigger.CollisionLayer = 0U;
        this.nPassageStairs.Visible = false;
        this.nPassageFence.Visible = false;
        this.nLightStationAccess.Visible = false;
      }
      this.nEntranceL.Visible = this.Vars.GetFlag("ch1.forest_entrance_south_opened_secret_1");
      this.nEntranceR.Visible = this.Vars.GetFlag("ch1.forest_entrance_south_opened_secret_2");
      if (!this.nStairsTrigger.OverlapsBody((Node) Game.Player.Node))
        return;
      this.ActivateStairs((Node) Game.Player.Node);
    }

    public void ActivateStairs(Node _)
    {
      this.nTunnelWalls.CollisionLayer = 10U;
      this.nPassageEvent.Enabled = true;
      this.nHidePlayerMask.Visible = true;
      this.nStairs.Enabled = true;
    }

    public void DeactivateStairs(Node _)
    {
      if (this.nStairs.OverlapsBody((Node) Game.Player.Node))
        return;
      this.nTunnelWalls.CollisionLayer = 0U;
      this.nPassageEvent.Enabled = false;
      this.nHidePlayerMask.Visible = false;
      this.nStairs.Enabled = false;
    }

    public void RotateBlue1() => this._signToRotate = "ch1.forest_entrance_sign_blue_1";

    public void RotateBlue2() => this._signToRotate = "ch1.forest_entrance_sign_blue_2";

    public void RotateBlue3() => this._signToRotate = "ch1.forest_entrance_sign_blue_3";

    public void RotateYellow1() => this._signToRotate = "ch1.forest_entrance_sign_yellow_1";

    public void RotateYellow2() => this._signToRotate = "ch1.forest_entrance_sign_yellow_2";

    public void RotateYellow3() => this._signToRotate = "ch1.forest_entrance_sign_yellow_3";

    public void RotateRed1() => this._signToRotate = "ch1.forest_entrance_sign_red_1";

    public void RotateRed2() => this._signToRotate = "ch1.forest_entrance_sign_red_2";

    public void RotateLeft() => this.RotateSignVariable(this._signToRotate, Direction.Left);

    public void RotateUp() => this.RotateSignVariable(this._signToRotate, Direction.Up);

    public void RotateRight() => this.RotateSignVariable(this._signToRotate, Direction.Right);

    public void RotateDown() => this.RotateSignVariable(this._signToRotate, Direction.Down);

    private async void TriggerEventTryReset()
    {
      Ch1ForestPath2 baseNode = this;
      baseNode.varTryResetPuzzle.NewValue = (PVar.PVarSetProxy) true;
      await baseNode.DelaySeconds(10.0);
      baseNode.varTryResetPuzzle.NewValue = (PVar.PVarSetProxy) false;
    }

    private void RotateSignVariable(string variable, Direction direction)
    {
      this.Vars.SetVariable(variable, direction.ToString());
      this.UpdateSignSolutions();
    }

    private void UpdateSignDirection(Sprite sign, string variable)
    {
      string variable1 = this.Vars.GetVariable(variable);
      Sprite sprite = sign;
      int num;
      switch (variable1)
      {
        case "left":
          num = 0;
          break;
        case "up":
          num = 1;
          break;
        case "right":
          num = 2;
          break;
        case "down":
          num = 3;
          break;
        default:
          throw new InvalidOperationException();
      }
      sprite.Frame = num;
    }

    private void UpdateSignSolutions()
    {
      this.Vars.SetFlag("ch1.forest_entrance_south_opened_exit", this.Vars.GetVariable("ch1.forest_entrance_sign_blue_1") == "up" && this.Vars.GetVariable("ch1.forest_entrance_sign_blue_2") == "down" && this.Vars.GetVariable("ch1.forest_entrance_sign_blue_3") == "up");
      this.Vars.SetFlag("ch1.forest_entrance_south_opened_station", this.Vars.GetVariable("ch1.forest_entrance_sign_yellow_1") == "up" && this.Vars.GetVariable("ch1.forest_entrance_sign_yellow_2") == "right" && this.Vars.GetVariable("ch1.forest_entrance_sign_yellow_3") == "right");
      this.Vars.SetFlag("ch1.forest_entrance_south_opened_secret_1", this.Vars.GetVariable("ch1.forest_entrance_sign_red_1") == "left" && this.Vars.GetVariable("ch1.forest_entrance_sign_red_2") == "down");
      this.Vars.SetFlag("ch1.forest_entrance_south_opened_secret_2", this.Vars.GetVariable("ch1.forest_entrance_sign_red_1") == "up" && this.Vars.GetVariable("ch1.forest_entrance_sign_red_2") == "right");
    }
  }
}
