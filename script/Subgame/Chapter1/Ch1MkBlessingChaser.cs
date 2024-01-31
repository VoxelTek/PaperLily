// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1MkBlessingChaser
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class Ch1MkBlessingChaser : NPCChaser
  {
    public const int INTERACT_RADIUS = 6;
    public const float LIFETIME = 15f;
    private NavigationAgent2D nNavigation;
    private float _elapsed;

    public Ch1MkBlessingChaser()
      : base("ch1_mk_ghost")
    {
    }

    public override void _EnterTree()
    {
      this.DefaultDirection = Direction.Right;
      this.TouchHitboxRadius = 6;
      base._EnterTree();
      this.nNavigation = new NavigationAgent2D();
      this.AddChild((Godot.Node) this.nNavigation);
    }
  }
}
