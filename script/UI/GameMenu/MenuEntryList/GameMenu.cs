// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.GameMenu
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
  public class GameMenu : SimpleVerticalMenu
  {
    private Control _submenuContainer;
    private Control _openContainer;

    public GameMenu(Control submenuContainer, IMenuEntryContainer container)
    {
      this.Container = container;
      this.Entries = new List<IMenuEntry>();
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.inventory", (Action) (() => this.Inventory()), (IMenuEntryList) this));
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.objectives", (Action) (() => this.Objectives()), (IMenuEntryList) this));
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.settings", (Action) (() => this.Settings()), (IMenuEntryList) this));
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.load", (Action) (() => this.Load()), (IMenuEntryList) this));
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.quit", (Action) (() => this.Quit()), (IMenuEntryList) this));
      this._submenuContainer = submenuContainer;
    }

    public void Settings()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      IngameSettingsMenuContainer settingsMenuContainer = new IngameSettingsMenuContainer();
      settingsMenuContainer.OnClose = (Action) (() => this.Close());
      this._submenuContainer.AddChild((Node) settingsMenuContainer);
      this._openContainer = (Control) settingsMenuContainer;
    }

    public void Load()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      LoadMenuContainer loadMenuContainer = new LoadMenuContainer();
      loadMenuContainer.OnClose = (Action) (() => this.Close());
      this._submenuContainer.AddChild((Node) loadMenuContainer);
      this._openContainer = (Control) loadMenuContainer;
    }

    public void Inventory()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      InventoryMenuContainer inventoryMenuContainer = new InventoryMenuContainer();
      inventoryMenuContainer.OnClose = (Action) (() => this.Close());
      this._submenuContainer.AddChild((Node) inventoryMenuContainer);
      this._openContainer = (Control) inventoryMenuContainer;
    }

    public void Objectives()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      ObjectivesMenuContainer objectivesMenuContainer = new ObjectivesMenuContainer();
      objectivesMenuContainer.OnClose = (Action) (() => this.Close());
      this._submenuContainer.AddChild((Node) objectivesMenuContainer);
      this._openContainer = (Control) objectivesMenuContainer;
    }

    public void Quit()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      QuitMenuContainer quitMenuContainer = new QuitMenuContainer();
      quitMenuContainer.OnClose = (Action) (() => this.Close());
      Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) quitMenuContainer);
      this._openContainer = (Control) quitMenuContainer;
    }

    public void Close()
    {
      this._openContainer.Delete();
      this._openContainer = (Control) null;
    }
  }
}
