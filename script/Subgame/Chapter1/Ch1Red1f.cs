// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1Red1f
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1Red1f : GameRoom
  {
    [LacieEngine.API.GetNode("Background/FloorHatch")]
    private Node2D nHatch;
    [LacieEngine.API.GetNode("Background/FloorHatch/GrateClosed")]
    private Node2D nGrateClosed;
    [LacieEngine.API.GetNode("Background/FloorHatch/GrateOpen")]
    private Node2D nGrateOpen;
    [LacieEngine.API.GetNode("Background/FurnitureNotapizzaoven/Thing")]
    private Node2D nThing;
    [LacieEngine.API.GetNode("Background/MiscNotes")]
    private Node2D nNotes;
    [LacieEngine.API.GetNode("Background/CarpetMoved")]
    private Node2D nCarpetMoved;
    [LacieEngine.API.GetNode("Other/Events/move_downstairs")]
    private EventTrigger nLadderEvt;
    private PVar varTookThing = (PVar) "ch1.forest_red_took_pot";
    private PVar varHatchExposed = (PVar) "ch1.forest_red_hatch_exposed";
    private PVar varHatchOpen = (PVar) "ch1.forest_red_hatch_open";
    private PVar varTookNotes = (PVar) "ch1.forest_red_took_notes";

    public override void _UpdateRoom()
    {
      this.nHatch.Visible = (bool) this.varHatchExposed.Value;
      this.nCarpetMoved.Visible = (bool) this.varHatchExposed.Value;
      this.nThing.Visible = !(bool) this.varTookThing.Value;
      this.nNotes.Visible = !(bool) this.varTookNotes.Value;
      this.nGrateOpen.Visible = (bool) this.varHatchOpen.Value;
      this.nGrateClosed.Visible = !this.nGrateOpen.Visible;
      this.nLadderEvt.Enabled = (bool) this.varHatchOpen.Value;
    }
  }
}
