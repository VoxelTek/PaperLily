// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.InventoryMenuEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Items;

#nullable disable
namespace LacieEngine.UI
{
  public class InventoryMenuEntry : IInventoryMenuEntry, IMenuEntry
  {
    private const string BackgroundTexture = "res://assets/img/ui/item_icon_bg.png";
    private const string SelectedTexture = "res://assets/img/ui/item_icon_bg_selected.png";
    private static readonly Vector2 IconSize = new Vector2(40f, 40f);
    private static readonly Vector2 QtyLabelPosition = new Vector2(0.0f, 5f);
    private static readonly Color PrimaryQtyLabelColor = new Color("#BD274D");
    private Label nQtyLabel;
    private InventoryEntry _entry;

    public IMenuEntryList Parent { get; set; }

    public TextureRect BgTextureRect { get; private set; }

    public InventoryMenuEntry(InventoryEntry entry, IMenuEntryList parent)
    {
      this.Parent = parent;
      this._entry = entry;
    }

    public Control DrawEntry()
    {
      MarginContainer marginContainer = GDUtil.MakeNode<MarginContainer>("ItemContainer");
      marginContainer.SizeFlagsHorizontal = 0;
      marginContainer.SizeFlagsVertical = 0;
      this.BgTextureRect = GDUtil.MakeNode<TextureRect>("ItemBg");
      this.BgTextureRect.Expand = true;
      this.BgTextureRect.RectMinSize = InventoryMenuEntry.IconSize;
      this.BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg.png");
      marginContainer.AddChild((Node) this.BgTextureRect);
      TextureRect textureRect = GDUtil.MakeNode<TextureRect>("ItemIcon");
      textureRect.Expand = true;
      textureRect.RectMinSize = InventoryMenuEntry.IconSize;
      textureRect.Texture = GD.Load<Texture>("res://assets/sprite/common/item/" + (this._entry.Item.Icon ?? this._entry.Item.Id) + ".png");
      textureRect.Texture.Flags = 4U;
      textureRect.Material = GD.Load<Material>("res://resources/material/pixel_perfect.tres");
      marginContainer.AddChild((Node) textureRect);
      if (this._entry.Amount > 1)
      {
        this.nQtyLabel = GDUtil.MakeNode<Label>("ItemQty");
        this.nQtyLabel.Align = Label.AlignEnum.Center;
        this.nQtyLabel.Valign = Label.VAlign.Bottom;
        this.nQtyLabel.Text = this._entry.Amount.ToString();
        this.nQtyLabel.SetFont("res://resources/font/inventory_stack_size.tres");
        this.nQtyLabel.RectMinSize = InventoryMenuEntry.IconSize;
        this.nQtyLabel.RectPosition = InventoryMenuEntry.QtyLabelPosition;
        Control control = GDUtil.MakeNode<Control>("ItemQtyContainer");
        control.RectMinSize = InventoryMenuEntry.IconSize;
        control.AddChild((Node) this.nQtyLabel);
        marginContainer.AddChild((Node) control);
      }
      return (Control) marginContainer;
    }

    public void Accept()
    {
      Node itemInteractingNode = (Node) Game.Player.GetItemInteractingNode(this._entry.Item.Id);
      if (itemInteractingNode != null)
      {
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
        this.Parent.Back();
        Game.Screen.CloseMenu();
        Game.Events.ExecuteItemObjectMapping(this._entry.Item.Id, itemInteractingNode.Name);
      }
      else if (Game.Events.HasItemMapping(this._entry.Item.Id))
      {
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
        this.Parent.Back();
        Game.Screen.CloseMenu();
        Game.Events.ExecuteItemMapping(this._entry.Item.Id);
      }
      else
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
    }

    public void Deselect()
    {
      this.BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg.png");
      if (this.nQtyLabel == null)
        return;
      this.nQtyLabel.SetFont("res://resources/font/inventory_stack_size.tres");
      this.nQtyLabel.SetFontColor(Godot.Colors.White);
      this.nQtyLabel.SetFontOutlineColor(InventoryMenuEntry.PrimaryQtyLabelColor);
    }

    public void Select()
    {
      this.BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg_selected.png");
      if (this.nQtyLabel == null)
        return;
      this.nQtyLabel.SetFont("res://resources/font/inventory_stack_size.tres");
      this.nQtyLabel.SetFontColor(InventoryMenuEntry.PrimaryQtyLabelColor);
      this.nQtyLabel.SetFontOutlineColor(Godot.Colors.White);
    }
  }
}
