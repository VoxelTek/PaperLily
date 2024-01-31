// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1Home2fHallway
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
  public class Ch1Home2fHallway : GameRoom
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
    [Export(PropertyHint.None, "")]
    private AudioStream sfxPhoneRing;
    [LacieEngine.API.GetNode("Background/wall_door2")]
    private Sprite nLastDoor;
    [LacieEngine.API.GetNode("Lighting/WindowLights")]
    private Node2D nWindowLights;
    [LacieEngine.API.GetNode("Lighting/lamp_light")]
    private Node2D nLampLight;
    [LacieEngine.API.GetNode("Lighting/lamp_light_top")]
    private Node2D nLampLightTop;
    private PVar varChapter = (PVar) "general.chapter";
    private PVar varInbeteween = (PVar) "general.inbetween";
    private PVar varDoorSpook = (PVar) "ch1.home_seen_spook_door";
    private PVar varPhoneCallRinging = (PVar) "ch1.home_phone_call_ringing";
    private PVar varLampOn = (PVar) "general.lights_home_2f_hallway_lamp";
    private PVar varMissedBus = (PVar) "ch1.home_missed_bus";
    private const float PhoneVolume = 0.1f;

    public override void _AfterFadeIn()
    {
      if (!(bool) this.varPhoneCallRinging.Value)
        return;
      this.AddChild((Node) new HomePhoneRinger(this.sfxPhoneRing, 0.1f));
    }

    public override void _UpdateRoom()
    {
      Game.Room.ResetLighting();
      this.nWindowLights.Visible = false;
      this.nLampLight.Visible = false;
      this.nLampLightTop.Visible = false;
      if (Game.Variables.GetVariable("general.part_of_day") == "day")
      {
        if (!Game.Variables.GetFlag("general.lights_home_2f_hallway"))
        {
          this.lightDayLightsOff.Apply();
          this.nWindowLights.Visible = true;
        }
        else
          this.lightDayLightsOn.Apply();
        if ((bool) this.varLampOn.Value)
          this.nLampLightTop.Visible = true;
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
      {
        if (!Game.Variables.GetFlag("general.lights_home_2f_hallway"))
        {
          this.lightEveningLightsOff.Apply();
          this.nWindowLights.Visible = true;
          if ((bool) this.varLampOn.Value)
            this.nLampLight.Visible = true;
        }
        else
        {
          this.lightEveningLightsOn.Apply();
          if ((bool) this.varLampOn.Value)
            this.nLampLightTop.Visible = true;
        }
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "night")
      {
        if (!Game.Variables.GetFlag("general.lights_home_2f_hallway"))
        {
          this.lightNightLightsOff.Apply();
          this.nWindowLights.Visible = true;
          if ((bool) this.varLampOn.Value)
            this.nLampLight.Visible = true;
        }
        else
        {
          this.lightNightLightsOn.Apply();
          if ((bool) this.varLampOn.Value)
            this.nLampLightTop.Visible = true;
        }
      }
      if (!(this.varChapter.Value == 1) || !(bool) this.varInbeteween.Value || (bool) this.varMissedBus.Value)
        return;
      this.nLastDoor.Frame = !(bool) this.varDoorSpook.Value ? 1 : 0;
    }
  }
}
