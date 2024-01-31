// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1HomeExteriorBack
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1HomeExteriorBack : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private Lighting lightDay;
    [Export(PropertyHint.None, "")]
    private Lighting lightEvening;
    [Export(PropertyHint.None, "")]
    private Lighting lightNight;
    [LacieEngine.API.GetNode("Foreground/livingroom_door")]
    private Node2D nLivingroomDoor;
    [LacieEngine.API.GetNode("Foreground/hallway_window")]
    private Node2D nHallwayWindow;
    [LacieEngine.API.GetNode("Foreground/hallway_window_2")]
    private Node2D nHallwayWindow2;
    [LacieEngine.API.GetNode("Foreground/lamp_1")]
    private Node2D nLamp1;
    [LacieEngine.API.GetNode("Foreground/lamp_2")]
    private Node2D nLamp2;
    [LacieEngine.API.GetNode("Foreground/lamp_3")]
    private Node2D lamp_3;
    [LacieEngine.API.GetNode("Foreground/lamp_4")]
    private Node2D nLamp4;
    [LacieEngine.API.GetNode("Background/furniture_bench/chr_hiro")]
    private NpcStaticTurnableVer2 nHiroBench;
    [LacieEngine.API.GetNode("Other/Events/chr_hiro")]
    private EventTrigger nHiroEvent;
    private PVar varHiroAllTalks = (PVar) "ch1.home_hiro_alltalks";

    public override void _UpdateRoom()
    {
      Game.Room.ResetLighting();
      this.nLivingroomDoor.Visible = false;
      this.nHallwayWindow.Visible = false;
      this.nHallwayWindow2.Visible = false;
      this.nLamp1.Visible = false;
      this.nLamp2.Visible = false;
      this.lamp_3.Visible = false;
      this.nLamp4.Visible = false;
      if (Game.Variables.GetVariable("general.part_of_day") == "day")
        this.lightDay.Apply();
      else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
        this.lightEvening.Apply();
      else if (Game.Variables.GetVariable("general.part_of_day") == "night")
      {
        this.lightNight.Apply();
        this.nLamp1.Visible = true;
        this.nLamp2.Visible = true;
        this.lamp_3.Visible = true;
        this.nLamp4.Visible = true;
        if (Game.Variables.GetFlag("general.lights_home1f_livingroom"))
          this.nLivingroomDoor.Visible = true;
        if (Game.Variables.GetFlag("general.lights_home_2f_hallway"))
        {
          this.nHallwayWindow.Visible = true;
          this.nHallwayWindow2.Visible = true;
        }
      }
      if (!(Game.Variables.GetVariable("general.chapter") == "1"))
        return;
      if (Game.Variables.GetVariable("general.part_of_day") == "evening")
      {
        this.nHiroBench.Visible = true;
        this.nHiroEvent.Enabled = true;
      }
      else
      {
        this.nHiroBench.Visible = false;
        this.nHiroEvent.Enabled = false;
      }
      if (!(bool) this.varHiroAllTalks.Value)
        return;
      this.nHiroBench.TurningEnabled = true;
      this.nHiroBench.TurnToDefault();
      this.nHiroBench.TurningEnabled = false;
    }
  }
}
