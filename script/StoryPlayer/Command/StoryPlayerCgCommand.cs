// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerCgCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerCgCommand : StoryPlayerCommand
  {
    public StoryPlayerCgCommand.CGOperation Operation { get; set; }

    public CgDisplayManager.CgLayer Layer { get; set; }

    public StoryPlayerCgCommand.CGPosition Position { get; set; }

    public string Image { get; set; }

    public float? Time { get; set; }

    public bool Mini { get; set; }

    public bool Scene { get; set; }

    public Direction Direction { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      storyPlayer.CG.Execute(this);
      storyPlayer.SetNextBlockContinue();
      storyPlayer.Next();
    }

    public override IList<string> GetDependencies()
    {
      if (this.Operation != StoryPlayerCgCommand.CGOperation.Show)
        return (IList<string>) Array.Empty<string>();
      if (this.Scene)
        return (IList<string>) new List<string>(1)
        {
          "res://resources/nodes/cg/" + this.Image + ".tscn"
        };
      return (IList<string>) new List<string>(1)
      {
        "res://assets/img/cg/" + this.Image + ".png"
      };
    }

    public enum CGOperation
    {
      Show,
      Hide,
      Pan,
    }

    public enum CGPosition
    {
      Fill,
      Top,
      Bottom,
      AboveText,
    }

    public enum TransitionType
    {
      Fade,
      Instant,
    }
  }
}
