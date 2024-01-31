// Decompiled with JetBrains decompiler
// Type: LacieEngine.Objectives.ObjectiveDTO
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Objectives
{
  internal class ObjectiveDTO
  {
    public List<ObjectiveDTO.Objective> Objectives;

    public class Objective
    {
      public string Id;
      public string Name;
      public string Description;
      public bool Hidden;
      public List<string> OnComplete;
      public List<ObjectiveDTO.Objective> Children;
    }
  }
}
