// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.ICharacter
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.API
{
  public interface ICharacter
  {
    string Id { get; set; }

    string Name { get; set; }

    string OriginalName { get; set; }

    bool IdleAnimation { get; set; }

    List<string> Costumes { get; set; }

    List<string> Moods { get; set; }

    void UpdateSpriteState(Node node, string state);

    void UpdateSpriteDirection(Node node, Direction direction, string state);
  }
}
