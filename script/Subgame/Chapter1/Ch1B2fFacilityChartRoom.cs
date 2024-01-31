// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1B2fFacilityChartRoom
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1B2fFacilityChartRoom : Ch1FacilityRoom
  {
    [LacieEngine.API.GetNode("Background/Plank")]
    private Node2D nPlank;
    [LacieEngine.API.GetNode("Main/Table/ShinyFuse")]
    private Node2D nFuseShiny;
    [LacieEngine.API.GetNode("Background/WallScreenOn")]
    private Node2D nScreen;
    private PVar varPlacedPlank = (PVar) "ch1.facility_chartroom_placed_plank";
    private PVar varTookFuse = (PVar) "ch1.facility_chart_took_fuse";
    private PVar varScreenOn = (PVar) "ch1.facility_chart_screen_on";

    public override void _UpdateRoom()
    {
      base._UpdateRoom();
      this.nPlank.Visible = !(bool) this.varPlacedPlank.Value;
      this.nFuseShiny.Visible = !(bool) this.varTookFuse.Value;
      this.nScreen.Visible = (bool) this.varScreenOn.Value;
    }
  }
}
