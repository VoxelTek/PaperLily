// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.IObjective
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System.Collections.Generic;

#nullable disable
namespace LacieEngine.API
{
  public interface IObjective
  {
    string Id { get; set; }

    string Group { get; set; }

    string Name { get; set; }

    string Description { get; set; }

    int Order { get; set; }

    bool Hidden { get; set; }

    List<string> OnComplete { get; set; }

    IObjective Parent { get; set; }

    List<IObjective> Children { get; set; }

    bool HasParent();

    bool HasChildren();
  }
}
