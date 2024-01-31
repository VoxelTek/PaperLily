// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.MessageContainer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class MessageContainer : CenterContainer, IMenuEntryContainer
  {
    private static readonly Vector2 MainPanelSize = new Vector2(150f, 0.0f);

    public string Text { get; set; }

    public Frame Control { get; private set; }

    public IMenuEntryList Menu { get; set; }

    public Action OnClose { get; set; }

    public override void _EnterTree()
    {
      ColorRect darkenerOverlay = UIUtil.CreateDarkenerOverlay();
      this.SetAnchorsAndMarginsPreset(Godot.Control.LayoutPreset.Wide);
      this.Control = (Frame) new MenuFrame();
      this.Control.MinimumSize = MessageContainer.MainPanelSize;
      VBoxContainer vboxContainer = GDUtil.MakeNode<VBoxContainer>(nameof (MessageContainer));
      vboxContainer.SetSeparation(10);
      Label label = GDUtil.MakeNode<Label>("MessageText");
      label.Text = this.Text;
      label.SetDefaultFontAndColor();
      vboxContainer.AddChild((Node) label);
      this.Menu = (IMenuEntryList) new MessageMenu((IMenuEntryContainer) this);
      this.Menu.OnBack = (Action) (() => this.OnClose());
      vboxContainer.AddChild((Node) this.Menu.DrawContent());
      this.Menu.Selection = 0;
      this.Menu.HighlightSelection();
      this.Control.AddToFrame((Node) vboxContainer);
      this.AddChild((Node) darkenerOverlay);
      this.AddChild((Node) this.Control);
    }

    public override void _Input(InputEvent @event) => this.Menu.HandleInput(@event);
  }
}
