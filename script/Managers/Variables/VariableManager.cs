// Decompiled with JetBrains decompiler
// Type: LacieEngine.Variables.VariableManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Variables
{
  [Injectable]
  public class VariableManager : IVariableManager, IModule
  {
    private List<string> _vars;

    public string GetVariable(string variableName)
    {
      Dictionary<string, string> variableSource = this.GetVariableSource(variableName);
      if (OS.IsDebugBuild() && !this.IsValid(variableName))
        Log.Warn((object) "Variable is not valid: ", (object) variableName);
      return variableSource.ContainsKey(variableName) ? variableSource[variableName] : (string) null;
    }

    public bool GetFlag(string flagName)
    {
      string variable = this.GetVariable(flagName);
      return !variable.IsNullOrEmpty() && variable != "false";
    }

    public int GetIntVariable(string variableName)
    {
      string variable = this.GetVariable(variableName);
      if (variable.IsNullOrEmpty())
        return 0;
      int result;
      if (int.TryParse(variable, out result))
        return result;
      Log.Warn((object) "Variable was not numeric: ", (object) variableName);
      return 0;
    }

    public void SetVariable(string variableName, string value)
    {
      this.GetVariableSource(variableName)[variableName] = value;
    }

    public void SetFlag(string variableName, bool value)
    {
      this.GetVariableSource(variableName)[variableName] = value ? "true" : "false";
    }

    public void ToggleFlag(string variableName)
    {
      this.GetVariableSource(variableName)[variableName] = !this.GetFlag(variableName) ? "true" : "false";
    }

    public void SetInitialValue(string variableName, string initialValue)
    {
      Dictionary<string, string> variableSource = this.GetVariableSource(variableName);
      if (variableSource.ContainsKey(variableName) && !variableSource[variableName].IsNullOrEmpty())
        return;
      variableSource[variableName] = initialValue;
    }

    public bool IsValid(string variableId) => this._vars.BinarySearch(variableId) >= 0;

    public void Validate(string variableId)
    {
      if (!OS.IsDebugBuild() || this.IsValid(variableId))
        return;
      Log.Warn((object) "Variable is not valid: ", (object) variableId);
    }

    public void Init()
    {
      this._vars = new List<string>();
      foreach (string str in GDUtil.ListFilesInPath("res://definitions/variables/", suffix: ".jsonc", fullPath: false))
      {
        foreach (string variable in GDUtil.ReadJsonFile<VariableDTO>("res://definitions/variables/" + str).Variables)
          this._vars.Add(variable);
      }
      this._vars.Sort();
    }

    private Dictionary<string, string> GetVariableSource(string variableName)
    {
      return variableName.StartsWith("persistent.") ? Game.PersistentState.Variables : Game.State.Variables;
    }
  }
}
