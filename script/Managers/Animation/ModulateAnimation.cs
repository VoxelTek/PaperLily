using Godot;

namespace LacieEngine.Animation
{
	public class ModulateAnimation : LacieAnimation
	{
		private Color _initialColor;

		private Color _finalColor;

		private CanvasItem _node;

		private float _trackR;

		private float _trackG;

		private float _trackB;

		private float _trackA;

		public ModulateAnimation(CanvasItem node, float duration, Color initialColor, Color finalColor)
		{
			base.Node = (_node = node);
			base.Duration = duration;
			_initialColor = initialColor;
			_finalColor = finalColor;
		}

		public override void InitialCalculations()
		{
			_trackR = _finalColor.r - _initialColor.r;
			_trackG = _finalColor.g - _initialColor.g;
			_trackB = _finalColor.b - _initialColor.b;
			_trackA = _finalColor.a - _initialColor.a;
		}

		public override void UpdateToFirstFrame()
		{
			_node.Modulate = _initialColor;
		}

		public override void UpdateToCurrentFrame()
		{
			float progress = base.Elapsed / base.Duration;
			_node.Modulate = new Color(_initialColor.r + _trackR * progress, _initialColor.g + _trackG * progress, _initialColor.b + _trackB * progress, _initialColor.a + _trackA * progress);
		}

		public override void UpdateToFinalFrame()
		{
			_node.Modulate = _finalColor;
		}
	}
}
