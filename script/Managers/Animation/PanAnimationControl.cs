using Godot;

namespace LacieEngine.Animation
{
	public class PanAnimationControl : LacieAnimation
	{
		private Control _node;

		private Vector2 _from;

		private Vector2 _to;

		private Vector2 _track;

		public PanAnimationControl(Control node, Vector2 from, Vector2 to, float duration)
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
			_node.RectPosition = _from;
		}

		public override void UpdateToCurrentFrame()
		{
			float progress = base.Elapsed / base.Duration;
			_node.RectPosition = _from + _track * progress;
		}

		public override void UpdateToFinalFrame()
		{
			_node.RectPosition = _to;
		}
	}
}
