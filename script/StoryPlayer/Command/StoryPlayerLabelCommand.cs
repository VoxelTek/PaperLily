﻿// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerLabelCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerLabelCommand : StoryPlayerCommand
  {
    public string Label { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      storyPlayer.IncreaseIndent();
      storyPlayer.SetNextBlockContinue();
      storyPlayer.Next();
    }
  }
}
