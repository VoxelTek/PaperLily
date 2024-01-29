using System;
using Godot;
using LacieEngine.Core;

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
			base.CharacterId = characterName;
			base.DefaultDirection = defaultDirection;
			_followDistance = followDistance;
		}

		public override void _EnterTree()
		{
			base.Name = base.CharacterId.ToPascalCase();
			base.CollisionLayer = 0u;
			base.CollisionMask = 3u;
			CreateCharacterSprite();
			nCollision = MakeCollisionNode();
			nFloorDetection = MakeFloorDetectionNode();
			AddChild(nCollision);
			AddChild(nFloorDetection);
		}

		protected override void _CharacterReady()
		{
			base.SpriteState = "stand";
			nSprite.Offset = new Vector2(0f, nSprite.Texture.GetSize().y / 8f * -1f);
			Direction = base.DefaultDirection;
			Game.Room.RegisteredNPCs[base.Name] = this;
			TurnToDefault();
			if (nTarget != null)
			{
				Ready = true;
			}
		}

		public override void _PhysicsProcess(float delta)
		{
			if (!Ready)
			{
				nTarget = (IFollowable)Game.Player;
				if (nTarget != null)
				{
					Ready = true;
				}
				return;
			}
			if (TotalDistanceToTarget() >= _followDistance)
			{
				AutoDirection = (NewDirection = nTarget.FollowableSegments[_targetStepIndex].Direction);
				AutoDestination = nTarget.FollowableSegments[_targetStepIndex].Destination;
				AutoSpeed = (nTarget.IsRunning() ? IWalker.MoveSpeed.Running : IWalker.MoveSpeed.Normal);
				AutoMovement = true;
			}
			else
			{
				AutoMovement = false;
			}
			UpdateState();
			Vector2 motion = EightDirectionalMotion();
			Vector2 actualMotion = MoveAndSlide(motion);
			if (AutoMovement)
			{
				CheckIfAutoMovementOver();
			}
			if (base.Position.DistanceTo(nTarget.FollowableSegments[_targetStepIndex].Destination) < 1f && nTarget.FollowableSegments.Count - 1 > _targetStepIndex)
			{
				_targetStepIndex++;
			}
			this.PixelSnap(actualMotion.x == 0f, actualMotion.y == 0f);
			PlayAnimations();
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

		private float TotalDistanceToTarget()
		{
			float distance = 0f;
			Vector2 measurer = base.Position;
			for (int i = _targetStepIndex; i < nTarget.FollowableSegments.Count; i++)
			{
				distance += Math.Abs(nTarget.FollowableSegments[i].Destination.DistanceTo(measurer));
				measurer = nTarget.FollowableSegments[i].Destination;
			}
			return distance;
		}
	}
}
