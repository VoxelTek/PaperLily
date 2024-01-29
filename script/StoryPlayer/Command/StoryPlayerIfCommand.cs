using System;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerIfCommand : StoryPlayerCommand
	{
		[NonSerialized]
		[Inject]
		private IVariableManager Variables;

		[NonSerialized]
		[Inject]
		private IItemManager Items;

		[NonSerialized]
		[Inject]
		private IObjectiveManager Objectives;

		public LogicStatement.EType Type { get; set; }

		public LogicStatement.EOperator Operator { get; set; }

		public bool Negate { get; set; }

		public string Variable { get; set; }

		public string Value { get; set; }

		public string Item { get; set; }

		public int Amount { get; set; }

		public StoryPlayerFlowCommand Jump { get; set; }

		public StoryPlayerFlowCommand Else { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			if (MakeLogicStatement().Evaluate())
			{
				Jump.Execute(storyPlayer);
			}
			else
			{
				Else.Execute(storyPlayer);
			}
		}

		public LogicStatement MakeLogicStatement()
		{
			LogicStatement statement = new LogicStatement();
			if (Type == LogicStatement.EType.Variable)
			{
				statement.Type = LogicStatement.EType.Variable;
				statement.Variable = Variable;
				statement.Value = Value;
				statement.Operator = Operator;
			}
			else if (Type == LogicStatement.EType.Item)
			{
				statement.Type = LogicStatement.EType.Item;
				statement.Item = Item;
				statement.Amount = ((Amount <= 0) ? 1 : Amount);
			}
			else if (Type == LogicStatement.EType.Character)
			{
				statement.Type = LogicStatement.EType.Character;
				statement.Value = Value;
			}
			else if (Type == LogicStatement.EType.HasObjective)
			{
				statement.Type = LogicStatement.EType.HasObjective;
				statement.Value = Value;
			}
			else if (Type == LogicStatement.EType.ObjectiveDone)
			{
				statement.Type = LogicStatement.EType.ObjectiveDone;
				statement.Value = Value;
			}
			else if (Type == LogicStatement.EType.Repeat)
			{
				statement.Type = LogicStatement.EType.Repeat;
				statement.Value = base.Event.Id;
				statement.Amount = ((Amount > 0) ? Amount : 2);
			}
			statement.Not = Negate;
			return statement;
		}
	}
}
