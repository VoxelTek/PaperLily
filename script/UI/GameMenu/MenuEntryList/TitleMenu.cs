// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.TitleMenu
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
  public class TitleMenu : SimpleVerticalMenu
  {
    private TitleSettingsMenuContainer nSettingsContainer;
    private TitleLoadMenuContainer nLoadContainer;

    public TitleMenu(IMenuEntryContainer container) => this.Container = container;

    public void Continue()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      this.Container.Control.Visible = false;
      this.nLoadContainer = GDUtil.MakeNode<TitleLoadMenuContainer>("LoadMenuContainer");
      this.nLoadContainer.OnClose = (Action) (() => this.CloseLoad());
      Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) this.nLoadContainer);
      this.nLoadContainer.Menu.ResetSelection();
    }

    public void NewGame()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      Game.Core.StartGameFromEvent(Game.Settings.NewGameEvent);
    }

    public void DebugRoom()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      Game.Core.StartGameFromRoom(Game.Settings.DebugRoom, (string) null, Vector2.Zero, "down");
    }

    public void Settings()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      this.Container.Control.Visible = false;
      this.nSettingsContainer = GDUtil.MakeNode<TitleSettingsMenuContainer>("SettingsContainer");
      this.nSettingsContainer.OnClose = (Action) (() => this.CloseSettings());
      Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) this.nSettingsContainer);
    }

    public void CloseSettings()
    {
      this.nSettingsContainer.Delete();
      this.nSettingsContainer = (TitleSettingsMenuContainer) null;
      this.Container.Control.Visible = true;
      this.Root();
      if (!(this.Container is TitleScreen container))
        return;
      container.UpdateExtraInfo();
    }

    public void CloseLoad()
    {
      this.nLoadContainer.Delete();
      this.nLoadContainer = (TitleLoadMenuContainer) null;
      this.Container.Control.Visible = true;
      this.Root();
    }

    public void HomePage()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      int num = (int) OS.ShellOpen(Game.Settings.WebsiteLink);
    }

    public void TranslatorPage()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      int num = (int) OS.ShellOpen(Game.Language.GetTranslatorWebsite());
    }

    public void Quit()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
      Game.InputProcessor = Inputs.Processor.None;
      Game.Tree.Quit();
    }

    public override Control DrawContent()
    {
      this.Entries = new List<IMenuEntry>();
      if (GameState.AnySaveExists())
        this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.continue", (Action) (() => this.Continue()), (IMenuEntryList) this));
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.newgame", (Action) (() => this.NewGame()), (IMenuEntryList) this));
      if (OS.IsDebugBuild())
        this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.debugroom", (Action) (() => this.DebugRoom()), (IMenuEntryList) this));
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.settings", (Action) (() => this.Settings()), (IMenuEntryList) this));
      if (Game.Settings.WebsiteEnabled)
        this.Entries.Add((IMenuEntry) new SimpleMenuEntry(Game.Settings.WebsiteCaption, (Action) (() => this.HomePage()), (IMenuEntryList) this));
      if (!Game.Language.GetTranslatorWebsite().IsNullOrEmpty())
        this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.website.translator", (Action) (() => this.TranslatorPage()), (IMenuEntryList) this));
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.quit", (Action) (() => this.Quit()), (IMenuEntryList) this));
      return UIUtil.CreateVerticalEntryList(this.Entries, out this._selectBgs, 2);
    }
  }
}
