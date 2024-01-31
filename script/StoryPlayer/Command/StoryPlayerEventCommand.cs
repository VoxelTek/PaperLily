// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerEventCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerEventCommand : StoryPlayerCommand
  {
    public StoryPlayerEventCommand.EventOperation Operation { get; set; }

    public string Value { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      switch (this.Operation)
      {
        case StoryPlayerEventCommand.EventOperation.Start:
          storyPlayer.Event = true;
          break;
        case StoryPlayerEventCommand.EventOperation.End:
          storyPlayer.Event = false;
          break;
        case StoryPlayerEventCommand.EventOperation.HideUi:
          storyPlayer.HideAllUI();
          break;
        case StoryPlayerEventCommand.EventOperation.SkipOn:
          storyPlayer.Text.SkipDisabled = false;
          break;
        case StoryPlayerEventCommand.EventOperation.SkipOff:
          storyPlayer.Text.SkipDisabled = true;
          break;
        case StoryPlayerEventCommand.EventOperation.AutoOn:
          storyPlayer.Text.AutoNext = true;
          break;
        case StoryPlayerEventCommand.EventOperation.AutoOff:
          storyPlayer.Text.AutoNext = false;
          break;
        case StoryPlayerEventCommand.EventOperation.Call:
          storyPlayer.SetNextBlockContinue();
          storyPlayer.CallEvent(this.Value);
          return;
        case StoryPlayerEventCommand.EventOperation.Queue:
          storyPlayer.AddToCallQueue(this.Value);
          break;
      }
      storyPlayer.SetNextBlockContinue();
      storyPlayer.Next();
    }

    public enum EventOperation
    {
      Start,
      End,
      HideUi,
      SkipOn,
      SkipOff,
      AutoOn,
      AutoOff,
      Call,
      Queue,
    }
  }
}
