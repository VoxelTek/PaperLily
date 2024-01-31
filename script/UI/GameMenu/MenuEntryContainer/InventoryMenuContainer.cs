// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.InventoryMenuContainer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class InventoryMenuContainer : Godot.Control, IMenuEntryContainer
  {
    private static readonly Vector2 MainPanelSize = new Vector2(300f, 200f);
    private static readonly Vector2 DescriptionPanelSize = new Vector2(182f, 200f);

    public Frame Control { get; private set; }

    public IMenuEntryList Menu { get; set; }

    public Action OnClose { get; set; }

    public override void _EnterTree()
    {
      this.Name = nameof (InventoryMenuContainer);
      TitledMenuFrame frame = new TitledMenuFrame();
      frame.MinimumSize = InventoryMenuContainer.DescriptionPanelSize;
      frame.DividerTexture = "res://assets/img/ui/divider_sm.png";
      frame.DecorBgTexture = "res://assets/img/ui/menu_bg_decor.png";
      frame.DecorBgAlignment = Godot.Control.LayoutPreset.BottomRight;
      RichTextLabel richTextLabel = GDUtil.MakeNode<RichTextLabel>("ItemDescription");
      richTextLabel.ScrollActive = false;
      richTextLabel.BbcodeEnabled = true;
      richTextLabel.SetDefaultFontAndColor();
      frame.AddToFrame((Node) richTextLabel);
      TitledMenuFrame titledMenuFrame = new TitledMenuFrame();
      titledMenuFrame.MinimumSize = InventoryMenuContainer.MainPanelSize;
      titledMenuFrame.TitleText = "system.menu.inventory";
      titledMenuFrame.DividerTexture = "res://assets/img/ui/divider_md.png";
      this.Control = (Frame) titledMenuFrame;
      this.Menu = (IMenuEntryList) new InventoryMenu((IMenuEntryContainer) this, frame, richTextLabel);
      this.Menu.OnBack = (Action) (() => this.OnClose());
      this.Menu.Root();
      HBoxContainer box = GDUtil.MakeNode<HBoxContainer>("InventoryTwoPanelContainer");
      box.SetSeparation(2);
      box.AddChild((Node) this.Control);
      box.AddChild((Node) frame);
      this.AddChild((Node) box);
    }

    public override void _Input(InputEvent @event) => this.Menu.HandleInput(@event);
  }
}
