using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Subgame.Chapter1
{
	public class Ch1MkBlessingChaser : NPCChaser
	{
		public const int INTERACT_RADIUS = 6;

		public const float LIFETIME = 15f;

		private NavigationAgent2D nNavigation;

		private float _elapsed;

		public Ch1MkBlessingChaser()
			: base("ch1_mk_ghost")
		{
		}

		public override void _EnterTree()
		{
			base.DefaultDirection = Direction.Right;
			base.TouchHitboxRadius = 6;
			base._EnterTree();
			nNavigation = new NavigationAgent2D();
			AddChild(nNavigation);
		}
	}
}
