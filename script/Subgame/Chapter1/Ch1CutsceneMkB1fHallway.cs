// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1CutsceneMkB1fHallway
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1CutsceneMkB1fHallway : GameRoom
  {
    [LacieEngine.API.GetNode("Room")]
    private GameRoom nRoom;

    public override Node2D GetMainLayer() => this.nRoom.GetMainLayer();

    public override Node FindNodeInRoom(string nodeName) => this.nRoom.FindNodeInRoom(nodeName);
  }
}
