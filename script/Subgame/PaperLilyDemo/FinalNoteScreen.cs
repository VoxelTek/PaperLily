// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.ProjectKat.FinalNoteScreen
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.UI;

#nullable disable
namespace LacieEngine.Subgame.ProjectKat
{
  public class FinalNoteScreen : SimpleScreen
  {
    [Export(PropertyHint.None, "")]
    public AudioStream Bgm;
    private AnimationPlayer nAnimation;

    public override void _Ready()
    {
      base._Ready();
      this.SetProcessInput(false);
      Game.Audio.PlayBgm(this.Bgm);
      this.nAnimation = this.GetNode<AnimationPlayer>((NodePath) "CenterContainer/VBoxContainer/CenterContainer/Arrow/Animation");
    }

    public void EnableContinue()
    {
      this.nAnimation.PlayFirstAnimation();
      this.SetProcessInput(true);
    }
  }
}
