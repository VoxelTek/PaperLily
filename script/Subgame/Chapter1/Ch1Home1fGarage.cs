// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1Home1fGarage
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
  public class Ch1Home1fGarage : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private Lighting lightDay;
    [Export(PropertyHint.None, "")]
    private Lighting lightEvening;
    [Export(PropertyHint.None, "")]
    private Lighting lightNight;
    [Export(PropertyHint.None, "")]
    private AudioStream sfxPhoneRing;
    [LacieEngine.API.GetNode("Background/item_redpaint")]
    private Node2D nRedPaint;
    private PVar varPhoneCallRinging = (PVar) "ch1.home_phone_call_ringing";
    private const float PhoneVolume = 0.4f;

    public override void _AfterFadeIn()
    {
      if (!(bool) this.varPhoneCallRinging.Value)
        return;
      this.AddChild((Node) new HomePhoneRinger(this.sfxPhoneRing, 0.4f));
    }

    public override void _UpdateRoom()
    {
      Game.Room.ResetLighting();
      if (Game.Variables.GetVariable("general.part_of_day") == "day")
      {
        if (!Game.Variables.GetFlag("general.lights_home_1f_garage"))
          this.lightDay.Apply();
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
      {
        if (!Game.Variables.GetFlag("general.lights_home_1f_garage"))
          this.lightEvening.Apply();
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "night" && !Game.Variables.GetFlag("general.lights_home_1f_garage"))
        this.lightNight.Apply();
      if (!(Game.Variables.GetVariable("general.chapter") == "1"))
        return;
      if (Game.Variables.GetFlag("ch1.took_redpaint"))
        this.nRedPaint.Visible = false;
      else
        this.nRedPaint.Visible = true;
    }
  }
}
