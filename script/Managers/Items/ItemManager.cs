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
      if (this._items.ContainsKey(id))
        return (IItem) this._items[id];
      Log.Error((object) "Item ", (object) id, (object) " not found!");
      return (IItem) null;
    }

    public List<IItem> GetOwnedItems()
    {
      List<IItem> ownedItems = new List<IItem>();
      foreach (StateInventoryEntry stateInventoryEntry in Game.State.Items)
        ownedItems.Add(this.Get(stateInventoryEntry.itemId));
      return ownedItems;
    }

    public List<IItem> GetAllItems()
    {
      List<IItem> allItems = new List<IItem>();
      allItems.AddRange((IEnumerable<IItem>) this._items.Values);
      return allItems;
    }

    public int GetOwnedAmount(string itemId)
    {
      StateInventoryEntry inInventory = this.FindInInventory(itemId);
      return inInventory != null ? inInventory.amount : 0;
    }

    public void AddItem(string itemId) => this.AddItem(itemId, 1);

    public void AddItem(string itemId, int amount)
    {
      StateInventoryEntry inInventory = this.FindInInventory(itemId);
      if (inInventory != null)
        inInventory.amount += amount;
      else
        Game.State.Items.Add(new StateInventoryEntry(itemId, amount));
    }

    public void RemoveItem(string itemId, int amount)
    {
      StateInventoryEntry inInventory = this.FindInInventory(itemId);
      if (inInventory == null)
        return;
      if (inInventory.amount > amount)
        inInventory.amount -= amount;
      else
        Game.State.Items.Remove(inInventory);
    }

    public void ClearInventory()
    {
      List<StateInventoryEntry> stateInventoryEntryList = new List<StateInventoryEntry>();
      foreach (StateInventoryEntry stateInventoryEntry in Game.State.Items)
      {
        if (this._items[stateInventoryEntry.itemId].Persistent)
          stateInventoryEntryList.Add(stateInventoryEntry);
      }
      Game.State.Items = stateInventoryEntryList;
    }

    public bool IsItemValid(string itemId) => this._items.ContainsKey(itemId);

    public void Init()
    {
      this._items = new Dictionary<string, Item>();
      foreach (string str1 in GDUtil.ListFilesInPath("res://definitions/items/", ".json"))
      {
        ItemDTO itemDto = GDUtil.ReadJsonFile<ItemDTO>(str1);
        string str2 = str1.StripPrefix("res://definitions/items/").StripSuffix(".json");
        foreach (ItemDTO.Item obj1 in itemDto.Items)
        {
          Item obj2 = new Item()
          {
            Id = obj1.Id,
            Name = obj1.Name,
            OriginalName = obj1.Name,
            Description = obj1.Description,
            OriginalDescription = obj1.Description,
            Icon = obj1.Icon ?? "question_mark",
            Event = obj1.Event,
            Group = str2,
            Hidden = obj1.Hidden,
            Persistent = obj1.Persistent,
            Tags = obj1.Tags ?? Array.Empty<string>()
          };
          this._items[obj2.Id] = obj2;
        }
      }
    }

    public void OverrideItemNameAndDescription(
      string itemId,
      string newName,
      string newDescription)
    {
      if (!newName.IsNullOrEmpty())
      {
        Game.State.OverrideItemNames[itemId] = newName;
        this._items[itemId].Name = newName;
      }
      if (newDescription.IsNullOrEmpty())
        return;
      Game.State.OverrideItemDescriptions[itemId] = newDescription;
      this._items[itemId].Description = newDescription;
    }

    public void ApplyTranslationOverrides()
    {
      foreach (Item obj in this._items.Values)
      {
        obj.Name = Game.Language.GetCaption(obj.OriginalName);
        obj.Description = Game.Language.GetCaption(obj.OriginalDescription);
      }
    }

    public void ApplyStateOverrides()
    {
      foreach (Item obj in this._items.Values)
      {
        if (Game.State.OverrideItemNames.ContainsKey(obj.Id))
          obj.Name = Game.State.OverrideItemNames[obj.Id];
        if (Game.State.OverrideItemDescriptions.ContainsKey(obj.Id))
          obj.Description = Game.State.OverrideItemDescriptions[obj.Id];
      }
    }

    private StateInventoryEntry FindInInventory(string itemId)
    {
      foreach (StateInventoryEntry inInventory in Game.State.Items)
      {
        if (inInventory.itemId == itemId)
          return inInventory;
      }
      return (StateInventoryEntry) null;
    }

    public void Clean()
    {
      foreach (Item obj in this._items.Values)
      {
        obj.Name = obj.OriginalName;
        obj.Description = obj.OriginalDescription;
      }
    }

    private string ItemNameContext(IItem item) => "items.name." + item.Id;

    private string ItemDescriptionContext(IItem item) => "items.desc." + item.Id;
  }
}
