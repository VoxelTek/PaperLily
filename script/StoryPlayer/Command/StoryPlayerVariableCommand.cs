using System;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerVariableCommand : StoryPlayerCommand
	{
		public enum VarOperation
		{
			SetVal,
			SetVar,
			Add,
			Sub
		}

		[NonSerialized]
		[Inject]
		private IVariableManager Variables;

		public VarOperation Operation { get; set; }

		public string Variable { get; set; }

		public string Value { get; set; }

		public bool Not { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			if (Operation == VarOperation.SetVal)
			{
				if (Not)
				{
					Log.Warn("NOT not supported on value assignment operation! It will be ignored.");
				}
				Variables.SetVariable(Variable, Value);
			}
			else if (Operation == VarOperation.SetVar)
			{
				if (Not)
				{
					bool flagVal = !Variables.GetFlag(Value);
					Variables.SetVariable(Variable, flagVal.ToString().ToLower());
				}
				else
				{
					string val = Variables.GetVariable(Value);
					Variables.SetVariable(Variable, val);
				}
			}
			else if (Operation == VarOperation.Add || Operation == VarOperation.Sub)
			{
				if (Not)
				{
					Log.Warn("NOT not supported on add operation! It will be ignored.");
				}
				if (Variables.GetVariable(Variable) == null)
				{
					Variables.SetVariable(Variable, "0");
				}
				if (double.TryParse(Variables.GetVariable(Variable), out var numValue))
				{
					double valueToAdd = double.Parse(Value);
					if (Operation == VarOperation.Sub)
					{
						valueToAdd *= -1.0;
					}
					double total = numValue + valueToAdd;
					Variables.SetVariable(Variable, total.ToString());
				}
				else
				{
					Log.Error("Failed to parse numeric value ", Value);
				}
			}
			storyPlayer.SetNextBlockContinue();
			storyPlayer.Next();
		}
	}
}
