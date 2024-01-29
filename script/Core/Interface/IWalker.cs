using System;
using Godot;

namespace LacieEngine.Core
{
	public interface IWalker
	{
		public enum MoveSpeed
		{
			VerySlow,
			Slow,
			Normal,
			Running,
			Sprinting
		}

		string SpriteState { get; set; }

		bool Backwards { get; set; }

		void Walk(int displacementX, int displacenemtY, Direction direction, MoveSpeed speed, Action callback = null);

		void Walk(int displacementX, int displacenemtY, Direction direction, Action callback = null);

		void WalkPath(string pathName, MoveSpeed speed, Action callback = null);

		void WalkPath(string pathName, Action callback = null);

		void WalkToPoint(Vector2 point, Action callback);

		void WalkToPoint(Vector2 point);
	}
}
