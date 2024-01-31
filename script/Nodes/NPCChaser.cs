// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.NPCChaser
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Nodes
{
  public class NPCChaser : NPCWalker
  {
    public const int DEFAULT_HITBOX_RADIUS = 5;
    private NavigationAgent2D nNavigation;
    private Area2D nTriggerArea;

    public bool Chasing { get; set; }

    public int TouchHitboxRadius { get; set; } = 5;

    public event Action Caught;

    public NPCChaser()
    {
    }

    public NPCChaser(string characterName)
      : base(characterName)
    {
    }

    public override void _EnterTree()
    {
      base._EnterTree();
      this.nNavigation = new NavigationAgent2D();
      this.AddChild((Godot.Node) this.nNavigation);
      this.nTriggerArea = GDUtil.MakeNode<Area2D>("Trigger");
      this.nTriggerArea.AddChild((Godot.Node) GDUtil.MakeCollisionCircle((float) this.TouchHitboxRadius));
      this.nTriggerArea.CollisionLayer = 0U;
      this.nTriggerArea.CollisionMask = 4U;
      this.AddChild((Godot.Node) this.nTriggerArea);
    }

    public override void _PhysicsProcess(float delta)
    {
      if (!this.Ready)
        return;
      if (this.Chasing && Game.Player != null && Game.Player.Node.IsValid())
      {
        this.nNavigation.SetTargetLocation(Game.Player.Node.Position);
        Vector2 nextLocation = this.nNavigation.GetNextLocation();
        if ((double) Math.Abs(nextLocation.x - this.Position.x) <= 1.0)
          this.Position = new Vector2(nextLocation.x, this.Position.y);
        if ((double) Math.Abs(nextLocation.y - this.Position.y) <= 1.0)
          this.Position = new Vector2(this.Position.x, nextLocation.y);
        this.WalkToPoint(nextLocation);
        if (this.nTriggerArea.GetOverlappingBodies().Count > 0 && !Game.StoryPlayer.Running)
        {
          this.Chasing = false;
          this.nTriggerArea.CollisionMask = 0U;
          this.Caught();
        }
      }
      else
      {
        this.nNavigation.SetTargetLocation(this.Position);
        this.AutoMovement = false;
      }
      base._PhysicsProcess(delta);
    }

    public void SetNavigation(Navigation2D navigation)
    {
      this.nNavigation.SetNavigation((Godot.Node) navigation);
    }

    public override void CheckIfAutoMovementOver()
    {
      if ((double) this.Position.DistanceTo(this.nNavigation.GetTargetLocation()) >= (double) this.TouchHitboxRadius)
        return;
      FinishAutoMovement();

      void FinishAutoMovement()
      {
        this.AutoMovement = false;
        this.Position = this.AutoDestination;
        this.UpdateState();
        Action autoCallback = this.AutoCallback;
        if (autoCallback == null)
          return;
        autoCallback();
      }
    }
  }
}
