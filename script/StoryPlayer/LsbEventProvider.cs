using System;
using System.Collections.Generic;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Injectable]
	public class LsbEventProvider : IEventProvider, IModule, ITranslatable
	{
		private Dictionary<string, StoryPlayerEvent[]> _events;

		private ISet<string> _eventIds;

		public void Init()
		{
			Log.Info("Loading events...");
			Dictionary<string, List<StoryPlayerEvent>> tempEvents = new Dictionary<string, List<StoryPlayerEvent>>();
			_eventIds = new HashSet<string>();
			foreach (string item in GDUtil.ListFilesInPath("res://definitions/events/", ".lsb"))
			{
				StoryPlayerEvent[] array = GDUtil.ReadBinaryFile(item, Game.Settings.LsbKey) as StoryPlayerEvent[];
				foreach (StoryPlayerEvent @event in array)
				{
					ProcessEvent(@event);
					if (!tempEvents.ContainsKey(@event.Room))
					{
						tempEvents[@event.Room] = new List<StoryPlayerEvent>();
					}
					tempEvents[@event.Room].Add(@event);
					_eventIds.Add(@event.Id);
				}
			}
			_events = new Dictionary<string, StoryPlayerEvent[]>();
			foreach (string pack in tempEvents.Keys)
			{
				_events[pack] = tempEvents[pack].ToArray();
			}
		}

		public StoryPlayerEvent[] GetEventsForRoom(string room)
		{
			if (_events.ContainsKey(room))
			{
				return _events[room];
			}
			return Array.Empty<StoryPlayerEvent>();
		}

		public StoryPlayerEvent LoadEvent(string name, string room = null)
		{
			string eventFolder = (room.IsNullOrEmpty() ? "Global" : room);
			StoryPlayerEvent[] array = _events[eventFolder];
			foreach (StoryPlayerEvent @event in array)
			{
				if (@event.Name == name)
				{
					return @event;
				}
			}
			Log.Error("Event not found: ", room, ".", name);
			return null;
		}

		public bool EventExists(string eventId)
		{
			return _eventIds.Contains(eventId);
		}

		public StoryPlayerEvent LoadEventFromPath(string _)
		{
			throw new NotImplementedException("LSB Event Provider should not be called from plugin!");
		}

		public void ApplyTranslationOverrides()
		{
			foreach (string pack in _events.Keys)
			{
				StoryPlayerEvent[] array = _events[pack];
				foreach (StoryPlayerEvent @event in array)
				{
					OverrideCaptions(@event);
				}
			}
		}

		private void ProcessEvent(StoryPlayerEvent @event)
		{
			StoryPlayerCommand[] dialogue = @event.Dialogue;
			foreach (StoryPlayerCommand obj in dialogue)
			{
				Injector.Resolve(obj);
				OverrideCaptions(@event);
				obj.Event = @event;
			}
		}

		private void OverrideCaptions(StoryPlayerEvent @event)
		{
			if (!Game.Language.TranslationEnabled)
			{
				return;
			}
			ICaptionSet captions = Game.Language.GetStoryCaptions(@event.Room);
			if (captions != null)
			{
				StoryPlayerCommand[] dialogue = @event.Dialogue;
				for (int i = 0; i < dialogue.Length; i++)
				{
					dialogue[i].OverrideCaptions(captions);
				}
			}
		}
	}
}
