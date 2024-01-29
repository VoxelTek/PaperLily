using Godot;

namespace LacieEngine.Animation
{
	public class SlideInRightAnimation : SlideInAnimation
	{
		public SlideInRightAnimation(Control node, float? displacement = null, Vector2? to = null, float duration = 0.15f)
			: base(node, displacement, to, duration)
		{
		}

		public override void InitialCalculations()
		{
			_to = _pTo ?? _node.RectPosition;
			_from = _to + new Vector2(_pDisplacement ?? _node.RectSize.x, 0f);
			_track = _to - _from;
		}
	}
}
