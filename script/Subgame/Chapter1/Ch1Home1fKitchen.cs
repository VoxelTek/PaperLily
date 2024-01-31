// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1Home1fKitchen
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
  public class Ch1Home1fKitchen : GameRoom
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
    [Inject]
    private IExtraFunctions Functions;
    [LacieEngine.API.GetNode("Background/wall_window_day")]
    private Node2D nWallWindowDay;
    [LacieEngine.API.GetNode("Background/wall_window_evening")]
    private Node2D nWallWindowEvening;
    [LacieEngine.API.GetNode("Background/wall_window_night")]
    private Node2D nWallWindowNight;
    [LacieEngine.API.GetNode("Foreground/window_light")]
    private Node2D nWindowLight;
    [LacieEngine.API.GetNode("Foreground/window_light_projection")]
    private Node2D nWindowLightProjection;
    [LacieEngine.API.GetNode("Background/misc_sink/misc_sink_water")]
    private Node2D nMiscSinkWater;
    [LacieEngine.API.GetNode("Background/misc_sink/misc_sink_water/Animation")]
    private AnimationPlayer nMiscSinkWaterAnimation;
    [LacieEngine.API.GetNode("Main/furniture_table/misc_note")]
    private Node2D nMiscNote;
    [LacieEngine.API.GetNode("Background/misc_telephone")]
    private Node2D nMiscTelephone;
    [LacieEngine.API.GetNode("Background/misc_telephone_ribbon")]
    private Node2D nMiscTelephoneRibbon;
    [LacieEngine.API.GetNode("Background/misc_fridge")]
    private Sprite nMiscFridge;
    [LacieEngine.API.GetNode("Background/furniture_counter3/item_birdfood")]
    private Sprite nItemBirdfood;
    private PVar varPhoneCallRinging = (PVar) "ch1.home_phone_call_ringing";
    private PVar varTookBirdFood = (PVar) "ch1.took_birdfood";
    private PVar varReadCursedLetter = (PVar) "ch1.home_read_cursed_letter";
    private PVar varInbetween = (PVar) "general.inbetween";
    private const float PhoneVolume = 1f;

    public override void _AfterFadeIn()
    {
      if (!(bool) this.varPhoneCallRinging.Value)
        return;
      this.AddChild((Node) new HomePhoneRinger(this.sfxPhoneRing, 1f));
    }

    public override void _UpdateRoom()
    {
      Game.Room.ResetLighting();
      this.nWallWindowDay.Visible = true;
      this.nWallWindowEvening.Visible = false;
      this.nWallWindowNight.Visible = false;
      this.nWindowLight.Visible = false;
      this.nWindowLightProjection.Visible = false;
      if (Game.Variables.GetVariable("general.part_of_day") == "day")
      {
        if (!Game.Variables.GetFlag("general.lights_home_1f_kitchen"))
        {
          this.lightDayLightsOff.Apply();
          this.nWindowLightProjection.Visible = true;
        }
        else
          this.lightDayLightsOn.Apply();
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
      {
        this.nWallWindowEvening.Visible = true;
        if (!Game.Variables.GetFlag("general.lights_home_1f_kitchen"))
        {
          this.lightEveningLightsOff.Apply();
          this.nWindowLightProjection.Visible = true;
        }
        else
          this.lightEveningLightsOn.Apply();
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "night")
      {
        this.nWallWindowNight.Visible = true;
        if (!Game.Variables.GetFlag("general.lights_home_1f_kitchen"))
        {
          this.lightNightLightsOff.Apply();
          this.nWindowLightProjection.Visible = true;
          this.nWindowLight.Visible = true;
        }
        else
          this.lightNightLightsOn.Apply();
      }
      if (Game.Variables.GetFlag("general.home1f_kitchen_tap"))
      {
        this.nMiscSinkWater.Visible = true;
        this.nMiscSinkWaterAnimation.PlayFirstAnimation();
      }
      else
      {
        this.nMiscSinkWaterAnimation.Stop();
        this.nMiscSinkWater.Visible = false;
      }
      if (!(Game.Variables.GetVariable("general.chapter") == "1"))
        return;
      this.nMiscNote.Visible = true;
      this.nItemBirdfood.Visible = !(bool) this.varTookBirdFood.Value;
      if (Game.Variables.GetFlag("ch1.set_phoneribbon"))
      {
        this.nMiscTelephoneRibbon.Visible = true;
        this.nMiscTelephone.Visible = false;
      }
      else
      {
        this.nMiscTelephoneRibbon.Visible = false;
        this.nMiscTelephone.Visible = true;
      }
      if (!(bool) this.varInbetween.Value || !(bool) this.varReadCursedLetter.Value)
        return;
      this.nMiscNote.Visible = false;
    }

    public void OpenFridge() => this.nMiscFridge.Frame = 1;

    public void CloseFridge() => this.nMiscFridge.Frame = 0;

    public void Ch1StartTimer()
    {
      this.Functions.StartTimer(20f, (Action) (() => Game.Events.PlayEvent("ch1_event_busleft")), true);
    }
  }
}
