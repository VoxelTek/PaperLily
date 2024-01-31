// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.IObjectiveManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System.Collections.Generic;

#nullable disable
namespace LacieEngine.API
{
  [InjectableInterface(unique = true)]
  public interface IObjectiveManager : IModule
  {
    void Add(string objectiveId);

    void Remove(string objectiveId);

    void Complete(string objectiveId);

    void Fail(string objectiveId);

    List<IObjective> GetCurrentObjectives();

    List<IObjective> GetAllObjectives();

    void ClearObjectives();

    void SilenceNotifications();

    void ShowNotification();

    bool IsObjectiveInProgress(string objectiveId);

    bool IsObjectiveCompleted(string objectiveId);

    bool IsObjectiveFailed(string objectiveId);

    bool IsObjectiveValid(string objectiveId);

    bool ObjectiveHasPendingChildren(IObjective objective);
  }
}
