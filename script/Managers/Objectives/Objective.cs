// Decompiled with JetBrains decompiler
// Type: LacieEngine.Objectives.Objective
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Objectives
{
  public class Objective : IObjective
  {
    public string Id { get; set; }

    public string Group { get; set; }

    public int Order { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool Hidden { get; set; }

    public List<string> OnComplete { get; set; }

    public IObjective Parent { get; set; }

    public List<IObjective> Children { get; set; }

    public bool HasParent() => this.Parent != null;

    public bool HasChildren() => this.Children != null && this.Children.Count > 0;
  }
}
