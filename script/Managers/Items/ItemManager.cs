using System;
using System.Collections.Generic;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Items
{
	[Injectable]
	public class ItemManager : IItemManager, IModule, ITranslatable, IStateOverridable
	{
		private Dictionary<string, Item> _items;

		private const string DEFAULT_ICON = "question_mark";

		public IItem Get(string id)
		{
			if (_items.ContainsKey(id))
			{
				return _items[id];
			}
			Log.Error("Item ", id, " not found!");
			return null;
		}

		public List<IItem> GetOwnedItems()
		{
			List<IItem> items = new List<IItem>();
			foreach (StateInventoryEntry stateEntry in Game.State.Items)
			{
				items.Add(Get(stateEntry.itemId));
			}
			return items;
		}

		public List<IItem> GetAllItems()
		{
			List<IItem> list = new List<IItem>();
			list.AddRange(_items.Values);
			return list;
		}

		public int GetOwnedAmount(string itemId)
		{
			return FindInInventory(itemId)?.amount ?? 0;
		}

		public void AddItem(string itemId)
		{
			AddItem(itemId, 1);
		}

		public void AddItem(string itemId, int amount)
		{
			StateInventoryEntry existingEntry = FindInInventory(itemId);
			if (existingEntry != null)
			{
				existingEntry.amount += amount;
			}
			else
			{
				Game.State.Items.Add(new StateInventoryEntry(itemId, amount));
			}
		}

		public void RemoveItem(string itemId, int amount)
		{
			StateInventoryEntry entryToRemove = FindInInventory(itemId);
			if (entryToRemove != null)
			{
				if (entryToRemove.amount > amount)
				{
					entryToRemove.amount -= amount;
				}
				else
				{
					Game.State.Items.Remove(entryToRemove);
				}
			}
		}

		public void ClearInventory()
		{
			List<StateInventoryEntry> newInventory = new List<StateInventoryEntry>();
			foreach (StateInventoryEntry inventoryEntry in Game.State.Items)
			{
				if (_items[inventoryEntry.itemId].Persistent)
				{
					newInventory.Add(inventoryEntry);
				}
			}
			Game.State.Items = newInventory;
		}

		public bool IsItemValid(string itemId)
		{
			return _items.ContainsKey(itemId);
		}

		public void Init()
		{
			_items = new Dictionary<string, Item>();
			foreach (string item2 in GDUtil.ListFilesInPath("res://definitions/items/", ".json"))
			{
				ItemDTO itemDto = GDUtil.ReadJsonFile<ItemDTO>(item2);
				string itemGroup = item2.StripPrefix("res://definitions/items/").StripSuffix(".json");
				foreach (ItemDTO.Item parsedItem in itemDto.Items)
				{
					Item item = new Item();
					item.Id = parsedItem.Id;
					item.Name = parsedItem.Name;
					item.OriginalName = parsedItem.Name;
					item.Description = parsedItem.Description;
					item.OriginalDescription = parsedItem.Description;
					item.Icon = parsedItem.Icon ?? "question_mark";
					item.Event = parsedItem.Event;
					item.Group = itemGroup;
					item.Hidden = parsedItem.Hidden;
					item.Persistent = parsedItem.Persistent;
					item.Tags = parsedItem.Tags ?? Array.Empty<string>();
					_items[item.Id] = item;
				}
			}
		}

		public void OverrideItemNameAndDescription(string itemId, string newName, string newDescription)
		{
			if (!newName.IsNullOrEmpty())
			{
				Game.State.OverrideItemNames[itemId] = newName;
				_items[itemId].Name = newName;
			}
			if (!newDescription.IsNullOrEmpty())
			{
				Game.State.OverrideItemDescriptions[itemId] = newDescription;
				_items[itemId].Description = newDescription;
			}
		}

		public void ApplyTranslationOverrides()
		{
			foreach (Item item in _items.Values)
			{
				item.Name = Game.Language.GetCaption(item.OriginalName);
				item.Description = Game.Language.GetCaption(item.OriginalDescription);
			}
		}

		public void ApplyStateOverrides()
		{
			foreach (Item item in _items.Values)
			{
				if (Game.State.OverrideItemNames.ContainsKey(item.Id))
				{
					item.Name = Game.State.OverrideItemNames[item.Id];
				}
				if (Game.State.OverrideItemDescriptions.ContainsKey(item.Id))
				{
					item.Description = Game.State.OverrideItemDescriptions[item.Id];
				}
			}
		}

		private StateInventoryEntry FindInInventory(string itemId)
		{
			foreach (StateInventoryEntry item in Game.State.Items)
			{
				if (item.itemId == itemId)
				{
					return item;
				}
			}
			return null;
		}

		public void Clean()
		{
			foreach (Item value in _items.Values)
			{
				value.Name = value.OriginalName;
				value.Description = value.OriginalDescription;
			}
		}

		private string ItemNameContext(IItem item)
		{
			return "items.name." + item.Id;
		}

		private string ItemDescriptionContext(IItem item)
		{
			return "items.desc." + item.Id;
		}
	}
}
