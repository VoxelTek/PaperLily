using Godot;

namespace LacieEngine.Animation
{
	public class ShaderProgressAnimation : LacieAnimation
	{
		private CanvasItem _node;

		private bool _invert;

		public ShaderProgressAnimation(CanvasItem node, float duration, bool invert = false)
		{
			base.Node = (_node = node);
			base.Duration = duration;
			_invert = invert;
		}

		public override void UpdateToFirstFrame()
		{
			((ShaderMaterial)_node.Material).SetShaderParam("progress", _invert ? 1 : 0);
		}

		public override void UpdateToCurrentFrame()
		{
			float progress = base.Elapsed / base.Duration;
			if (_invert)
			{
				progress = 1f - progress;
			}
			((ShaderMaterial)_node.Material).SetShaderParam("progress", progress);
		}

		public override void UpdateToFinalFrame()
		{
			((ShaderMaterial)_node.Material).SetShaderParam("progress", (!_invert) ? 1 : 0);
		}
	}
}
