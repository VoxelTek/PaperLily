// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerObjectiveCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using System;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerObjectiveCommand : StoryPlayerCommand
  {
    [Inject]
    [NonSerialized]
    private IObjectiveManager Objectives;

    public StoryPlayerObjectiveCommand.ObjectiveOperation Operation { get; set; }

    public string Objective { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      switch (this.Operation)
      {
        case StoryPlayerObjectiveCommand.ObjectiveOperation.Add:
          this.Objectives.Add(this.Objective);
          break;
        case StoryPlayerObjectiveCommand.ObjectiveOperation.Complete:
          this.Objectives.Complete(this.Objective);
          break;
        case StoryPlayerObjectiveCommand.ObjectiveOperation.Fail:
          this.Objectives.Fail(this.Objective);
          break;
        case StoryPlayerObjectiveCommand.ObjectiveOperation.Remove:
          this.Objectives.Remove(this.Objective);
          break;
        case StoryPlayerObjectiveCommand.ObjectiveOperation.Clear:
          this.Objectives.ClearObjectives();
          break;
        case StoryPlayerObjectiveCommand.ObjectiveOperation.Silence:
          this.Objectives.SilenceNotifications();
          break;
      }
      storyPlayer.SetNextBlockContinue();
      storyPlayer.Next();
    }

    public enum ObjectiveOperation
    {
      Add,
      Complete,
      Fail,
      Remove,
      Clear,
      Silence,
    }
  }
}
