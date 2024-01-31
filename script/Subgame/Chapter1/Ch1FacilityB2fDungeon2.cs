// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityB2fDungeon2
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1FacilityB2fDungeon2 : Ch1FacilityRoom
  {
    [LacieEngine.API.GetNode("Background/Door1")]
    private Sprite nDoor1;
    [LacieEngine.API.GetNode("Background/Door2")]
    private Sprite nDoor2;
    [LacieEngine.API.GetNode("Background/Door3")]
    private Sprite nDoor3;
    [LacieEngine.API.GetNode("Background/Door4")]
    private Sprite nDoor4;
    [LacieEngine.API.GetNode("Background/Door5")]
    private Sprite nDoor5;
    [LacieEngine.API.GetNode("Background/Door6")]
    private Sprite nDoor6;
    [LacieEngine.API.GetNode("Background/Door3/Eyes")]
    private Node2D nEyes1;
    [LacieEngine.API.GetNode("Background/Door6/Eyes")]
    private Node2D nEyes2;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_2")]
    private EventTrigger nDoor2ClosedEvt;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_3")]
    private EventTrigger nDoor3ClosedEvt;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_4")]
    private EventTrigger nDoor4ClosedEvt;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_5")]
    private EventTrigger nDoor5ClosedEvt;
    [LacieEngine.API.GetNode("Other/Events/misc_cell_6")]
    private EventTrigger nDoor6ClosedEvt;
    [LacieEngine.API.GetNode("Other/Events/move_cell_2")]
    private EventTrigger nDoor2OpenEvt;
    [LacieEngine.API.GetNode("Other/Events/move_cell_3")]
    private EventTrigger nDoor3OpenEvt;
    [LacieEngine.API.GetNode("Other/Events/move_cell_4")]
    private EventTrigger nDoor4OpenEvt;
    [LacieEngine.API.GetNode("Other/Events/move_cell_5")]
    private EventTrigger nDoor5OpenEvt;
    [LacieEngine.API.GetNode("Other/Events/move_cell_6")]
    private EventTrigger nDoor6OpenEvt;
    private PVar varDoorsOpen = (PVar) "ch1.facility_dungeon_2_doors_open";
    private PVar varPlacedPlank = (PVar) "ch1.facility_dungeon_2_placed_plank";
    private PVar varReleasedPrimals = (PVar) "ch1.facility_dungeon_2_released";

    public override void _UpdateRoom()
    {
      base._UpdateRoom();
      bool flag = (bool) this.varDoorsOpen.Value;
      this.nDoor1.Frame = flag ? 1 : 0;
      if ((bool) this.varPlacedPlank.Value)
        this.nDoor1.Frame = 1;
      this.nDoor2.Frame = flag ? 1 : 0;
      this.nDoor3.Frame = flag ? 1 : 0;
      this.nDoor4.Frame = flag ? 1 : 0;
      this.nDoor5.Frame = flag ? 1 : 0;
      this.nDoor6.Frame = flag ? 1 : 0;
      this.nDoor2OpenEvt.Enabled = flag;
      this.nDoor3OpenEvt.Enabled = flag;
      this.nDoor4OpenEvt.Enabled = flag;
      this.nDoor5OpenEvt.Enabled = flag;
      this.nDoor6OpenEvt.Enabled = flag;
      this.nDoor2ClosedEvt.Enabled = !flag;
      this.nDoor3ClosedEvt.Enabled = !flag;
      this.nDoor4ClosedEvt.Enabled = !flag;
      this.nDoor5ClosedEvt.Enabled = !flag;
      this.nDoor6ClosedEvt.Enabled = !flag;
      this.nEyes1.Visible = !flag && !(bool) this.varReleasedPrimals.Value;
      this.nEyes2.Visible = !flag && !(bool) this.varReleasedPrimals.Value;
    }
  }
}
