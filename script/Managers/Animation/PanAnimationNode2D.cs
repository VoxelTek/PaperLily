using Godot;

namespace LacieEngine.Animation
{
	public class PanAnimationNode2D : LacieAnimation
	{
		private Node2D _node;

		private Vector2 _from;

		private Vector2 _to;

		private Vector2 _track;

		public PanAnimationNode2D(Node2D node, Vector2 from, Vector2 to, float duration)
		{
			base.Node = (_node = node);
			base.Duration = duration;
			_from = from;
			_to = to;
		}

		public override void InitialCalculations()
		{
			_track = _to - _from;
		}

		public override void UpdateToFirstFrame()
		{
			_node.Position = _from;
		}

		public override void UpdateToCurrentFrame()
		{
			float progress = base.Elapsed / base.Duration;
			_node.Position = _from + _track * progress;
		}

		public override void UpdateToFinalFrame()
		{
			_node.Position = _to;
		}
	}
}
