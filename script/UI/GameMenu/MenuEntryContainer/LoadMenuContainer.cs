// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.LoadMenuContainer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class LoadMenuContainer : Godot.Control, IMenuEntryContainer
  {
    private static readonly Vector2 MainPanelSize = new Vector2(484f, 200f);

    public Frame Control { get; private set; }

    public IMenuEntryList Menu { get; set; }

    public Action OnClose { get; set; }

    public override void _EnterTree()
    {
      this.SetAnchorsAndMarginsPreset(Godot.Control.LayoutPreset.Wide);
      TitledMenuFrame titledMenuFrame = new TitledMenuFrame();
      titledMenuFrame.MinimumSize = LoadMenuContainer.MainPanelSize;
      titledMenuFrame.TitleText = "system.menu.load.game";
      titledMenuFrame.DividerTexture = "res://assets/img/ui/divider_lg.png";
      this.Control = (Frame) titledMenuFrame;
      this.Menu = (IMenuEntryList) new SaveLoadMenu(SaveLoadMenu.Mode.Load, (IMenuEntryList) null, (IMenuEntryContainer) this);
      this.Menu.OnBack = (Action) (() => this.OnClose());
      this.Menu.Root();
      this.AddChild((Node) this.Control);
    }

    public override void _Input(InputEvent @event) => this.Menu.HandleInput(@event);
  }
}
