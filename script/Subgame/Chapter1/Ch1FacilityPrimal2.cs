using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Subgame.Chapter1
{
	public class Ch1FacilityPrimal2 : NPCChaser
	{
		public const int INTERACT_RADIUS = 8;

		public const float MOVE_SPEED = 0.6f;

		private NavigationAgent2D nNavigation;

		public Ch1FacilityPrimal2()
			: base("ch1_primal_2")
		{
		}

		public override void _EnterTree()
		{
			base.DefaultDirection = Direction.Right;
			base.TouchHitboxRadius = 8;
			base.MoveSpeedMultiplier = 0.6f;
			base._EnterTree();
			nNavigation = new NavigationAgent2D();
			AddChild(nNavigation);
		}
	}
}
