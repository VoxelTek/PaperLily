// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1Home2fBathroom
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
  public class Ch1Home2fBathroom : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private Lighting lightDay;
    [Export(PropertyHint.None, "")]
    private Lighting lightEvening;
    [Export(PropertyHint.None, "")]
    private Lighting lightNight;
    [LacieEngine.API.GetNode("Background/furniture_toilet")]
    private Sprite nToilet;

    public override void _UpdateRoom()
    {
      Game.Room.ResetLighting();
      if (Game.Variables.GetVariable("general.part_of_day") == "day")
      {
        if (!Game.Variables.GetFlag("general.lights_home_2f_bathroom"))
          this.lightDay.Apply();
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
      {
        if (!Game.Variables.GetFlag("general.lights_home_2f_bathroom"))
          this.lightEvening.Apply();
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "night" && !Game.Variables.GetFlag("general.lights_home_2f_bathroom"))
        this.lightNight.Apply();
      if (!Game.Variables.GetFlag("general.home_2f_bathroom_toilet_down"))
        this.nToilet.Frame = 1;
      else
        this.nToilet.Frame = 0;
    }
  }
}
