// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1CutsceneIntro
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1CutsceneIntro : GameRoom
  {
    [LacieEngine.API.GetNode("Home_Exterior_Front")]
    private GameRoom nRoom;
    [LacieEngine.API.GetNode("Animation")]
    private AnimationPlayer nAnimation;
    [LacieEngine.API.GetNode("Home_Exterior_Front/Foreground/lacie_lean")]
    private Sprite nLacieLean;

    public override void _UpdateRoom() => this.nRoom._UpdateRoom();

    public void PlayCutscene() => this.nAnimation.PlayFirstAnimation();

    public void LacieLean() => this.nLacieLean.Frame = 0;
  }
}
