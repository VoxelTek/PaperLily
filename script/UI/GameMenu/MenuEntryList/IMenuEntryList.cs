// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.IMenuEntryList
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
  public interface IMenuEntryList
  {
    IMenuEntryContainer Container { get; set; }

    IMenuEntryList Parent { get; set; }

    int Selection { get; set; }

    List<IMenuEntry> Entries { get; }

    Action OnBack { get; set; }

    Control DrawContent();

    void Update()
    {
      foreach (IMenuEntry entry in this.Entries)
        entry.Update();
    }

    void Back();

    void Root()
    {
      this.Container.Clear();
      this.Container.AddToFrame(this.DrawContent());
      this.Container.Menu = this;
      this.ResetSelection();
    }

    void HandleInput(InputEvent @event);

    void HighlightSelection();

    void ResetSelection()
    {
      this.Selection = 0;
      this.HighlightSelection();
    }
  }
}
