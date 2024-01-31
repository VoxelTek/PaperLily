// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.AreYouSureMenu
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
  public class AreYouSureMenu : SimpleVerticalMenu
  {
    private Action _yesAction;

    public AreYouSureMenu(Action yesAction, IMenuEntryContainer container)
    {
      this.Container = container;
      this._yesAction = yesAction;
      this.Entries = new List<IMenuEntry>();
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.common.yes", (Action) (() => this.Yes()), (IMenuEntryList) this));
      this.Entries.Add((IMenuEntry) new SimpleMenuEntry("system.common.no", (Action) (() => this.No()), (IMenuEntryList) this));
    }

    private void Yes()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
      this._yesAction();
      this.Back();
    }

    private void No()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
      this.Back();
    }
  }
}
