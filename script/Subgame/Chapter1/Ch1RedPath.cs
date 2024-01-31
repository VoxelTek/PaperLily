// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1RedPath
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1RedPath : GameRoom
  {
    [LacieEngine.API.GetNode("Main/Gate1")]
    private Node2D nGate1;
    [LacieEngine.API.GetNode("Main/Gate2")]
    private Node2D nGate2;
    [LacieEngine.API.GetNode("Main/Gate3")]
    private Node2D nGate3;
    [LacieEngine.API.GetNode("Other/Events/misc_gate_1")]
    private EventTrigger nGate1Evt;
    [LacieEngine.API.GetNode("Other/Events/misc_gate_2")]
    private EventTrigger nGate2Evt;
    [LacieEngine.API.GetNode("Other/Events/misc_gate_3")]
    private EventTrigger nGate3Evt;
    private PVar varGate1Open = (PVar) "ch1.forest_red_passage_gate_1_open";
    private PVar varGate2Open = (PVar) "ch1.forest_red_passage_gate_2_open";
    private PVar varGate3Open = (PVar) "ch1.forest_red_passage_gate_3_open";

    public override void _UpdateRoom()
    {
      this.nGate1.Visible = this.nGate1Evt.Enabled = !(bool) this.varGate1Open.Value;
      this.nGate2.Visible = this.nGate2Evt.Enabled = !(bool) this.varGate2Open.Value;
      this.nGate3.Visible = this.nGate3Evt.Enabled = !(bool) this.varGate3Open.Value;
    }
  }
}
