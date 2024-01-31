// Decompiled with JetBrains decompiler
// Type: LacieEngine.Animation.FadeAnimation
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Animation
{
  public abstract class FadeAnimation : LacieAnimation
  {
    protected static readonly Color VisibleColor = Colors.White;
    protected static readonly Color HiddenColor = new Color(1f, 1f, 1f, 0.0f);
  }
}
