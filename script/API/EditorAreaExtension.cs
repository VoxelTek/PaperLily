// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.EditorAreaExtension
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.API
{
  public static class EditorAreaExtension
  {
	public static Vector2 GetPixelPerfectOffset(this IEditorArea rect)
	{
	  return new Vector2((double) rect.Area.x % 2.0 != 0.0 ? 0.5f : 0.0f, (double) rect.Area.y % 2.0 != 0.0 ? 0.5f : 0.0f);
	}

	public static Rect2 ToRect2(this IEditorArea rect)
	{
	  return new Rect2(-1f * (rect.Area / 2f) + rect.GetPixelPerfectOffset(), rect.Area);
	}
  }
}
