// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.ObjectivesMenu
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
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
      this.Container = container;
      this.Entries = new List<IMenuEntry>();
      this.nDescription = description;
      this._entries = Game.Objectives.GetCurrentObjectives();
      if (this._entries.Count != 0)
      {
        foreach (IObjective entry in this._entries)
          this.Entries.Add((IMenuEntry) new ObjectivesMenuEntry(entry, (IMenuEntryList) this));
      }
      else
        this.Entries.Add((IMenuEntry) new ObjectivesMenuEmptyEntry((IMenuEntryList) this));
    }

    public override void HighlightSelection()
    {
      base.HighlightSelection();
      if (this.Selection <= -1 || this._entries.Count == 0)
        return;
      IObjective entry = this._entries[this.Selection];
      string str = entry.Description;
      if (entry.HasChildren())
      {
        foreach (IObjective child in entry.Children)
        {
          if (Game.Objectives.IsObjectiveCompleted(child.Id))
            str = str + "\n" + ObjectivesMenu.ObjectiveCompletePrefix + child.Name + ObjectivesMenu.ObjectiveCompleteSuffix;
          else
            str = str + "\n[img=11]res://assets/img/ui/bullet_point.png[/img] " + child.Name;
        }
      }
      this.nDescription.BbcodeText = str;
    }
  }
}
