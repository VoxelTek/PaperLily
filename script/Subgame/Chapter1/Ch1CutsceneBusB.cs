// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1CutsceneBusB
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
  public class Ch1CutsceneBusB : GameRoom
  {
    [LacieEngine.API.GetNode("Ch1_Bus_B")]
    private GameRoom nRoom;
    [LacieEngine.API.GetNode("Ch1_Bus_B/Background/seats_b5/lacie/Animation")]
    private AnimationPlayer nLacieScoochAnimation;

    public override Node FindNodeInRoom(string nodeName) => this.nRoom.FindNodeInRoom(nodeName);

    public void LacieScoochOut() => this.nLacieScoochAnimation.PlayFirstAnimation();
  }
}
