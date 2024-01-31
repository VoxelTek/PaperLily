// Decompiled with JetBrains decompiler
// Type: LacieEngine.Items.Item
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;

#nullable disable
namespace LacieEngine.Items
{
  public class Item : IItem
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string OriginalName { get; set; }

    public string Description { get; set; }

    public string OriginalDescription { get; set; }

    public string Icon { get; set; }

    public string Event { get; set; }

    public string Group { get; set; }

    public string[] Tags { get; set; }

    public bool Hidden { get; set; }

    public bool Persistent { get; set; }
  }
}
