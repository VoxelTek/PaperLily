// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.IFollowable
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Core
{
  public interface IFollowable
  {
    List<IFollowable.Segment> FollowableSegments { get; }

    bool IsRunning();

    class Segment
    {
      public Segment(Direction direction, Vector2 destination)
      {
        this.Direction = direction;
        this.Destination = destination;
      }

      public Direction Direction { get; }

      public Vector2 Destination { get; set; }
    }
  }
}
