using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class InventoryMenuContainer : Control, IMenuEntryContainer
	{
		private static readonly Vector2 MainPanelSize = new Vector2(300f, 200f);

		private static readonly Vector2 DescriptionPanelSize = new Vector2(182f, 200f);

		public Frame Control { get; private set; }

		public IMenuEntryList Menu { get; set; }

		public Action OnClose { get; set; }

		public override void _EnterTree()
		{
			base.Name = "InventoryMenuContainer";
			TitledMenuFrame descriptionFrame = new TitledMenuFrame();
			descriptionFrame.MinimumSize = DescriptionPanelSize;
			descriptionFrame.DividerTexture = "res://assets/img/ui/divider_sm.png";
			descriptionFrame.DecorBgTexture = "res://assets/img/ui/menu_bg_decor.png";
			descriptionFrame.DecorBgAlignment = LayoutPreset.BottomRight;
			RichTextLabel descriptionLabel = GDUtil.MakeNode<RichTextLabel>("ItemDescription");
			descriptionLabel.ScrollActive = false;
			descriptionLabel.BbcodeEnabled = true;
			descriptionLabel.SetDefaultFontAndColor();
			descriptionFrame.AddToFrame(descriptionLabel);
			TitledMenuFrame inventoryFrame = new TitledMenuFrame();
			inventoryFrame.MinimumSize = MainPanelSize;
			inventoryFrame.TitleText = "system.menu.inventory";
			inventoryFrame.DividerTexture = "res://assets/img/ui/divider_md.png";
			Control = inventoryFrame;
			Menu = new InventoryMenu(this, descriptionFrame, descriptionLabel);
			Menu.OnBack = delegate
			{
				OnClose();
			};
			Menu.Root();
			HBoxContainer hBox = GDUtil.MakeNode<HBoxContainer>("InventoryTwoPanelContainer");
			hBox.SetSeparation(2);
			hBox.AddChild(Control);
			hBox.AddChild(descriptionFrame);
			AddChild(hBox);
		}

		public override void _Input(InputEvent @event)
		{
			Menu.HandleInput(@event);
		}
	}
}
