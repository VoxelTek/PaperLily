// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.UIUtil
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
  public static class UIUtil
  {
    public static readonly Color Transparent = new Color(1f, 1f, 1f, 0.0f);
    public static readonly Color SelectionColor = new Color(0.93f, 0.87f, 0.87f, 0.2f);
    public static readonly Color ScreenDarkenerColor = new Color(0.0f, 0.0f, 0.0f, 0.7f);
    public static readonly Color MenuBgColor = new Color("#11080D");
    public const string BulletPoint = "[img=11]res://assets/img/ui/bullet_point.png[/img]";

    public static ColorRect CreateOverlay(Color color)
    {
      ColorRect overlay = GDUtil.MakeNode<ColorRect>("Overlay");
      overlay.Color = color;
      overlay.RectMinSize = Game.Settings.BaseResolution;
      return overlay;
    }

    public static ColorRect CreateDarkenerOverlay()
    {
      return UIUtil.CreateOverlay(UIUtil.ScreenDarkenerColor);
    }

    public static Frame CreateInfoBox(string text)
    {
      InfoBoxFrame infoBox = new InfoBoxFrame();
      Label label = GDUtil.MakeNode<Label>("Info");
      label.Text = text;
      label.Align = Label.AlignEnum.Center;
      label.SetDefaultFontAndColor();
      infoBox.AddToFrame((Node) label);
      return (Frame) infoBox;
    }

    public static Frame CreateInfoBoxWithImages(string bbcode)
    {
      InfoBoxFrame infoBoxWithImages = new InfoBoxFrame();
      LabelWithImages child = GDUtil.MakeNode<LabelWithImages>("Text");
      child.Text = bbcode;
      infoBoxWithImages.AddToFrame((Node) child);
      return (Frame) infoBoxWithImages;
    }

    public static Frame CreateInfoBoxWithInputIcons(string captionKey, params string[] inputs)
    {
      InfoBoxFrame boxWithInputIcons = new InfoBoxFrame();
      LabelWithInputIcons child = GDUtil.MakeNode<LabelWithInputIcons>("Text");
      child.SetContent(captionKey, inputs);
      boxWithInputIcons.AddToFrame((Node) child);
      return (Frame) boxWithInputIcons;
    }

    public static Frame CreateInfoBoxWithIcon(string text, string iconPath)
    {
      InfoBoxFrame infoBoxWithIcon = new InfoBoxFrame();
      MarginContainer marginContainer = new MarginContainer();
      marginContainer.SetContainerMarginRight(5);
      HBoxContainer box = new HBoxContainer();
      box.SetSeparation(3);
      TextureRect textureRect = GDUtil.MakeNode<TextureRect>("Icon");
      textureRect.RectMinSize = new Vector2(40f, 40f);
      textureRect.Expand = true;
      textureRect.Texture = GD.Load("res://assets/sprite/common/item/" + iconPath + ".png") as Texture;
      Label label = GDUtil.MakeNode<Label>("Info");
      label.Text = text;
      label.Align = Label.AlignEnum.Center;
      label.SetDefaultFontAndColor();
      box.AddChild((Node) textureRect);
      box.AddChild((Node) label);
      marginContainer.AddChild((Node) box);
      infoBoxWithIcon.AddToFrame((Node) marginContainer);
      return (Frame) infoBoxWithIcon;
    }

    public static Control CreateOptionList(List<string> options)
    {
      VBoxContainer box = GDUtil.MakeNode<VBoxContainer>("List");
      box.SetSeparation(0);
      int num = 0;
      foreach (string option in options)
      {
        MarginContainer marginContainer = GDUtil.MakeNode<MarginContainer>("Option" + num++.ToString());
        ColorRect colorRect = GDUtil.MakeNode<ColorRect>("Bg");
        colorRect.Color = UIUtil.Transparent;
        MarginContainer container = new MarginContainer();
        container.SetContainerMarginLeft(10);
        container.SetContainerMarginRight(10);
        Label label = GDUtil.MakeNode<Label>("Label");
        label.Text = option;
        label.Align = Label.AlignEnum.Center;
        label.SetDefaultFontAndColor();
        container.AddChild((Node) label);
        marginContainer.AddChild((Node) colorRect);
        marginContainer.AddChild((Node) container);
        box.AddChild((Node) marginContainer);
      }
      return (Control) box;
    }

    public static Control CreateVerticalEntryList(
      List<IMenuEntry> entries,
      out List<ColorRect> selectBgs,
      int separation = 5)
    {
      VBoxContainer box = GDUtil.MakeNode<VBoxContainer>("EntryList");
      box.SetSeparation(separation);
      selectBgs = new List<ColorRect>();
      int num = 0;
      foreach (IMenuEntry entry in entries)
      {
        MarginContainer marginContainer = GDUtil.MakeNode<MarginContainer>("Option" + num++.ToString());
        ColorRect colorRect = GDUtil.MakeNode<ColorRect>("Bg");
        colorRect.Color = UIUtil.Transparent;
        selectBgs.Add(colorRect);
        Control control = entry.DrawEntry();
        marginContainer.AddChild((Node) colorRect);
        marginContainer.AddChild((Node) control);
        box.AddChild((Node) marginContainer);
      }
      return (Control) box;
    }

    public static Control CreateSimpleEntry(string caption)
    {
      MarginContainer simpleEntry = GDUtil.MakeNode<MarginContainer>("LabelContainer");
      Label label = GDUtil.MakeNode<Label>("Label");
      label.Text = caption;
      label.Align = Label.AlignEnum.Center;
      label.SetAnchorsPreset(Control.LayoutPreset.Wide);
      label.SetDefaultFontAndColor();
      simpleEntry.AddChild((Node) label);
      return (Control) simpleEntry;
    }

    public static void HighlightItem(Control list, int selection)
    {
      if (list.Name == "List")
      {
        foreach (Node child in list.GetChildren())
          ((ColorRect) child.GetNode((NodePath) "Bg")).Color = UIUtil.Transparent;
        if (selection == -1)
          return;
        ((ColorRect) list.GetNode((NodePath) ("Option" + selection.ToString() + "/Bg"))).Color = UIUtil.SelectionColor;
      }
      else
      {
        if (!(list.Name == "LargeList"))
          return;
        foreach (Node child in list.GetChildren())
        {
          ((ColorRect) child.GetNode((NodePath) "Bg")).Color = UIUtil.Transparent;
          ((CanvasItem) child.GetNode((NodePath) "Border")).Visible = false;
        }
        if (selection == -1)
          return;
        ((ColorRect) list.GetNode((NodePath) ("Option" + selection.ToString() + "/Bg"))).Color = UIUtil.SelectionColor;
        ((CanvasItem) list.GetNode((NodePath) ("Option" + selection.ToString() + "/Border"))).Visible = true;
      }
    }

    public static void HandleVerticalNavigationInput(IMenuEntryList menu, InputEvent @event)
    {
      switch (Inputs.Handle(@event, Inputs.Processor.Menu, Inputs.AllUi, true))
      {
        case "input_up":
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
          if (--menu.Selection <= -1)
            menu.Selection = menu.Entries.Count - 1;
          menu.HighlightSelection();
          break;
        case "input_down":
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
          if (++menu.Selection >= menu.Entries.Count)
            menu.Selection = 0;
          menu.HighlightSelection();
          break;
        case "input_left":
          menu.Entries[menu.Selection].Left();
          break;
        case "input_right":
          menu.Entries[menu.Selection].Right();
          break;
        case "input_action":
          menu.Entries[menu.Selection].Accept();
          break;
        case "input_cancel":
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
          menu.Back();
          break;
      }
    }

    public static float GetScaleFactor() => Game.Root.Size.x / Game.Root.GetSizeOverride().x;

    public static float GetScaled(float pixels) => pixels;

    public static int GetScaled(int pixels) => pixels;

    public static Vector2 GetScaled(Vector2 size) => size;
  }
}
