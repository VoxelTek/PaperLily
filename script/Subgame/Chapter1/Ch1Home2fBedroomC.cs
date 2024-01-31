// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1Home2fBedroomC
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
  public class Ch1Home2fBedroomC : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private Lighting lightDayLightsOff;
    [Export(PropertyHint.None, "")]
    private Lighting lightDayLightsOn;
    [Export(PropertyHint.None, "")]
    private Lighting lightEveningLightsOff;
    [Export(PropertyHint.None, "")]
    private Lighting lightEveningLightsOn;
    [Export(PropertyHint.None, "")]
    private Lighting lightNightLightsOff;
    [Export(PropertyHint.None, "")]
    private Lighting lightNightLightsOn;
    [LacieEngine.API.GetNode("Background/wall_window_day")]
    private Node2D nWallWindowDay;
    [LacieEngine.API.GetNode("Background/wall_window_evening")]
    private Node2D nWallWindowEvening;
    [LacieEngine.API.GetNode("Background/wall_window_night")]
    private Node2D nWallWindowNight;
    [LacieEngine.API.GetNode("Foreground/lamp_light")]
    private Node2D nLampLight;
    [LacieEngine.API.GetNode("Foreground/lamp_light_top")]
    private Node2D nLampLightTop;
    [LacieEngine.API.GetNode("Background/furniture_wardrobe")]
    private Sprite nWardrobe;
    [LacieEngine.API.GetNode("Foreground/WindowLights")]
    private Node2D nWindowLights;
    private PVar varLampOn = (PVar) "general.lights_home_2f_bedroom_c_lamp";

    public override void _UpdateRoom()
    {
      Game.Room.ResetLighting();
      this.nWallWindowDay.Visible = true;
      this.nWallWindowEvening.Visible = false;
      this.nWallWindowNight.Visible = false;
      this.nLampLight.Visible = false;
      this.nLampLightTop.Visible = false;
      this.nWindowLights.Visible = false;
      if (Game.Variables.GetVariable("general.part_of_day") == "day")
      {
        if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_c"))
        {
          this.lightDayLightsOff.Apply();
          this.nWindowLights.Visible = true;
        }
        else
          this.lightDayLightsOn.Apply();
        if (!(bool) this.varLampOn.Value)
          return;
        this.nLampLightTop.Visible = true;
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
      {
        this.nWallWindowDay.Visible = false;
        this.nWallWindowEvening.Visible = true;
        if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_c"))
        {
          this.lightEveningLightsOff.Apply();
          this.nWindowLights.Visible = true;
          if (!(bool) this.varLampOn.Value)
            return;
          this.nLampLight.Visible = true;
        }
        else
        {
          this.lightEveningLightsOn.Apply();
          if (!(bool) this.varLampOn.Value)
            return;
          this.nLampLightTop.Visible = true;
        }
      }
      else
      {
        if (!(Game.Variables.GetVariable("general.part_of_day") == "night"))
          return;
        this.nWallWindowDay.Visible = false;
        this.nWallWindowNight.Visible = true;
        if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_c"))
        {
          this.lightNightLightsOff.Apply();
          this.nWindowLights.Visible = true;
          if (!(bool) this.varLampOn.Value)
            return;
          this.nLampLight.Visible = true;
        }
        else
        {
          this.lightNightLightsOn.Apply();
          if (!(bool) this.varLampOn.Value)
            return;
          this.nLampLightTop.Visible = true;
        }
      }
    }

    public void OpenWardrobe() => this.nWardrobe.Frame = 2;

    public async void CloseWardrobe()
    {
      Ch1Home2fBedroomC baseNode = this;
      baseNode.nWardrobe.Frame = 1;
      await baseNode.DelaySeconds(0.4);
      baseNode.nWardrobe.Frame = 0;
      await baseNode.DelaySeconds(0.4);
      baseNode.nWardrobe.Frame = 1;
    }
  }
}
