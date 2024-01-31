// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.TitleSettingsMenuContainer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class TitleSettingsMenuContainer : CenterContainer, IMenuEntryContainer
  {
    private static readonly Vector2 FrameSize = new Vector2(300f, 0.0f);

    public Frame Control { get; private set; }

    public IMenuEntryList Menu { get; set; }

    public Action OnClose { get; set; }

    public override void _EnterTree()
    {
      this.SetAnchorsAndMarginsPreset(Godot.Control.LayoutPreset.Wide);
      this.Control = Game.Settings.UiProvider.MakeDefaultFrame();
      this.Control.MinimumSize = TitleSettingsMenuContainer.FrameSize;
      this.Menu = (IMenuEntryList) new TitleSettingsMenu((IMenuEntryList) null, (IMenuEntryContainer) this);
      this.Menu.OnBack = (Action) (() => this.OnClose());
      this.Menu.Root();
      this.AddChild((Node) UIUtil.CreateDarkenerOverlay());
      this.AddChild((Node) this.Control);
    }

    public override void _Input(InputEvent @event) => this.Menu.HandleInput(@event);
  }
}
