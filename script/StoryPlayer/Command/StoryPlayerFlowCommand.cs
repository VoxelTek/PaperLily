using System;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerFlowCommand : StoryPlayerCommand
	{
		public enum FlowType
		{
			Jump,
			Goto,
			Continue,
			Tab,
			Skip,
			End
		}

		public FlowType Type { get; set; }

		public string Label { get; set; }

		public int Line { get; set; }

		public static StoryPlayerFlowCommand JumpBlock(string label)
		{
			return new StoryPlayerFlowCommand
			{
				Type = FlowType.Jump,
				Label = label
			};
		}

		public static StoryPlayerFlowCommand GotoBlock(int line)
		{
			return new StoryPlayerFlowCommand
			{
				Type = FlowType.Goto,
				Line = line
			};
		}

		public static StoryPlayerFlowCommand ContinueBlock()
		{
			return new StoryPlayerFlowCommand
			{
				Type = FlowType.Continue
			};
		}

		public static StoryPlayerFlowCommand TabBlock()
		{
			return new StoryPlayerFlowCommand
			{
				Type = FlowType.Tab
			};
		}

		public static StoryPlayerFlowCommand SkipBlock()
		{
			return new StoryPlayerFlowCommand
			{
				Type = FlowType.Skip
			};
		}

		public static StoryPlayerFlowCommand EndBlock()
		{
			return new StoryPlayerFlowCommand
			{
				Type = FlowType.End
			};
		}

		public override void Execute(StoryPlayer storyPlayer)
		{
			switch (Type)
			{
			case FlowType.Jump:
				storyPlayer.SetNextBlockToLabel(Label);
				break;
			case FlowType.Goto:
				storyPlayer.SetNextBlockToLine(Line);
				break;
			case FlowType.Continue:
				storyPlayer.SetNextBlockContinue();
				break;
			case FlowType.Tab:
				storyPlayer.IncreaseIndent();
				storyPlayer.SetNextBlockContinue();
				break;
			case FlowType.Skip:
				storyPlayer.SetNextBlockSkip();
				break;
			case FlowType.End:
				storyPlayer.SetNextBlockEnd();
				break;
			}
			storyPlayer.Next();
		}
	}
}
