using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	public class Player : WalkingCharacter, IPlayer, IPhysicsInterpolated, ITurnable, IWalker, IReflectable, IFollowable
	{
		private Area2D nInteractions;

		private Area2D nThinInteractions;

		private Area2D nFloorDetection;

		private Position2D nCenter;

		private Light2D nLight;

		private Dictionary<Direction, CollisionShape2D> nDirectionalInteractions;

		private Dictionary<Direction, CollisionShape2D> nThinDirectionalInteractions;

		private static readonly string[] HandledInputs = new string[2] { "input_action", "input_menu" };

		private static readonly string[] HandledMovementInputs = new string[4] { "input_up", "input_down", "input_left", "input_right" };

		private static readonly string[] RecordedInputs = new string[5] { "input_up", "input_down", "input_left", "input_right", "input_run" };

		private IFollowable.Segment _currentFollowableSegment;

		private uint _collisionLayer;

		private uint _collisionMask;

		public Direction SpriteDirection
		{
			get
			{
				return nSprite.Direction;
			}
			set
			{
				nSprite.Direction = value;
			}
		}

		public List<IFollowable.Segment> FollowableSegments { get; private set; }

		public bool CollisionEnabled
		{
			get
			{
				return base.CollisionLayer != 0;
			}
			set
			{
				if (!value)
				{
					base.CollisionMask = 0u;
					base.CollisionLayer = 0u;
				}
				else
				{
					base.CollisionLayer = _collisionLayer;
					base.CollisionMask = _collisionMask;
				}
			}
		}

		public override void _EnterTree()
		{
			base.CharacterId = Game.State.Party[0];
			CreateCharacterSprite();
			base.DefaultDirection = Direction.Down;
			base.PlayerCharacter = true;
		}

		protected override void _CharacterReady()
		{
			nInteractions = GetNode("InteractionBox") as Area2D;
			nThinInteractions = GetNode("ThinInteractionBox") as Area2D;
			nFloorDetection = GetNode("FloorDetection") as Area2D;
			nCenter = GetNode("Center") as Position2D;
			nDirectionalInteractions = new Dictionary<Direction, CollisionShape2D>();
			nDirectionalInteractions.Add(Direction.Up, nInteractions.GetNode("Up") as CollisionShape2D);
			nDirectionalInteractions.Add(Direction.Down, nInteractions.GetNode("Down") as CollisionShape2D);
			nDirectionalInteractions.Add(Direction.Left, nInteractions.GetNode("Left") as CollisionShape2D);
			nDirectionalInteractions.Add(Direction.Right, nInteractions.GetNode("Right") as CollisionShape2D);
			nThinDirectionalInteractions = new Dictionary<Direction, CollisionShape2D>();
			nThinDirectionalInteractions.Add(Direction.Up, nThinInteractions.GetNode("Up") as CollisionShape2D);
			nThinDirectionalInteractions.Add(Direction.Down, nThinInteractions.GetNode("Down") as CollisionShape2D);
			nThinDirectionalInteractions.Add(Direction.Left, nThinInteractions.GetNode("Left") as CollisionShape2D);
			nThinDirectionalInteractions.Add(Direction.Right, nThinInteractions.GetNode("Right") as CollisionShape2D);
			nThinInteractions.Connect("area_entered", this, "CheckAndRunInstantEvents");
			SetPlayerSprite();
			Turn();
			FollowableSegments = new List<IFollowable.Segment>
			{
				new IFollowable.Segment(Direction, base.Position)
			};
			_currentFollowableSegment = FollowableSegments[0];
			Game.Room.RegisterMirrorReflection(this);
			_collisionLayer = base.CollisionLayer;
			_collisionMask = base.CollisionMask;
			Ready = true;
		}

		public override void _Input(InputEvent @event)
		{
			string text = Inputs.Handle(@event, Inputs.Processor.Player, HandledInputs);
			if (!(text == "input_action"))
			{
				if (text == "input_menu")
				{
					if (!Game.State.MenuDisabled)
					{
						Game.Screen.OpenMenu();
					}
					else
					{
						Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
					}
				}
				return;
			}
			EventTrigger interactingNode = GetInteractingNode();
			if (interactingNode != null)
			{
				if (!interactingNode.RelatedNode.IsNullOrEmpty() && interactingNode.GetNode(interactingNode.RelatedNode) is ITurnable npc)
				{
					npc.Turn(SpriteDirectionTemp.GetOpposite());
					Game.StoryPlayer.Connect("DialogueEnded", (Godot.Object)npc, "TurnToDefault", null, 4u);
				}
				Game.Events.ExecuteMapping(interactingNode.Name);
			}
			else
			{
				GetInteractingActionableNode()?.Activate();
			}
		}

		public override void _PhysicsProcess(float delta)
		{
			if (!Ready)
			{
				return;
			}
			_currentFollowableSegment.Destination = base.Position;
			if (AutoMovement)
			{
				NewDirection = AutoDirection;
			}
			else if (Game.InputProcessor == Inputs.Processor.Player)
			{
				NewDirection = Inputs.GetDirectionFromInput(Direction, Inputs.Processor.Player);
			}
			UpdateState();
			PlayerMotion(out var inputMotion, out var actualMotion);
			if (AutoMovement)
			{
				CheckIfAutoMovementOver();
			}
			if (NewDirection != Direction.None && Math.Abs(Mathf.Rad2Deg(NewDirection.ToVector().AngleTo(_currentFollowableSegment.Direction.ToVector()))) >= 45f)
			{
				DelimitFollowableSegment();
			}
			if (Game.Characters.Get(base.CharacterId).IdleAnimation)
			{
				if (!Game.StoryPlayer.Running && nSprite.State != "idle" && Game.InputProcessor == Inputs.Processor.Player && inputMotion == Vector2.Zero)
				{
					IdleTime += delta;
				}
				else
				{
					IdleTime = 0f;
				}
				if (Game.InputProcessor == Inputs.Processor.Player && IdleTime > 10f)
				{
					IdleTime = 0f;
					IdleStart = true;
					State = CharacterState.Idle;
				}
			}
			this.PixelSnap(actualMotion.x == 0f, actualMotion.y == 0f);
			PlayAnimations();
			TurnInteractions();
		}

		protected virtual void PlayerMotion(out Vector2 inputMotion, out Vector2 actualMotion)
		{
			inputMotion = EightDirectionalMotion();
			actualMotion = MoveAndSlide(inputMotion);
		}

		public override Vector2 GetMovementWithFloorModifier(Vector2 movement)
		{
			foreach (Node2D overlappingArea in nFloorDetection.GetOverlappingAreas())
			{
				if (overlappingArea is IFloorMovementModifier floor)
				{
					return floor.GetModifiedMovement(movement);
				}
			}
			return movement;
		}

		public void DisableRunning()
		{
			base.RunningDisabled = true;
		}

		public void EnableRunning()
		{
			base.RunningDisabled = false;
		}

		public Direction GetDirection()
		{
			return Direction;
		}

		public void SetDirection(Direction dir)
		{
			NewDirection = dir;
			UpdateSpriteDirection();
			State = CharacterState.Standing;
		}

		private void Turn()
		{
			base.SpriteState = "stand";
			Direction = SpriteDirectionTemp;
			Turn(Direction);
		}

		public Vector2 GetCenter()
		{
			return nCenter.GlobalPosition;
		}

		private void TurnInteractions()
		{
			foreach (CollisionShape2D value in nDirectionalInteractions.Values)
			{
				value.SetDeferred("disabled", true);
			}
			foreach (CollisionShape2D value2 in nThinDirectionalInteractions.Values)
			{
				value2.SetDeferred("disabled", true);
			}
			nDirectionalInteractions[SpriteDirectionTemp].SetDeferred("disabled", false);
			nThinDirectionalInteractions[SpriteDirectionTemp].SetDeferred("disabled", false);
		}

		private void SetPlayerSprite()
		{
			base.CharacterId = Game.State.Party[0];
			base.SpriteState = "stand";
		}

		public IActionObject GetInteractingActionableNode()
		{
			foreach (Node2D overlappingBody in nThinInteractions.GetOverlappingBodies())
			{
				if (overlappingBody is IActionObject actionableObject && actionableObject.IsValidForDirection(SpriteDirectionTemp))
				{
					return actionableObject;
				}
			}
			return null;
		}

		public EventTrigger GetInteractingNode()
		{
			List<EventTrigger> nodes = new List<EventTrigger>();
			foreach (Node2D overlappingArea in nFloorDetection.GetOverlappingAreas())
			{
				if (overlappingArea.GetParent() is EventTrigger et3 && et3.Trigger == EventTrigger.ETrigger.Action && Game.Events.HasMapping(et3.Name, SpriteDirectionTemp))
				{
					nodes.Add(et3);
				}
			}
			foreach (Node2D overlappingBody in nThinInteractions.GetOverlappingBodies())
			{
				if (overlappingBody.GetParent() is EventTrigger et2 && et2.Trigger == EventTrigger.ETrigger.Action && Game.Events.HasMapping(et2.Name, SpriteDirectionTemp))
				{
					nodes.Add(et2);
				}
			}
			if (nodes.Count == 1)
			{
				return nodes[0];
			}
			if (nodes.Count > 1)
			{
				EventTrigger max = nodes[0];
				for (int i = 1; i < nodes.Count; i++)
				{
					if (nodes[i].Priority > max.Priority)
					{
						max = nodes[i];
					}
				}
				return max;
			}
			if (nodes.IsEmpty())
			{
				nodes.Clear();
				foreach (Node2D overlappingBody2 in nInteractions.GetOverlappingBodies())
				{
					if (overlappingBody2.GetParent() is EventTrigger et && et.Trigger == EventTrigger.ETrigger.Action && Game.Events.HasMapping(et.Name, SpriteDirectionTemp))
					{
						nodes.Add(et);
					}
				}
				if (nodes.Count == 1)
				{
					return nodes[0];
				}
				if (OS.IsDebugBuild() && nodes.Count > 1)
				{
					Log.Warn("Attention: Player is facing 2 interactions which are too close from one another. Consider redrawing them.");
				}
			}
			return null;
		}

		public EventTrigger GetItemInteractingNode(string itemId)
		{
			List<EventTrigger> nodes = new List<EventTrigger>();
			foreach (Node2D overlappingArea in nFloorDetection.GetOverlappingAreas())
			{
				if (overlappingArea.GetParent() is EventTrigger et3 && (et3.Trigger == EventTrigger.ETrigger.Action || et3.Trigger == EventTrigger.ETrigger.Item) && Game.Events.HasItemObjectMapping(itemId, et3.Name))
				{
					nodes.Add(et3);
				}
			}
			foreach (Node2D overlappingBody in nThinInteractions.GetOverlappingBodies())
			{
				if (overlappingBody.GetParent() is EventTrigger et2 && (et2.Trigger == EventTrigger.ETrigger.Action || et2.Trigger == EventTrigger.ETrigger.Item) && Game.Events.HasItemObjectMapping(itemId, et2.Name))
				{
					nodes.Add(et2);
				}
			}
			if (nodes.Count == 1)
			{
				return nodes[0];
			}
			if (nodes.Count > 1)
			{
				EventTrigger max = nodes[0];
				for (int i = 1; i < nodes.Count; i++)
				{
					if (nodes[i].Priority > max.Priority)
					{
						max = nodes[i];
					}
				}
				return max;
			}
			if (nodes.IsEmpty())
			{
				nodes.Clear();
				foreach (Node2D overlappingBody2 in nInteractions.GetOverlappingBodies())
				{
					if (overlappingBody2.GetParent() is EventTrigger et && (et.Trigger == EventTrigger.ETrigger.Action || et.Trigger == EventTrigger.ETrigger.Item) && Game.Events.HasItemObjectMapping(itemId, et.Name))
					{
						nodes.Add(et);
					}
				}
				if (nodes.Count == 1)
				{
					return nodes[0];
				}
				if (OS.IsDebugBuild() && nodes.Count > 1)
				{
					Log.Warn("Attention: Player is facing 2 interactions which are too close from one another. Consider redrawing them.");
				}
			}
			return null;
		}

		public void CheckAndRunInstantEvents(Area2D area)
		{
			if (!AutoMovement && area.GetParent() is EventTrigger et && et.Trigger == EventTrigger.ETrigger.Touch && Game.Events.HasInstantMapping(et.Name))
			{
				Game.Events.ExecuteInstantMapping(et.Name);
			}
		}

		public void SetLight(Light2D light)
		{
			nLight.DeleteIfValid();
			if (light != null)
			{
				AddChild(light);
			}
			nLight = light;
		}

		public Light2D GetLight()
		{
			return nLight;
		}

		public SpriteState GetReflectedSprite()
		{
			return new SpriteState
			{
				Name = base.Name,
				Texture = nSprite.Texture,
				Frame = GetReflectedFrame(),
				HFrames = nSprite.Hframes,
				VFrames = nSprite.Vframes,
				Pos = base.VisualNode.GlobalPosition
			};
		}

		private int GetReflectedFrame()
		{
			if (nSprite.State == "stand" || nSprite.State == "walk" || nSprite.State == "run")
			{
				return SpriteDirectionTemp.ToEnum() switch
				{
					Direction.Enum.Left => nSprite.Frame + 9, 
					Direction.Enum.Up => nSprite.Frame - 27, 
					Direction.Enum.Right => nSprite.Frame - 9, 
					Direction.Enum.Down => nSprite.Frame + 27, 
					_ => 0, 
				};
			}
			if (nSprite.State == "idle")
			{
				if (SpriteDirectionTemp.ToEnum() == Direction.Enum.Up)
				{
					return nSprite.Frame - nSprite.Hframes;
				}
				return nSprite.Frame + nSprite.Hframes;
			}
			return 0;
		}

		public void SetPlayerState(CharacterState state)
		{
			State = state;
		}

		public CharacterState GetPlayerState()
		{
			return State;
		}

		private void DelimitFollowableSegment()
		{
			_currentFollowableSegment = new IFollowable.Segment(NewDirection, base.Position);
			FollowableSegments.Add(_currentFollowableSegment);
		}

		public bool IsRunning()
		{
			return State == CharacterState.Running;
		}
	}
}
