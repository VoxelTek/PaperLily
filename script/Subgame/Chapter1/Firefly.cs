using System;
using Godot;

namespace LacieEngine.Subgame.Chapter1
{
	public class Firefly : Node2D
	{
		[Export(PropertyHint.None, "")]
		public float Speed = 5f;

		[Export(PropertyHint.None, "")]
		public float Spread = 160f;

		private Random random = new Random();

		private Vector2 _direction = Vector2.One;

		private Vector2 _spawnPoint;

		public override void _Ready()
		{
			_spawnPoint = base.Position;
		}

		public override void _Process(float delta)
		{
			if (base.Position.DistanceTo(_spawnPoint) < Spread)
			{
				_direction = _direction.Rotated((float)(random.NextDouble() - 0.5));
			}
			else
			{
				_direction = base.Position.DirectionTo(_spawnPoint);
			}
			base.Position += delta * Speed * _direction;
		}
	}
}
