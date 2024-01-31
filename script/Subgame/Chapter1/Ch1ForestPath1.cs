// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1ForestPath1
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Rooms;
using System;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1ForestPath1 : GameRoom
  {
    [LacieEngine.API.GetNode("Main/StructSignpostBlue")]
    private Sprite nSignpost;
    [LacieEngine.API.GetNode("TilesBlock1")]
    private TileMap nTilesNorthBlock1;
    [LacieEngine.API.GetNode("TilesBlock2")]
    private TileMap nTilesNorthBlock2;

    public override void _BeforeFadeIn()
    {
      Game.Variables.SetInitialValue("ch1.forest_path_1_sign_direction", "left");
    }

    public override void _UpdateRoom()
    {
      this.UpdateSignDirection(this.nSignpost, "ch1.forest_path_1_sign_direction");
      if (Game.Variables.GetFlag("ch1.forest_path_1_opened_exit"))
      {
        this.nTilesNorthBlock1.Visible = false;
        this.nTilesNorthBlock1.CollisionLayer = 0U;
        this.nTilesNorthBlock2.Visible = false;
        this.nTilesNorthBlock2.CollisionLayer = 0U;
      }
      else
      {
        this.nTilesNorthBlock1.Visible = true;
        this.nTilesNorthBlock1.CollisionLayer = 2U;
        this.nTilesNorthBlock2.Visible = true;
        this.nTilesNorthBlock2.CollisionLayer = 2U;
      }
    }

    public void RotateSign() => this.RotateSignVariable("ch1.forest_path_1_sign_direction");

    private void RotateSignVariable(string variable)
    {
      string str1;
      switch (Game.Variables.GetVariable(variable))
      {
        case "left":
          str1 = "up";
          break;
        case "right":
          str1 = "down";
          break;
        case "up":
          str1 = "right";
          break;
        case "down":
          str1 = "left";
          break;
        default:
          throw new InvalidOperationException();
      }
      string str2 = str1;
      Game.Variables.SetVariable(variable, str2);
      this.UpdateSignSolutions();
    }

    private void UpdateSignDirection(Sprite sign, string variable)
    {
      string variable1 = Game.Variables.GetVariable(variable);
      Sprite sprite = sign;
      int num;
      switch (variable1)
      {
        case "left":
          num = 0;
          break;
        case "up":
          num = 1;
          break;
        case "right":
          num = 2;
          break;
        case "down":
          num = 3;
          break;
        default:
          throw new InvalidOperationException();
      }
      sprite.Frame = num;
    }

    private void UpdateSignSolutions()
    {
      Game.Variables.SetFlag("ch1.forest_path_1_opened_exit", Game.Variables.GetVariable("ch1.forest_path_1_sign_direction") == "up");
    }
  }
}
