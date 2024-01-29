using System;
using Godot;
using LacieEngine.Core;

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
			nNavigation = new NavigationAgent2D();
			AddChild(nNavigation);
			nTriggerArea = GDUtil.MakeNode<Area2D>("Trigger");
			nTriggerArea.AddChild(GDUtil.MakeCollisionCircle(TouchHitboxRadius));
			nTriggerArea.CollisionLayer = 0u;
			nTriggerArea.CollisionMask = 4u;
			AddChild(nTriggerArea);
		}

		public override void _PhysicsProcess(float delta)
		{
			if (!Ready)
			{
				return;
			}
			if (Chasing && Game.Player != null && Game.Player.Node.IsValid())
			{
				nNavigation.SetTargetLocation(Game.Player.Node.Position);
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
				if (nTriggerArea.GetOverlappingBodies().Count > 0 && !Game.StoryPlayer.Running)
				{
					Chasing = false;
					nTriggerArea.CollisionMask = 0u;
					this.Caught();
				}
			}
			else
			{
				nNavigation.SetTargetLocation(base.Position);
				AutoMovement = false;
			}
			base._PhysicsProcess(delta);
		}

		public void SetNavigation(Navigation2D navigation)
		{
			nNavigation.SetNavigation(navigation);
		}

		public override void CheckIfAutoMovementOver()
		{
			if (base.Position.DistanceTo(nNavigation.GetTargetLocation()) < (float)TouchHitboxRadius)
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
	}
}
