// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerActionCallCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerActionCallCommand : StoryPlayerCommand
  {
    [Inject]
    [NonSerialized]
    private IGameRoomManager Room;

    public string Value { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      if (this.Room.RegisteredRoomActions.ContainsKey(this.Value))
        this.Room.RegisteredRoomActions[this.Value].Execute();
      else
        Log.Warn((object) "Action ", (object) this.Value, (object) " not found in room: ", (object) this.Room.CurrentRoom.Name);
      storyPlayer.SetNextBlockContinue();
      storyPlayer.Next();
    }
  }
}
