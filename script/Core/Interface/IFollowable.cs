using System.Collections.Generic;
using Godot;

namespace LacieEngine.Core
{
	public interface IFollowable
	{
		public class Segment
		{
			public Direction Direction { get; }

			public Vector2 Destination { get; set; }

			public Segment(Direction direction, Vector2 destination)
			{
				Direction = direction;
				Destination = destination;
			}
		}

		List<Segment> FollowableSegments { get; }

		bool IsRunning();
	}
}
