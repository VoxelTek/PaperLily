// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.RandomExtension
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;

#nullable disable
namespace LacieEngine.Core
{
  public static class RandomExtension
  {
    public static float NextFloat(this Random random) => (float) random.NextDouble();

    public static Vector2 NextVector2(this Random random)
    {
      return new Vector2(random.NextFloat(), random.NextFloat());
    }

    public static Direction NextDirection(this Random random)
    {
      Direction direction;
      switch (random.Next(8))
      {
        case 0:
          direction = Direction.Left;
          break;
        case 1:
          direction = Direction.UpLeft;
          break;
        case 2:
          direction = Direction.Up;
          break;
        case 3:
          direction = Direction.UpRight;
          break;
        case 4:
          direction = Direction.Right;
          break;
        case 5:
          direction = Direction.DownRight;
          break;
        case 6:
          direction = Direction.Down;
          break;
        default:
          direction = Direction.DownLeft;
          break;
      }
      return direction;
    }
  }
}
