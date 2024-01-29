using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	public class StoryPlayerMapping
	{
		public string Node;

		public string Event;

		public StoryPlayerEventTrigger Trigger;

		public Direction Direction;

		public StoryPlayerMapping(string eventName)
			: this(eventName, eventName)
		{
		}

		public StoryPlayerMapping(string nodeName, string eventName)
			: this(nodeName, eventName, StoryPlayerEventTrigger.Action)
		{
		}

		public StoryPlayerMapping(string nodeName, string eventName, StoryPlayerEventTrigger trigger)
			: this(nodeName, eventName, trigger, Direction.None)
		{
		}

		public StoryPlayerMapping(string nodeName, string eventName, StoryPlayerEventTrigger trigger, Direction direction)
		{
			Node = nodeName;
			Event = eventName;
			Trigger = trigger;
			Direction = direction;
		}

		public bool IsDirectionValid(Direction direction)
		{
			if (Direction == Direction.None)
			{
				return true;
			}
			return ((byte)Direction & (byte)direction) == (byte)direction;
		}
	}
}
