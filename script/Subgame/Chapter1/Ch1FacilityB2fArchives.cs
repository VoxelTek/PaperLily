// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityB2fArchives
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1FacilityB2fArchives : Ch1FacilityRoom
  {
    [LacieEngine.API.GetNode("Background/PreArrange")]
    private Node2D nPreArrangeLayout;
    [LacieEngine.API.GetNode("Background/PostArrange")]
    private Node2D nPostArrangeLayout;
    [LacieEngine.API.GetNode("Background/Vent")]
    private Sprite nVent;
    [LacieEngine.API.GetNode("Background/Cover")]
    private Node2D nFloorVentCover;
    [LacieEngine.API.GetNode("Background/shiny")]
    private Node2D nShiny;
    private PVar varSolved = (PVar) "ch1.facility_boxpuzzle_solved";
    private PVar varOpenedVent = (PVar) "ch1.facility_archive_vent_open";
    private PVar varTookValve = (PVar) "ch1.facility_took_valve_star";

    public override void _BeforeFadeIn()
    {
      this.nPreArrangeLayout.Visible = true;
      this.nPostArrangeLayout.Visible = false;
      if (!(bool) this.varSolved.Value)
        return;
      this.nPreArrangeLayout.Visible = false;
      this.nPostArrangeLayout.Visible = true;
      foreach (Node child in this.nPostArrangeLayout.GetChildren())
        child.Reparent((Node) this.GetMainLayer());
    }

    public override void _UpdateRoom()
    {
      base._UpdateRoom();
      this.nVent.Frame = (bool) this.varOpenedVent.Value ? 1 : 0;
      this.nFloorVentCover.Visible = (bool) this.varOpenedVent.Value;
      this.nShiny.Visible = !(bool) this.varTookValve.Value;
    }
  }
}
