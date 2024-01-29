using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Items;

namespace LacieEngine.UI
{
	public class InventoryMenu : IMenuEntryList
	{
		private const int MinItemSlots = 15;

		private const int Columns = 5;

		private TitledMenuFrame nFrame;

		private RichTextLabel nDescription;

		private List<InventoryEntry> _entries;

		public IMenuEntryContainer Container { get; set; }

		public IMenuEntryList Parent { get; set; }

		public List<IMenuEntry> Entries { get; private set; }

		public int Selection { get; set; }

		public Action OnBack { get; set; }

		public InventoryMenu(IMenuEntryContainer container, TitledMenuFrame frame, RichTextLabel description)
		{
			Container = container;
			Entries = new List<IMenuEntry>();
			nFrame = frame;
			nDescription = description;
			List<IItem> ownedItems = Game.Items.GetOwnedItems();
			_entries = new List<InventoryEntry>();
			foreach (IItem item2 in ownedItems)
			{
				if (!item2.Hidden)
				{
					_entries.Add(new InventoryEntry(item2.Id, Game.Items.GetOwnedAmount(item2.Id)));
				}
			}
			foreach (InventoryEntry item in _entries)
			{
				Entries.Add(new InventoryMenuEntry(item, this));
			}
			int inventorySlots = Math.Max(15, 5 * (int)Math.Ceiling((double)Entries.Count / 5.0));
			for (int i = Entries.Count; i < inventorySlots; i++)
			{
				Entries.Add(new InventoryMenuEmptyEntry(this));
			}
		}

		public void HighlightSelection()
		{
			foreach (IInventoryMenuEntry entry in Entries)
			{
				entry.Deselect();
			}
			nFrame.TitleVisible = false;
			nDescription.Text = "";
			if (Selection > -1)
			{
				((IInventoryMenuEntry)Entries[Selection]).Select();
				if (_entries.Count > Selection)
				{
					nFrame.TitleVisible = true;
					nFrame.TitleText = _entries[Selection].Item.Name;
					nDescription.Text = _entries[Selection].Item.Description;
				}
			}
		}

		public Control DrawContent()
		{
			CenterContainer centerContainer = GDUtil.MakeNode<CenterContainer>("CenterContainer");
			centerContainer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
			GridContainer container = GDUtil.MakeNode<GridContainer>("EntryGrid");
			container.Columns = 5;
			container.SetSeparation(10, 10);
			int i = 0;
			int emptySlotsToDraw = 15;
			foreach (IMenuEntry entry in Entries)
			{
				MarginContainer entryContainer = GDUtil.MakeNode<MarginContainer>("Option" + i++);
				Control item = entry.DrawEntry();
				entryContainer.AddChild(item);
				container.AddChild(entryContainer);
				emptySlotsToDraw--;
			}
			centerContainer.AddChild(container);
			return centerContainer;
		}

		public void HandleInput(InputEvent @event)
		{
			string input = Inputs.Handle(@event, Inputs.Processor.Menu, Inputs.AllUi, allowEcho: true);
			if (input == null)
			{
				return;
			}
			switch (input)
			{
			case "input_up":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				Selection -= 4;
				if (--Selection <= -1)
				{
					Selection = Entries.Count + Selection;
				}
				HighlightSelection();
				break;
			case "input_down":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				Selection += 5;
				if (Selection >= Entries.Count)
				{
					Selection %= 5;
				}
				HighlightSelection();
				break;
			case "input_left":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				if (Selection-- % 5 == 0)
				{
					Selection += 5;
				}
				HighlightSelection();
				break;
			case "input_right":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				if (++Selection % 5 == 0)
				{
					Selection -= 5;
				}
				HighlightSelection();
				break;
			case "input_action":
				Entries[Selection].Accept();
				break;
			case "input_cancel":
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
				Back();
				break;
			}
		}

		public void Back()
		{
			if (OnBack != null)
			{
				OnBack();
			}
			else if (Parent != null)
			{
				Parent.Root();
			}
		}
	}
}
