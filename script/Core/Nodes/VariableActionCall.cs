// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.VariableActionCall
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType]
  public class VariableActionCall : Node, IAction, IToggleable
  {
    [Export(PropertyHint.None, "")]
    public bool Enabled { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public string Variable { get; set; }

    [Export(PropertyHint.None, "")]
    public VariableActionCall.VariableOperator Operator { get; set; }

    [Export(PropertyHint.None, "")]
    public string Value { get; set; }

    [Export(PropertyHint.None, "")]
    public NodePath ActionTrue { get; set; } = new NodePath();

    [Export(PropertyHint.None, "")]
    public NodePath ActionFalse { get; set; } = new NodePath();

    public override void _Ready()
    {
      if (Engine.EditorHint)
        return;
      Game.Room.RegisteredRoomUpdates.Add((IAction) this);
    }

    public void Execute()
    {
      if (!this.Enabled)
        return;
      if ((this.Value.IsNullOrEmpty() ? (Game.Variables.GetFlag(this.Variable) ? 1 : 0) : (Game.Variables.GetVariable(this.Variable) == this.Value ? 1 : 0)) != 0)
        this.ExecuteAction(this.ActionTrue);
      else
        this.ExecuteAction(this.ActionFalse);
    }

    private bool ExecuteComparison()
    {
      if (this.Value.IsNullOrEmpty())
        return Game.Variables.GetFlag(this.Variable);
      string variable = Game.Variables.GetVariable(this.Variable);
      switch (this.Operator)
      {
        case VariableActionCall.VariableOperator.Eq:
          return this.Value == variable;
        case VariableActionCall.VariableOperator.Ne:
          return this.Value != variable;
        case VariableActionCall.VariableOperator.Gt:
        case VariableActionCall.VariableOperator.Ge:
        case VariableActionCall.VariableOperator.Lt:
        case VariableActionCall.VariableOperator.Le:
          return this.CompareNumeric(variable, this.Value, this.Operator);
        default:
          return false;
      }
    }

    private bool CompareNumeric(
      string value1,
      string value2,
      VariableActionCall.VariableOperator op)
    {
      try
      {
        double num1 = double.Parse(value1);
        double num2 = double.Parse(value2);
        switch (this.Operator)
        {
          case VariableActionCall.VariableOperator.Gt:
            return num1 > num2;
          case VariableActionCall.VariableOperator.Ge:
            return num1 >= num2;
          case VariableActionCall.VariableOperator.Lt:
            return num1 < num2;
          case VariableActionCall.VariableOperator.Le:
            return num1 <= num2;
        }
      }
      catch (Exception ex)
      {
        Log.Error((object) "[VariableActionCall] Either ", (object) value1, (object) " or ", (object) value2, (object) " are not valid numbers to compare.");
      }
      return false;
    }

    private void ExecuteAction(NodePath actionPath)
    {
      if (actionPath.IsNullOrEmpty() || !(this.GetNode(actionPath) is IAction node))
        return;
      node.Execute();
    }

    public enum VariableOperator
    {
      Eq,
      Ne,
      Gt,
      Ge,
      Lt,
      Le,
    }
  }
}
