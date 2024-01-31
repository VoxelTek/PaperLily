// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityB3fHallway
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1FacilityB3fHallway : Ch1FacilityRoom
  {
    [LacieEngine.API.GetNode("Main/DoorAirtight")]
    private Node2D nAirtight;
    [LacieEngine.API.GetNode("TilesWater")]
    private Node2D nWater;
    [LacieEngine.API.GetNode("Other/Events/misc_airtight_door")]
    private EventTrigger nAirtightEvt;
    [LacieEngine.API.GetNode("Background/Elevator")]
    private SimpleAnimatedSprite nElevator;
    [LacieEngine.API.GetNode("Other/Events/misc_elevator_door")]
    private EventTrigger nElevatorClosed;
    [LacieEngine.API.GetNode("Other/Events/move_elevator")]
    private EventTrigger nElevatorOpen;
    private PVar varFloodClear = (PVar) "ch1.facility_flood_engaged";
    private PVar varElevatorLocation = (PVar) "ch1.facility_elevator_location";

    public override void _UpdateRoom()
    {
      base._UpdateRoom();
      this.nAirtight.Visible = !(bool) this.varFloodClear.Value;
      this.nAirtightEvt.Enabled = !(bool) this.varFloodClear.Value;
      this.nWater.Visible = !(bool) this.varFloodClear.Value;
      bool flag = this.varElevatorLocation.Value == 3;
      this.nElevator.Frame = flag ? 5 : 0;
      this.nElevatorClosed.Enabled = !flag;
      this.nElevatorOpen.Enabled = flag;
    }

    public void OpenElevatorDoors() => this.nElevator.Playing = true;
  }
}
