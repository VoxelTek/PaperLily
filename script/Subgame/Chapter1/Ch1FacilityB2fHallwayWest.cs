// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityB2fHallwayWest
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1FacilityB2fHallwayWest : Ch1FacilityRoom
  {
    [LacieEngine.API.GetNode("Other/Passthrough")]
    private Passthrough nChartPassthough;
    [LacieEngine.API.GetNode("Background/DoorOpen")]
    private Node2D nChartDoor;
    [LacieEngine.API.GetNode("Background/DoorOpen/Plank")]
    private Node2D nChartPlank;
    [LacieEngine.API.GetNode("Background/Elevator")]
    private SimpleAnimatedSprite nElevator;
    [LacieEngine.API.GetNode("Other/Events/misc_elevator_door")]
    private EventTrigger nElevatorClosed;
    [LacieEngine.API.GetNode("Other/Events/move_elevator")]
    private EventTrigger nElevatorOpen;
    [LacieEngine.API.GetNode("Other/Navigation")]
    private Navigation2D nNavigation;
    private PVar varPlacedPlank = (PVar) "ch1.facility_chartroom_placed_plank";
    private PVar varChartRoomOpen = (PVar) "ch1.facility_chartroom_seen_exit";
    private PVar varElevatorLocation = (PVar) "ch1.facility_elevator_location";
    private const int PRIMAL_SPAWNS = 5;

    public override void _BeforeFadeIn() => this.TrySpawnPrimal(this.nNavigation, 5);

    public override void _UpdateRoom()
    {
      base._UpdateRoom();
      this.nChartPassthough.Enabled = (bool) this.varPlacedPlank.Value;
      this.nChartDoor.Visible = (bool) this.varChartRoomOpen.Value;
      this.nChartPlank.Visible = (bool) this.varPlacedPlank.Value;
      bool flag = this.varElevatorLocation.Value == 2;
      this.nElevator.Frame = flag ? 5 : 0;
      this.nElevatorClosed.Enabled = !flag;
      this.nElevatorOpen.Enabled = flag;
    }

    public void OpenElevatorDoors() => this.nElevator.Playing = true;
  }
}
