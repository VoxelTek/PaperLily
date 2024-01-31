// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.QuitMenu
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
  public class QuitMenu : SimpleVerticalMenu
  {
    public QuitMenu(IMenuEntryContainer container)
    {
      this.Container = container;
      this.Entries = new List<IMenuEntry>();
      if (OS.IsDebugBuild())
        this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.quit.debugroom", (Action) (() => this.GoToDebugRoom()), (IMenuEntryList) this));
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.quit.mainmenu", (Action) (() => this.QuitToTitle()), (IMenuEntryList) this));
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.menu.quit.desktop", (Action) (() => this.QuitToDesktop()), (IMenuEntryList) this));
    }

    private void GoToDebugRoom()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      AreYouSureContainer areYouSure = new AreYouSureContainer()
      {
        OnYes = (Action) (() => Game.Core.StartGameFromRoom(Game.Settings.DebugRoom, (string) null, Vector2.Zero, "down"))
      };
      areYouSure.OnClose = (Action) (() => areYouSure.Delete());
      areYouSure.Text = "system.menu.quit.areyousure";
      Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) areYouSure);
    }

    private void QuitToTitle()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      AreYouSureContainer areYouSure = new AreYouSureContainer()
      {
        OnYes = (Action) (() => Game.Core.SwitchToScreen(Game.Settings.SceneProvider.TitleScreen))
      };
      areYouSure.OnClose = (Action) (() => areYouSure.Delete());
      areYouSure.Text = "system.menu.quit.areyousure";
      Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) areYouSure);
    }

    private void QuitToDesktop()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      AreYouSureContainer areYouSure = new AreYouSureContainer()
      {
        OnYes = (Action) (() => Game.Tree.Quit())
      };
      areYouSure.OnClose = (Action) (() => areYouSure.Delete());
      areYouSure.Text = "system.menu.quit.areyousure";
      Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) areYouSure);
    }
  }
}
