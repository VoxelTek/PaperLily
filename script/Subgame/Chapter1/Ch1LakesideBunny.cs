// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1LakesideBunny
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using System;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class Ch1LakesideBunny : NPCWalker
  {
    public const int INTERACT_RADIUS = 8;
    public const int MELT_RADIUS = 64;
    public const int PLAYER_MIN_DISTANCE_SQ = 22500;
    public const int PLAYER_MAX_DISTANCE_SQ = 90000;
    private static readonly double[] DIRECTIONS_TO_FLEE = new double[6]
    {
      0.0,
      Math.PI / 4.0,
      -1.0 * Math.PI / 4.0,
      Math.PI / 2.0,
      -1.0 * Math.PI / 2.0,
      0.0
    };
    private NavigationAgent2D nNavigation;
    private Area2D nPlayerTouchArea;
    private EventTriggerCircular nInteractEvent;
    private bool _defeated;

    public string EventWhileRunning { get; set; }

    public string EventAfterMelt { get; set; }

    public bool CanInteract { get; set; }

    public bool CanMelt { get; set; }

    public AudioStream SfxMelt { get; set; }

    public override void _EnterTree()
    {
      this.CharacterId = "ch1_bunny";
      this.DefaultDirection = Direction.Right;
      base._EnterTree();
      this.nNavigation = new NavigationAgent2D();
      this.AddChild((Godot.Node) this.nNavigation);
      if (this.CanInteract)
      {
        this.nInteractEvent = GDUtil.MakeNode<EventTriggerCircular>(this.EventWhileRunning);
        this.nInteractEvent.Area = Vector2.One * 16f;
        this.nInteractEvent.Solid = false;
        this.nInteractEvent.Event = this.EventWhileRunning;
        this.AddChild((Godot.Node) this.nInteractEvent);
      }
      if (this.CanMelt)
      {
        this.nPlayerTouchArea = GDUtil.MakeNode<Area2D>("MeltTrigger");
        this.nPlayerTouchArea.AddChild((Godot.Node) GDUtil.MakeCollisionCircle(64f));
        this.nPlayerTouchArea.CollisionLayer = 0U;
        this.nPlayerTouchArea.CollisionMask = 4U;
        int num = (int) this.nPlayerTouchArea.Connect("body_entered", (Godot.Object) this, "Defeat", flags: 4U);
        this.AddChild((Godot.Node) this.nPlayerTouchArea);
      }
      Timer timer = GDUtil.MakeNode<Timer>("RouteChangeTimer");
      timer.WaitTime = 0.2f;
      timer.Autostart = true;
      int num1 = (int) timer.Connect("timeout", (Godot.Object) this, "CheckRoute");
      this.AddChild((Godot.Node) timer);
    }

    public void CheckRoute()
    {
      if (this._defeated || Game.Player == null || !Game.Player.Node.IsValid())
        return;
      Vector2 position = this.Position;
      if ((double) position.DistanceSquaredTo(Game.Player.Node.Position) <= 22500.0)
      {
        this.SetEscapeRoute();
        this.AutoMovement = true;
      }
      else
      {
        position = this.Position;
        if ((double) position.DistanceSquaredTo(Game.Player.Node.Position) > 90000.0)
        {
          this.nNavigation.SetTargetLocation(this.Position);
          this.AutoMovement = false;
        }
      }
      if (!this.AutoMovement)
        return;
      Vector2 nextLocation = this.nNavigation.GetNextLocation();
      if ((double) Math.Abs(nextLocation.x - this.Position.x) <= 1.0)
        this.Position = new Vector2(nextLocation.x, this.Position.y);
      if ((double) Math.Abs(nextLocation.y - this.Position.y) <= 1.0)
        this.Position = new Vector2(this.Position.x, nextLocation.y);
      this.WalkToPoint(nextLocation);
    }

    private void SetEscapeRoute()
    {
      Vector2 vector2 = Direction.FromVectorAnalog(this.Position.DirectionTo(Game.Player.Node.Position)).ToVector() * -1f * 200f;
      foreach (double angle in Ch1LakesideBunny.DIRECTIONS_TO_FLEE)
      {
        this.nNavigation.SetTargetLocation(vector2.Rotated((float) angle) + this.Position);
        if (this.nNavigation.IsTargetReachable())
          break;
      }
    }

    public void SetNavigation(Navigation2D navigation)
    {
      this.nNavigation.SetNavigation((Godot.Node) navigation);
    }

    public void ConnectChaseCatch(Godot.Node target, string method)
    {
      int num = (int) this.nPlayerTouchArea.Connect("body_entered", (Godot.Object) target, method, flags: 4U);
    }

    public override void CheckIfAutoMovementOver()
    {
      if ((double) this.Position.DistanceTo(this.nNavigation.GetTargetLocation()) >= 64.0)
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

    public void Defeat(Godot.Node _)
    {
      this._defeated = true;
      this.AutoMovement = false;
      this.nInteractEvent.DeleteIfValid();
      this.SpriteState = "melt";
      this.SetProcess(false);
      this.SetPhysicsProcess(false);
      this.nSprite.Reparent(Game.Room.CurrentRoom.GetNode((NodePath) "Background"));
      this.nSprite.Position = this.Position;
      if (this.SfxMelt != null)
        Game.Audio.PlaySfx(this.SfxMelt);
      EventTriggerCircular nodeToAdd = new EventTriggerCircular();
      nodeToAdd.Area = new Vector2(16f, 16f);
      nodeToAdd.Solid = false;
      nodeToAdd.Event = this.EventAfterMelt;
      nodeToAdd.Name = this.EventAfterMelt;
      this.AddChildDeferred((Godot.Node) nodeToAdd);
    }
  }
}
