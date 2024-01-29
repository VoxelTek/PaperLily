using System;

namespace LacieEngine.Core
{
	[Serializable]
	public class LogicStatement
	{
		public enum EType
		{
			Variable,
			Item,
			Character,
			HasObjective,
			ObjectiveDone,
			Repeat,
			Random
		}

		public enum EOperator
		{
			Eq,
			Lt,
			Gt,
			Le,
			Ge
		}

		public EType Type;

		public bool Not;

		public EOperator Operator;

		public string Variable;

		public string Value;

		public string Item;

		public int Amount;
	}
}
