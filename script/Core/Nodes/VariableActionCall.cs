using System;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType]
	public class VariableActionCall : Node, IAction, IToggleable
	{
		public enum VariableOperator
		{
			Eq,
			Ne,
			Gt,
			Ge,
			Lt,
			Le
		}

		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public string Variable { get; set; }

		[Export(PropertyHint.None, "")]
		public VariableOperator Operator { get; set; }

		[Export(PropertyHint.None, "")]
		public string Value { get; set; }

		[Export(PropertyHint.None, "")]
		public NodePath ActionTrue { get; set; } = new NodePath();


		[Export(PropertyHint.None, "")]
		public NodePath ActionFalse { get; set; } = new NodePath();


		public override void _Ready()
		{
			if (!Engine.EditorHint)
			{
				Game.Room.RegisteredRoomUpdates.Add(this);
			}
		}

		public void Execute()
		{
			if (Enabled)
			{
				if (Value.IsNullOrEmpty() ? Game.Variables.GetFlag(Variable) : (Game.Variables.GetVariable(Variable) == Value))
				{
					ExecuteAction(ActionTrue);
				}
				else
				{
					ExecuteAction(ActionFalse);
				}
			}
		}

		private bool ExecuteComparison()
		{
			if (Value.IsNullOrEmpty())
			{
				return Game.Variables.GetFlag(Variable);
			}
			string valueStr = Game.Variables.GetVariable(Variable);
			switch (Operator)
			{
			case VariableOperator.Eq:
				return Value == valueStr;
			case VariableOperator.Ne:
				return Value != valueStr;
			case VariableOperator.Gt:
			case VariableOperator.Ge:
			case VariableOperator.Lt:
			case VariableOperator.Le:
				return CompareNumeric(valueStr, Value, Operator);
			default:
				return false;
			}
		}

		private bool CompareNumeric(string value1, string value2, VariableOperator op)
		{
			try
			{
				double value1Num = double.Parse(value1);
				double value2Num = double.Parse(value2);
				switch (Operator)
				{
				case VariableOperator.Gt:
					return value1Num > value2Num;
				case VariableOperator.Ge:
					return value1Num >= value2Num;
				case VariableOperator.Lt:
					return value1Num < value2Num;
				case VariableOperator.Le:
					return value1Num <= value2Num;
				}
			}
			catch (Exception)
			{
				Log.Error("[VariableActionCall] Either ", value1, " or ", value2, " are not valid numbers to compare.");
			}
			return false;
		}

		private void ExecuteAction(NodePath actionPath)
		{
			if (!actionPath.IsNullOrEmpty() && GetNode(actionPath) is IAction action)
			{
				action.Execute();
			}
		}
	}
}
