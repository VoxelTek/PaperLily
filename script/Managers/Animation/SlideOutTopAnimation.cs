using Godot;

namespace LacieEngine.Animation
{
	public class SlideOutTopAnimation : SlideOutAnimation
	{
		public SlideOutTopAnimation(Control node, float? displacement = null, Vector2? from = null, float duration = 0.15f)
			: base(node, displacement, from, duration)
		{
		}

		public override void InitialCalculations()
		{
			_from = pFrom ?? _node.RectPosition;
			_to = _from - new Vector2(0f, _pDisplacement ?? _node.RectSize.y);
			_track = _to - _from;
		}
	}
}
