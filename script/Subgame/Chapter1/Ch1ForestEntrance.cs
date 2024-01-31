// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1ForestEntrance
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1ForestEntrance : GameRoom
  {
    [LacieEngine.API.GetNode("Foreground/lamp_light_1")]
    private Node2D nLight1;
    [LacieEngine.API.GetNode("Foreground/lamp_light_2")]
    private Node2D nLight2;
    [LacieEngine.API.GetNode("Foreground/lamp_light_3")]
    private Node2D nLight3;
    [LacieEngine.API.GetNode("Foreground/lamp_light_4")]
    private Node2D nLight4;
    [LacieEngine.API.GetNode("Foreground/lamp_light_5")]
    private Node2D nLight5;
    [LacieEngine.API.GetNode("Foreground/lamp_light_6")]
    private Node2D nLight6;
    [LacieEngine.API.GetNode("Ground/Cables/Cable1")]
    private Node2D nCable1;
    [LacieEngine.API.GetNode("Ground/Cables/Cable2")]
    private Node2D nCable2;
    [LacieEngine.API.GetNode("Ground/Cables/Cable3")]
    private Node2D nCable3;
    [LacieEngine.API.GetNode("Ground/Cables/Cable4")]
    private Node2D nCable4;
    [LacieEngine.API.GetNode("Ground/Cables/Cable5")]
    private Node2D nCable5;
    [LacieEngine.API.GetNode("Ground/Cables/Cable6")]
    private Node2D nCable6;
    [LacieEngine.API.GetNode("Ground/Cables/Cable1Cut")]
    private Node2D nCable1Cut;
    [LacieEngine.API.GetNode("Ground/Cables/Cable2Cut")]
    private Node2D nCable2Cut;
    [LacieEngine.API.GetNode("Ground/Cables/Cable3Cut")]
    private Node2D nCable3Cut;
    [LacieEngine.API.GetNode("Ground/Cables/Cable4Cut")]
    private Node2D nCable4Cut;
    [LacieEngine.API.GetNode("Ground/Cables/Cable5Cut")]
    private Node2D nCable5Cut;
    [LacieEngine.API.GetNode("Ground/Cables/Cable6Cut")]
    private Node2D nCable6Cut;
    [LacieEngine.API.GetNode("Ground/StructWall/Gate")]
    private Node2D nGate;
    [LacieEngine.API.GetNode("Other/Events/misc_door")]
    private EventTrigger nGateCollision;
    private PVar varGateOpen = (PVar) "ch1.forest_entrance_gate_open";
    private PVar varLight1SwitchOn = (PVar) "ch1.forest_entrance_light_1_switch_on";
    private PVar varLight2SwitchOn = (PVar) "ch1.forest_entrance_light_2_switch_on";
    private PVar varLight3SwitchOn = (PVar) "ch1.forest_entrance_light_3_switch_on";
    private PVar varLight4SwitchOn = (PVar) "ch1.forest_entrance_light_4_switch_on";
    private PVar varLight5SwitchOn = (PVar) "ch1.forest_entrance_light_5_switch_on";
    private PVar varLight6SwitchOn = (PVar) "ch1.forest_entrance_light_6_switch_on";
    private PVar varCable1Cut = (PVar) "ch1.forest_entrance_light_1_cable_cut";
    private PVar varCable2Cut = (PVar) "ch1.forest_entrance_light_2_cable_cut";
    private PVar varCable3Cut = (PVar) "ch1.forest_entrance_light_3_cable_cut";
    private PVar varCable4Cut = (PVar) "ch1.forest_entrance_light_4_cable_cut";
    private PVar varCable5Cut = (PVar) "ch1.forest_entrance_light_5_cable_cut";
    private PVar varCable6Cut = (PVar) "ch1.forest_entrance_light_6_cable_cut";
    private PVar varLight1Broken = (PVar) "ch1.forest_entrance_light_1_broken";
    private PVar varLight2Broken = (PVar) "ch1.forest_entrance_light_2_broken";
    private PVar varLight3Broken = (PVar) "ch1.forest_entrance_light_3_broken";
    private PVar varLight4Broken = (PVar) "ch1.forest_entrance_light_4_broken";
    private PVar varLight5Broken = (PVar) "ch1.forest_entrance_light_5_broken";
    private PVar varLight6Broken = (PVar) "ch1.forest_entrance_light_6_broken";
    private bool[] lightSwitchesTouched = new bool[6];

    public override void _UpdateRoom()
    {
      this.nGate.Visible = !(bool) this.varGateOpen.Value;
      this.nGateCollision.Enabled = !(bool) this.varGateOpen.Value;
      this.nCable1.Visible = !(bool) this.varCable1Cut.Value;
      this.nCable2.Visible = !(bool) this.varCable2Cut.Value;
      this.nCable3.Visible = !(bool) this.varCable3Cut.Value;
      this.nCable4.Visible = !(bool) this.varCable4Cut.Value;
      this.nCable5.Visible = !(bool) this.varCable5Cut.Value;
      this.nCable6.Visible = !(bool) this.varCable6Cut.Value;
      this.nCable1Cut.Visible = !this.nCable1.Visible;
      this.nCable2Cut.Visible = !this.nCable2.Visible;
      this.nCable3Cut.Visible = !this.nCable3.Visible;
      this.nCable4Cut.Visible = !this.nCable4.Visible;
      this.nCable5Cut.Visible = !this.nCable5.Visible;
      this.nCable6Cut.Visible = !this.nCable6.Visible;
      this.UpdateLights();
      this.UpdateLightSolutions();
    }

    public void LightSwitch1() => this.SwitchLights(0, 1, 2, 3);

    public void LightSwitch2() => this.SwitchLights(1, 2, 3, 4, 6);

    public void LightSwitch3() => this.SwitchLights(2, 3, 5, 6);

    public void LightSwitch4() => this.SwitchLights(3, 1, 3, 4, 6);

    public void LightSwitch5() => this.SwitchLights(4, 2, 4, 5, 6);

    public void LightSwitch6() => this.SwitchLights(5, 1, 2, 3, 4);

    public void CheckEvents()
    {
      if (Game.Events.SeenEvent(this.Name + ".event_figure_out"))
        return;
      if (this.CountInteractedLightSwitches() >= 3)
      {
        Game.Events.PlayEvent("event_figure_out");
      }
      else
      {
        if (this.CountInteractedLightSwitches() != 0 || Game.Events.SeenEvent(this.Name + ".event_noticed_switch") || Game.Events.GetEventInteractionCount(this.Name + ".misc_light") < 3)
          return;
        Game.Events.PlayEvent("event_noticed_switch");
      }
    }

    private void SwitchLights(int lightIdx, params int[] lights)
    {
      this.lightSwitchesTouched[lightIdx] = true;
      foreach (int light in lights)
        Game.Variables.ToggleFlag("ch1.forest_entrance_light_" + light.ToString() + "_switch_on");
      this.UpdateLights();
      this.UpdateLightSolutions();
    }

    private void UpdateLights()
    {
      this.nLight1.Visible = (bool) this.varLight1SwitchOn.Value && !(bool) this.varCable1Cut.Value && !(bool) this.varLight1Broken.Value;
      this.nLight2.Visible = (bool) this.varLight2SwitchOn.Value && !(bool) this.varCable2Cut.Value && !(bool) this.varLight2Broken.Value;
      this.nLight3.Visible = (bool) this.varLight3SwitchOn.Value && !(bool) this.varCable3Cut.Value && !(bool) this.varLight3Broken.Value;
      this.nLight4.Visible = (bool) this.varLight4SwitchOn.Value && !(bool) this.varCable4Cut.Value && !(bool) this.varLight4Broken.Value;
      this.nLight5.Visible = (bool) this.varLight5SwitchOn.Value && !(bool) this.varCable5Cut.Value && !(bool) this.varLight5Broken.Value;
      this.nLight6.Visible = (bool) this.varLight6SwitchOn.Value && !(bool) this.varCable6Cut.Value && !(bool) this.varLight6Broken.Value;
    }

    private void UpdateLightSolutions()
    {
      if (Game.Events.SeenEvent(this.Name + ".event_just_opened") || this.nLight1.Visible || this.nLight2.Visible || this.nLight3.Visible || this.nLight4.Visible || this.nLight5.Visible || this.nLight6.Visible)
        return;
      Game.Events.PlayEvent("event_just_opened");
    }

    private int CountInteractedLightSwitches()
    {
      return ((IEnumerable<bool>) this.lightSwitchesTouched).Count<bool>((Func<bool, bool>) (x => x));
    }
  }
}
