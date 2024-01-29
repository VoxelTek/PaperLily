using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Objectives
{
	[Injectable]
	public class ObjectiveManager : IObjectiveManager, IModule, ITranslatable
	{
		private Dictionary<string, Objective> objectives;

		private bool _objectivesUpdated;

		public void Add(string objectiveId)
		{
			if (!IsObjectiveValid(objectiveId))
			{
				Log.Error("Attempting to add an invalid objective: ", objectiveId);
			}
			else
			{
				Add(objectives[objectiveId]);
			}
		}

		private void Add(IObjective objective)
		{
			if (objective.HasParent())
			{
				Add(objective.Parent);
				if (!Game.State.CurrentObjectives.Contains(objective.Parent.Id))
				{
					return;
				}
			}
			if (!Game.State.CurrentObjectives.Contains(objective.Id) && !Game.State.CompletedObjectives.Contains(objective.Id) && !Game.State.FailedObjectives.Contains(objective.Id))
			{
				Game.State.CurrentObjectives.Add(objective.Id);
				_objectivesUpdated = true;
			}
		}

		public void Remove(string objectiveId)
		{
			if (!IsObjectiveValid(objectiveId))
			{
				Log.Error("Attempting to remove an invalid objective: ", objectiveId);
			}
			else
			{
				Remove(objectives[objectiveId]);
			}
		}

		private void Remove(IObjective objective)
		{
			Game.State.CurrentObjectives.Remove(objective.Id);
			if (objective.HasChildren())
			{
				foreach (Objective child in objective.Children)
				{
					if (Game.State.CurrentObjectives.Contains(child.Id))
					{
						Remove(child);
					}
				}
			}
			if (objective.HasParent() && !ObjectiveHasPendingChildren(objective.Parent))
			{
				Remove(objective.Parent);
			}
		}

		public void Complete(string objectiveId)
		{
			if (!IsObjectiveValid(objectiveId))
			{
				Log.Error("Attempting to complete an invalid objective: ", objectiveId);
			}
			else
			{
				Complete(objectives[objectiveId]);
			}
		}

		private void Complete(IObjective objective)
		{
			if (Game.State.CompletedObjectives.Contains(objective.Id) || Game.State.FailedObjectives.Contains(objective.Id))
			{
				return;
			}
			Game.State.CurrentObjectives.Remove(objective.Id);
			Game.State.CompletedObjectives.Add(objective.Id);
			if (objective.HasChildren())
			{
				foreach (Objective child in objective.Children)
				{
					if (Game.State.CurrentObjectives.Contains(child.Id))
					{
						Complete(child);
					}
				}
			}
			if (objective.HasParent() && !ObjectiveHasPendingChildren(objective.Parent))
			{
				Complete(objective.Parent);
			}
			foreach (string triggeredObjectiveId in objective.OnComplete)
			{
				Add(triggeredObjectiveId);
			}
		}

		public void Fail(string objectiveId)
		{
			if (!IsObjectiveValid(objectiveId))
			{
				Log.Error("Attempting to fail an invalid objective: ", objectiveId);
			}
			else
			{
				Fail(objectives[objectiveId]);
			}
		}

		private void Fail(IObjective objective)
		{
			if (Game.State.CompletedObjectives.Contains(objective.Id) || Game.State.FailedObjectives.Contains(objective.Id))
			{
				return;
			}
			Game.State.CurrentObjectives.Remove(objective.Id);
			Game.State.FailedObjectives.Add(objective.Id);
			if (objective.HasChildren())
			{
				foreach (Objective child in objective.Children)
				{
					if (Game.State.CurrentObjectives.Contains(child.Id))
					{
						Fail(child);
					}
				}
			}
			if (objective.HasParent() && !ObjectiveHasPendingChildren(objective.Parent))
			{
				Fail(objective.Parent);
			}
		}

		public List<IObjective> GetCurrentObjectives()
		{
			List<IObjective> currentObjectives = new List<IObjective>();
			foreach (string objectiveId in Game.State.CurrentObjectives)
			{
				if (objectives.ContainsKey(objectiveId) && !objectives[objectiveId].Hidden && !objectives[objectiveId].HasParent())
				{
					currentObjectives.Add(objectives[objectiveId]);
				}
			}
			currentObjectives.Sort((IObjective x, IObjective y) => x.Order.CompareTo(y.Order));
			return currentObjectives;
		}

		public List<IObjective> GetAllObjectives()
		{
			List<IObjective> list = new List<IObjective>();
			list.AddRange(objectives.Values);
			list.Sort((IObjective x, IObjective y) => x.Order.CompareTo(y.Order));
			return list;
		}

		public void ClearObjectives()
		{
			Game.State.CurrentObjectives.Clear();
		}

		public void SilenceNotifications()
		{
			_objectivesUpdated = false;
		}

		public void ShowNotification()
		{
			if (Game.Settings.ObjectiveNotifications && _objectivesUpdated)
			{
				Label label = new Label();
				label.Text = "system.objectives.updated";
				label.SetDefaultFontAndColor();
				Game.Screen.ShowFlyout(label);
			}
			_objectivesUpdated = false;
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

		public bool IsObjectiveValid(string objectiveId)
		{
			return objectives.ContainsKey(objectiveId);
		}

		public bool ObjectiveHasPendingChildren(IObjective objective)
		{
			if (objective.HasChildren())
			{
				foreach (Objective child in objective.Children)
				{
					if (!IsObjectiveCompleted(child.Id) && !IsObjectiveFailed(child.Id))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void Init()
		{
			int order = 0;
			objectives = new Dictionary<string, Objective>();
			foreach (string item in GDUtil.ListFilesInPath("res://definitions/objectives/", ".json"))
			{
				ObjectiveDTO parsedObjectives = GDUtil.ReadJsonFile<ObjectiveDTO>(item);
				string group2 = item.StripPrefix("res://definitions/objectives/").StripSuffix(".json");
				foreach (ObjectiveDTO.Objective objDto2 in parsedObjectives.Objectives)
				{
					ParseObjectiveFromDto(objDto2, objDto2.Id, group2);
				}
			}
			Objective ParseObjectiveFromDto(ObjectiveDTO.Objective objDto, string id, string group, Objective parent = null)
			{
				Objective obj = new Objective
				{
					Id = id,
					Group = group,
					Order = order++,
					Name = objDto.Name,
					Description = objDto.Description,
					Hidden = objDto.Hidden,
					OnComplete = (objDto.OnComplete ?? new List<string>()),
					Parent = parent,
					Children = new List<IObjective>()
				};
				if (objDto.Children != null)
				{
					foreach (ObjectiveDTO.Objective objDtoChild in objDto.Children)
					{
						obj.Children.Add(ParseObjectiveFromDto(objDtoChild, obj.Id + "." + objDtoChild.Id, group, obj));
					}
				}
				objectives[obj.Id] = obj;
				return obj;
			}
		}

		public void ApplyTranslationOverrides()
		{
			foreach (Objective objective in objectives.Values)
			{
				objective.Name = Game.Language.GetCaption(objective.Name, ObjectiveNameContext(objective));
				objective.Description = Game.Language.GetCaption(objective.Description, ObjectiveDescriptionContext(objective));
			}
		}

		private string ObjectiveNameContext(IObjective objective)
		{
			return "objectives.name." + objective.Id;
		}

		private string ObjectiveDescriptionContext(IObjective objective)
		{
			return "objectives.desc." + objective.Id;
		}
	}
}
