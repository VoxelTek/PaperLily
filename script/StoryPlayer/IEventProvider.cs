using LacieEngine.API;

namespace LacieEngine.StoryPlayer
{
	[InjectableInterface(unique = true)]
	public interface IEventProvider : IModule
	{
		StoryPlayerEvent[] GetEventsForRoom(string room);

		StoryPlayerEvent LoadEvent(string name, string room = null);

		StoryPlayerEvent LoadEventFromPath(string path);

		bool EventExists(string eventId);
	}
}
