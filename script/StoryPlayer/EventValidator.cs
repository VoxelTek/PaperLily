// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.EventValidator
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  internal class EventValidator
  {
    public static List<string> ValidateEvent(StoryPlayerEvent @event)
    {
      Log.Trace((object) "Validating event ", (object) @event.Id);
      List<string> messages1 = new List<string>();
      foreach (StoryPlayerCommand storyPlayerCommand in @event.Dialogue)
      {
        List<string> messages2;
        if (!storyPlayerCommand.Validate(out messages2))
          messages1.AddRange((IEnumerable<string>) messages2);
        switch (storyPlayerCommand)
        {
          case StoryPlayerFlowCommand playerFlowCommand:
            EventValidator.ValidateJump(playerFlowCommand, (StoryPlayerCommand) playerFlowCommand, ref messages1);
            break;
          case StoryPlayerIfCommand parentBlock:
            EventValidator.ValidateJump(parentBlock.Jump, (StoryPlayerCommand) parentBlock, ref messages1);
            EventValidator.ValidateJump(parentBlock.Else, (StoryPlayerCommand) parentBlock, ref messages1);
            break;
        }
      }
      return messages1;
    }

    private static void ValidateJump(
      StoryPlayerFlowCommand flow,
      StoryPlayerCommand parentBlock,
      ref List<string> messages)
    {
      if (flow.Type != StoryPlayerFlowCommand.FlowType.Jump)
        return;
      foreach (StoryPlayerCommand storyPlayerCommand in parentBlock.Event.Dialogue)
      {
        if (storyPlayerCommand is StoryPlayerLabelCommand playerLabelCommand && playerLabelCommand.Label == flow.Label)
          return;
      }
      messages.Add("Invalid JUMP. Label " + flow.Label + " does not exist");
    }
  }
}
