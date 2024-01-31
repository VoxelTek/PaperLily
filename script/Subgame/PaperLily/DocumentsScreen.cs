// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.DocumentsScreen
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.UI;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Rooms
{
  public class DocumentsScreen : Control
  {
    [LacieEngine.API.GetNode("Center")]
    private Control nContainer;

    public override void _Ready()
    {
      Game.InputProcessor = Inputs.Processor.None;
      List<IMenuEntry> entries = new List<IMenuEntry>();
      foreach (IItem ownedItem in Game.Items.GetOwnedItems())
      {
        IItem item = ownedItem;
        if (((IList<string>) item.Tags).Contains<string>("document"))
          entries.Add((IMenuEntry) new SimpleMenuEntry(item.Name, (Action) (() => this.ExecuteEvent(item.Event)), (IMenuEntryList) null));
      }
      this.nContainer.AddChild((Node) new SimpleChoicesMenuContainer(entries)
      {
        OnClose = (Action) (() => this.Close())
      });
      Game.InputProcessor = Inputs.Processor.Menu;
    }

    private void ExecuteEvent(string @event)
    {
      this.Delete();
      Game.Events.PlayEvent(@event);
    }

    private void Close()
    {
      this.Delete();
      Game.Core.AssignInputProcessor();
    }
  }
}
