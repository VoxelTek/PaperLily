// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.Player
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
  public class Player : 
    WalkingCharacter,
    IPlayer,
    IPhysicsInterpolated,
    ITurnable,
    IWalker,
    IReflectable,
    IFollowable
  {
    private Area2D nInteractions;
    private Area2D nThinInteractions;
    private Area2D nFloorDetection;
    private Position2D nCenter;
    private Light2D nLight;
    private Dictionary<Direction, CollisionShape2D> nDirectionalInteractions;
    private Dictionary<Direction, CollisionShape2D> nThinDirectionalInteractions;
    private static readonly string[] HandledInputs = new string[2]
    {
      "input_action",
      "input_menu"
    };
    private static readonly string[] HandledMovementInputs = new string[4]
    {
      "input_up",
      "input_down",
      "input_left",
      "input_right"
    };
    private static readonly string[] RecordedInputs = new string[5]
    {
      "input_up",
      "input_down",
      "input_left",
      "input_right",
      "input_run"
    };
    private IFollowable.Segment _currentFollowableSegment;
    private uint _collisionLayer;
    private uint _collisionMask;

    public Direction SpriteDirection
    {
      get => (Direction) this.nSprite.Direction;
      set => this.nSprite.Direction = (Direction.Enum) value;
    }

    public List<IFollowable.Segment> FollowableSegments { get; private set; }

    public override void _EnterTree()
    {
      this.CharacterId = Game.State.Party[0];
      this.CreateCharacterSprite();
      this.DefaultDirection = Direction.Down;
      this.PlayerCharacter = true;
    }

    protected override void _CharacterReady()
    {
      this.nInteractions = this.GetNode((NodePath) "InteractionBox") as Area2D;
      this.nThinInteractions = this.GetNode((NodePath) "ThinInteractionBox") as Area2D;
      this.nFloorDetection = this.GetNode((NodePath) "FloorDetection") as Area2D;
      this.nCenter = this.GetNode((NodePath) "Center") as Position2D;
      this.nDirectionalInteractions = new Dictionary<Direction, CollisionShape2D>();
      this.nDirectionalInteractions.Add(Direction.Up, this.nInteractions.GetNode((NodePath) "Up") as CollisionShape2D);
      this.nDirectionalInteractions.Add(Direction.Down, this.nInteractions.GetNode((NodePath) "Down") as CollisionShape2D);
      this.nDirectionalInteractions.Add(Direction.Left, this.nInteractions.GetNode((NodePath) "Left") as CollisionShape2D);
      this.nDirectionalInteractions.Add(Direction.Right, this.nInteractions.GetNode((NodePath) "Right") as CollisionShape2D);
      this.nThinDirectionalInteractions = new Dictionary<Direction, CollisionShape2D>();
      this.nThinDirectionalInteractions.Add(Direction.Up, this.nThinInteractions.GetNode((NodePath) "Up") as CollisionShape2D);
      this.nThinDirectionalInteractions.Add(Direction.Down, this.nThinInteractions.GetNode((NodePath) "Down") as CollisionShape2D);
      this.nThinDirectionalInteractions.Add(Direction.Left, this.nThinInteractions.GetNode((NodePath) "Left") as CollisionShape2D);
      this.nThinDirectionalInteractions.Add(Direction.Right, this.nThinInteractions.GetNode((NodePath) "Right") as CollisionShape2D);
      int num = (int) this.nThinInteractions.Connect("area_entered", (Godot.Object) this, "CheckAndRunInstantEvents");
      this.SetPlayerSprite();
      this.Turn();
      this.FollowableSegments = new List<IFollowable.Segment>()
      {
        new IFollowable.Segment(this.Direction, this.Position)
      };
      this._currentFollowableSegment = this.FollowableSegments[0];
      Game.Room.RegisterMirrorReflection((IReflectable) this);
      this._collisionLayer = this.CollisionLayer;
      this._collisionMask = this.CollisionMask;
      this.Ready = true;
    }

    public override void _Input(InputEvent @event)
    {
      switch (Inputs.Handle(@event, Inputs.Processor.Player, Player.HandledInputs))
      {
        case "input_action":
          EventTrigger interactingNode = this.GetInteractingNode();
          if (interactingNode != null)
          {
            if (!interactingNode.RelatedNode.IsNullOrEmpty() && interactingNode.GetNode(interactingNode.RelatedNode) is ITurnable node)
            {
              node.Turn(this.SpriteDirectionTemp.GetOpposite());
              int num = (int) Game.StoryPlayer.Connect("DialogueEnded", (Godot.Object) node, "TurnToDefault", flags: 4U);
            }
            Game.Events.ExecuteMapping(interactingNode.Name);
            break;
          }
          this.GetInteractingActionableNode()?.Activate();
          break;
        case "input_menu":
          if (!Game.State.MenuDisabled)
          {
            Game.Screen.OpenMenu();
            break;
          }
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
          break;
      }
    }

    public override void _PhysicsProcess(float delta)
    {
      if (!this.Ready)
        return;
      this._currentFollowableSegment.Destination = this.Position;
      if (this.AutoMovement)
        this.NewDirection = this.AutoDirection;
      else if (Game.InputProcessor == Inputs.Processor.Player)
        this.NewDirection = Inputs.GetDirectionFromInput(this.Direction, Inputs.Processor.Player);
      this.UpdateState();
      Vector2 inputMotion;
      Vector2 actualMotion;
      this.PlayerMotion(out inputMotion, out actualMotion);
      if (this.AutoMovement)
        this.CheckIfAutoMovementOver();
      if (this.NewDirection != Direction.None && (double) Math.Abs(Mathf.Rad2Deg(this.NewDirection.ToVector().AngleTo(this._currentFollowableSegment.Direction.ToVector()))) >= 45.0)
        this.DelimitFollowableSegment();
      if (Game.Characters.Get(this.CharacterId).IdleAnimation)
      {
        if (!Game.StoryPlayer.Running && this.nSprite.State != "idle" && Game.InputProcessor == Inputs.Processor.Player && inputMotion == Vector2.Zero)
          this.IdleTime += delta;
        else
          this.IdleTime = 0.0f;
        if (Game.InputProcessor == Inputs.Processor.Player && (double) this.IdleTime > 10.0)
        {
          this.IdleTime = 0.0f;
          this.IdleStart = true;
          this.State = CharacterState.Idle;
        }
      }
      this.PixelSnap((double) actualMotion.x == 0.0, (double) actualMotion.y == 0.0);
      this.PlayAnimations();
      this.TurnInteractions();
    }

    protected virtual void PlayerMotion(out Vector2 inputMotion, out Vector2 actualMotion)
    {
      inputMotion = this.EightDirectionalMotion();
      actualMotion = this.MoveAndSlide(inputMotion);
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

    public void DisableRunning() => this.RunningDisabled = true;

    public void EnableRunning() => this.RunningDisabled = false;

    public Direction GetDirection() => this.Direction;

    public void SetDirection(Direction dir)
    {
      this.NewDirection = dir;
      this.UpdateSpriteDirection();
      this.State = CharacterState.Standing;
    }

    private void Turn()
    {
      this.SpriteState = "stand";
      this.Direction = this.SpriteDirectionTemp;
      this.Turn(this.Direction);
    }

    public Vector2 GetCenter() => this.nCenter.GlobalPosition;

    private void TurnInteractions()
    {
      foreach (Godot.Object @object in this.nDirectionalInteractions.Values)
        @object.SetDeferred("disabled", (object) true);
      foreach (Godot.Object @object in this.nThinDirectionalInteractions.Values)
        @object.SetDeferred("disabled", (object) true);
      this.nDirectionalInteractions[this.SpriteDirectionTemp].SetDeferred("disabled", (object) false);
      this.nThinDirectionalInteractions[this.SpriteDirectionTemp].SetDeferred("disabled", (object) false);
    }

    private void SetPlayerSprite()
    {
      this.CharacterId = Game.State.Party[0];
      this.SpriteState = "stand";
    }

    public IActionObject GetInteractingActionableNode()
    {
      foreach (Node2D overlappingBody in this.nThinInteractions.GetOverlappingBodies())
      {
        if (overlappingBody is IActionObject interactingActionableNode && interactingActionableNode.IsValidForDirection(this.SpriteDirectionTemp))
          return interactingActionableNode;
      }
      return (IActionObject) null;
    }

    public EventTrigger GetInteractingNode()
    {
      List<EventTrigger> collection = new List<EventTrigger>();
      foreach (Godot.Node overlappingArea in this.nFloorDetection.GetOverlappingAreas())
      {
        if (overlappingArea.GetParent() is EventTrigger parent && parent.Trigger == EventTrigger.ETrigger.Action && Game.Events.HasMapping(parent.Name, this.SpriteDirectionTemp))
          collection.Add(parent);
      }
      foreach (Godot.Node overlappingBody in this.nThinInteractions.GetOverlappingBodies())
      {
        if (overlappingBody.GetParent() is EventTrigger parent && parent.Trigger == EventTrigger.ETrigger.Action && Game.Events.HasMapping(parent.Name, this.SpriteDirectionTemp))
          collection.Add(parent);
      }
      if (collection.Count == 1)
        return collection[0];
      if (collection.Count > 1)
      {
        EventTrigger interactingNode = collection[0];
        for (int index = 1; index < collection.Count; ++index)
        {
          if (collection[index].Priority > interactingNode.Priority)
            interactingNode = collection[index];
        }
        return interactingNode;
      }
      if (collection.IsEmpty<EventTrigger>())
      {
        collection.Clear();
        foreach (Godot.Node overlappingBody in this.nInteractions.GetOverlappingBodies())
        {
          if (overlappingBody.GetParent() is EventTrigger parent && parent.Trigger == EventTrigger.ETrigger.Action && Game.Events.HasMapping(parent.Name, this.SpriteDirectionTemp))
            collection.Add(parent);
        }
        if (collection.Count == 1)
          return collection[0];
        if (OS.IsDebugBuild() && collection.Count > 1)
          Log.Warn((object) "Attention: Player is facing 2 interactions which are too close from one another. Consider redrawing them.");
      }
      return (EventTrigger) null;
    }

    public EventTrigger GetItemInteractingNode(string itemId)
    {
      List<EventTrigger> collection = new List<EventTrigger>();
      foreach (Godot.Node overlappingArea in this.nFloorDetection.GetOverlappingAreas())
      {
        if (overlappingArea.GetParent() is EventTrigger parent && (parent.Trigger == EventTrigger.ETrigger.Action || parent.Trigger == EventTrigger.ETrigger.Item) && Game.Events.HasItemObjectMapping(itemId, parent.Name))
          collection.Add(parent);
      }
      foreach (Godot.Node overlappingBody in this.nThinInteractions.GetOverlappingBodies())
      {
        if (overlappingBody.GetParent() is EventTrigger parent && (parent.Trigger == EventTrigger.ETrigger.Action || parent.Trigger == EventTrigger.ETrigger.Item) && Game.Events.HasItemObjectMapping(itemId, parent.Name))
          collection.Add(parent);
      }
      if (collection.Count == 1)
        return collection[0];
      if (collection.Count > 1)
      {
        EventTrigger itemInteractingNode = collection[0];
        for (int index = 1; index < collection.Count; ++index)
        {
          if (collection[index].Priority > itemInteractingNode.Priority)
            itemInteractingNode = collection[index];
        }
        return itemInteractingNode;
      }
      if (collection.IsEmpty<EventTrigger>())
      {
        collection.Clear();
        foreach (Godot.Node overlappingBody in this.nInteractions.GetOverlappingBodies())
        {
          if (overlappingBody.GetParent() is EventTrigger parent && (parent.Trigger == EventTrigger.ETrigger.Action || parent.Trigger == EventTrigger.ETrigger.Item) && Game.Events.HasItemObjectMapping(itemId, parent.Name))
            collection.Add(parent);
        }
        if (collection.Count == 1)
          return collection[0];
        if (OS.IsDebugBuild() && collection.Count > 1)
          Log.Warn((object) "Attention: Player is facing 2 interactions which are too close from one another. Consider redrawing them.");
      }
      return (EventTrigger) null;
    }

    public void CheckAndRunInstantEvents(Area2D area)
    {
      if (this.AutoMovement || !(area.GetParent() is EventTrigger parent) || parent.Trigger != EventTrigger.ETrigger.Touch || !Game.Events.HasInstantMapping(parent.Name))
        return;
      Game.Events.ExecuteInstantMapping(parent.Name);
    }

    public void SetLight(Light2D light)
    {
      this.nLight.DeleteIfValid();
      if (light != null)
        this.AddChild((Godot.Node) light);
      this.nLight = light;
    }

    public Light2D GetLight() => this.nLight;

    public LacieEngine.API.SpriteState GetReflectedSprite()
    {
      return new LacieEngine.API.SpriteState()
      {
        Name = this.Name,
        Texture = this.nSprite.Texture,
        Frame = this.GetReflectedFrame(),
        HFrames = this.nSprite.Hframes,
        VFrames = this.nSprite.Vframes,
        Pos = this.VisualNode.GlobalPosition
      };
    }

    private int GetReflectedFrame()
    {
      if (this.nSprite.State == "stand" || this.nSprite.State == "walk" || this.nSprite.State == "run")
      {
        int reflectedFrame;
        switch (this.SpriteDirectionTemp.ToEnum())
        {
          case Direction.Enum.Left:
            reflectedFrame = this.nSprite.Frame + 9;
            break;
          case Direction.Enum.Up:
            reflectedFrame = this.nSprite.Frame - 27;
            break;
          case Direction.Enum.Right:
            reflectedFrame = this.nSprite.Frame - 9;
            break;
          case Direction.Enum.Down:
            reflectedFrame = this.nSprite.Frame + 27;
            break;
          default:
            reflectedFrame = 0;
            break;
        }
        return reflectedFrame;
      }
      return this.nSprite.State == "idle" ? (this.SpriteDirectionTemp.ToEnum() != Direction.Enum.Up ? this.nSprite.Frame + this.nSprite.Hframes : this.nSprite.Frame - this.nSprite.Hframes) : 0;
    }

    public void SetPlayerState(CharacterState state) => this.State = state;

    public CharacterState GetPlayerState() => this.State;

    private void DelimitFollowableSegment()
    {
      this._currentFollowableSegment = new IFollowable.Segment(this.NewDirection, this.Position);
      this.FollowableSegments.Add(this._currentFollowableSegment);
    }

    public bool IsRunning() => this.State == CharacterState.Running;

    public bool CollisionEnabled
    {
      get => this.CollisionLayer > 0U;
      set
      {
        if (!value)
        {
          this.CollisionMask = 0U;
          this.CollisionLayer = 0U;
        }
        else
        {
          this.CollisionLayer = this._collisionLayer;
          this.CollisionMask = this._collisionMask;
        }
      }
    }
  }
}
