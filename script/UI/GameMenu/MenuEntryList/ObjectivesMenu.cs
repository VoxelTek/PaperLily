using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class ObjectivesMenu : SimpleVerticalMenu
	{
		private static readonly string ObjectiveCompletePrefix = "[color=gray][img=11]res://assets/img/ui/bullet_point.png[/img] [s]";

		private static readonly string ObjectiveCompleteSuffix = "[/s] " + Game.Language.GetCaption("system.objectives.done") + "[/color]";

		private RichTextLabel nDescription;

		private List<IObjective> _entries;

		public ObjectivesMenu(IMenuEntryContainer container, RichTextLabel description)
		{
			base.Container = container;
			base.Entries = new List<IMenuEntry>();
			nDescription = description;
			_entries = Game.Objectives.GetCurrentObjectives();
			if (_entries.Count != 0)
			{
				foreach (IObjective objective in _entries)
				{
					base.Entries.Add(new ObjectivesMenuEntry(objective, this));
				}
				return;
			}
			base.Entries.Add(new ObjectivesMenuEmptyEntry(this));
		}

		public override void HighlightSelection()
		{
			base.HighlightSelection();
			if (base.Selection <= -1 || _entries.Count == 0)
			{
				return;
			}
			IObjective objective = _entries[base.Selection];
			string bbcodeText = objective.Description;
			if (objective.HasChildren())
			{
				foreach (IObjective child in objective.Children)
				{
					bbcodeText = ((!Game.Objectives.IsObjectiveCompleted(child.Id)) ? (bbcodeText + "\n[img=11]res://assets/img/ui/bullet_point.png[/img] " + child.Name) : (bbcodeText + "\n" + ObjectiveCompletePrefix + child.Name + ObjectiveCompleteSuffix));
				}
			}
			nDescription.BbcodeText = bbcodeText;
		}
	}
}
