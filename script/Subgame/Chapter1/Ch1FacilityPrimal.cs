// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1FacilityPrimal
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class Ch1FacilityPrimal : NPCChaser
  {
    public const int INTERACT_RADIUS = 16;
    public const int DETECT_RADIUS_RUN = 160;
    public const int DETECT_RADIUS_WALK = 112;
    public const int DETECT_LENGTH_SNEAK = 128;
    public const int DETECT_WIDTH_SNEAK = 48;
    public const float MOVE_SPEED = 3.5f;
    private NavigationAgent2D nNavigation;
    private Area2D nPlayerRunDetectArea;
    private Area2D nPlayerWalkDetectArea;
    private Area2D nPlayerSneakDetectArea;

    public Ch1FacilityPrimal()
      : base("ch1_primal_1")
    {
    }

    public override void _EnterTree()
    {
      this.TouchHitboxRadius = 16;
      this.MoveSpeedMultiplier = 3.5f;
      base._EnterTree();
      this.nNavigation = new NavigationAgent2D();
      this.AddChild((Godot.Node) this.nNavigation);
      this.nPlayerRunDetectArea = GDUtil.MakeNode<Area2D>("DetectRunTrigger");
      this.nPlayerRunDetectArea.AddChild((Godot.Node) GDUtil.MakeCollisionCircle(160f));
      this.nPlayerRunDetectArea.CollisionLayer = 0U;
      this.nPlayerRunDetectArea.CollisionMask = 4U;
      this.AddChild((Godot.Node) this.nPlayerRunDetectArea);
      this.nPlayerWalkDetectArea = GDUtil.MakeNode<Area2D>("DetectWalkTrigger");
      this.nPlayerWalkDetectArea.AddChild((Godot.Node) GDUtil.MakeCollisionCircle(112f));
      this.nPlayerWalkDetectArea.CollisionLayer = 0U;
      this.nPlayerWalkDetectArea.CollisionMask = 4U;
      this.AddChild((Godot.Node) this.nPlayerWalkDetectArea);
      this.nPlayerSneakDetectArea = GDUtil.MakeNode<Area2D>("DetectSneakTrigger");
      this.nPlayerSneakDetectArea.AddChild((Godot.Node) GDUtil.MakeCollisionCapsule(new Vector2(128f, 48f), this.DefaultDirection.ToVector() * 48f));
      this.nPlayerSneakDetectArea.CollisionLayer = 0U;
      this.nPlayerSneakDetectArea.CollisionMask = 4U;
      this.AddChild((Godot.Node) this.nPlayerSneakDetectArea);
    }

    public override void _PhysicsProcess(float delta)
    {
      if (!this.Chasing)
      {
        if (this.nPlayerRunDetectArea.GetOverlappingBodies().Count > 0 && Game.Player.GetPlayerState() == CharacterState.Running)
          this.Detected();
        if (this.nPlayerWalkDetectArea.GetOverlappingBodies().Count > 0 && (Game.Player.GetPlayerState() == CharacterState.Walking || Game.Player.GetPlayerState() == CharacterState.Running))
          this.Detected();
        if (this.nPlayerSneakDetectArea.GetOverlappingBodies().Count > 0)
          this.Detected();
      }
      base._PhysicsProcess(delta);
    }

    private void Detected() => this.Chasing = true;
  }
}
