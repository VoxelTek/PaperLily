using Godot;

namespace LacieEngine.Animation
{
	public class SlideOutLeftAnimation : SlideOutAnimation
	{
		public SlideOutLeftAnimation(Control node, float? displacement = null, Vector2? from = null, float duration = 0.15f)
			: base(node, displacement, from, duration)
		{
		}

		public override void InitialCalculations()
		{
			_from = pFrom ?? _node.RectPosition;
			_to = _from - new Vector2(_pDisplacement ?? _node.RectSize.x, 0f);
			_track = _to - _from;
		}
	}
}
