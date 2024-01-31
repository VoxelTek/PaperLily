// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.NPCFollower
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Nodes
{
  public class NPCFollower : WalkingCharacter
  {
    private IFollowable nTarget;
    private CollisionShape2D nCollision;
    private Area2D nFloorDetection;
    private int _targetStepIndex;
    private float _followDistance;

    public NPCFollower()
    {
    }

    public NPCFollower(string characterName, Direction defaultDirection, float followDistance)
    {
      this.CharacterId = characterName;
      this.DefaultDirection = defaultDirection;
      this._followDistance = followDistance;
    }

    public override void _EnterTree()
    {
      this.Name = this.CharacterId.ToPascalCase();
      this.CollisionLayer = 0U;
      this.CollisionMask = 3U;
      this.CreateCharacterSprite();
      this.nCollision = this.MakeCollisionNode();
      this.nFloorDetection = this.MakeFloorDetectionNode();
      this.AddChild((Godot.Node) this.nCollision);
      this.AddChild((Godot.Node) this.nFloorDetection);
    }

    protected override void _CharacterReady()
    {
      this.SpriteState = "stand";
      this.nSprite.Offset = new Vector2(0.0f, (float) ((double) this.nSprite.Texture.GetSize().y / 8.0 * -1.0));
      this.Direction = this.DefaultDirection;
      Game.Room.RegisteredNPCs[this.Name] = (Node2D) this;
      this.TurnToDefault();
      if (this.nTarget == null)
        return;
      this.Ready = true;
    }

    public override void _PhysicsProcess(float delta)
    {
      if (!this.Ready)
      {
        this.nTarget = (IFollowable) Game.Player;
        if (this.nTarget == null)
          return;
        this.Ready = true;
      }
      else
      {
        if ((double) this.TotalDistanceToTarget() >= (double) this._followDistance)
        {
          this.AutoDirection = this.NewDirection = this.nTarget.FollowableSegments[this._targetStepIndex].Direction;
          this.AutoDestination = this.nTarget.FollowableSegments[this._targetStepIndex].Destination;
          this.AutoSpeed = this.nTarget.IsRunning() ? IWalker.MoveSpeed.Running : IWalker.MoveSpeed.Normal;
          this.AutoMovement = true;
        }
        else
          this.AutoMovement = false;
        this.UpdateState();
        Vector2 vector2 = this.MoveAndSlide(this.EightDirectionalMotion());
        if (this.AutoMovement)
          this.CheckIfAutoMovementOver();
        if ((double) this.Position.DistanceTo(this.nTarget.FollowableSegments[this._targetStepIndex].Destination) < 1.0 && this.nTarget.FollowableSegments.Count - 1 > this._targetStepIndex)
          ++this._targetStepIndex;
        this.PixelSnap((double) vector2.x == 0.0, (double) vector2.y == 0.0);
        this.PlayAnimations();
      }
    }

    public override Vector2 GetMovementWithFloorModifier(Vector2 movement)
    {
      foreach (Node2D overlappingArea in this.nFloorDetection.GetOverlappingAreas())
      {
        if (overlappingArea is IFloorMovementModifier movementModifier)
          return movementModifier.GetModifiedMovement(movement);
      }
      return movement;
    }

    private float TotalDistanceToTarget()
    {
      float target = 0.0f;
      Vector2 to = this.Position;
      for (int targetStepIndex = this._targetStepIndex; targetStepIndex < this.nTarget.FollowableSegments.Count; ++targetStepIndex)
      {
        target += Math.Abs(this.nTarget.FollowableSegments[targetStepIndex].Destination.DistanceTo(to));
        to = this.nTarget.FollowableSegments[targetStepIndex].Destination;
      }
      return target;
    }
  }
}
