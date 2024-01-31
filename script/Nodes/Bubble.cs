// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.Bubble
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Nodes
{
  public class Bubble : Sprite
  {
    [Export(PropertyHint.None, "")]
    private AudioStream sfxSound;
    private AnimationPlayer nAnimation;

    public override void _Ready()
    {
      this.nAnimation = this.GetNode<AnimationPlayer>((NodePath) "Animation");
      this.PlayAndDestroyWhenDone();
    }

    public void PlaySound() => Game.Audio.PlaySfx(this.sfxSound);

    private async void PlayAndDestroyWhenDone()
    {
      Bubble node = this;
      node.nAnimation.PlayFirstAnimation();
      await node.nAnimation.WaitUntilFinished();
      node.Delete();
    }
  }
}
