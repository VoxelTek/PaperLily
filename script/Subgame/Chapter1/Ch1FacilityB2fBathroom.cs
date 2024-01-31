// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityB2fBathroom
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1FacilityB2fBathroom : Ch1FacilityRoom
  {
    [LacieEngine.API.GetNode("Background/Stall1")]
    private Sprite nStall1;
    [LacieEngine.API.GetNode("Background/Stall2")]
    private Sprite nStall2;
    [LacieEngine.API.GetNode("Background/Stall3")]
    private Sprite nStall3;
    [LacieEngine.API.GetNode("Background/Stall4")]
    private Sprite nStall4;
    [LacieEngine.API.GetNode("Background/Stall5")]
    private Sprite nStall5;
    [LacieEngine.API.GetNode("Background/Stall6")]
    private Sprite nStall6;
    [LacieEngine.API.GetNode("Background/Stall7")]
    private Sprite nStall7;
    [LacieEngine.API.GetNode("Background/Stall8")]
    private Sprite nStall8;
    [LacieEngine.API.GetNode("Other/Events/misc_stall_1")]
    private EventTrigger nStall1Evt;
    [LacieEngine.API.GetNode("Other/Events/misc_stall_2")]
    private EventTrigger nStall2Evt;
    [LacieEngine.API.GetNode("Other/Events/misc_stall_3")]
    private EventTrigger nStall3Evt;
    [LacieEngine.API.GetNode("Other/Events/misc_stall_4")]
    private EventTrigger nStall4Evt;
    [LacieEngine.API.GetNode("Other/Events/misc_stall_5")]
    private EventTrigger nStall5Evt;
    [LacieEngine.API.GetNode("Other/Events/misc_stall_6")]
    private EventTrigger nStall6Evt;
    [LacieEngine.API.GetNode("Other/Events/misc_stall_7")]
    private EventTrigger nStall7Evt;
    [LacieEngine.API.GetNode("Other/Events/misc_stall_8")]
    private EventTrigger nStall8Evt;
    private PVar varStall1Open = (PVar) "ch1.facility_bathroom_stall_1_open";
    private PVar varStall2Open = (PVar) "ch1.facility_bathroom_stall_2_open";
    private PVar varStall3Open = (PVar) "ch1.facility_bathroom_stall_3_open";
    private PVar varStall4Open = (PVar) "ch1.facility_bathroom_stall_4_open";
    private PVar varStall5Open = (PVar) "ch1.facility_bathroom_stall_5_open";
    private PVar varStall6Open = (PVar) "ch1.facility_bathroom_stall_6_open";
    private PVar varStall7Open = (PVar) "ch1.facility_bathroom_stall_7_open";
    private PVar varStall8Open = (PVar) "ch1.facility_bathroom_stall_8_open";

    public override void _UpdateRoom()
    {
      base._UpdateRoom();
      this.nStall1.Frame = (bool) this.varStall1Open.Value ? 1 : 0;
      this.nStall2.Frame = (bool) this.varStall2Open.Value ? 1 : 0;
      this.nStall3.Frame = (bool) this.varStall3Open.Value ? 1 : 0;
      this.nStall4.Frame = (bool) this.varStall4Open.Value ? 1 : 0;
      this.nStall5.Frame = (bool) this.varStall5Open.Value ? 1 : 0;
      this.nStall6.Frame = (bool) this.varStall6Open.Value ? 1 : 0;
      this.nStall7.Frame = (bool) this.varStall7Open.Value ? 1 : 0;
      this.nStall8.Frame = (bool) this.varStall8Open.Value ? 1 : 0;
      this.nStall1Evt.Enabled = !(bool) this.varStall1Open.Value;
      this.nStall2Evt.Enabled = !(bool) this.varStall2Open.Value;
      this.nStall3Evt.Enabled = !(bool) this.varStall3Open.Value;
      this.nStall4Evt.Enabled = !(bool) this.varStall4Open.Value;
      this.nStall5Evt.Enabled = !(bool) this.varStall5Open.Value;
      this.nStall6Evt.Enabled = !(bool) this.varStall6Open.Value;
      this.nStall7Evt.Enabled = !(bool) this.varStall7Open.Value;
      this.nStall8Evt.Enabled = !(bool) this.varStall8Open.Value;
    }
  }
}
