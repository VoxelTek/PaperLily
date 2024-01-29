using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.Animation
{
	public class BounceAnimation : LacieAnimation
	{
		public const float DEFAULT_FREQUENCY = 0.5f;

		protected Control _node;

		protected Vector2 _direction;

		protected Vector2 _originalPosition;

		protected float? _pAmplitude;

		protected double _amplitude;

		protected float _pFrequency;

		protected double _frequency;

		public BounceAnimation(Control node, Direction direction, float? amplitude = null, float? frequency = null)
		{
			base.Duration = float.MaxValue;
			base.Node = (_node = node);
			_direction = direction.ToVector();
			_pAmplitude = amplitude;
			_pFrequency = frequency ?? 0.5f;
		}

		public override void InitialCalculations()
		{
			_originalPosition = _node.RectPosition;
			_amplitude = _pAmplitude ?? (_node.RectSize.Length() / 2f);
			_frequency = Math.PI * (double)(1f / _pFrequency);
		}

		public override void UpdateToFirstFrame()
		{
		}

		public override void UpdateToCurrentFrame()
		{
			_node.RectPosition = _originalPosition + _direction * (float)Math.Abs(Math.Sin((double)base.Elapsed * _frequency) * _amplitude);
		}

		public override void UpdateToFinalFrame()
		{
		}
	}
}
