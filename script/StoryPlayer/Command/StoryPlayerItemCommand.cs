using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Translations;
using LacieEngine.UI;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerItemCommand : StoryPlayerCommand
	{
		public enum ItemOperation
		{
			Add,
			Remove,
			Rename
		}

		private static readonly Vector2 ItemIconSize = new Vector2(40f, 40f);

		[NonSerialized]
		[Inject]
		private IItemManager Items;

		public ItemOperation Operation { get; set; }

		public string Item { get; set; }

		public string NewName { get; set; }

		public string NewDescription { get; set; }

		public int? Amount { get; set; }

		public bool Silent { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			int amount = Amount ?? 1;
			if (Operation == ItemOperation.Add && !Silent)
			{
				Items.AddItem(Item, amount);
				string formattedCaption = Game.Language.GetFormattedCaption("system.storyplayer.item.get", Items.Get(Item).Name, (amount > 1) ? (" x" + amount) : "");
				storyPlayer.UI.HideDialogueBox();
				Control itemPopup = CreateItemPopup(formattedCaption, Items.Get(Item).Icon);
				storyPlayer.nItemPopup.AddChild(itemPopup);
				storyPlayer.nItemPopup.Visible = true;
				storyPlayer.Characters.HideAllCharacters();
				storyPlayer.Characters.ShowCharacter(null);
				storyPlayer.SetNextBlockContinue();
				storyPlayer.AdvanceDisabled = false;
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_item.ogg");
			}
			else if (Operation == ItemOperation.Add && Silent)
			{
				Items.AddItem(Item, amount);
				storyPlayer.SetNextBlockContinue();
				storyPlayer.Next();
			}
			else if (Operation == ItemOperation.Remove)
			{
				Items.RemoveItem(Item, amount);
				storyPlayer.SetNextBlockContinue();
				storyPlayer.Next();
			}
			else if (Operation == ItemOperation.Rename)
			{
				Items.OverrideItemNameAndDescription(Item, NewName, NewDescription);
				storyPlayer.SetNextBlockContinue();
				storyPlayer.Next();
			}
		}

		public override IList<string> GetDependencies()
		{
			return new List<string>(1) { "res://assets/sprite/common/item/" + Items.Get(Item).Icon + ".png" };
		}

		public override void FindCaptions(TranslatableMessages captions)
		{
			if (Operation == ItemOperation.Rename)
			{
				if (!NewName.IsNullOrEmpty())
				{
					captions.Add(NewName, ItemRenameNameContext());
				}
				if (!NewDescription.IsNullOrEmpty())
				{
					captions.Add(NewDescription, ItemRenameDescriptionContext());
				}
			}
		}

		public override void OverrideCaptions(ICaptionSet captions)
		{
			if (Operation == ItemOperation.Rename)
			{
				if (!NewName.IsNullOrEmpty() && captions.ContainsKey(NewName))
				{
					NewName = captions.Get(NewName, ItemRenameNameContext());
				}
				if (!NewDescription.IsNullOrEmpty() && captions.ContainsKey(NewDescription))
				{
					NewDescription = captions.Get(NewDescription, ItemRenameDescriptionContext());
				}
			}
		}

		private string ItemRenameNameContext()
		{
			return "events." + base.Event.Id + ".item.rename.name." + Item;
		}

		private string ItemRenameDescriptionContext()
		{
			return "events." + base.Event.Id + ".item.rename.desc." + Item;
		}

		private static Control CreateItemPopup(string text, string iconPath)
		{
			Frame frame = Game.Settings.UiProvider.MakeDefaultFrame();
			frame.ContentMarginTop = 5;
			frame.ContentMarginBottom = 5;
			frame.ContentMarginLeft -= 5;
			HBoxContainer container = new HBoxContainer();
			container.SetSeparation(0);
			TextureRect icon = new TextureRect();
			icon.RectMinSize = ItemIconSize;
			icon.Expand = true;
			icon.Texture = GD.Load("res://assets/sprite/common/item/" + iconPath + ".png") as Texture;
			icon.Texture.Flags = 4u;
			icon.Material = GD.Load<Material>("res://resources/material/pixel_perfect.tres");
			Label label = GDUtil.MakeNode<Label>("Info");
			label.Text = text;
			label.Align = Label.AlignEnum.Center;
			label.SetDefaultFontAndColor();
			container.AddChild(icon);
			container.AddChild(label);
			frame.AddToFrame(container);
			return frame;
		}
	}
}
