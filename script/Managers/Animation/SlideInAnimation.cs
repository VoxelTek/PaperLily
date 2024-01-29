using Godot;
using LacieEngine.Core;

namespace LacieEngine.Animation
{
	public abstract class SlideInAnimation : FadeAnimation
	{
		public const float DefaultDuration = 0.15f;

		protected Control _node;

		protected Vector2 _from;

		protected Vector2 _to;

		protected Vector2 _track;

		protected Vector2? _pTo;

		protected float? _pDisplacement;

		public SlideInAnimation(Control node, float? displacement = null, Vector2? to = null, float duration = 0.15f)
		{
			base.Node = (_node = node);
			base.Duration = duration;
			_pTo = to;
			_pDisplacement = displacement;
		}

		public override void UpdateToFirstFrame()
		{
			_node.Modulate = FadeAnimation.HiddenColor;
			_node.RectPosition = _from;
		}

		public override void UpdateToCurrentFrame()
		{
			float progress = DrkieUtil.EaseOutQuad(base.Elapsed / base.Duration);
			_node.Modulate = new Color(1f, 1f, 1f, progress);
			_node.RectPosition = _from + _track * progress;
		}

		public override void UpdateToFinalFrame()
		{
			_node.Modulate = FadeAnimation.VisibleColor;
			_node.RectPosition = _to;
		}
	}
}
