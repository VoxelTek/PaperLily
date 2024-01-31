// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerVariableCommand
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
  public class StoryPlayerVariableCommand : StoryPlayerCommand
  {
    [Inject]
    [NonSerialized]
    private IVariableManager Variables;

    public StoryPlayerVariableCommand.VarOperation Operation { get; set; }

    public string Variable { get; set; }

    public string Value { get; set; }

    public bool Not { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      if (this.Operation == StoryPlayerVariableCommand.VarOperation.SetVal)
      {
        if (this.Not)
          Log.Warn((object) "NOT not supported on value assignment operation! It will be ignored.");
        this.Variables.SetVariable(this.Variable, this.Value);
      }
      else if (this.Operation == StoryPlayerVariableCommand.VarOperation.SetVar)
      {
        if (this.Not)
          this.Variables.SetVariable(this.Variable, (!this.Variables.GetFlag(this.Value)).ToString().ToLower());
        else
          this.Variables.SetVariable(this.Variable, this.Variables.GetVariable(this.Value));
      }
      else if (this.Operation == StoryPlayerVariableCommand.VarOperation.Add || this.Operation == StoryPlayerVariableCommand.VarOperation.Sub)
      {
        if (this.Not)
          Log.Warn((object) "NOT not supported on add operation! It will be ignored.");
        if (this.Variables.GetVariable(this.Variable) == null)
          this.Variables.SetVariable(this.Variable, "0");
        double result;
        if (double.TryParse(this.Variables.GetVariable(this.Variable), out result))
        {
          double num = double.Parse(this.Value);
          if (this.Operation == StoryPlayerVariableCommand.VarOperation.Sub)
            num *= -1.0;
          this.Variables.SetVariable(this.Variable, (result + num).ToString());
        }
        else
          Log.Error((object) "Failed to parse numeric value ", (object) this.Value);
      }
      storyPlayer.SetNextBlockContinue();
      storyPlayer.Next();
    }

    public enum VarOperation
    {
      SetVal,
      SetVar,
      Add,
      Sub,
    }
  }
}
