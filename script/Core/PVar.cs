using Godot;

namespace LacieEngine.Core
{
	public class PVar
	{
		public struct PVarGetProxy
		{
			private readonly string id;

			internal PVarGetProxy(string variableId)
			{
				id = variableId;
			}

			public static implicit operator string(PVarGetProxy val)
			{
				return Game.Variables.GetVariable(val.id);
			}

			public static implicit operator bool(PVarGetProxy val)
			{
				return Game.Variables.GetFlag(val.id);
			}

			public static implicit operator int(PVarGetProxy val)
			{
				return Game.Variables.GetIntVariable(val.id);
			}

			public static bool operator ==(PVarGetProxy val1, string val2)
			{
				return Game.Variables.GetVariable(val1.id) == val2;
			}

			public static bool operator !=(PVarGetProxy val1, string val2)
			{
				return Game.Variables.GetVariable(val1.id) != val2;
			}

			public static bool operator ==(PVarGetProxy val1, bool val2)
			{
				return Game.Variables.GetFlag(val1.id) == val2;
			}

			public static bool operator !=(PVarGetProxy val1, bool val2)
			{
				return Game.Variables.GetFlag(val1.id) != val2;
			}

			public static bool operator ==(PVarGetProxy val1, int val2)
			{
				return Game.Variables.GetIntVariable(val1.id) == val2;
			}

			public static bool operator !=(PVarGetProxy val1, int val2)
			{
				return Game.Variables.GetIntVariable(val1.id) != val2;
			}

			public override bool Equals(object obj)
			{
				if (obj is string str)
				{
					return this == str;
				}
				if (obj is int integer)
				{
					return this == integer;
				}
				if (obj is bool flag)
				{
					return this == flag;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return ((string)this).GetHashCode();
			}

			public override string ToString()
			{
				return ((string)this).ToString();
			}
		}

		public struct PVarSetProxy
		{
			private readonly string value;

			private PVarSetProxy(string variableValue)
			{
				value = variableValue;
			}

			public static implicit operator PVarSetProxy(string val)
			{
				return new PVarSetProxy(val);
			}

			public static implicit operator PVarSetProxy(bool val)
			{
				return new PVarSetProxy(val ? "true" : "false");
			}

			public static implicit operator PVarSetProxy(int val)
			{
				return new PVarSetProxy(val.ToString());
			}

			public static implicit operator PVarSetProxy(Vector2 val)
			{
				return new PVarSetProxy(val.x + "," + val.y);
			}

			public static implicit operator PVarSetProxy(PVarGetProxy val)
			{
				return new PVarSetProxy(val);
			}

			public void Apply(string variableId)
			{
				Game.Variables.SetVariable(variableId, value);
			}
		}

		public string Id { get; private set; }

		public PVarGetProxy Value => new PVarGetProxy(Id);

		public PVarSetProxy NewValue
		{
			set
			{
				value.Apply(Id);
			}
		}

		private PVar(string variableId)
		{
			Id = variableId;
		}

		public static implicit operator PVar(string variableId)
		{
			return new PVar(variableId);
		}
	}
}
