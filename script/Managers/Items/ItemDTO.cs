// Decompiled with JetBrains decompiler
// Type: LacieEngine.Items.ItemDTO
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Items
{
  internal class ItemDTO
  {
    public List<ItemDTO.Item> Items;

    public class Item
    {
      public string Id;
      public string Name;
      public string Description;
      public string Icon;
      public string Event;
      public string[] Tags;
      public bool Hidden;
      public bool Persistent;
    }
  }
}
