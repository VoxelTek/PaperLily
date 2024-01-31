// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.LogicStatement
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;

#nullable disable
namespace LacieEngine.Core
{
  [Serializable]
  public class LogicStatement
  {
    public LogicStatement.EType Type;
    public bool Not;
    public LogicStatement.EOperator Operator;
    public string Variable;
    public string Value;
    public string Item;
    public int Amount;

    public enum EType
    {
      Variable,
      Item,
      Character,
      HasObjective,
      ObjectiveDone,
      Repeat,
      Random,
    }

    public enum EOperator
    {
      Eq,
      Lt,
      Gt,
      Le,
      Ge,
    }
  }
}
