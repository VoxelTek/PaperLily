using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class SaveLoadSlotUsedEntry : SaveLoadSlotMenuEntry
	{
		private static readonly Vector2 SaveDecorPos = new Vector2(4f, 4f);

		private static readonly Vector2 SaveLabelPos = new Vector2(4f, 1f);

		private static readonly Vector2 SaveRoomLabelPos = new Vector2(50f, 2f);

		private static readonly Vector2 SavePlaytimeLabelPos = new Vector2(50f, 18f);

		private static readonly Vector2 LocationImgPos = new Vector2(290f, 2f);

		private static readonly Color PrimaryColor = new Color("#BD274D");

		private TextureRect nSaveSlotBg;

		private TextureRect nSlotDecor;

		private Label nSlotNum;

		public SaveLoadSlotUsedEntry(SaveLoadMenu.Mode mode, SaveFileInformation saveInfo, IMenuEntryList parentMenu)
			: base(mode, saveInfo, parentMenu)
		{
		}

		public override Control DrawEntry()
		{
			Control row = new Control();
			row.SizeFlagsHorizontal = 0;
			row.SizeFlagsVertical = 0;
			row.RectMinSize = SaveLoadSlotMenuEntry.SaveSlotSize;
			nSaveSlotBg = GDUtil.MakeNode<TextureRect>("SaveSlotBg");
			nSaveSlotBg.Expand = true;
			nSaveSlotBg.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot.png");
			nSaveSlotBg.RectMinSize = SaveLoadSlotMenuEntry.SaveSlotSize;
			nSlotDecor = GDUtil.MakeNode<TextureRect>("SaveSlotDecor");
			nSlotDecor.Expand = true;
			nSlotDecor.Texture = GD.Load<Texture>("res://assets/img/ui/save_decor.png");
			nSlotDecor.RectMinSize = nSlotDecor.Texture.GetSize() / 3f;
			nSlotDecor.RectPosition = SaveDecorPos;
			string locationImgPath = saveInfo.Image;
			if (!ResourceLoader.Exists(locationImgPath))
			{
				locationImgPath = "res://assets/img/ui/save/unknown.png";
			}
			TextureRect locationImg = GDUtil.MakeNode<TextureRect>("LocationImg");
			locationImg.Expand = true;
			locationImg.Texture = GD.Load<Texture>(locationImgPath);
			locationImg.RectMinSize = locationImg.Texture.GetSize() / 3f;
			locationImg.RectPosition = LocationImgPos;
			HBoxContainer characters = GDUtil.MakeNode<HBoxContainer>("Characters");
			characters.AnchorRight = 0.8f;
			characters.AnchorBottom = 1f;
			characters.Alignment = BoxContainer.AlignMode.End;
			characters.GrowHorizontal = Control.GrowDirection.Begin;
			string[] party = saveInfo.Party;
			foreach (string chr in party)
			{
				string imgPath = "res://assets/img/ui/chr_face/" + chr + ".png";
				if (!ResourceLoader.Exists(imgPath))
				{
					imgPath = "res://assets/img/ui/chr_face/unknown.png";
				}
				TextureRect characterImg = GDUtil.MakeNode<TextureRect>("Character");
				characterImg.Expand = true;
				characterImg.Texture = GD.Load<Texture>(imgPath);
				characterImg.Material = GD.Load<Material>("res://resources/material/pixel_perfect.tres");
				characterImg.RectMinSize = characterImg.Texture.GetSize() * 0.8f;
				characterImg.SizeFlagsHorizontal = 4;
				characterImg.SizeFlagsVertical = 4;
				characters.AddChild(characterImg);
			}
			nSlotNum = GDUtil.MakeNode<Label>("SlotNum");
			nSlotNum.Text = saveInfo.SlotNum;
			nSlotNum.SetDefaultFontAndColor();
			nSlotNum.RectPosition = SaveLabelPos;
			Label roomName = GDUtil.MakeNode<Label>("RoomName");
			roomName.Text = saveInfo.Location;
			roomName.SetDefaultFontAndColor();
			roomName.RectPosition = SaveRoomLabelPos;
			Label playtime = GDUtil.MakeNode<Label>("Playtime");
			playtime.Text = saveInfo.Playtime;
			playtime.SetDefaultFontAndColor();
			playtime.RectPosition = SavePlaytimeLabelPos;
			row.AddChild(nSaveSlotBg);
			row.AddChild(locationImg);
			row.AddChild(characters);
			row.AddChild(nSlotDecor);
			row.AddChild(nSlotNum);
			row.AddChild(roomName);
			row.AddChild(playtime);
			return row;
		}

		public override void Deselect()
		{
			nSaveSlotBg.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot.png");
			nSlotDecor.Visible = true;
			nSlotNum.SetDefaultFontAndColor();
		}

		public override void Select()
		{
			nSaveSlotBg.Texture = GD.Load<Texture>("res://assets/img/ui/save_slot_selected.png");
			nSlotDecor.Visible = false;
			nSlotNum.SetFontColor(PrimaryColor);
		}
	}
}
