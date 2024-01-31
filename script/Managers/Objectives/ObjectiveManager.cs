// Decompiled with JetBrains decompiler
// Type: LacieEngine.Objectives.ObjectiveManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Objectives
{
  [Injectable]
  public class ObjectiveManager : IObjectiveManager, IModule, ITranslatable
  {
    private Dictionary<string, Objective> objectives;
    private bool _objectivesUpdated;

    public void Add(string objectiveId)
    {
      if (!this.IsObjectiveValid(objectiveId))
        Log.Error((object) "Attempting to add an invalid objective: ", (object) objectiveId);
      else
        this.Add((IObjective) this.objectives[objectiveId]);
    }

    private void Add(IObjective objective)
    {
      if (objective.HasParent())
      {
        this.Add(objective.Parent);
        if (!Game.State.CurrentObjectives.Contains(objective.Parent.Id))
          return;
      }
      if (Game.State.CurrentObjectives.Contains(objective.Id) || Game.State.CompletedObjectives.Contains(objective.Id) || Game.State.FailedObjectives.Contains(objective.Id))
        return;
      Game.State.CurrentObjectives.Add(objective.Id);
      this._objectivesUpdated = true;
    }

    public void Remove(string objectiveId)
    {
      if (!this.IsObjectiveValid(objectiveId))
        Log.Error((object) "Attempting to remove an invalid objective: ", (object) objectiveId);
      else
        this.Remove((IObjective) this.objectives[objectiveId]);
    }

    private void Remove(IObjective objective)
    {
      Game.State.CurrentObjectives.Remove(objective.Id);
      if (objective.HasChildren())
      {
        foreach (Objective child in objective.Children)
        {
          if (Game.State.CurrentObjectives.Contains(child.Id))
            this.Remove((IObjective) child);
        }
      }
      if (!objective.HasParent() || this.ObjectiveHasPendingChildren(objective.Parent))
        return;
      this.Remove(objective.Parent);
    }

    public void Complete(string objectiveId)
    {
      if (!this.IsObjectiveValid(objectiveId))
        Log.Error((object) "Attempting to complete an invalid objective: ", (object) objectiveId);
      else
        this.Complete((IObjective) this.objectives[objectiveId]);
    }

    private void Complete(IObjective objective)
    {
      if (Game.State.CompletedObjectives.Contains(objective.Id) || Game.State.FailedObjectives.Contains(objective.Id))
        return;
      Game.State.CurrentObjectives.Remove(objective.Id);
      Game.State.CompletedObjectives.Add(objective.Id);
      if (objective.HasChildren())
      {
        foreach (Objective child in objective.Children)
        {
          if (Game.State.CurrentObjectives.Contains(child.Id))
            this.Complete((IObjective) child);
        }
      }
      if (objective.HasParent() && !this.ObjectiveHasPendingChildren(objective.Parent))
        this.Complete(objective.Parent);
      foreach (string objectiveId in objective.OnComplete)
        this.Add(objectiveId);
    }

    public void Fail(string objectiveId)
    {
      if (!this.IsObjectiveValid(objectiveId))
        Log.Error((object) "Attempting to fail an invalid objective: ", (object) objectiveId);
      else
        this.Fail((IObjective) this.objectives[objectiveId]);
    }

    private void Fail(IObjective objective)
    {
      if (Game.State.CompletedObjectives.Contains(objective.Id) || Game.State.FailedObjectives.Contains(objective.Id))
        return;
      Game.State.CurrentObjectives.Remove(objective.Id);
      Game.State.FailedObjectives.Add(objective.Id);
      if (objective.HasChildren())
      {
        foreach (Objective child in objective.Children)
        {
          if (Game.State.CurrentObjectives.Contains(child.Id))
            this.Fail((IObjective) child);
        }
      }
      if (!objective.HasParent() || this.ObjectiveHasPendingChildren(objective.Parent))
        return;
      this.Fail(objective.Parent);
    }

    public List<IObjective> GetCurrentObjectives()
    {
      List<IObjective> currentObjectives = new List<IObjective>();
      foreach (string currentObjective in Game.State.CurrentObjectives)
      {
        if (this.objectives.ContainsKey(currentObjective) && !this.objectives[currentObjective].Hidden && !this.objectives[currentObjective].HasParent())
          currentObjectives.Add((IObjective) this.objectives[currentObjective]);
      }
      currentObjectives.Sort((Comparison<IObjective>) ((x, y) => x.Order.CompareTo(y.Order)));
      return currentObjectives;
    }

    public List<IObjective> GetAllObjectives()
    {
      List<IObjective> allObjectives = new List<IObjective>();
      allObjectives.AddRange((IEnumerable<IObjective>) this.objectives.Values);
      allObjectives.Sort((Comparison<IObjective>) ((x, y) => x.Order.CompareTo(y.Order)));
      return allObjectives;
    }

    public void ClearObjectives() => Game.State.CurrentObjectives.Clear();

    public void SilenceNotifications() => this._objectivesUpdated = false;

    public void ShowNotification()
    {
      if (Game.Settings.ObjectiveNotifications && this._objectivesUpdated)
      {
        Label label = new Label();
        label.Text = "system.objectives.updated";
        label.SetDefaultFontAndColor();
        Game.Screen.ShowFlyout((Control) label);
      }
      this._objectivesUpdated = false;
    }

    public bool IsObjectiveInProgress(string objectiveId)
    {
      return Game.State.CurrentObjectives.Contains(objectiveId);
    }

    public bool IsObjectiveCompleted(string objectiveId)
    {
      return Game.State.CompletedObjectives.Contains(objectiveId);
    }

    public bool IsObjectiveFailed(string objectiveId)
    {
      return Game.State.FailedObjectives.Contains(objectiveId);
    }

    public bool IsObjectiveValid(string objectiveId) => this.objectives.ContainsKey(objectiveId);

    public bool ObjectiveHasPendingChildren(IObjective objective)
    {
      if (objective.HasChildren())
      {
        foreach (Objective child in objective.Children)
        {
          if (!this.IsObjectiveCompleted(child.Id) && !this.IsObjectiveFailed(child.Id))
            return true;
        }
      }
      return false;
    }

    public void Init()
    {
      int order = 0;
      this.objectives = new Dictionary<string, Objective>();
      foreach (string str in GDUtil.ListFilesInPath("res://definitions/objectives/", ".json"))
      {
        ObjectiveDTO objectiveDto = GDUtil.ReadJsonFile<ObjectiveDTO>(str);
        string group = str.StripPrefix("res://definitions/objectives/").StripSuffix(".json");
        foreach (ObjectiveDTO.Objective objective in objectiveDto.Objectives)
          ParseObjectiveFromDto(objective, objective.Id, group);
      }

      Objective ParseObjectiveFromDto(
        ObjectiveDTO.Objective objDto,
        string id,
        string group,
        Objective parent = null)
      {
        Objective parent1 = new Objective();
        parent1.Id = id;
        parent1.Group = group;
        parent1.Order = order++;
        parent1.Name = objDto.Name;
        parent1.Description = objDto.Description;
        parent1.Hidden = objDto.Hidden;
        parent1.OnComplete = objDto.OnComplete ?? new List<string>();
        parent1.Parent = (IObjective) parent;
        parent1.Children = new List<IObjective>();
        if (objDto.Children != null)
        {
          foreach (ObjectiveDTO.Objective child in objDto.Children)
            parent1.Children.Add((IObjective) ParseObjectiveFromDto(child, parent1.Id + "." + child.Id, group, parent1));
        }
        this.objectives[parent1.Id] = parent1;
        return parent1;
      }
    }

    public void ApplyTranslationOverrides()
    {
      foreach (Objective objective in this.objectives.Values)
      {
        objective.Name = Game.Language.GetCaption(objective.Name, this.ObjectiveNameContext((IObjective) objective));
        objective.Description = Game.Language.GetCaption(objective.Description, this.ObjectiveDescriptionContext((IObjective) objective));
      }
    }

    private string ObjectiveNameContext(IObjective objective) => "objectives.name." + objective.Id;

    private string ObjectiveDescriptionContext(IObjective objective)
    {
      return "objectives.desc." + objective.Id;
    }
  }
}
