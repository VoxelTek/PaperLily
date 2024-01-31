// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SimpleChoicesMenuContainer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
  public class SimpleChoicesMenuContainer : CenterContainer, IMenuEntryContainer
  {
    private List<IMenuEntry> _entries;

    public Frame Control { get; private set; }

    public IMenuEntryList Menu { get; set; }

    public Action OnClose { get; set; }

    public bool DarkenBackground { get; set; }

    public SimpleChoicesMenuContainer()
    {
    }

    public SimpleChoicesMenuContainer(List<IMenuEntry> entries) => this._entries = entries;

    public override void _EnterTree()
    {
      this.SetAnchorsAndMarginsPreset(Godot.Control.LayoutPreset.Wide);
      this.Control = Game.Settings.UiProvider.MakeChoicesFrame();
      this.Menu = (IMenuEntryList) new SimpleChoicesMenuContainer.SimpleChoicesMenu((IMenuEntryContainer) this, this._entries);
      this.Menu.OnBack = (Action) (() => this.OnClose());
      this.Menu.Root();
      if (this.DarkenBackground)
        this.AddChild((Node) UIUtil.CreateDarkenerOverlay());
      this.AddChild((Node) this.Control);
    }

    public override void _Input(InputEvent @event) => this.Menu.HandleInput(@event);

    private class SimpleChoicesMenu : SimpleVerticalMenu
    {
      public SimpleChoicesMenu(IMenuEntryContainer container, List<IMenuEntry> entries)
      {
        this.Container = container;
        this.Entries = entries;
      }
    }
  }
}
