// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.BlinkTransition
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class BlinkTransition : TextureRect, ITransitionScene
  {
    public void TransitionIn(float duration)
    {
      Game.Animations.Play((LacieAnimation) new ShaderProgressAnimation((CanvasItem) this, duration));
    }

    public void TransitionOut(float duration)
    {
      Game.Animations.Play((LacieAnimation) new ShaderProgressAnimation((CanvasItem) this, duration, true));
    }
  }
}
