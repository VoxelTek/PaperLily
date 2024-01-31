// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1FacilityB2fDungeon1
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Rooms;
using LacieEngine.Subgame.PaperLily;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1FacilityB2fDungeon1 : Ch1FacilityRoom
  {
    [LacieEngine.API.GetNode("Background/WallDoor1")]
    private SimpleAnimatedSprite nDoor1;
    [LacieEngine.API.GetNode("Background/WallDoor2")]
    private SimpleAnimatedSprite nDoor2;
    [LacieEngine.API.GetNode("Background/WallDoor3")]
    private SimpleAnimatedSprite nDoor3;
    [LacieEngine.API.GetNode("Background/WallDoor4")]
    private SimpleAnimatedSprite nDoor4;
    [LacieEngine.API.GetNode("Background/WallDoor5")]
    private SimpleAnimatedSprite nDoor5;
    [LacieEngine.API.GetNode("Background/WallDoor6")]
    private SimpleAnimatedSprite nDoor6;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_1")]
    private EventTrigger nInteractDoor1;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_2")]
    private EventTrigger nInteractDoor2;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_3")]
    private EventTrigger nInteractDoor3;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_4")]
    private EventTrigger nInteractDoor4;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_5")]
    private EventTrigger nInteractDoor5;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_6")]
    private EventTrigger nInteractDoor6;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_1_close")]
    private EventTrigger nInteractDoor1Close;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_2_close")]
    private EventTrigger nInteractDoor2Close;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_3_close")]
    private EventTrigger nInteractDoor3Close;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_4_close")]
    private EventTrigger nInteractDoor4Close;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_5_close")]
    private EventTrigger nInteractDoor5Close;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_6_close")]
    private EventTrigger nInteractDoor6Close;
    [LacieEngine.API.GetNode("Other/Events/move_cell_1")]
    private EventTrigger nMoveDoor1;
    [LacieEngine.API.GetNode("Other/Events/move_cell_2")]
    private EventTrigger nMoveDoor2;
    [LacieEngine.API.GetNode("Other/Events/move_cell_3")]
    private EventTrigger nMoveDoor3;
    [LacieEngine.API.GetNode("Other/Events/move_cell_4")]
    private EventTrigger nMoveDoor4;
    [LacieEngine.API.GetNode("Other/Events/move_cell_5")]
    private EventTrigger nMoveDoor5;
    [LacieEngine.API.GetNode("Other/Events/move_cell_6")]
    private EventTrigger nMoveDoor6;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_2_interior")]
    private EventTrigger nEscapeCellInteriorEvent;
    [LacieEngine.API.GetNode("Other/Events/move_exit_cell_2")]
    private EventTrigger nEscapeCellTouchEvent;
    [LacieEngine.API.GetNode("Main/misc_dungeon_desk/ch1_lacie_items_shiny")]
    private Node2D nLacieItemsShiny;
    [LacieEngine.API.GetNode("Background/CutsceneLacieRise")]
    private SimpleAnimatedSpriteV2 nCutsceneLacieRise;
    [LacieEngine.API.GetNode("Main/PushBox")]
    private Node2D nPushBox;
    [LacieEngine.API.GetNode("Background/ClimbBox")]
    private Node2D nClimbBox;
    [LacieEngine.API.GetNode("Other/Events/move_climb")]
    private EventTrigger nClimbBoxEvt;
    [LacieEngine.API.GetNode("Lighting/Cell2")]
    private Node2D nLightsCell2Open;
    [LacieEngine.API.GetNode("Lighting/Cell2Closed")]
    private Node2D nLightsCell2Closed;
    [LacieEngine.API.GetNode("Background/VentCover")]
    private Node2D nVentCover;
    private PVar varDoor1Open = (PVar) "ch1.facility_dungeon_1_door_1_open";
    private PVar varDoor2Open = (PVar) "ch1.facility_dungeon_1_door_2_open";
    private PVar varDoor3Open = (PVar) "ch1.facility_dungeon_1_door_3_open";
    private PVar varDoor4Open = (PVar) "ch1.facility_dungeon_1_door_4_open";
    private PVar varDoor5Open = (PVar) "ch1.facility_dungeon_1_door_5_open";
    private PVar varDoor6Open = (PVar) "ch1.facility_dungeon_1_door_6_open";
    private PVar varTookBackItems = (PVar) "ch1.facility_took_back_items";
    private PVar varBoxLocation = (PVar) "ch1.facility_dungeon_box_pos";
    private PVar varPlacedBox = (PVar) "ch1.facility_dungeon_placed_box";
    private PVar varTookVentCover = (PVar) "ch1.facility_dungeon_1_took_vent_cover";
    private PVar varItemStorage = (PVar) "ch1.temp_blackpass_item_storage";
    private PVar varTalkedToSai = (PVar) "ch1.facility_seen_cutscene_todolist";
    private PVar varSeenVentMan = (PVar) "ch1.facility_seen_ventman";

    public override void _BeforeFadeIn()
    {
      if ((bool) this.varPlacedBox.Value)
        this.nPushBox.DeleteIfValid();
      if ((bool) this.varTalkedToSai.Value)
      {
        this.varDoor5Open.NewValue = (PVar.PVarSetProxy) false;
      }
      else
      {
        if (!(bool) this.varSeenVentMan.Value)
          return;
        this.varDoor1Open.NewValue = (PVar.PVarSetProxy) false;
      }
    }

    public override void _UpdateRoom()
    {
      this.SetDoorState((Sprite) this.nDoor1, this.nInteractDoor1, this.nInteractDoor1Close, this.nMoveDoor1, (bool) this.varDoor1Open.Value);
      this.SetDoorState((Sprite) this.nDoor2, this.nInteractDoor2, this.nInteractDoor2Close, this.nMoveDoor2, (bool) this.varDoor2Open.Value);
      this.SetDoorState((Sprite) this.nDoor3, this.nInteractDoor3, this.nInteractDoor3Close, this.nMoveDoor3, (bool) this.varDoor3Open.Value);
      this.SetDoorState((Sprite) this.nDoor4, this.nInteractDoor4, this.nInteractDoor4Close, this.nMoveDoor4, (bool) this.varDoor4Open.Value);
      this.SetDoorState((Sprite) this.nDoor5, this.nInteractDoor5, this.nInteractDoor5Close, this.nMoveDoor5, (bool) this.varDoor5Open.Value);
      this.SetDoorState((Sprite) this.nDoor6, this.nInteractDoor6, this.nInteractDoor6Close, this.nMoveDoor6, (bool) this.varDoor6Open.Value);
      base._UpdateRoom();
      this.nLacieItemsShiny.Visible = !(bool) this.varTookBackItems.Value;
      this.nEscapeCellInteriorEvent.Enabled = !(bool) this.varDoor2Open.Value;
      this.nEscapeCellTouchEvent.Enabled = (bool) this.varDoor2Open.Value;
      this.nClimbBox.Visible = this.nClimbBoxEvt.Enabled = (bool) this.varPlacedBox.Value;
      this.nLightsCell2Open.Visible = (bool) this.varDoor2Open.Value;
      this.nLightsCell2Closed.Visible = !this.nLightsCell2Open.Visible;
      this.nVentCover.Visible = !(bool) this.varTookVentCover.Value;
    }

    private void SetDoorState(
      Sprite door,
      EventTrigger interactEvent,
      EventTrigger closeEvent,
      EventTrigger moveEvent,
      bool isOpen)
    {
      door.Frame = isOpen ? 3 : 0;
      interactEvent.Enabled = !isOpen;
      closeEvent.Enabled = isOpen;
      moveEvent.Enabled = isOpen;
    }

    public void OpenDoor1() => this.AnimateDoorOpening(this.nDoor1);

    public void OpenDoor2() => this.AnimateDoorOpening(this.nDoor2);

    public void OpenDoor3() => this.AnimateDoorOpening(this.nDoor3);

    public void OpenDoor4() => this.AnimateDoorOpening(this.nDoor4);

    public void OpenDoor5() => this.AnimateDoorOpening(this.nDoor5);

    public void OpenDoor6() => this.AnimateDoorOpening(this.nDoor6);

    public void CloseDoor1() => this.AnimateDoorClosing(this.nDoor1);

    public void CloseDoor2() => this.AnimateDoorClosing(this.nDoor2);

    public void CloseDoor3() => this.AnimateDoorClosing(this.nDoor3);

    public void CloseDoor4() => this.AnimateDoorClosing(this.nDoor4);

    public void CloseDoor5() => this.AnimateDoorClosing(this.nDoor5);

    public void CloseDoor6() => this.AnimateDoorClosing(this.nDoor6);

    public void MoveBoxIntoDoor1() => this.MoveBoxIntoDoor(1);

    public void MoveBoxIntoDoor2() => this.MoveBoxIntoDoor(2);

    public void MoveBoxIntoDoor3() => this.MoveBoxIntoDoor(3);

    public void MoveBoxIntoDoor4() => this.MoveBoxIntoDoor(4);

    public void MoveBoxIntoDoor5() => this.MoveBoxIntoDoor(5);

    public void MoveBoxIntoDoor6() => this.MoveBoxIntoDoor(6);

    public void MoveBoxOutOfDoor1() => this.MoveBoxOutOfDoor(1);

    public void MoveBoxOutOfDoor2() => this.MoveBoxOutOfDoor(2);

    public void MoveBoxOutOfDoor3() => this.MoveBoxOutOfDoor(3);

    public void MoveBoxOutOfDoor4() => this.MoveBoxOutOfDoor(4);

    public void MoveBoxOutOfDoor5() => this.MoveBoxOutOfDoor(5);

    public void MoveBoxOutOfDoor6() => this.MoveBoxOutOfDoor(6);

    public void LacieRise()
    {
      this.nCutsceneLacieRise.Visible = true;
      this.nCutsceneLacieRise.Playing = true;
    }

    public void LacieRiseHide() => this.nCutsceneLacieRise.Visible = false;

    private void AnimateDoorOpening(SimpleAnimatedSprite door)
    {
      door.Playing = false;
      door.AnimationFrames = "";
      door.Playing = true;
    }

    private void AnimateDoorClosing(SimpleAnimatedSprite door)
    {
      door.Playing = false;
      door.AnimationFrames = "3,2,1,0";
      door.Playing = true;
    }

    private void MoveBoxIntoDoor(int door)
    {
      this.varBoxLocation.NewValue = (PVar.PVarSetProxy) this.GetPoint("cell_box_" + door.ToString());
    }

    private void MoveBoxOutOfDoor(int door)
    {
      this.varBoxLocation.NewValue = (PVar.PVarSetProxy) this.GetPoint("box_from_" + door.ToString());
    }

    public void RestoreInventory() => PaperLilyFunctions.RestoreInventory(this.varItemStorage);
  }
}
