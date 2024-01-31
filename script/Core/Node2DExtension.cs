// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.Node2DExtension
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;

#nullable disable
namespace LacieEngine.Core
{
  public static class Node2DExtension
  {
    public static void PixelSnap(this Node2D node, bool x = true, bool y = true)
    {
      if (!x && !y)
        return;
      Vector2 position = node.Position;
      if (x)
        position.x = (float) (int) Math.Round((double) position.x);
      if (y)
        position.y = (float) (int) Math.Round((double) position.y);
      node.Position = position;
    }
  }
}
