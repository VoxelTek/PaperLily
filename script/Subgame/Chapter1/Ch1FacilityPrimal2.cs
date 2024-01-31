// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1FacilityPrimal2
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class Ch1FacilityPrimal2 : NPCChaser
  {
    public const int INTERACT_RADIUS = 8;
    public const float MOVE_SPEED = 0.6f;
    private NavigationAgent2D nNavigation;

    public Ch1FacilityPrimal2()
      : base("ch1_primal_2")
    {
    }

    public override void _EnterTree()
    {
      this.DefaultDirection = Direction.Right;
      this.TouchHitboxRadius = 8;
      this.MoveSpeedMultiplier = 0.6f;
      base._EnterTree();
      this.nNavigation = new NavigationAgent2D();
      this.AddChild((Godot.Node) this.nNavigation);
    }
  }
}
