// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.TitleLanguageMenuContainer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class TitleLanguageMenuContainer : CenterContainer, IMenuEntryContainer
  {
    private static readonly Vector2 FrameSize = new Vector2(300f, 0.0f);
    private Godot.Control nInfoBox;

    public Frame Control { get; private set; }

    public IMenuEntryList Menu { get; set; }

    public Action OnClose { get; set; }

    public override void _EnterTree()
    {
      this.SetAnchorsAndMarginsPreset(Godot.Control.LayoutPreset.Wide);
      TitledMenuFrame titledMenuFrame = new TitledMenuFrame();
      titledMenuFrame.MinimumSize = TitleLanguageMenuContainer.FrameSize;
      titledMenuFrame.TitleText = "system.settings.game.language.select";
      titledMenuFrame.DividerTexture = "res://assets/img/ui/divider_md.png";
      this.Control = (Frame) titledMenuFrame;
      this.Menu = (IMenuEntryList) new StartupLanguageMenu(false, (IMenuEntryList) null, (IMenuEntryContainer) this);
      this.Menu.OnBack = (Action) (() => this.Close());
      this.Menu.Root();
      this.AddChild((Node) UIUtil.CreateDarkenerOverlay());
      this.AddChild((Node) this.Control);
      this.UpdateInfoBox();
    }

    public override void _Input(InputEvent @event) => this.Menu.HandleInput(@event);

    private void Close()
    {
      this.nInfoBox.Delete();
      this.nInfoBox = (Godot.Control) null;
      this.OnClose();
    }

    private void UpdateInfoBox()
    {
      if (this.nInfoBox.IsValid())
        this.nInfoBox.Delete();
      this.nInfoBox = (Godot.Control) UIUtil.CreateInfoBoxWithInputIcons("system.menu.startup.settings.info", "input_up", "input_down", "input_action");
      this.nInfoBox.SetAnchorsPreset(Godot.Control.LayoutPreset.BottomRight);
      this.nInfoBox.GrowHorizontal = Godot.Control.GrowDirection.Begin;
      this.nInfoBox.GrowVertical = Godot.Control.GrowDirection.Begin;
      Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) this.nInfoBox);
    }
  }
}
