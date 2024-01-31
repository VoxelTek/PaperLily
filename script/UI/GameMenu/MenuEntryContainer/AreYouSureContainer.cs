// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.AreYouSureContainer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class AreYouSureContainer : CenterContainer, IMenuEntryContainer
  {
    private static readonly Vector2 MainPanelSize = new Vector2(150f, 0.0f);

    public string Text { get; set; }

    public Action OnYes { get; set; }

    public Frame Control { get; private set; }

    public IMenuEntryList Menu { get; set; }

    public Action OnClose { get; set; }

    public override void _EnterTree()
    {
      ColorRect darkenerOverlay = UIUtil.CreateDarkenerOverlay();
      this.SetAnchorsAndMarginsPreset(Godot.Control.LayoutPreset.Wide);
      this.Control = (Frame) new MenuFrame();
      this.Control.MinimumSize = AreYouSureContainer.MainPanelSize;
      VBoxContainer vboxContainer = GDUtil.MakeNode<VBoxContainer>(nameof (AreYouSureContainer));
      vboxContainer.SetSeparation(10);
      Label label = GDUtil.MakeNode<Label>("AreYouSureText");
      label.Text = this.Text;
      label.SetDefaultFontAndColor();
      label.Align = Label.AlignEnum.Center;
      vboxContainer.AddChild((Node) label);
      this.Menu = (IMenuEntryList) new AreYouSureMenu(this.OnYes, (IMenuEntryContainer) this);
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
