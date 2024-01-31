// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.GDExtension
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using Godot.Collections;

#nullable disable
namespace LacieEngine.Core
{
  public static class GDExtension
  {
    public static bool IsNullOrEmpty(this NodePath nodePath)
    {
      return nodePath == null || nodePath.IsEmpty();
    }

    public static T[] ToArray<T>(this Godot.Collections.Array godotArray)
    {
      T[] array = new T[godotArray.Count];
      godotArray.CopyTo((System.Array) array, 0);
      return array;
    }

    public static T[] ToArray<T>(this Array<T> godotArray)
    {
      T[] array = new T[godotArray.Count];
      godotArray.CopyTo(array, 0);
      return array;
    }
  }
}
