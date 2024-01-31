// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerUnparsedCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  public class StoryPlayerUnparsedCommand : StoryPlayerCommand
  {
    public List<string> Tokens { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      Log.Error((object) "Error: Attempted to execute without assigning parser! in event [", (object) this.Event.Id, (object) "] line ", (object) (this.Index + 1));
      storyPlayer.SetNextBlockContinue();
      storyPlayer.Next();
    }
  }
}
