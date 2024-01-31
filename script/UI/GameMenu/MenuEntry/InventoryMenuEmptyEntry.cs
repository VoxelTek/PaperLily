// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.InventoryMenuEmptyEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class InventoryMenuEmptyEntry : IInventoryMenuEntry, IMenuEntry
  {
    private const string NoItemTexture = "res://assets/img/ui/item_icon_bg_empty.png";
    private const string SelectedTexture = "res://assets/img/ui/item_icon_bg_empty_selected.png";
    private static readonly Vector2 IconSize = new Vector2(40f, 40f);

    public IMenuEntryList Parent { get; set; }

    public TextureRect BgTextureRect { get; private set; }

    public InventoryMenuEmptyEntry(IMenuEntryList parent) => this.Parent = parent;

    public Control DrawEntry()
    {
      MarginContainer marginContainer = GDUtil.MakeNode<MarginContainer>("ItemContainer");
      marginContainer.SizeFlagsHorizontal = 0;
      marginContainer.SizeFlagsVertical = 0;
      this.BgTextureRect = GDUtil.MakeNode<TextureRect>("ItemBg");
      this.BgTextureRect.Expand = true;
      this.BgTextureRect.RectMinSize = InventoryMenuEmptyEntry.IconSize;
      this.BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg_empty.png");
      marginContainer.AddChild((Node) this.BgTextureRect);
      return (Control) marginContainer;
    }

    public void Deselect()
    {
      this.BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg_empty.png");
    }

    public void Select()
    {
      this.BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg_empty_selected.png");
    }
  }
}
