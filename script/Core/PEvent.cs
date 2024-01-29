namespace LacieEngine.Core
{
	public class PEvent
	{
		public string Id { get; private set; }

		private PEvent(string eventId)
		{
			Id = eventId;
		}

		public static implicit operator PEvent(string eventId)
		{
			return new PEvent(eventId);
		}

		public void Play()
		{
			Game.Events.PlayEvent(Id);
		}
	}
}
