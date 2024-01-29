using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class InventoryMenuEmptyEntry : IInventoryMenuEntry, IMenuEntry
	{
		private const string NoItemTexture = "res://assets/img/ui/item_icon_bg_empty.png";

		private const string SelectedTexture = "res://assets/img/ui/item_icon_bg_empty_selected.png";

		private static readonly Vector2 IconSize = new Vector2(40f, 40f);

		public IMenuEntryList Parent { get; set; }

		public TextureRect BgTextureRect { get; private set; }

		public InventoryMenuEmptyEntry(IMenuEntryList parent)
		{
			Parent = parent;
		}

		public Control DrawEntry()
		{
			MarginContainer marginContainer = GDUtil.MakeNode<MarginContainer>("ItemContainer");
			marginContainer.SizeFlagsHorizontal = 0;
			marginContainer.SizeFlagsVertical = 0;
			BgTextureRect = GDUtil.MakeNode<TextureRect>("ItemBg");
			BgTextureRect.Expand = true;
			BgTextureRect.RectMinSize = IconSize;
			BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg_empty.png");
			marginContainer.AddChild(BgTextureRect);
			return marginContainer;
		}

		public void Deselect()
		{
			BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg_empty.png");
		}

		public void Select()
		{
			BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/item_icon_bg_empty_selected.png");
		}
	}
}
