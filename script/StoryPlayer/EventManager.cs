using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Injectable]
	public class EventManager : IEventManager, IModule, ITranslatable
	{
		public class EventBundle
		{
			public string Room;

			public Dictionary<string, StoryPlayerEvent> Events;

			public Dictionary<string, StoryPlayerMapping> Mappings;

			public Dictionary<string, string> ItemMappings;

			public List<StoryPlayerItemMapping> ItemObjectMappings;

			public List<string> OnEnter;

			public Dictionary<string, List<LogicStatement>> EventPrerequisites;

			public HashSet<string> BannedEvents;
		}

		private Lazy<IEventProvider> _lazyEventProvider = new Lazy<IEventProvider>(() => Injector.Get<IEventProvider>());

		private EventBundle curEvents;

		private Task _bannedEventsUpdateTask;

		public StoryPlayer StoryPlayer { get; private set; }

		private IEventProvider EventProvider => _lazyEventProvider.Value;

		public void ClearMappings()
		{
			curEvents = CreateEventBundle();
		}

		public void LoadMappings(string room)
		{
			LoadEventBundle(curEvents, room);
		}

		public bool Exists(string eventId)
		{
			if (eventId.Contains("."))
			{
				return EventProvider.EventExists(eventId);
			}
			if (!EventProvider.EventExists("Global." + eventId))
			{
				if (curEvents != null)
				{
					return curEvents.Events.ContainsKey(eventId);
				}
				return false;
			}
			return true;
		}

		public bool HasMapping(string objectId, Direction direction)
		{
			StoryPlayerMapping mapping = GetMapping(objectId);
			if (mapping != null && mapping.IsDirectionValid(direction))
			{
				return EventCanPlay(mapping.Event);
			}
			return false;
		}

		public bool HasMapping(string objectId)
		{
			return HasMapping(objectId, Direction.None);
		}

		public bool HasInstantMapping(string objectId)
		{
			string eventId = GetInstantMapping(objectId);
			if (eventId != null)
			{
				return EventCanPlay(eventId);
			}
			return false;
		}

		public bool HasItemMapping(string itemId)
		{
			string eventId = GetItemMapping(itemId);
			if (eventId != null)
			{
				return EventCanPlay(eventId);
			}
			return false;
		}

		public bool HasItemObjectMapping(string itemId, string objectId)
		{
			string eventId = GetItemObjectMapping(itemId, objectId);
			if (eventId != null)
			{
				return EventCanPlay(eventId);
			}
			return false;
		}

		public void ExecuteMapping(string objectId)
		{
			Log.Trace("Executing mapping for ", objectId);
			if (!HasMapping(objectId))
			{
				Log.Warn("There is no event mapping for node ", objectId);
			}
			else
			{
				PlayEvent(GetMapping(objectId).Event);
			}
		}

		public void ExecuteInstantMapping(string objectId)
		{
			Log.Trace("Executing instant mapping for ", objectId);
			if (!HasInstantMapping(objectId))
			{
				Log.Warn("There is no instant event mapping for node ", objectId);
			}
			else
			{
				PlayEvent(GetInstantMapping(objectId));
			}
		}

		public void ExecuteItemMapping(string itemId)
		{
			if (!HasItemMapping(itemId))
			{
				Log.Warn("There is no item mapping for item ", itemId);
			}
			else
			{
				PlayEvent(GetItemMapping(itemId));
			}
		}

		public void ExecuteItemObjectMapping(string itemId, string objectId)
		{
			if (!HasItemObjectMapping(itemId, objectId))
			{
				Log.Warn("There is no item event mapping for item ", itemId, " and node ", objectId);
			}
			else
			{
				PlayEvent(GetItemObjectMapping(itemId, objectId));
			}
		}

		public StoryPlayerEvent GetEvent(string name)
		{
			StoryPlayerEvent @event = null;
			if (curEvents != null)
			{
				curEvents.Events.TryGetValue(name, out @event);
				if (@event == null)
				{
					curEvents.Events.TryGetValue("Global." + name, out @event);
				}
			}
			if (@event == null && GlobalEventExists(name))
			{
				@event = EventProvider.LoadEvent(name);
			}
			return @event;
		}

		public void PlayEvent(string evt)
		{
			if (EventCanPlay(evt))
			{
				StoryPlayerEvent @event = GetEvent(evt);
				if (@event == null)
				{
					Log.Warn("Mapped event does not exist: ", evt);
				}
				else
				{
					Game.Screen.CloseMenu();
					StoryPlayer.Play(@event);
				}
			}
		}

		public void PlayOnEnterEvents()
		{
			foreach (string evt in curEvents.OnEnter)
			{
				PlayEvent(evt);
			}
		}

		public void UpdateBannedEvents()
		{
			if (_bannedEventsUpdateTask == null || _bannedEventsUpdateTask.IsCompleted)
			{
				_bannedEventsUpdateTask = Task.Run((Action)UpdateBannedEventsProc);
			}
		}

		private void UpdateBannedEventsProc()
		{
			curEvents.BannedEvents.Clear();
			foreach (string eventName in curEvents.EventPrerequisites.Keys)
			{
				Log.Trace("Checking prerequisites for event: ", eventName);
				foreach (LogicStatement item in curEvents.EventPrerequisites[eventName])
				{
					if (item.Evaluate())
					{
						curEvents.BannedEvents.Add(eventName);
					}
				}
			}
		}

		public bool SeenEvent(string eventId)
		{
			return GetEventInteractionCount(eventId) > 0;
		}

		public int GetEventInteractionCount(string eventId)
		{
			if (Game.State.Interactions.ContainsKey(eventId))
			{
				return Game.State.Interactions[eventId];
			}
			return 0;
		}

		public void AddToEventInteractionCount(string eventId)
		{
			Game.State.Interactions[eventId] = GetEventInteractionCount(eventId) + 1;
		}

		public void AddTrigger(string nodeName, string eventName, StoryPlayerEventTrigger trigger, Direction direction)
		{
			curEvents.Mappings[nodeName] = new StoryPlayerMapping(nodeName, eventName, trigger, direction);
			LoadEventIntoBundle(eventName, curEvents);
		}

		public void AddActionTrigger(string nodeName, string eventName)
		{
			curEvents.Mappings[nodeName] = new StoryPlayerMapping(nodeName, eventName);
			LoadEventIntoBundle(eventName, curEvents);
		}

		public void AddContactTrigger(string nodeName, string eventName)
		{
			curEvents.Mappings[nodeName] = new StoryPlayerMapping(nodeName, eventName, StoryPlayerEventTrigger.Touch);
			LoadEventIntoBundle(eventName, curEvents);
		}

		public void AddItemTrigger(string itemId, string nodeName, string eventName)
		{
			StoryPlayerItemMapping itemMapping = new StoryPlayerItemMapping();
			itemMapping.Object = nodeName;
			itemMapping.Event = eventName;
			itemMapping.Item = itemId;
			curEvents.ItemObjectMappings.Add(itemMapping);
			LoadEventIntoBundle(eventName, curEvents);
		}

		public void AddItemRoomTrigger(string itemId, string eventName)
		{
			curEvents.ItemMappings[itemId] = eventName;
			LoadEventIntoBundle(eventName, curEvents);
		}

		public void AddOnEnterTrigger(string eventName)
		{
			curEvents.OnEnter.Add(eventName);
			LoadEventIntoBundle(eventName, curEvents);
		}

		public void PreloadEvent(string eventName)
		{
			LoadEventIntoBundle(eventName, curEvents);
		}

		public void ApplyTranslationOverrides()
		{
		}

		private EventBundle CreateEventBundle()
		{
			return new EventBundle
			{
				Events = new Dictionary<string, StoryPlayerEvent>(),
				Mappings = new Dictionary<string, StoryPlayerMapping>(),
				ItemMappings = new Dictionary<string, string>(),
				ItemObjectMappings = new List<StoryPlayerItemMapping>(),
				OnEnter = new List<string>(),
				EventPrerequisites = new Dictionary<string, List<LogicStatement>>(),
				BannedEvents = new HashSet<string>()
			};
		}

		private EventBundle LoadEventBundle(EventBundle eventBundle, string room)
		{
			eventBundle.Room = room;
			StoryPlayerEvent[] eventsForRoom = EventProvider.GetEventsForRoom(room);
			foreach (StoryPlayerEvent @event in eventsForRoom)
			{
				LoadEventIntoBundle(@event, eventBundle);
			}
			return eventBundle;
		}

		private void LoadEventIntoBundle(string name, EventBundle eventBundle)
		{
			if (eventBundle.Events.ContainsKey(name))
			{
				return;
			}
			Log.Trace("Searching for event: ", name, ". Curent room: ", eventBundle.Room);
			string roomName = eventBundle.Room;
			if (name.Contains("."))
			{
				string[] array = name.Split('.');
				roomName = array[0];
				name = array[1];
			}
			StoryPlayerEvent @event;
			if (EventExists(roomName, name))
			{
				@event = EventProvider.LoadEvent(name, roomName);
			}
			else
			{
				if (!GlobalEventExists(name))
				{
					Log.Warn("Attempted to load an event that doesn't exist: ", name);
					return;
				}
				@event = EventProvider.LoadEvent(name);
			}
			LoadEventIntoBundle(@event, eventBundle);
		}

		private void LoadEventIntoBundle(StoryPlayerEvent @event, EventBundle eventBundle)
		{
			if (@event.Room == eventBundle.Room)
			{
				eventBundle.Events[@event.Name] = @event;
			}
			else
			{
				eventBundle.Events[@event.Id] = @event;
			}
			LoadPrerequisites(@event, eventBundle);
			LoadRelatedEvents(@event, eventBundle);
		}

		private void LoadPrerequisites(StoryPlayerEvent @event, EventBundle eventBundle)
		{
			StoryPlayerCommand[] dialogue = @event.Dialogue;
			foreach (StoryPlayerCommand block in dialogue)
			{
				if (!(block is StoryPlayerLabelCommand))
				{
					if (!(block is StoryPlayerIfCommand ifBlock) || (ifBlock.Jump.Type != StoryPlayerFlowCommand.FlowType.End && ifBlock.Else.Type != StoryPlayerFlowCommand.FlowType.End))
					{
						break;
					}
					Log.Trace("Found prerequisite for event: ", @event.Name);
					LogicStatement statement = ifBlock.MakeLogicStatement();
					if (ifBlock.Else.Type == StoryPlayerFlowCommand.FlowType.End)
					{
						statement.Not = !statement.Not;
					}
					if (!eventBundle.EventPrerequisites.ContainsKey(@event.Name))
					{
						eventBundle.EventPrerequisites.Add(@event.Name, new List<LogicStatement>());
					}
					if (statement.Type == LogicStatement.EType.Repeat)
					{
						statement.Amount--;
					}
					eventBundle.EventPrerequisites[@event.Name].Add(statement);
				}
			}
		}

		private void LoadRelatedEvents(StoryPlayerEvent @event, EventBundle eventBundle)
		{
			StoryPlayerCommand[] dialogue = @event.Dialogue;
			for (int i = 0; i < dialogue.Length; i++)
			{
				if (dialogue[i] is StoryPlayerEventCommand eventBlock && (eventBlock.Operation == StoryPlayerEventCommand.EventOperation.Call || eventBlock.Operation == StoryPlayerEventCommand.EventOperation.Queue))
				{
					Log.Trace("Found event dependency: ", eventBlock.Value);
					LoadEventIntoBundle(eventBlock.Value, eventBundle);
				}
			}
		}

		public bool EventExists(string roomName, string eventName)
		{
			return EventProvider.EventExists(roomName + "." + eventName);
		}

		public bool GlobalEventExists(string eventName)
		{
			return EventProvider.EventExists("Global." + eventName);
		}

		private StoryPlayerMapping GetMapping(string objectId)
		{
			if (curEvents.Mappings.ContainsKey(objectId) && curEvents.Mappings[objectId].Trigger == StoryPlayerEventTrigger.Action)
			{
				return curEvents.Mappings[objectId];
			}
			return null;
		}

		private string GetInstantMapping(string objectId)
		{
			if (curEvents.Mappings.ContainsKey(objectId) && curEvents.Mappings[objectId].Trigger == StoryPlayerEventTrigger.Touch)
			{
				return curEvents.Mappings[objectId].Event;
			}
			return null;
		}

		private string GetItemMapping(string itemId)
		{
			if (curEvents.ItemMappings.ContainsKey(itemId))
			{
				return curEvents.ItemMappings[itemId];
			}
			IItem item = Game.Items.Get(itemId);
			if (item.Event != null)
			{
				return item.Event;
			}
			return null;
		}

		private string GetItemObjectMapping(string itemId, string objectId)
		{
			foreach (StoryPlayerItemMapping mapping in curEvents.ItemObjectMappings)
			{
				if (mapping.Item == itemId && mapping.Object == objectId)
				{
					return mapping.Event;
				}
			}
			return null;
		}

		private bool EventCanPlay(string eventName)
		{
			bool banned = (curEvents?.BannedEvents?.Contains(eventName)).GetValueOrDefault();
			Log.Trace("Event [", eventName, "] is banned? ", banned);
			return !banned;
		}

		public void ClearStoryPlayer()
		{
			StoryPlayer.DeleteIfValid();
			StoryPlayer = null;
		}

		public void InitStoryPlayer()
		{
			StoryPlayer.DeleteIfValid();
			StoryPlayer = GDUtil.Instance<StoryPlayer>("res://resources/nodes/common/StoryPlayer.tscn");
			Game.Screen.AddToLayer(IScreenManager.Layer.StoryPlayer, StoryPlayer);
		}
	}
}
