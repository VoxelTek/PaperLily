// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.IItem
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

#nullable disable
namespace LacieEngine.API
{
  public interface IItem
  {
    string Id { get; set; }

    string Name { get; set; }

    string Description { get; set; }

    string Icon { get; set; }

    string[] Tags { get; set; }

    string Event { get; set; }

    string Group { get; set; }

    bool Hidden { get; set; }

    bool Persistent { get; set; }
  }
}
