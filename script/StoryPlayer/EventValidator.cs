using System.Collections.Generic;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	internal class EventValidator
	{
		public static List<string> ValidateEvent(StoryPlayerEvent @event)
		{
			Log.Trace("Validating event ", @event.Id);
			List<string> eventMessages = new List<string>();
			StoryPlayerCommand[] dialogue = @event.Dialogue;
			foreach (StoryPlayerCommand block in dialogue)
			{
				if (!block.Validate(out var messages))
				{
					eventMessages.AddRange(messages);
				}
				if (block is StoryPlayerFlowCommand flowBlock)
				{
					ValidateJump(flowBlock, flowBlock, ref eventMessages);
				}
				else if (block is StoryPlayerIfCommand ifBlock)
				{
					ValidateJump(ifBlock.Jump, ifBlock, ref eventMessages);
					ValidateJump(ifBlock.Else, ifBlock, ref eventMessages);
				}
			}
			return eventMessages;
		}

		private static void ValidateJump(StoryPlayerFlowCommand flow, StoryPlayerCommand parentBlock, ref List<string> messages)
		{
			if (flow.Type != 0)
			{
				return;
			}
			StoryPlayerCommand[] dialogue = parentBlock.Event.Dialogue;
			for (int i = 0; i < dialogue.Length; i++)
			{
				if (dialogue[i] is StoryPlayerLabelCommand label && label.Label == flow.Label)
				{
					return;
				}
			}
			messages.Add("Invalid JUMP. Label " + flow.Label + " does not exist");
		}
	}
}
