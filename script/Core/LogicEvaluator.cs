// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.LogicEvaluator
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

#nullable disable
namespace LacieEngine.Core
{
  internal static class LogicEvaluator
  {
    public static bool Evaluate(this LogicStatement s)
    {
      bool flag = LogicEvaluator.PerformCheck(s);
      if (s.Not)
        flag = !flag;
      Log.Trace((object) "Logic evaluator: evaluated to ", (object) flag);
      return flag;
    }

    private static bool PerformCheck(LogicStatement s)
    {
      if (s.Type == LogicStatement.EType.Variable)
      {
        string s1 = !s.Value.IsNullOrEmpty() ? s.Value : (string) null;
        string variable = Game.Variables.GetVariable(s.Variable);
        bool flag = s1 == null;
        Log.Trace((object) "Logic evaluator: VAR: ", (object) s.Variable, (object) " expected: ", (object) (s1 ?? "<truthy>"), (object) " actual: ", (object) (variable ?? "<null>"));
        if (variable != null)
        {
          double result;
          if (double.TryParse(variable, out result))
          {
            if (flag)
              return result != 0.0;
            double num = double.Parse(s1);
            if (s.Operator == LogicStatement.EOperator.Eq && result == num || s.Operator == LogicStatement.EOperator.Lt && result < num || s.Operator == LogicStatement.EOperator.Gt && result > num || s.Operator == LogicStatement.EOperator.Le && result <= num || s.Operator == LogicStatement.EOperator.Ge && result >= num)
              return true;
          }
          else if (s1 == variable || flag && variable != "false")
            return true;
        }
      }
      else
      {
        if (s.Type == LogicStatement.EType.Item)
        {
          int ownedAmount = Game.Items.GetOwnedAmount(s.Item);
          Log.Trace((object) "Logic evaluator: ITEM: ", (object) s.Item, (object) ", expected: ", (object) s.Amount, (object) " actual: ", (object) ownedAmount);
          return ownedAmount >= s.Amount;
        }
        if (s.Type == LogicStatement.EType.Character)
        {
          bool flag = Game.State.Party.Contains(s.Value);
          Log.Trace((object) "Logic evaluator: CHARACTER: ", (object) s.Value, (object) ", result: ", (object) flag);
          return flag;
        }
        if (s.Type == LogicStatement.EType.HasObjective)
        {
          Log.Trace((object) "Logic evaluator: HAS OBJECTIVE: ", (object) s.Value);
          return Game.Objectives.IsObjectiveInProgress(s.Value);
        }
        if (s.Type == LogicStatement.EType.ObjectiveDone)
        {
          Log.Trace((object) "Logic evaluator: OBJECTIVE DONE: ", (object) s.Value);
          return Game.Objectives.IsObjectiveCompleted(s.Value);
        }
        if (s.Type == LogicStatement.EType.Repeat)
        {
          int interactionCount = Game.Events.GetEventInteractionCount(s.Value);
          Log.Trace((object) "Logic evaluator: REPEAT: ", (object) s.Value, (object) " expected: ", (object) s.Amount, (object) " actual: ", (object) interactionCount);
          return interactionCount >= s.Amount;
        }
        if (s.Type == LogicStatement.EType.Random)
        {
          double result;
          if (double.TryParse(s.Value, out result))
            return DrkieUtil.RollPercent(result);
          Log.Error((object) "Logic evaluator: Random Value invalid.");
        }
        else
          Log.Warn((object) "Logic evaluator: Malformed statement");
      }
      return false;
    }
  }
}
