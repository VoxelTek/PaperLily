// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1ForestBusStop
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
  public class Ch1ForestBusStop : GameRoom
  {
    [LacieEngine.API.GetNode("Foreground/Bus")]
    private Sprite nBus;

    public override void _UpdateRoom()
    {
      this.nBus.Visible = !Game.Variables.GetFlag("ch1.bus_got_off_cutscene_end");
    }
  }
}
