using System;
using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

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
			-Math.PI / 4.0,
			Math.PI / 2.0,
			-Math.PI / 2.0,
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
			base.CharacterId = "ch1_bunny";
			base.DefaultDirection = Direction.Right;
			base._EnterTree();
			nNavigation = new NavigationAgent2D();
			AddChild(nNavigation);
			if (CanInteract)
			{
				nInteractEvent = GDUtil.MakeNode<EventTriggerCircular>(EventWhileRunning);
				nInteractEvent.Area = Vector2.One * 16f;
				nInteractEvent.Solid = false;
				nInteractEvent.Event = EventWhileRunning;
				AddChild(nInteractEvent);
			}
			if (CanMelt)
			{
				nPlayerTouchArea = GDUtil.MakeNode<Area2D>("MeltTrigger");
				nPlayerTouchArea.AddChild(GDUtil.MakeCollisionCircle(64f));
				nPlayerTouchArea.CollisionLayer = 0u;
				nPlayerTouchArea.CollisionMask = 4u;
				nPlayerTouchArea.Connect("body_entered", this, "Defeat", null, 4u);
				AddChild(nPlayerTouchArea);
			}
			Timer timer = GDUtil.MakeNode<Timer>("RouteChangeTimer");
			timer.WaitTime = 0.2f;
			timer.Autostart = true;
			timer.Connect("timeout", this, "CheckRoute");
			AddChild(timer);
		}

		public void CheckRoute()
		{
			if (_defeated || Game.Player == null || !Game.Player.Node.IsValid())
			{
				return;
			}
			if (base.Position.DistanceSquaredTo(Game.Player.Node.Position) <= 22500f)
			{
				SetEscapeRoute();
				AutoMovement = true;
			}
			else if (base.Position.DistanceSquaredTo(Game.Player.Node.Position) > 90000f)
			{
				nNavigation.SetTargetLocation(base.Position);
				AutoMovement = false;
			}
			if (AutoMovement)
			{
				Vector2 nextLocation = nNavigation.GetNextLocation();
				if (Math.Abs(nextLocation.x - base.Position.x) <= 1f)
				{
					base.Position = new Vector2(nextLocation.x, base.Position.y);
				}
				if (Math.Abs(nextLocation.y - base.Position.y) <= 1f)
				{
					base.Position = new Vector2(base.Position.x, nextLocation.y);
				}
				WalkToPoint(nextLocation);
			}
		}

		private void SetEscapeRoute()
		{
			Vector2 dest = Direction.FromVectorAnalog(base.Position.DirectionTo(Game.Player.Node.Position)).ToVector() * -1f * 200f;
			double[] dIRECTIONS_TO_FLEE = DIRECTIONS_TO_FLEE;
			foreach (double rotation in dIRECTIONS_TO_FLEE)
			{
				nNavigation.SetTargetLocation(dest.Rotated((float)rotation) + base.Position);
				if (nNavigation.IsTargetReachable())
				{
					break;
				}
			}
		}

		public void SetNavigation(Navigation2D navigation)
		{
			nNavigation.SetNavigation(navigation);
		}

		public void ConnectChaseCatch(Node target, string method)
		{
			nPlayerTouchArea.Connect("body_entered", target, method, null, 4u);
		}

		public override void CheckIfAutoMovementOver()
		{
			if (base.Position.DistanceTo(nNavigation.GetTargetLocation()) < 64f)
			{
				FinishAutoMovement();
			}
			void FinishAutoMovement()
			{
				AutoMovement = false;
				base.Position = AutoDestination;
				UpdateState();
				AutoCallback?.Invoke();
			}
		}

		public void Defeat(Node _)
		{
			_defeated = true;
			AutoMovement = false;
			nInteractEvent.DeleteIfValid();
			base.SpriteState = "melt";
			SetProcess(enable: false);
			SetPhysicsProcess(enable: false);
			nSprite.Reparent(Game.Room.CurrentRoom.GetNode("Background"));
			nSprite.Position = base.Position;
			if (SfxMelt != null)
			{
				Game.Audio.PlaySfx(SfxMelt);
			}
			EventTriggerCircular bunnyEvt = new EventTriggerCircular();
			bunnyEvt.Area = new Vector2(16f, 16f);
			bunnyEvt.Solid = false;
			bunnyEvt.Event = EventAfterMelt;
			bunnyEvt.Name = EventAfterMelt;
			this.AddChildDeferred(bunnyEvt);
		}
	}
}
