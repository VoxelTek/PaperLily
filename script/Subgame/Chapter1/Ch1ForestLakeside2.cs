// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1ForestLakeside2
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1ForestLakeside2 : GameRoom
  {
    [LacieEngine.API.GetNode("Ground/Paddle")]
    private Node2D nPaddle;
    [LacieEngine.API.GetNode("Background/FaucetOn")]
    private Node2D nFaucetOn;
    [LacieEngine.API.GetNode("Background/FaucetOff")]
    private Node2D nFaucetOff;
    private PVar varTookPaddle = (PVar) "ch1.forest_lakeside_took_paddle_temple";
    private PVar varFaucetOn = (PVar) "ch1.forest_lakeside_faucet_on";
    private PVar varEnding8 = (PVar) "ch1.temp_ending_1_8";
    private PVar varCorrectPetals = (PVar) "ch1.forest_lakeside_correct_petals";
    private PVar varCorrectAlgae = (PVar) "ch1.forest_lakeside_correct_algae";
    private PVar varSeenLakeside3 = (PVar) "persistent.ch1_seen_lakeside_3";

    public override void _BeforeFadeIn()
    {
      if (!(bool) this.varCorrectPetals.Value)
      {
        int num = new Random().Next((bool) this.varSeenLakeside3.Value ? 3 : 2);
        PVar varCorrectPetals = this.varCorrectPetals;
        string str;
        switch (num)
        {
          case 0:
            str = "pink";
            break;
          case 1:
            str = "purple";
            break;
          case 2:
            str = "white";
            break;
          default:
            str = "pink";
            break;
        }
        varCorrectPetals.NewValue = (PVar.PVarSetProxy) str;
      }
      if ((bool) this.varCorrectAlgae.Value)
        return;
      int num1 = new Random().Next(3);
      PVar varCorrectAlgae = this.varCorrectAlgae;
      string str1;
      switch (num1)
      {
        case 0:
          str1 = "green";
          break;
        case 1:
          str1 = "blue";
          break;
        case 2:
          str1 = "black";
          break;
        default:
          str1 = "green";
          break;
      }
      varCorrectAlgae.NewValue = (PVar.PVarSetProxy) str1;
    }

    public override void _UpdateRoom()
    {
      this.nPaddle.Visible = !(bool) this.varEnding8.Value && !(bool) this.varTookPaddle.Value;
      this.nFaucetOn.Visible = !(bool) this.varEnding8.Value && (bool) this.varFaucetOn.Value;
      this.nFaucetOff.Visible = !this.nFaucetOn.Visible;
    }
  }
}
