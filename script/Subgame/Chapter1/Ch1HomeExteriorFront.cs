// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1HomeExteriorFront
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1HomeExteriorFront : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private Lighting lightDay;
    [Export(PropertyHint.None, "")]
    private Lighting lightEvening;
    [Export(PropertyHint.None, "")]
    private Lighting lightNight;
    [Export(PropertyHint.None, "")]
    private AudioStream sfxBusIdle;
    [Inject]
    private IAudioManager Audio;
    [Inject]
    private IExtraFunctions Functions;
    [LacieEngine.API.GetNode("Foreground/bus")]
    private Node2D nBus;
    [LacieEngine.API.GetNode("Foreground/livingroom_door")]
    private Node2D nLightingLivingroomDoor;
    [LacieEngine.API.GetNode("Foreground/hallway_window")]
    private Node2D nLightingHallwayWindow;
    [LacieEngine.API.GetNode("Foreground/hallway_window_2")]
    private Node2D nLightingHallwayWindow2;
    [LacieEngine.API.GetNode("Foreground/bedroom_a_window")]
    private Node2D nLightingBedroomAWindow;
    [LacieEngine.API.GetNode("Foreground/night")]
    private Node2D nLightingNight;
    [LacieEngine.API.GetNode("Background/misc_markings")]
    private Node2D nMiscMarkings;
    [LacieEngine.API.GetNode("Foreground/Crow")]
    private Node2D nCrow;
    [LacieEngine.API.GetNode("Other/Events/misc_save")]
    private IToggleable nCrowEvt;
    [LacieEngine.API.GetNode("Foreground/Crow/indicator_arrow1")]
    private Node2D nSaveArrow;
    [LacieEngine.API.GetNode("Foreground/Crow/indicator_arrow1/Animation")]
    private AnimationPlayer nSaveArrowAnimation;
    private PVar varPartOfDay = (PVar) "general.part_of_day";
    private PVar varChapter = (PVar) "general.chapter";
    private PVar varLightsHome1fLivingroom = (PVar) "general.lights_home1f_livingroom";
    private PVar varLightsHome2fHallway = (PVar) "general.lights_home_2f_hallway";
    private PVar varLightsHome2fBedroomA = (PVar) "general.lights_home_2f_bedroom_a";
    private PVar varRitualBusWaiting = (PVar) "ch1.home_ritualbuswaiting";
    private PVar varBusNotThere = (PVar) "ch1.bus_not_there";
    private PVar varHomeDrewMarkings = (PVar) "ch1.home_drew_markings";
    private PVar varCheckedSave = (PVar) "ch1.checked_save";

    public override void _BeforeFadeIn()
    {
      if (!(this.varChapter.Value == 1))
        return;
      if (this.varPartOfDay.Value == "evening")
      {
        this.SaveLocation = "system.locations.ch1.home";
        this.SaveImage = "ch1_home_afternoon";
      }
      else
      {
        if (!(this.varPartOfDay.Value == "night"))
          return;
        this.SaveLocation = "system.locations.ch1.home_night";
        this.SaveImage = "ch1_home_night";
      }
    }

    public override void _UpdateRoom()
    {
      Game.Room.ResetLighting();
      this.nLightingLivingroomDoor.Visible = false;
      this.nLightingHallwayWindow.Visible = false;
      this.nLightingHallwayWindow2.Visible = false;
      this.nLightingBedroomAWindow.Visible = false;
      this.nLightingNight.Visible = false;
      if (this.varPartOfDay.Value == "day")
        this.lightDay.Apply();
      else if (this.varPartOfDay.Value == "evening")
        this.lightEvening.Apply();
      else if (this.varPartOfDay.Value == "night")
      {
        this.lightNight.Apply();
        this.nLightingNight.Visible = true;
        if ((bool) this.varLightsHome1fLivingroom.Value)
          this.nLightingLivingroomDoor.Visible = true;
        if ((bool) this.varLightsHome2fHallway.Value)
        {
          this.nLightingHallwayWindow.Visible = true;
          this.nLightingHallwayWindow2.Visible = true;
        }
        if ((bool) this.varLightsHome2fBedroomA.Value)
          this.nLightingBedroomAWindow.Visible = true;
      }
      if (!(this.varChapter.Value == 1))
        return;
      if ((bool) this.varRitualBusWaiting.Value && !(bool) this.varBusNotThere.Value)
      {
        this.Audio.ChangeAmbianceVolume(0.6f);
        this.Audio.PlayAmbiance(this.sfxBusIdle);
        this.nBus.Visible = true;
      }
      else
        this.nBus.Visible = false;
      if ((bool) this.varBusNotThere.Value)
      {
        this.nCrow.Visible = false;
        this.nCrowEvt.Enabled = false;
      }
      else
      {
        this.nCrow.Visible = true;
        this.nCrowEvt.Enabled = true;
      }
      if ((bool) this.varHomeDrewMarkings.Value)
        this.nMiscMarkings.Visible = true;
      if (!(bool) this.varCheckedSave.Value)
      {
        this.nSaveArrow.Visible = true;
        this.nSaveArrowAnimation.PlayFirstAnimation();
      }
      else
      {
        this.nSaveArrow.Visible = false;
        this.nSaveArrowAnimation.Stop();
      }
    }

    public void Ch1StopTimer() => this.Functions.StopTimer();
  }
}
