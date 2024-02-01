// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.WalkingCharacter
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Nodes
{
  public abstract class WalkingCharacter : KinematicBody2D, ITurnable, IWalker, IPhysicsInterpolated
  {
    public bool Ready;
    public const float IDLE_ANIMATION_TIMER = 10f;
    public static readonly Vector2 COLLISION_SIZE = new Vector2(23f, 8f);
    public static readonly Vector2 FLOOR_OFFSET = new Vector2(0.0f, -4f);
    public static readonly Vector2 GRAVITY_2D_WALK = new Vector2(0.0f, 80f);
    public static readonly Vector2 GRAVITY_2D_RUN = new Vector2(0.0f, 150f);
    protected CharacterSprite nSprite;
    protected Direction Direction = Direction.Down;
    protected Direction NewDirection = Direction.Down;
    protected Direction SpriteDirectionTemp = Direction.Down;
    protected CharacterState State;
    protected IWalker.MoveSpeed MovementSpeed;
    protected float IdleTime;
    protected bool IdleStart;
    protected bool AutoMovement;
    protected Vector2 AutoDestination = Vector2.Zero;
    protected Direction AutoDirection = Direction.None;
    protected IWalker.MoveSpeed AutoSpeed = IWalker.MoveSpeed.Normal;
    protected Action AutoCallback;
    protected Queue<Vector2> AutoMovementQueue = new Queue<Vector2>();
    private static readonly Dictionary<IWalker.MoveSpeed, int> _movementSpeeds = new Dictionary<IWalker.MoveSpeed, int>();
    private static readonly Dictionary<IWalker.MoveSpeed, float> _animationSpeeds;
    private Smoothing2D _visualNode;
    private bool _reparenting;

    public string CharacterId { get; set; }

    public Direction DefaultDirection { get; set; } = Direction.Down;

    public bool PlayerCharacter { get; set; }

    public bool RunningDisabled { get; set; }

    public bool SneakingEnabled { get; set; }

    public bool Backwards { get; set; }

    public bool TurningEnabled { get; } = true;

    public KinematicBody2D Node => (KinematicBody2D) this;

    public bool OverrideSpriteState { get; set; }

    public string SpriteState
    {
      get => this.nSprite.State;
      set => this.nSprite.State = value;
    }

    public float MoveSpeedMultiplier { get; set; } = 1f;

    public CharacterSprite CharacterSprite => this.nSprite;

    public Node2D VisualNode => (Node2D) this._visualNode;

    static WalkingCharacter()
    {
      WalkingCharacter._movementSpeeds.Add(IWalker.MoveSpeed.VerySlow, 50);
      WalkingCharacter._movementSpeeds.Add(IWalker.MoveSpeed.Slow, 100);
      WalkingCharacter._movementSpeeds.Add(IWalker.MoveSpeed.Normal, 160);
      WalkingCharacter._movementSpeeds.Add(IWalker.MoveSpeed.Running, 280);
      WalkingCharacter._movementSpeeds.Add(IWalker.MoveSpeed.Sprinting, 350);
      WalkingCharacter._animationSpeeds = new Dictionary<IWalker.MoveSpeed, float>();
      WalkingCharacter._animationSpeeds.Add(IWalker.MoveSpeed.VerySlow, 0.7f);
      WalkingCharacter._animationSpeeds.Add(IWalker.MoveSpeed.Slow, 0.8f);
      WalkingCharacter._animationSpeeds.Add(IWalker.MoveSpeed.Normal, 1f);
      WalkingCharacter._animationSpeeds.Add(IWalker.MoveSpeed.Running, 1f);
      WalkingCharacter._animationSpeeds.Add(IWalker.MoveSpeed.Sprinting, 1.2f);
    }

    public override sealed void _Ready()
    {
      this._CharacterReady();
      int num = (int) this.Connect("visibility_changed", (Godot.Object) this, "UpdateVisibility");
      this.UpdateVisibility();
    }

    protected virtual void _CharacterReady()
    {
    }

    public override void _ExitTree()
    {
      if (this._reparenting)
        return;
      this.VisualNode.DeleteIfValid();
    }

    public virtual void Turn(Direction direction)
    {
      this.Direction = this.SpriteDirectionTemp = direction;
      this.nSprite.Direction = (Direction.Enum) direction;
    }

    public void TurnToDefault() => this.Turn(this.DefaultDirection);

    public void Walk(
      int displacementX,
      int displacenemtY,
      Direction direction,
      IWalker.MoveSpeed speed,
      Action callback = null)
    {
      this.AutoMovement = true;
      if (direction.FlattenY() == Direction.Up)
        displacenemtY *= -1;
      if (direction.FlattenX() == Direction.Left)
        displacementX *= -1;
      this.AutoDestination = this.Position + new Vector2((float) displacementX, (float) displacenemtY);
      this.AutoDirection = direction;
      this.AutoCallback = callback;
      this.AutoSpeed = speed;
    }

    public void Walk(int displacementX, int displacenemtY, Direction direction, Action callback = null)
    {
      this.Walk(displacementX, displacenemtY, direction, IWalker.MoveSpeed.Normal, callback);
    }

    public void WalkPath(string pathName, IWalker.MoveSpeed speed, Action callback = null)
    {
      Path2D path = Game.Room.GetPath(pathName);
      this.AutoCallback = callback;
      this.AutoSpeed = speed;
      for (int idx = 0; idx < path.Curve.GetPointCount(); ++idx)
        this.AutoMovementQueue.Enqueue(path.Curve.GetPointPosition(idx));
      this.Position = this.AutoMovementQueue.Dequeue();
      this.WalkToPoint(this.AutoMovementQueue.Dequeue());
    }

    public void WalkPath(string pathName, Action callback = null)
    {
      this.WalkPath(pathName, IWalker.MoveSpeed.Normal, callback);
    }

    public void WalkToPoint(Vector2 point)
    {
      this.AutoMovement = true;
      this.AutoDestination = point;
      this.AutoDirection = Direction.FromVector(point - this.Position);
    }

    public void WalkToPoint(Vector2 point, Action callback)
    {
      this.AutoCallback = callback;
      this.WalkToPoint(point);
    }

    public void Reparent(Godot.Node newParent)
    {
      this._reparenting = true;
      this.GetParent().RemoveChild((Godot.Node) this);
      this.VisualNode.GetParent().RemoveChild((Godot.Node) this.VisualNode);
      newParent.AddChild((Godot.Node) this);
      newParent.AddChild((Godot.Node) this.VisualNode);
      this.Teleport();
      this._reparenting = false;
    }

    public virtual void CheckIfAutoMovementOver()
    {
      if ((double) this.Position.DistanceTo(this.AutoDestination) < 1.0)
      {
        FinishAutoMovement();
      }
      else
      {
        if (this.AutoDirection.HasX())
        {
          switch (this.AutoDirection.ToEnum())
          {
            case Direction.Enum.Left:
            case Direction.Enum.UpLeft:
            case Direction.Enum.DownLeft:
              if ((double) this.Position.x <= (double) this.AutoDestination.x)
              {
                FinishOnX();
                break;
              }
              break;
            case Direction.Enum.Right:
            case Direction.Enum.UpRight:
            case Direction.Enum.DownRight:
              if ((double) this.Position.x >= (double) this.AutoDestination.x)
              {
                FinishOnX();
                break;
              }
              break;
          }
        }
        if (this.AutoDirection.HasY())
        {
          switch (this.AutoDirection.ToEnum())
          {
            case Direction.Enum.Up:
            case Direction.Enum.UpLeft:
            case Direction.Enum.UpRight:
              if ((double) this.Position.y <= (double) this.AutoDestination.y)
              {
                FinishOnY();
                break;
              }
              break;
            case Direction.Enum.Down:
            case Direction.Enum.DownLeft:
            case Direction.Enum.DownRight:
              if ((double) this.Position.y >= (double) this.AutoDestination.y)
              {
                FinishOnY();
                break;
              }
              break;
          }
        }
        if (!(this.AutoDirection == Direction.None))
          return;
        FinishAutoMovement();
      }

      void FinishOnX()
      {
        this.Position = this.Position.ReplaceX(this.AutoDestination.x);
        this.AutoDirection = this.AutoDirection.FlattenY();
      }

      void FinishOnY()
      {
        this.Position = this.Position.ReplaceY(this.AutoDestination.y);
        this.AutoDirection = this.AutoDirection.FlattenX();
      }

      void FinishAutoMovement()
      {
        this.AutoMovement = false;
        this.Position = this.AutoDestination;
        if (!this.AutoMovementQueue.IsEmpty<Vector2>())
        {
          this.WalkToPoint(this.AutoMovementQueue.Dequeue());
        }
        else
        {
          this.UpdateState();
          this.Backwards = false;
          Action autoCallback = this.AutoCallback;
          if (autoCallback == null)
            return;
          autoCallback();
        }
      }
    }

    public void UpdateState()
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      if (this.AutoMovement)
      {
        flag1 = true;
        flag2 = this.AutoSpeed == IWalker.MoveSpeed.Running || this.AutoSpeed == IWalker.MoveSpeed.Sprinting;
        this.MovementSpeed = this.AutoSpeed;
      }
      else if (this.PlayerCharacter && Game.InputProcessor == Inputs.Processor.Player)
      {
        flag1 = Inputs.IsActionPressed("input_up") || Inputs.IsActionPressed("input_down") || Inputs.IsActionPressed("input_left") || Inputs.IsActionPressed("input_right");
        flag2 = !this.RunningDisabled && Inputs.IsActionPressed("input_run");
        flag3 = !flag2 && this.SneakingEnabled && Inputs.IsActionPressed("input_special");
        this.MovementSpeed = !flag2 ? (!flag3 ? IWalker.MoveSpeed.Normal : IWalker.MoveSpeed.VerySlow) : IWalker.MoveSpeed.Running;
      }
      if (flag1 & flag2)
      {
        this.nSprite.FpsMultiplier = WalkingCharacter._animationSpeeds[this.MovementSpeed];
        this.State = CharacterState.Running;
      }
      else if (flag1 & flag3)
      {
        this.nSprite.FpsMultiplier = WalkingCharacter._animationSpeeds[this.MovementSpeed];
        this.State = CharacterState.Sneaking;
      }
      else if (flag1)
      {
        this.nSprite.FpsMultiplier = WalkingCharacter._animationSpeeds[this.MovementSpeed];
        this.State = CharacterState.Walking;
      }
      else if (this.State == CharacterState.Idle || this.State == CharacterState.InObject)
      {
        this.NewDirection = Direction.None;
      }
      else
      {
        this.State = CharacterState.Standing;
        this.NewDirection = Direction.None;
      }
    }

    public Vector2 EightDirectionalMotion()
    {
      Vector2 vector2;
      switch (this.State)
      {
        case CharacterState.Walking:
          vector2 = this.GetMovementWithFloorModifier(this.NewDirection.ToVector() * (float) WalkingCharacter._movementSpeeds[this.MovementSpeed] * this.MoveSpeedMultiplier);
          break;
        case CharacterState.Running:
          vector2 = this.GetMovementWithFloorModifier(this.NewDirection.ToVector() * (float) WalkingCharacter._movementSpeeds[this.MovementSpeed] * this.MoveSpeedMultiplier);
          break;
        case CharacterState.Sneaking:
          vector2 = this.GetMovementWithFloorModifier(this.NewDirection.ToVector() * (float) WalkingCharacter._movementSpeeds[this.MovementSpeed] * this.MoveSpeedMultiplier);
          break;
        default:
          vector2 = Vector2.Zero;
          break;
      }
      return vector2;
    }

    public Vector2 TwoDirectionalMotion(bool forceRun = false)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      if (this.AutoMovement)
      {
        flag1 = true;
        flag2 = forceRun;
      }
      else if (this.PlayerCharacter && Game.InputProcessor == Inputs.Processor.Player)
      {
        flag1 = Inputs.IsActionPressed("input_left") || Inputs.IsActionPressed("input_right");
        flag2 = !this.RunningDisabled && Inputs.IsActionPressed("input_run");
        flag3 = !flag2 && this.SneakingEnabled && Inputs.IsActionPressed("input_special");
      }
      if (this.NewDirection == this.NewDirection.FlattenY())
      {
        this.UpdateSpriteDirection();
        flag1 = false;
      }
      this.NewDirection = this.NewDirection.FlattenX();
      Vector2 vector2 = this.NewDirection.ToVector();
      if (flag1 & flag2)
      {
        vector2 = this.GetMovementWithFloorModifier(vector2 * (float) WalkingCharacter._movementSpeeds[IWalker.MoveSpeed.Running] * this.MoveSpeedMultiplier);
        this.State = CharacterState.Running;
      }
      else if (flag1 & flag3)
      {
        vector2 = this.GetMovementWithFloorModifier(vector2 * (float) WalkingCharacter._movementSpeeds[IWalker.MoveSpeed.VerySlow] * this.MoveSpeedMultiplier);
        this.State = CharacterState.Sneaking;
      }
      else if (flag1)
      {
        vector2 = this.GetMovementWithFloorModifier(vector2 * (float) WalkingCharacter._movementSpeeds[IWalker.MoveSpeed.Normal] * this.MoveSpeedMultiplier);
        this.State = CharacterState.Walking;
      }
      else if (this.State == CharacterState.Idle)
        this.NewDirection = Direction.None;
      else if (!flag1 || vector2.Equals(Vector2.Zero))
      {
        this.State = CharacterState.Standing;
        this.NewDirection = Direction.None;
      }
      return !flag2 ? vector2 + WalkingCharacter.GRAVITY_2D_WALK : vector2 + WalkingCharacter.GRAVITY_2D_RUN;
    }

    public void Teleport() => this._visualNode.Teleport();

    protected void UpdateSpriteDirection()
    {
      if (this.SpriteDirectionTemp != Direction.None && (double) Math.Abs(Mathf.Rad2Deg(this.NewDirection.ToVector().AngleTo(this.SpriteDirectionTemp.ToVector()))) < 90.0)
        return;
      switch ((this.Backwards ? this.NewDirection.GetOpposite() : this.NewDirection).ToEnum())
      {
        case Direction.Enum.Left:
        case Direction.Enum.UpLeft:
        case Direction.Enum.DownLeft:
          this.SpriteDirectionTemp = Direction.Left;
          break;
        case Direction.Enum.Up:
          this.SpriteDirectionTemp = Direction.Up;
          break;
        case Direction.Enum.Right:
        case Direction.Enum.UpRight:
        case Direction.Enum.DownRight:
          this.SpriteDirectionTemp = Direction.Right;
          break;
        case Direction.Enum.Down:
          this.SpriteDirectionTemp = Direction.Down;
          break;
      }
    }

    protected void CreateCharacterSprite()
    {
      if (this._reparenting)
        return;
      this.nSprite = GDUtil.MakeNode<CharacterSprite>("Sprite");
      this.nSprite.CharacterId = this.CharacterId;
      this.nSprite.UseParentMaterial = true;
      Smoothing2D smoothing2D = GDUtil.MakeNode<Smoothing2D>("PlayerSprite");
      smoothing2D.Target = (Node2D) this;
      smoothing2D.translate = true;
      smoothing2D.enabled = true;
      smoothing2D.AddChild((Godot.Node) this.nSprite);
      this.GetParent().AddChild((Godot.Node) smoothing2D);
      this._visualNode = smoothing2D;
    }

    protected CollisionShape2D MakeCollisionNode()
    {
      return GDUtil.MakeCollisionCapsule(WalkingCharacter.COLLISION_SIZE, WalkingCharacter.FLOOR_OFFSET);
    }

    protected Area2D MakeFloorDetectionNode()
    {
      Area2D area2D = GDUtil.MakeNode<Area2D>("FloorDetection");
      area2D.CollisionLayer = 0U;
      area2D.AddChild((Godot.Node) GDUtil.MakeCollisionCircle(6f, WalkingCharacter.FLOOR_OFFSET));
      return area2D;
    }

    private void Turn()
    {
      this.Direction = this.SpriteDirectionTemp;
      this.Turn(this.Direction);
    }

    private void UpdateVisibility() => this.VisualNode.Visible = this.Visible;

    public abstract Vector2 GetMovementWithFloorModifier(Vector2 movement);

    public void PlayAnimations()
    {
      if (this.OverrideSpriteState)
        return;
      if (this.State == CharacterState.Idle)
      {
        if (this.IdleStart)
        {
          this.SpriteState = "idle";
          this.IdleStart = false;
        }
      }
      else if (this.State == CharacterState.Standing)
      {
        this.SpriteState = "stand";
        this.Turn();
      }
      else if (this.State == CharacterState.Walking)
      {
        this.SpriteState = "walk";
        if (this.Direction != this.NewDirection)
          this.UpdateSpriteDirection();
        this.Direction = this.NewDirection;
      }
      else if (this.State == CharacterState.Running)
      {
        this.SpriteState = "run";
        if (this.Direction != this.NewDirection)
          this.UpdateSpriteDirection();
        this.Direction = this.NewDirection;
      }
      else if (this.State == CharacterState.Sneaking)
      {
        this.SpriteState = "sneak";
        if (this.Direction != this.NewDirection)
          this.UpdateSpriteDirection();
        this.Direction = this.NewDirection;
      }
      this.nSprite.Direction = (Direction.Enum) this.SpriteDirectionTemp;
    }

    public void OverrideTextureForState(string state, Texture texture)
    {
      this.nSprite.OverrideTextureForState(state, texture);
    }
  }
}
