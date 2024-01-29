using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class SaveLoadSlotEmptyEntry : SaveLoadSlotMenuEntry
	{
		private TextureRect BgTextureRect;

		public SaveLoadSlotEmptyEntry(SaveLoadMenu.Mode mode, SaveFileInformation saveInfo, IMenuEntryList parentMenu)
			: base(mode, saveInfo, parentMenu)
		{
		}

		public override Control DrawEntry()
		{
			Control row = new Control();
			row.RectMinSize = SaveLoadSlotMenuEntry.SaveSlotSize;
			BgTextureRect = GDUtil.MakeNode<TextureRect>("SaveSlotEmptyBg");
			BgTextureRect.Expand = true;
			BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot_empty.png");
			BgTextureRect.RectMinSize = SaveLoadSlotMenuEntry.SaveSlotSize;
			Label label = GDUtil.MakeNode<Label>("SlotName");
			label.Text = Game.Language.GetFormattedCaption("system.menu.save.slot", saveInfo.SlotNum);
			label.Align = Label.AlignEnum.Center;
			label.Valign = Label.VAlign.Center;
			label.SetAnchorsPreset(Control.LayoutPreset.Wide);
			label.SetDefaultFontAndColor();
			row.AddChild(BgTextureRect);
			row.AddChild(label);
			return row;
		}

		public override void Deselect()
		{
			BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot_empty.png");
		}

		public override void Select()
		{
			BgTextureRect.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot_empty_selected.png");
		}
	}
}
