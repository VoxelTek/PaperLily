// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityB1fElevator
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Subgame.Chapter1;
using System;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1FacilityB1fElevator : Ch1FacilityRoom
  {
    [LacieEngine.API.GetNode("Background/Elevator")]
    private SimpleAnimatedSprite nElevator;
    [LacieEngine.API.GetNode("Other/Events/misc_elevator_door")]
    private EventTrigger nElevatorClosed;
    [LacieEngine.API.GetNode("Other/Events/move_elevator")]
    private EventTrigger nElevatorOpen;
    [LacieEngine.API.GetNode("Other/Navigation")]
    private Navigation2D nNavigation;
    private PVar varElevatorLocation = (PVar) "ch1.facility_elevator_location";
    private const int PRIMAL_SPAWNS = 4;

    public override void _BeforeFadeIn()
    {
      this.varPrimalSpawn = (PVar) this.primalSpawnVar;
      this.varPrimalType = (PVar) this.primalTypeVar;
      if (!(Game.Player.Node.Position != this.GetPoint("from_elevator")))
        return;
      this.TrySpawnPrimal(this.nNavigation, 4);
    }

    public override void _UpdateRoom()
    {
      base._UpdateRoom();
      bool flag = this.varElevatorLocation.Value == 1;
      this.nElevator.Frame = flag ? 5 : 0;
      this.nElevatorClosed.Enabled = !flag;
      this.nElevatorOpen.Enabled = flag;
    }

    public void SpawnTutorialEnemy()
    {
      Ch1FacilityPrimal ch1FacilityPrimal = new Ch1FacilityPrimal();
      this.varPrimalSpawn.NewValue = (PVar.PVarSetProxy) "tutorial_primal_spawn";
      this.varPrimalType.NewValue = (PVar.PVarSetProxy) 1;
      SpawnPoint spawnPoint = this.GetSpawnPoint((string) this.varPrimalSpawn.Value);
      ch1FacilityPrimal.Position = spawnPoint.Position;
      ch1FacilityPrimal.DefaultDirection = (Direction) spawnPoint.Direction;
      this.GetMainLayer().AddChild((Node) ch1FacilityPrimal);
      ch1FacilityPrimal.SetNavigation(this.nNavigation);
      ch1FacilityPrimal.Caught += new Action(((Ch1FacilityRoom) this).PlayerDeath);
    }

    public void OpenElevatorDoors() => this.nElevator.Playing = true;
  }
}
