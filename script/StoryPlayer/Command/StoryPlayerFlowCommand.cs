// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerFlowCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerFlowCommand : StoryPlayerCommand
  {
    public StoryPlayerFlowCommand.FlowType Type { get; set; }

    public string Label { get; set; }

    public int Line { get; set; }

    public static StoryPlayerFlowCommand JumpBlock(string label)
    {
      return new StoryPlayerFlowCommand()
      {
        Type = StoryPlayerFlowCommand.FlowType.Jump,
        Label = label
      };
    }

    public static StoryPlayerFlowCommand GotoBlock(int line)
    {
      return new StoryPlayerFlowCommand()
      {
        Type = StoryPlayerFlowCommand.FlowType.Goto,
        Line = line
      };
    }

    public static StoryPlayerFlowCommand ContinueBlock()
    {
      return new StoryPlayerFlowCommand()
      {
        Type = StoryPlayerFlowCommand.FlowType.Continue
      };
    }

    public static StoryPlayerFlowCommand TabBlock()
    {
      return new StoryPlayerFlowCommand()
      {
        Type = StoryPlayerFlowCommand.FlowType.Tab
      };
    }

    public static StoryPlayerFlowCommand SkipBlock()
    {
      return new StoryPlayerFlowCommand()
      {
        Type = StoryPlayerFlowCommand.FlowType.Skip
      };
    }

    public static StoryPlayerFlowCommand EndBlock()
    {
      return new StoryPlayerFlowCommand()
      {
        Type = StoryPlayerFlowCommand.FlowType.End
      };
    }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      switch (this.Type)
      {
        case StoryPlayerFlowCommand.FlowType.Jump:
          storyPlayer.SetNextBlockToLabel(this.Label);
          break;
        case StoryPlayerFlowCommand.FlowType.Goto:
          storyPlayer.SetNextBlockToLine(this.Line);
          break;
        case StoryPlayerFlowCommand.FlowType.Continue:
          storyPlayer.SetNextBlockContinue();
          break;
        case StoryPlayerFlowCommand.FlowType.Tab:
          storyPlayer.IncreaseIndent();
          storyPlayer.SetNextBlockContinue();
          break;
        case StoryPlayerFlowCommand.FlowType.Skip:
          storyPlayer.SetNextBlockSkip();
          break;
        case StoryPlayerFlowCommand.FlowType.End:
          storyPlayer.SetNextBlockEnd();
          break;
      }
      storyPlayer.Next();
    }

    public enum FlowType
    {
      Jump,
      Goto,
      Continue,
      Tab,
      Skip,
      End,
    }
  }
}
