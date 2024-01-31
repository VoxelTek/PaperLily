// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityB2fHallwayEast
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
  public class Ch1FacilityB2fHallwayEast : Ch1FacilityRoom
  {
    [LacieEngine.API.GetNode("Background/WallWindchimes/MiscVentman")]
    private Node2D nVentMan;
    [LacieEngine.API.GetNode("Other/Navigation")]
    private Navigation2D nNavigation;
    private PVar varSaiHide = (PVar) "ch1.facility_sai_hidden";
    private PVar varSeenRune = (PVar) "ch1.facility_seen_event_rune";
    private PVar varSeenVentman = (PVar) "ch1.facility_seen_ventman";
    private const int PRIMAL_SPAWNS = 5;

    public override void _BeforeFadeIn()
    {
      if ((bool) this.varSaiHide.Value && Game.Room.RegisteredNPCs.ContainsKey("sai") && Game.Room.RegisteredNPCs["sai"] is NPCFollower registeredNpC)
        registeredNpC.DeleteIfValid();
      if ((bool) this.varSeenRune.Value && !(bool) this.varSeenVentman.Value)
        this.nVentMan.Visible = true;
      this.TrySpawnPrimal(this.nNavigation, 5);
    }
  }
}
