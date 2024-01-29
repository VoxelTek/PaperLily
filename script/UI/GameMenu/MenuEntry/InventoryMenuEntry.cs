using Godot;
using LacieEngine.Core;
using LacieEngine.Items;

namespace LacieEngine.UI
{
	public class InventoryMenuEntry : IInventoryMenuEntry, IMenuEntry
	{
		private const string BackgroundTexture = "res://assets/img/ui/item_icon_bg.png";

		private const string SelectedTexture = "res://assets/img/ui/item_icon_bg_selected.png";

		private static readonly Vector2 IconSize = new Vector2(40f, 40f);

		private static readonly Vector2 QtyLabelPosition = new Vector2(0f, 5f);

		private static readonly Color PrimaryQtyLabelColor = new Color("#BD274D");

		private Label nQtyLabel;

		private InventoryEntry _entry;

		public IMenuEntryList Parent { get; set; }

		public TextureRect BgTextureRect { get; private set; }

		public InventoryMenuEntry(InventoryEntry entry, IMenuEntryList parent)
		{
			Parent = parent;
			_entry = entry;
		}

		public Control DrawEntry()
		{
			MarginContainer container = GDUtil.MakeNode<MarginContainer>("ItemContainer");
			container.SizeFlagsHorizontal = 0;
			container.SizeFlagsVertical = 0;
			BgTextureRect = GDUtil.MakeNode<TextureRect>("ItemBg");
			BgTextureRect.Expand = true;
			BgTextureRect.RectMinSize = IconSize;
			BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg.png");
			container.AddChild(BgTextureRect);
			TextureRect itemIcon = GDUtil.MakeNode<TextureRect>("ItemIcon");
			itemIcon.Expand = true;
			itemIcon.RectMinSize = IconSize;
			itemIcon.Texture = GD.Load<Texture>("res://assets/sprite/common/item/" + (_entry.Item.Icon ?? _entry.Item.Id) + ".png");
			itemIcon.Texture.Flags = 4u;
			itemIcon.Material = GD.Load<Material>("res://resources/material/pixel_perfect.tres");
			container.AddChild(itemIcon);
			if (_entry.Amount > 1)
			{
				nQtyLabel = GDUtil.MakeNode<Label>("ItemQty");
				nQtyLabel.Align = Label.AlignEnum.Center;
				nQtyLabel.Valign = Label.VAlign.Bottom;
				nQtyLabel.Text = _entry.Amount.ToString();
				nQtyLabel.SetFont("res://resources/font/inventory_stack_size.tres");
				nQtyLabel.RectMinSize = IconSize;
				nQtyLabel.RectPosition = QtyLabelPosition;
				Control qtyContainer = GDUtil.MakeNode<Control>("ItemQtyContainer");
				qtyContainer.RectMinSize = IconSize;
				qtyContainer.AddChild(nQtyLabel);
				container.AddChild(qtyContainer);
			}
			return container;
		}

		public void Accept()
		{
			Node interactingNode = Game.Player.GetItemInteractingNode(_entry.Item.Id);
			if (interactingNode != null)
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
				Parent.Back();
				Game.Screen.CloseMenu();
				Game.Events.ExecuteItemObjectMapping(_entry.Item.Id, interactingNode.Name);
			}
			else if (Game.Events.HasItemMapping(_entry.Item.Id))
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
				Parent.Back();
				Game.Screen.CloseMenu();
				Game.Events.ExecuteItemMapping(_entry.Item.Id);
			}
			else
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
			}
		}

		public void Deselect()
		{
			BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg.png");
			if (nQtyLabel != null)
			{
				nQtyLabel.SetFont("res://resources/font/inventory_stack_size.tres");
				nQtyLabel.SetFontColor(Colors.White);
				nQtyLabel.SetFontOutlineColor(PrimaryQtyLabelColor);
			}
		}

		public void Select()
		{
			BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg_selected.png");
			if (nQtyLabel != null)
			{
				nQtyLabel.SetFont("res://resources/font/inventory_stack_size.tres");
				nQtyLabel.SetFontColor(PrimaryQtyLabelColor);
				nQtyLabel.SetFontOutlineColor(Colors.White);
			}
		}
	}
}
