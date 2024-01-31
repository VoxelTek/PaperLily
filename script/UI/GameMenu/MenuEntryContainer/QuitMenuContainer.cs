// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.QuitMenuContainer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class QuitMenuContainer : CenterContainer, IMenuEntryContainer
  {
    private static readonly Vector2 MainPanelSize = new Vector2(300f, 0.0f);

    public Frame Control { get; private set; }

    public IMenuEntryList Menu { get; set; }

    public Action OnClose { get; set; }

    public override void _EnterTree()
    {
      ColorRect darkenerOverlay = UIUtil.CreateDarkenerOverlay();
      this.SetAnchorsAndMarginsPreset(Godot.Control.LayoutPreset.Wide);
      this.Control = (Frame) new MenuFrame();
      this.Control.MinimumSize = QuitMenuContainer.MainPanelSize;
      this.Menu = (IMenuEntryList) new QuitMenu((IMenuEntryContainer) this);
      this.Menu.OnBack = (Action) (() => this.OnClose());
      this.Menu.Root();
      this.AddChild((Node) darkenerOverlay);
      this.AddChild((Node) this.Control);
    }

    public override void _Input(InputEvent @event) => this.Menu.HandleInput(@event);
  }
}
