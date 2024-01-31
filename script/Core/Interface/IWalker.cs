// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.IWalker
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;

#nullable disable
namespace LacieEngine.Core
{
  public interface IWalker
  {
    string SpriteState { get; set; }

    bool Backwards { get; set; }

    void Walk(
      int displacementX,
      int displacenemtY,
      Direction direction,
      IWalker.MoveSpeed speed,
      Action callback = null);

    void Walk(int displacementX, int displacenemtY, Direction direction, Action callback = null);

    void WalkPath(string pathName, IWalker.MoveSpeed speed, Action callback = null);

    void WalkPath(string pathName, Action callback = null);

    void WalkToPoint(Vector2 point, Action callback);

    void WalkToPoint(Vector2 point);

    enum MoveSpeed
    {
      VerySlow,
      Slow,
      Normal,
      Running,
      Sprinting,
    }
  }
}
