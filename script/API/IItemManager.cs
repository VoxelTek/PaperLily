// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.IItemManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System.Collections.Generic;

#nullable disable
namespace LacieEngine.API
{
  [InjectableInterface(unique = true)]
  public interface IItemManager : IModule
  {
    IItem Get(string id);

    List<IItem> GetOwnedItems();

    List<IItem> GetAllItems();

    int GetOwnedAmount(string itemId);

    void AddItem(string itemId);

    void AddItem(string itemId, int amount);

    void RemoveItem(string itemId, int amount);

    void ClearInventory();

    bool IsItemValid(string itemId);

    void OverrideItemNameAndDescription(string itemId, string newName, string newDescription);
  }
}
