// Decompiled with JetBrains decompiler
// Type: LacieEngine.Items.ItemManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
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
                return _items[id];
            Log.Error("Item ", id, " not found!");
            return null;
        }

        public List<IItem> GetOwnedItems()
        {
            var ownedItems = new List<IItem>();
            foreach (var stateInventoryEntry in Game.State.Items)
                ownedItems.Add(Get(stateInventoryEntry.itemId));
            return ownedItems;
        }

        public List<IItem> GetAllItems()
        {
            var allItems = new List<IItem>();
            allItems.AddRange(_items.Values);
            return allItems;
        }

        public int GetOwnedAmount(string itemId)
        {
            var inInventory = FindInInventory(itemId);
            return inInventory != null ? inInventory.amount : 0;
        }

        public void AddItem(string itemId) => AddItem(itemId, 1);

        public void AddItem(string itemId, int amount)
        {
            var inInventory = FindInInventory(itemId);
            if (inInventory != null)
                inInventory.amount += amount;
            else
                Game.State.Items.Add(new StateInventoryEntry(itemId, amount));
        }

        public void RemoveItem(string itemId, int amount)
        {
            var inInventory = FindInInventory(itemId);
            if (inInventory == null)
                return;
            if (inInventory.amount > amount)
                inInventory.amount -= amount;
            else
                Game.State.Items.Remove(inInventory);
        }

        public void ClearInventory()
        {
            var stateInventoryEntryList = new List<StateInventoryEntry>();
            foreach (var stateInventoryEntry in Game.State.Items)
            {
                if (_items[stateInventoryEntry.itemId].Persistent)
                    stateInventoryEntryList.Add(stateInventoryEntry);
            }
            Game.State.Items = stateInventoryEntryList;
        }

        public bool IsItemValid(string itemId) => _items.ContainsKey(itemId);

        public void Init()
        {
            _items = new Dictionary<string, Item>();
            foreach (var str1 in GDUtil.ListFilesInPath("res://definitions/items/", ".json"))
            {
                var itemDto = GDUtil.ReadJsonFile<ItemDTO>(str1);
                var str2 = str1.StripPrefix("res://definitions/items/").StripSuffix(".json");
                foreach (var obj1 in itemDto.Items)
                {
                    var obj2 = new Item {
                        Id = obj1.Id,
                        Name = obj1.Name,
                        OriginalName = obj1.Name,
                        Description = obj1.Description,
                        OriginalDescription = obj1.Description,
                        Icon = obj1.Icon ?? DEFAULT_ICON,
                        Event = obj1.Event,
                        Group = str2,
                        Hidden = obj1.Hidden,
                        Persistent = obj1.Persistent,
                        Tags = obj1.Tags ?? Array.Empty<string>()
                    };
                    _items[obj2.Id] = obj2;
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
            if (newDescription.IsNullOrEmpty())
                return;
            Game.State.OverrideItemDescriptions[itemId] = newDescription;
            _items[itemId].Description = newDescription;
        }

        public void ApplyTranslationOverrides()
        {
            foreach (var obj in _items.Values)
            {
                obj.Name = Game.Language.GetCaption(obj.OriginalName, ItemNameContext(obj));
                obj.Description = Game.Language.GetCaption(obj.OriginalDescription, ItemDescriptionContext(obj));
            }
        }

        public void ApplyStateOverrides()
        {
            foreach (var obj in _items.Values)
            {
                if (Game.State.OverrideItemNames.ContainsKey(obj.Id))
                    obj.Name = Game.State.OverrideItemNames[obj.Id];
                if (Game.State.OverrideItemDescriptions.ContainsKey(obj.Id))
                    obj.Description = Game.State.OverrideItemDescriptions[obj.Id];
            }
        }

        private StateInventoryEntry FindInInventory(string itemId)
        {
            foreach (var inInventory in Game.State.Items)
            {
                if (inInventory.itemId == itemId)
                    return inInventory;
            }
            return null;
        }

        public void Clean()
        {
            foreach (Item obj in _items.Values)
            {
                obj.Name = obj.OriginalName;
                obj.Description = obj.OriginalDescription;
            }
        }

        private string ItemNameContext(IItem item) => "items.name." + item.Id;

        private string ItemDescriptionContext(IItem item) => "items.desc." + item.Id;
    }
}
