// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.AnimationPlayerExtension
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.Core
{
  public static class AnimationPlayerExtension
  {
    public static void ClearAnimations(this AnimationPlayer animationPlayer)
    {
      foreach (string animation in animationPlayer.GetAnimationList())
        animationPlayer.RemoveAnimation(animation);
    }

    public static void PlayFirstAnimation(this AnimationPlayer animationPlayer)
    {
      animationPlayer.Play(animationPlayer.GetAnimationList()[animationPlayer.GetAnimationList()[0] == "RESET" ? 1 : 0]);
    }

    public static async Task WaitUntilFinished(this AnimationPlayer animationPlayer)
    {
      object[] signal = await animationPlayer.ToSignal((Object) animationPlayer, "animation_finished");
    }
  }
}
