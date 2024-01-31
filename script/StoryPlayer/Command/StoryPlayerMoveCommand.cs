// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerMoveCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerMoveCommand : StoryPlayerCommand
  {
    public string Room { get; set; }

    public string Point { get; set; }

    public Vector2? Coordinates { get; set; }

    public string Direction { get; set; }

    public float? Time { get; set; }

    public bool Cutscene { get; set; }

    public override void Load()
    {
    }

    public override IList<string> GetDependencies()
    {
      return (IList<string>) new List<string>(1)
      {
        "res://resources/nodes/rooms/" + this.Room + ".tscn"
      };
    }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      storyPlayer.UI.HideDialogueBox();
      storyPlayer.Characters.HideAllCharacters();
      this.ChangeRoom(storyPlayer);
    }

    private void ChangeRoom(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      if (!this.Point.IsNullOrEmpty())
        Game.Room.ChangeRoom(this.Room, this.Point, Vector2.Zero, this.Direction, this.Time ?? new float?(), storyPlayer.Event || this.Cutscene);
      else
        Game.Room.ChangeRoom(this.Room, (string) null, this.Coordinates.Value, this.Direction, this.Time ?? new float?(), storyPlayer.Event || this.Cutscene);
    }
  }
}
