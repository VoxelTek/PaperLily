// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1Home1fLivingroom
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;
using System;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1Home1fLivingroom : GameRoom
  {
    [Inject]
    private IExtraFunctions ExtraFunctions;
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
    [LacieEngine.API.GetNode("Background/wall_window_day")]
    private Node2D nWallWindowDay;
    [LacieEngine.API.GetNode("Background/wall_window_2_day")]
    private Node2D nWallWindow2Day;
    [LacieEngine.API.GetNode("Background/wall_window_evening")]
    private Node2D nWallWindowEvening;
    [LacieEngine.API.GetNode("Background/wall_window_2_evening")]
    private Node2D nWallWindow2Evening;
    [LacieEngine.API.GetNode("Background/wall_window_night")]
    private Node2D nWallWindowNight;
    [LacieEngine.API.GetNode("Background/wall_window_2_night")]
    private Node2D nWallWindow2Night;
    [LacieEngine.API.GetNode("Foreground/window_light_projection")]
    private Node2D nWindowLightProjection;
    [LacieEngine.API.GetNode("Foreground/window_light_projection_2")]
    private Node2D nWindowLightProjection2;
    [LacieEngine.API.GetNode("Foreground/wall_window_light")]
    private Node2D nWallWindowLight;
    [LacieEngine.API.GetNode("Foreground/wall_window_light_2")]
    private Node2D nWallWindowLight2;
    [LacieEngine.API.GetNode("Foreground/lamp_light")]
    private Node2D nLampLight;
    [LacieEngine.API.GetNode("Foreground/lamp_light_top")]
    private Node2D nLampLightTop;
    [LacieEngine.API.GetNode("Background/misc_tv/Animation")]
    private AnimationPlayer nTvAnimation;
    [LacieEngine.API.GetNode("Background/wall_painting3")]
    private Sprite nCatPainting;
    private PVar varInbetween = (PVar) "general.inbetween";
    private PVar varPhoneCallRinging = (PVar) "ch1.home_phone_call_ringing";
    private PVar varPhoneCallDone = (PVar) "ch1.home_phone_call_done";
    private PVar varLampOn = (PVar) "general.lights_home1f_lamp";
    private const float PhoneVolume = 0.6f;

    public override void _AfterFadeIn()
    {
      if (!(bool) this.varPhoneCallRinging.Value)
        return;
      this.AddChild((Node) new HomePhoneRinger(this.sfxPhoneRing, 0.6f));
    }

    public override void _UpdateRoom()
    {
      Game.Room.ResetLighting();
      this.nWallWindowDay.Visible = true;
      this.nWallWindow2Day.Visible = true;
      this.nWallWindowEvening.Visible = false;
      this.nWallWindow2Evening.Visible = false;
      this.nWallWindowNight.Visible = false;
      this.nWallWindow2Night.Visible = false;
      this.nWindowLightProjection.Visible = false;
      this.nWindowLightProjection2.Visible = false;
      this.nWallWindowLight.Visible = false;
      this.nWallWindowLight2.Visible = false;
      this.nLampLight.Visible = false;
      this.nLampLightTop.Visible = false;
      if (Game.Variables.GetVariable("general.part_of_day") == "day")
      {
        if (!Game.Variables.GetFlag("general.lights_home1f_livingroom"))
        {
          this.lightDayLightsOff.Apply();
          this.nWindowLightProjection.Visible = true;
          this.nWindowLightProjection2.Visible = true;
        }
        else
          this.lightDayLightsOn.Apply();
        if ((bool) this.varLampOn.Value)
          this.nLampLightTop.Visible = true;
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
      {
        this.nWallWindowEvening.Visible = true;
        this.nWallWindow2Evening.Visible = true;
        if (!Game.Variables.GetFlag("general.lights_home1f_livingroom"))
        {
          this.lightEveningLightsOff.Apply();
          this.nWindowLightProjection.Visible = true;
          this.nWindowLightProjection2.Visible = true;
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
        this.nWallWindowNight.Visible = true;
        this.nWallWindow2Night.Visible = true;
        if (!Game.Variables.GetFlag("general.lights_home1f_livingroom"))
        {
          this.lightNightLightsOff.Apply();
          this.nWindowLightProjection.Visible = true;
          this.nWindowLightProjection2.Visible = true;
          this.nWallWindowLight.Visible = true;
          this.nWallWindowLight2.Visible = true;
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
      if (Game.Variables.GetFlag("general.tv_home1f"))
      {
        if (Game.Variables.GetFlag("general.inbetween"))
          this.nTvAnimation.Play("static");
        else if (Game.Variables.GetVariable("general.part_of_day") == "day")
          this.nTvAnimation.Play("program_1");
        else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
          this.nTvAnimation.Play("program_2");
        else if (Game.Variables.GetVariable("general.part_of_day") == "night")
          this.nTvAnimation.Play("program_3");
      }
      else
        this.nTvAnimation.Play("off");
      if (!(bool) this.varInbetween.Value)
        return;
      this.nCatPainting.Frame = 1;
    }

    public void AttemptTriggerPhoneCall()
    {
      if (!DrkieUtil.RollPercent(4.0))
        return;
      this.ExtraFunctions.StartTimer(30f, new Action(this.IgnorePhoneCall), true);
      this.ExtraFunctions.GetTimer().Visible = false;
      this.AddChild((Node) new HomePhoneRinger(this.sfxPhoneRing, 0.6f));
      this.varPhoneCallRinging.NewValue = (PVar.PVarSetProxy) true;
    }

    public void IgnorePhoneCall()
    {
      this.varPhoneCallRinging.NewValue = (PVar.PVarSetProxy) false;
      this.varPhoneCallDone.NewValue = (PVar.PVarSetProxy) true;
    }
  }
}
