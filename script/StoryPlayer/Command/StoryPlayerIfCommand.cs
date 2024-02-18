// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerIfCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.StoryPlayer
{
    [Serializable]
    public class StoryPlayerIfCommand : StoryPlayerCommand
    {
        [Inject]
        [NonSerialized]
        private IVariableManager Variables;
        [Inject]
        [NonSerialized]
        private IItemManager Items;
        [Inject]
        [NonSerialized]
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

        public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
        {
            if (this.MakeLogicStatement().Evaluate())
                this.Jump.Execute(storyPlayer);
            else
                this.Else.Execute(storyPlayer);
        }

        public LogicStatement MakeLogicStatement()
        {
            LogicStatement logicStatement = new LogicStatement();
            if (this.Type == LogicStatement.EType.Variable)
            {
                logicStatement.Type = LogicStatement.EType.Variable;
                logicStatement.Variable = this.Variable;
                logicStatement.Value = this.Value;
                logicStatement.Operator = this.Operator;
            }
            else if (this.Type == LogicStatement.EType.Item)
            {
                logicStatement.Type = LogicStatement.EType.Item;
                logicStatement.Item = this.Item;
                logicStatement.Amount = this.Amount > 0 ? this.Amount : 1;
            }
            else if (this.Type == LogicStatement.EType.Character)
            {
                logicStatement.Type = LogicStatement.EType.Character;
                logicStatement.Value = this.Value;
            }
            else if (this.Type == LogicStatement.EType.HasObjective)
            {
                logicStatement.Type = LogicStatement.EType.HasObjective;
                logicStatement.Value = this.Value;
            }
            else if (this.Type == LogicStatement.EType.ObjectiveDone)
            {
                logicStatement.Type = LogicStatement.EType.ObjectiveDone;
                logicStatement.Value = this.Value;
            }
            else if (this.Type == LogicStatement.EType.Repeat)
            {
                logicStatement.Type = LogicStatement.EType.Repeat;
                logicStatement.Value = this.Event.Id;
                logicStatement.Amount = this.Amount > 0 ? this.Amount : 2;
            }
            else if (this.Type == LogicStatement.EType.System)
            {
                logicStatement.Type = LogicStatement.EType.System;
                logicStatement.Value = this.Value;
            }
            logicStatement.Not = this.Negate;
            return logicStatement;
        }
    }
}
