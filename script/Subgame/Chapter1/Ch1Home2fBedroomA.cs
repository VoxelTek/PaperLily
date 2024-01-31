// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1Home2fBedroomA
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1Home2fBedroomA : GameRoom
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
    [LacieEngine.API.GetNode("Main/FurnitureChair/lacie")]
    private Node2D nLacieAtComputer;
    [LacieEngine.API.GetNode("Foreground/WindowLights")]
    private Node2D nWindowLights;
    [LacieEngine.API.GetNode("Foreground/wall_shelf_2/shiny")]
    private Node2D nCh1ItemsShiny;
    private PVar varTookCh1Items = (PVar) "ch1.home_took_wardrobe_items";

    public override void _UpdateRoom()
    {
      Game.Room.ResetLighting();
      this.nWindowLights.Visible = false;
      if (Game.Variables.GetVariable("general.part_of_day") == "day")
      {
        if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_a"))
        {
          this.lightDayLightsOff.Apply();
          this.nWindowLights.Visible = true;
        }
        else
          this.lightDayLightsOn.Apply();
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
      {
        if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_a"))
        {
          this.lightEveningLightsOff.Apply();
          this.nWindowLights.Visible = true;
        }
        else
          this.lightEveningLightsOn.Apply();
      }
      else if (Game.Variables.GetVariable("general.part_of_day") == "night")
      {
        if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_a"))
        {
          this.lightNightLightsOff.Apply();
          this.nWindowLights.Visible = true;
          this.nWindowLights.Visible = true;
        }
        else
          this.lightNightLightsOn.Apply();
      }
      if (!(Game.Variables.GetVariable("general.chapter") == "1"))
        return;
      this.nCh1ItemsShiny.Visible = !(bool) this.varTookCh1Items.Value;
    }

    public void ComputerSit()
    {
      Game.Player.Node.Visible = false;
      this.nLacieAtComputer.Visible = true;
    }

    public void ComputerGetUp()
    {
      Game.Player.Node.Visible = true;
      this.nLacieAtComputer.Visible = false;
    }

    public void HidePlayer() => Game.Player.Node.Visible = false;
  }
}
