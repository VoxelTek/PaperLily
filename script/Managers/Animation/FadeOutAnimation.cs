using Godot;

namespace LacieEngine.Animation
{
	public class FadeOutAnimation : FadeAnimation
	{
		private CanvasItem _node;

		public FadeOutAnimation(CanvasItem node, float duration)
		{
			base.Node = (_node = node);
			base.Duration = duration;
		}

		public override void UpdateToFirstFrame()
		{
			_node.Modulate = FadeAnimation.VisibleColor;
		}

		public override void UpdateToCurrentFrame()
		{
			float progress = base.Elapsed / base.Duration;
			_node.Modulate = new Color(1f, 1f, 1f, 1f - progress);
		}

		public override void UpdateToFinalFrame()
		{
			_node.Modulate = FadeAnimation.HiddenColor;
		}
	}
}
