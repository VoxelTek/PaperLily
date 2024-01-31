// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.VectorExtension
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Core
{
  public static class VectorExtension
  {
    public static Vector2 FlattenX(this Vector2 vec) => new Vector2(vec.x, 0.0f);

    public static Vector2 FlattenY(this Vector2 vec) => new Vector2(0.0f, vec.y);

    public static Vector2 ReplaceX(this Vector2 vec, float newX) => new Vector2(newX, vec.y);

    public static Vector2 ReplaceY(this Vector2 vec, float newY) => new Vector2(vec.x, newY);

    public static Vector2 SignAwareCeil(this Vector2 vec)
    {
      float x = vec.x;
      float y = vec.y;
      return new Vector2((double) x > 0.0 ? Mathf.Ceil(x) : Mathf.Floor(x), (double) y > 0.0 ? Mathf.Ceil(y) : Mathf.Floor(y));
    }
  }
}
