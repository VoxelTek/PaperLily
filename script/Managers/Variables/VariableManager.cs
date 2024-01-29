using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Variables
{
	[Injectable]
	public class VariableManager : IVariableManager, IModule
	{
		private List<string> _vars;

		public string GetVariable(string variableName)
		{
			Dictionary<string, string> variables = GetVariableSource(variableName);
			if (OS.IsDebugBuild() && !IsValid(variableName))
			{
				Log.Warn("Variable is not valid: ", variableName);
			}
			if (variables.ContainsKey(variableName))
			{
				return variables[variableName];
			}
			return null;
		}

		public bool GetFlag(string flagName)
		{
			string strVal = GetVariable(flagName);
			if (strVal.IsNullOrEmpty())
			{
				return false;
			}
			return strVal != "false";
		}

		public int GetIntVariable(string variableName)
		{
			string strVal = GetVariable(variableName);
			if (strVal.IsNullOrEmpty())
			{
				return 0;
			}
			if (int.TryParse(strVal, out var val))
			{
				return val;
			}
			Log.Warn("Variable was not numeric: ", variableName);
			return 0;
		}

		public void SetVariable(string variableName, string value)
		{
			GetVariableSource(variableName)[variableName] = value;
		}

		public void SetFlag(string variableName, bool value)
		{
			GetVariableSource(variableName)[variableName] = (value ? "true" : "false");
		}

		public void ToggleFlag(string variableName)
		{
			GetVariableSource(variableName)[variableName] = ((!GetFlag(variableName)) ? "true" : "false");
		}

		public void SetInitialValue(string variableName, string initialValue)
		{
			Dictionary<string, string> variables = GetVariableSource(variableName);
			if (!variables.ContainsKey(variableName) || variables[variableName].IsNullOrEmpty())
			{
				variables[variableName] = initialValue;
			}
		}

		public bool IsValid(string variableId)
		{
			return _vars.BinarySearch(variableId) >= 0;
		}

		public void Validate(string variableId)
		{
			if (OS.IsDebugBuild() && !IsValid(variableId))
			{
				Log.Warn("Variable is not valid: ", variableId);
			}
		}

		public void Init()
		{
			_vars = new List<string>();
			foreach (string varFilename in GDUtil.ListFilesInPath("res://definitions/variables/", null, ".jsonc", fullPath: false))
			{
				foreach (string parsedVar in GDUtil.ReadJsonFile<VariableDTO>("res://definitions/variables/" + varFilename).Variables)
				{
					_vars.Add(parsedVar);
				}
			}
			_vars.Sort();
		}

		private Dictionary<string, string> GetVariableSource(string variableName)
		{
			if (variableName.StartsWith("persistent."))
			{
				return Game.PersistentState.Variables;
			}
			return Game.State.Variables;
		}
	}
}
