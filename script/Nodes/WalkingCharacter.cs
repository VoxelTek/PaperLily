using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	public abstract class WalkingCharacter : KinematicBody2D, ITurnable, IWalker, IPhysicsInterpolated
	{
		public bool Ready;

		public const float IDLE_ANIMATION_TIMER = 10f;

		public static readonly Vector2 COLLISION_SIZE;

		public static readonly Vector2 FLOOR_OFFSET;

		public static readonly Vector2 GRAVITY_2D_WALK;

		public static readonly Vector2 GRAVITY_2D_RUN;

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

		private static readonly Dictionary<IWalker.MoveSpeed, int> _movementSpeeds;

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


		public KinematicBody2D Node => this;

		public bool OverrideSpriteState { get; set; }

		public string SpriteState
		{
			get
			{
				return nSprite.State;
			}
			set
			{
				nSprite.State = value;
			}
		}

		public float MoveSpeedMultiplier { get; set; } = 1f;


		public CharacterSprite CharacterSprite => nSprite;

		public Node2D VisualNode => _visualNode;

		static WalkingCharacter()
		{
			COLLISION_SIZE = new Vector2(23f, 8f);
			FLOOR_OFFSET = new Vector2(0f, -4f);
			GRAVITY_2D_WALK = new Vector2(0f, 80f);
			GRAVITY_2D_RUN = new Vector2(0f, 150f);
			_movementSpeeds = new Dictionary<IWalker.MoveSpeed, int>();
			_movementSpeeds.Add(IWalker.MoveSpeed.VerySlow, 50);
			_movementSpeeds.Add(IWalker.MoveSpeed.Slow, 100);
			_movementSpeeds.Add(IWalker.MoveSpeed.Normal, 160);
			_movementSpeeds.Add(IWalker.MoveSpeed.Running, 280);
			_movementSpeeds.Add(IWalker.MoveSpeed.Sprinting, 350);
			_animationSpeeds = new Dictionary<IWalker.MoveSpeed, float>();
			_animationSpeeds.Add(IWalker.MoveSpeed.VerySlow, 0.7f);
			_animationSpeeds.Add(IWalker.MoveSpeed.Slow, 0.8f);
			_animationSpeeds.Add(IWalker.MoveSpeed.Normal, 1f);
			_animationSpeeds.Add(IWalker.MoveSpeed.Running, 1f);
			_animationSpeeds.Add(IWalker.MoveSpeed.Sprinting, 1.2f);
		}

		public sealed override void _Ready()
		{
			_CharacterReady();
			Connect("visibility_changed", this, "UpdateVisibility");
			UpdateVisibility();
		}

		protected virtual void _CharacterReady()
		{
		}

		public override void _ExitTree()
		{
			if (!_reparenting)
			{
				VisualNode.DeleteIfValid();
			}
		}

		public virtual void Turn(Direction direction)
		{
			Direction = (SpriteDirectionTemp = direction);
			nSprite.Direction = direction;
		}

		public void TurnToDefault()
		{
			Turn(DefaultDirection);
		}

		public void Walk(int displacementX, int displacenemtY, Direction direction, IWalker.MoveSpeed speed, Action callback = null)
		{
			AutoMovement = true;
			if (direction.FlattenY() == Direction.Up)
			{
				displacenemtY *= -1;
			}
			if (direction.FlattenX() == Direction.Left)
			{
				displacementX *= -1;
			}
			AutoDestination = base.Position + new Vector2(displacementX, displacenemtY);
			AutoDirection = direction;
			AutoCallback = callback;
			AutoSpeed = speed;
		}

		public void Walk(int displacementX, int displacenemtY, Direction direction, Action callback = null)
		{
			Walk(displacementX, displacenemtY, direction, IWalker.MoveSpeed.Normal, callback);
		}

		public void WalkPath(string pathName, IWalker.MoveSpeed speed, Action callback = null)
		{
			Path2D path = Game.Room.GetPath(pathName);
			AutoCallback = callback;
			AutoSpeed = speed;
			for (int i = 0; i < path.Curve.GetPointCount(); i++)
			{
				AutoMovementQueue.Enqueue(path.Curve.GetPointPosition(i));
			}
			base.Position = AutoMovementQueue.Dequeue();
			WalkToPoint(AutoMovementQueue.Dequeue());
		}

		public void WalkPath(string pathName, Action callback = null)
		{
			WalkPath(pathName, IWalker.MoveSpeed.Normal, callback);
		}

		public void WalkToPoint(Vector2 point)
		{
			AutoMovement = true;
			AutoDestination = point;
			Direction dir = (AutoDirection = Direction.FromVector(point - base.Position));
		}

		public void WalkToPoint(Vector2 point, Action callback)
		{
			AutoCallback = callback;
			WalkToPoint(point);
		}

		public void Reparent(Node newParent)
		{
			_reparenting = true;
			GetParent().RemoveChild(this);
			VisualNode.GetParent().RemoveChild(VisualNode);
			newParent.AddChild(this);
			newParent.AddChild(VisualNode);
			Teleport();
			_reparenting = false;
		}

		public virtual void CheckIfAutoMovementOver()
		{
			if (base.Position.DistanceTo(AutoDestination) < 1f)
			{
				FinishAutoMovement();
				return;
			}
			if (AutoDirection.HasX())
			{
				switch (AutoDirection.ToEnum())
				{
				case Direction.Enum.Left:
				case Direction.Enum.UpLeft:
				case Direction.Enum.DownLeft:
					if (base.Position.x <= AutoDestination.x)
					{
						FinishOnX();
					}
					break;
				case Direction.Enum.Right:
				case Direction.Enum.UpRight:
				case Direction.Enum.DownRight:
					if (base.Position.x >= AutoDestination.x)
					{
						FinishOnX();
					}
					break;
				}
			}
			if (AutoDirection.HasY())
			{
				switch (AutoDirection.ToEnum())
				{
				case Direction.Enum.Up:
				case Direction.Enum.UpLeft:
				case Direction.Enum.UpRight:
					if (base.Position.y <= AutoDestination.y)
					{
						FinishOnY();
					}
					break;
				case Direction.Enum.Down:
				case Direction.Enum.DownLeft:
				case Direction.Enum.DownRight:
					if (base.Position.y >= AutoDestination.y)
					{
						FinishOnY();
					}
					break;
				}
			}
			if (AutoDirection == Direction.None)
			{
				FinishAutoMovement();
			}
			void FinishAutoMovement()
			{
				AutoMovement = false;
				base.Position = AutoDestination;
				if (!AutoMovementQueue.IsEmpty())
				{
					WalkToPoint(AutoMovementQueue.Dequeue());
				}
				else
				{
					UpdateState();
					Backwards = false;
					AutoCallback?.Invoke();
				}
			}
			void FinishOnX()
			{
				base.Position = base.Position.ReplaceX(AutoDestination.x);
				AutoDirection = AutoDirection.FlattenY();
			}
			void FinishOnY()
			{
				base.Position = base.Position.ReplaceY(AutoDestination.y);
				AutoDirection = AutoDirection.FlattenX();
			}
		}

		public void UpdateState()
		{
			bool holdingMove = false;
			bool holdingRun = false;
			bool holdingSneak = false;
			if (AutoMovement)
			{
				holdingMove = true;
				holdingRun = AutoSpeed == IWalker.MoveSpeed.Running || AutoSpeed == IWalker.MoveSpeed.Sprinting;
				MovementSpeed = AutoSpeed;
			}
			else if (PlayerCharacter && Game.InputProcessor == Inputs.Processor.Player)
			{
				holdingMove = Inputs.IsActionPressed("input_up") || Inputs.IsActionPressed("input_down") || Inputs.IsActionPressed("input_left") || Inputs.IsActionPressed("input_right");
				holdingRun = !RunningDisabled && Inputs.IsActionPressed("input_run");
				holdingSneak = !holdingRun && SneakingEnabled && Inputs.IsActionPressed("input_special");
				if (holdingRun)
				{
					MovementSpeed = IWalker.MoveSpeed.Running;
				}
				else if (holdingSneak)
				{
					MovementSpeed = IWalker.MoveSpeed.VerySlow;
				}
				else
				{
					MovementSpeed = IWalker.MoveSpeed.Normal;
				}
			}
			if (holdingMove && holdingRun)
			{
				nSprite.FpsMultiplier = _animationSpeeds[MovementSpeed];
				State = CharacterState.Running;
			}
			else if (holdingMove && holdingSneak)
			{
				nSprite.FpsMultiplier = _animationSpeeds[MovementSpeed];
				State = CharacterState.Sneaking;
			}
			else if (holdingMove)
			{
				nSprite.FpsMultiplier = _animationSpeeds[MovementSpeed];
				State = CharacterState.Walking;
			}
			else if (State == CharacterState.Idle || State == CharacterState.InObject)
			{
				NewDirection = Direction.None;
			}
			else
			{
				State = CharacterState.Standing;
				NewDirection = Direction.None;
			}
		}

		public Vector2 EightDirectionalMotion()
		{
			return State switch
			{
				CharacterState.Running => GetMovementWithFloorModifier(NewDirection.ToVector() * _movementSpeeds[MovementSpeed] * MoveSpeedMultiplier), 
				CharacterState.Walking => GetMovementWithFloorModifier(NewDirection.ToVector() * _movementSpeeds[MovementSpeed] * MoveSpeedMultiplier), 
				CharacterState.Sneaking => GetMovementWithFloorModifier(NewDirection.ToVector() * _movementSpeeds[MovementSpeed] * MoveSpeedMultiplier), 
				_ => Vector2.Zero, 
			};
		}

		public Vector2 TwoDirectionalMotion(bool forceRun = false)
		{
			bool holdingMove = false;
			bool holdingRun = false;
			bool holdingSneak = false;
			if (AutoMovement)
			{
				holdingMove = true;
				holdingRun = forceRun;
			}
			else if (PlayerCharacter && Game.InputProcessor == Inputs.Processor.Player)
			{
				holdingMove = Inputs.IsActionPressed("input_left") || Inputs.IsActionPressed("input_right");
				holdingRun = !RunningDisabled && Inputs.IsActionPressed("input_run");
				holdingSneak = !holdingRun && SneakingEnabled && Inputs.IsActionPressed("input_special");
			}
			if (NewDirection == NewDirection.FlattenY())
			{
				UpdateSpriteDirection();
				holdingMove = false;
			}
			NewDirection = NewDirection.FlattenX();
			Vector2 motion = NewDirection.ToVector();
			if (holdingMove && holdingRun)
			{
				motion = GetMovementWithFloorModifier(motion * _movementSpeeds[IWalker.MoveSpeed.Running] * MoveSpeedMultiplier);
				State = CharacterState.Running;
			}
			else if (holdingMove && holdingSneak)
			{
				motion = GetMovementWithFloorModifier(motion * _movementSpeeds[IWalker.MoveSpeed.VerySlow] * MoveSpeedMultiplier);
				State = CharacterState.Sneaking;
			}
			else if (holdingMove)
			{
				motion = GetMovementWithFloorModifier(motion * _movementSpeeds[IWalker.MoveSpeed.Normal] * MoveSpeedMultiplier);
				State = CharacterState.Walking;
			}
			else if (State == CharacterState.Idle)
			{
				NewDirection = Direction.None;
			}
			else if (!holdingMove || motion.Equals(Vector2.Zero))
			{
				State = CharacterState.Standing;
				NewDirection = Direction.None;
			}
			if (holdingRun)
			{
				return motion + GRAVITY_2D_RUN;
			}
			return motion + GRAVITY_2D_WALK;
		}

		public void Teleport()
		{
			_visualNode.Teleport();
		}

		protected void UpdateSpriteDirection()
		{
			if (!(SpriteDirectionTemp != Direction.None) || !(Math.Abs(Mathf.Rad2Deg(NewDirection.ToVector().AngleTo(SpriteDirectionTemp.ToVector()))) < 90f))
			{
				switch ((Backwards ? NewDirection.GetOpposite() : NewDirection).ToEnum())
				{
				case Direction.Enum.Left:
				case Direction.Enum.UpLeft:
				case Direction.Enum.DownLeft:
					SpriteDirectionTemp = Direction.Left;
					break;
				case Direction.Enum.Up:
					SpriteDirectionTemp = Direction.Up;
					break;
				case Direction.Enum.Right:
				case Direction.Enum.UpRight:
				case Direction.Enum.DownRight:
					SpriteDirectionTemp = Direction.Right;
					break;
				case Direction.Enum.Down:
					SpriteDirectionTemp = Direction.Down;
					break;
				case Direction.Enum.Left | Direction.Enum.Right:
				case Direction.Enum.UpLeft | Direction.Enum.Right:
				case Direction.Enum.Up | Direction.Enum.Down:
				case Direction.Enum.UpLeft | Direction.Enum.Down:
					break;
				}
			}
		}

		protected void CreateCharacterSprite()
		{
			if (!_reparenting)
			{
				nSprite = GDUtil.MakeNode<CharacterSprite>("Sprite");
				nSprite.CharacterId = CharacterId;
				nSprite.UseParentMaterial = true;
				Smoothing2D smoothing = GDUtil.MakeNode<Smoothing2D>("PlayerSprite");
				smoothing.Target = this;
				smoothing.translate = true;
				smoothing.enabled = true;
				smoothing.AddChild(nSprite);
				GetParent().AddChild(smoothing);
				_visualNode = smoothing;
			}
		}

		protected CollisionShape2D MakeCollisionNode()
		{
			return GDUtil.MakeCollisionCapsule(COLLISION_SIZE, FLOOR_OFFSET);
		}

		protected Area2D MakeFloorDetectionNode()
		{
			Area2D area2D = GDUtil.MakeNode<Area2D>("FloorDetection");
			area2D.CollisionLayer = 0u;
			area2D.AddChild(GDUtil.MakeCollisionCircle(6f, FLOOR_OFFSET));
			return area2D;
		}

		private void Turn()
		{
			Direction = SpriteDirectionTemp;
			Turn(Direction);
		}

		private void UpdateVisibility()
		{
			VisualNode.Visible = base.Visible;
		}

		public abstract Vector2 GetMovementWithFloorModifier(Vector2 movement);

		public void PlayAnimations()
		{
			if (OverrideSpriteState)
			{
				return;
			}
			if (State == CharacterState.Idle)
			{
				if (IdleStart)
				{
					SpriteState = "idle";
					IdleStart = false;
				}
			}
			else if (State == CharacterState.Standing)
			{
				SpriteState = "stand";
				Turn();
			}
			else if (State == CharacterState.Walking)
			{
				SpriteState = "walk";
				if (Direction != NewDirection)
				{
					UpdateSpriteDirection();
				}
				Direction = NewDirection;
			}
			else if (State == CharacterState.Running)
			{
				SpriteState = "run";
				if (Direction != NewDirection)
				{
					UpdateSpriteDirection();
				}
				Direction = NewDirection;
			}
			else if (State == CharacterState.Sneaking)
			{
				SpriteState = "sneak";
				if (Direction != NewDirection)
				{
					UpdateSpriteDirection();
				}
				Direction = NewDirection;
			}
			nSprite.Direction = SpriteDirectionTemp;
		}

		public void OverrideTextureForState(string state, Texture texture)
		{
			nSprite.OverrideTextureForState(state, texture);
		}
	}
}
