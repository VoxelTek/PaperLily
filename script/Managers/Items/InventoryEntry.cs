// Decompiled with JetBrains decompiler
// Type: LacieEngine.Items.InventoryEntry
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Items
{
  public class InventoryEntry
  {
    public IItem Item;
    public int Amount;

    public InventoryEntry(string id)
    {
      this.Item = Game.Items.Get(id);
      this.Amount = 1;
    }

    public InventoryEntry(string id, int amount)
    {
      this.Item = Game.Items.Get(id);
      this.Amount = amount;
    }
  }
}
