using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Subgame.Chapter1
{
	public class Ch1FacilityPrimal : NPCChaser
	{
		public const int INTERACT_RADIUS = 16;

		public const int DETECT_RADIUS_RUN = 160;

		public const int DETECT_RADIUS_WALK = 112;

		public const int DETECT_LENGTH_SNEAK = 128;

		public const int DETECT_WIDTH_SNEAK = 48;

		public const float MOVE_SPEED = 3.5f;

		private NavigationAgent2D nNavigation;

		private Area2D nPlayerRunDetectArea;

		private Area2D nPlayerWalkDetectArea;

		private Area2D nPlayerSneakDetectArea;

		public Ch1FacilityPrimal()
			: base("ch1_primal_1")
		{
		}

		public override void _EnterTree()
		{
			base.TouchHitboxRadius = 16;
			base.MoveSpeedMultiplier = 3.5f;
			base._EnterTree();
			nNavigation = new NavigationAgent2D();
			AddChild(nNavigation);
			nPlayerRunDetectArea = GDUtil.MakeNode<Area2D>("DetectRunTrigger");
			nPlayerRunDetectArea.AddChild(GDUtil.MakeCollisionCircle(160f));
			nPlayerRunDetectArea.CollisionLayer = 0u;
			nPlayerRunDetectArea.CollisionMask = 4u;
			AddChild(nPlayerRunDetectArea);
			nPlayerWalkDetectArea = GDUtil.MakeNode<Area2D>("DetectWalkTrigger");
			nPlayerWalkDetectArea.AddChild(GDUtil.MakeCollisionCircle(112f));
			nPlayerWalkDetectArea.CollisionLayer = 0u;
			nPlayerWalkDetectArea.CollisionMask = 4u;
			AddChild(nPlayerWalkDetectArea);
			nPlayerSneakDetectArea = GDUtil.MakeNode<Area2D>("DetectSneakTrigger");
			nPlayerSneakDetectArea.AddChild(GDUtil.MakeCollisionCapsule(new Vector2(128f, 48f), base.DefaultDirection.ToVector() * 48f));
			nPlayerSneakDetectArea.CollisionLayer = 0u;
			nPlayerSneakDetectArea.CollisionMask = 4u;
			AddChild(nPlayerSneakDetectArea);
		}

		public override void _PhysicsProcess(float delta)
		{
			if (!base.Chasing)
			{
				if (nPlayerRunDetectArea.GetOverlappingBodies().Count > 0 && Game.Player.GetPlayerState() == CharacterState.Running)
				{
					Detected();
				}
				if (nPlayerWalkDetectArea.GetOverlappingBodies().Count > 0 && (Game.Player.GetPlayerState() == CharacterState.Walking || Game.Player.GetPlayerState() == CharacterState.Running))
				{
					Detected();
				}
				if (nPlayerSneakDetectArea.GetOverlappingBodies().Count > 0)
				{
					Detected();
				}
			}
			base._PhysicsProcess(delta);
		}

		private void Detected()
		{
			base.Chasing = true;
		}
	}
}
