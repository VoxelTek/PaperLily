// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1ForestStationTracks
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
  public class Ch1ForestStationTracks : GameRoom
  {
    private PVar varCurStage = (PVar) "ch1.forest_station_tracks_stage";
    private PVar varCorrectPath = (PVar) "ch1.forest_station_correct_path";

    public override void _BeforeFadeIn()
    {
      if (this.varCurStage.Value != 3)
      {
        int num = new Random().Next(3);
        PVar varCorrectPath = this.varCorrectPath;
        string str;
        switch (num)
        {
          case 0:
            str = "1";
            break;
          case 1:
            str = "2";
            break;
          case 2:
            str = "3";
            break;
          default:
            str = "1";
            break;
        }
        varCorrectPath.NewValue = (PVar.PVarSetProxy) str;
      }
      else
        this.varCorrectPath.NewValue = (PVar.PVarSetProxy) "0";
    }
  }
}
