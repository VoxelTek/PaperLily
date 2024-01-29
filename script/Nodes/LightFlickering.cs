using System;
using Godot;

namespace LacieEngine.Nodes
{
	[Tool]
	public class LightFlickering : Light
	{
		private const double FPS = 4.0;

		private const float LOW_ENERGY = 0.96f;

		private float _time;

		private double _frequency;

		private double _halfAmplitude;

		public override void _Ready()
		{
			base._Ready();
			_frequency = Math.PI * 8.0;
			_halfAmplitude = (base.Energy - base.Energy * 0.96f) / 2f;
		}

		public override void _Process(float delta)
		{
			if (!Engine.EditorHint)
			{
				_time += delta;
				base.Energy = (float)(Math.Cos((double)_time * _frequency) * _halfAmplitude + (1.0 - _halfAmplitude));
			}
		}
	}
}
