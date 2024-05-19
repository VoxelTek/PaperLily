using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	// Token: 0x020000A8 RID: 168
	[Injectable]
	public class EventManager : IEventManager, IModule, ITranslatable
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001741C File Offset: 0x0001561C
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x00017424 File Offset: 0x00015624
		public StoryPlayer StoryPlayer { get; private set; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0001742D File Offset: 0x0001562D
		private IEventProvider EventProvider {
			get {
				return this._lazyEventProvider.Value;
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001743A File Offset: 0x0001563A
		public void ClearMappings()
		{
			Log.Info("mappings cleared");
			this.curEvents = this.CreateEventBundle();
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00017448 File Offset: 0x00015648
		public void LoadMappings(string room)
		{
			this.LoadEventBundle(this.curEvents, room);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00017458 File Offset: 0x00015658
		public bool Exists(string eventId)
		{
			if (eventId.Contains("."))
			{
				return this.EventProvider.EventExists(eventId);
			}
			return this.EventProvider.EventExists("Global." + eventId) || (this.curEvents != null && this.curEvents.Events.ContainsKey(eventId));
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000174B4 File Offset: 0x000156B4
		public bool HasMapping(string objectId, Direction direction)
		{
			StoryPlayerMapping mapping = this.GetMapping(objectId);
			return mapping != null && mapping.IsDirectionValid(direction) && this.EventCanPlay(mapping.Event);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x000174E3 File Offset: 0x000156E3
		public bool HasMapping(string objectId)
		{
			return this.HasMapping(objectId, Direction.None);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x000174F4 File Offset: 0x000156F4
		public bool HasInstantMapping(string objectId)
		{
			string eventId = this.GetInstantMapping(objectId);
			return eventId != null && this.EventCanPlay(eventId);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00017518 File Offset: 0x00015718
		public bool HasItemMapping(string itemId)
		{
			string eventId = this.GetItemMapping(itemId);
			return eventId != null && this.EventCanPlay(eventId);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001753C File Offset: 0x0001573C
		public bool HasItemObjectMapping(string itemId, string objectId)
		{
			string eventId = this.GetItemObjectMapping(itemId, objectId);
			return eventId != null && this.EventCanPlay(eventId);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00017560 File Offset: 0x00015760
		public void ExecuteMapping(string objectId)
		{
			Log.Trace(new object[] { "Executing mapping for ", objectId });
			if (!this.HasMapping(objectId))
			{
				Log.Warn(new object[] { "There is no event mapping for node ", objectId });
				return;
			}
			this.PlayEvent(this.GetMapping(objectId).Event);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x000175B8 File Offset: 0x000157B8
		public void ExecuteInstantMapping(string objectId)
		{
			Log.Trace(new object[] { "Executing instant mapping for ", objectId });
			if (!this.HasInstantMapping(objectId))
			{
				Log.Warn(new object[] { "There is no instant event mapping for node ", objectId });
				return;
			}
			this.PlayEvent(this.GetInstantMapping(objectId));
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001760A File Offset: 0x0001580A
		public void ExecuteItemMapping(string itemId)
		{
			if (!this.HasItemMapping(itemId))
			{
				Log.Warn(new object[] { "There is no item mapping for item ", itemId });
				return;
			}
			this.PlayEvent(this.GetItemMapping(itemId));
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001763A File Offset: 0x0001583A
		public void ExecuteItemObjectMapping(string itemId, string objectId)
		{
			if (!this.HasItemObjectMapping(itemId, objectId))
			{
				Log.Warn(new object[] { "There is no item event mapping for item ", itemId, " and node ", objectId });
				return;
			}
			this.PlayEvent(this.GetItemObjectMapping(itemId, objectId));
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00017678 File Offset: 0x00015878
		public StoryPlayerEvent GetEvent(string name)
		{
			StoryPlayerEvent @event = null;
			if (this.curEvents != null)
			{
				Log.Info("this curEvents is not null");
				this.curEvents.Events.TryGetValue(name, out @event);
				if (@event == null)
				{
					Log.Info("this @event is null");
					this.curEvents.Events.TryGetValue("Global." + name, out @event);
				}
			}
			Log.Info("this curEvents is null");
			if (@event == null && this.GlobalEventExists(name))
			{
				Log.Info("event is null and something else");
				@event = this.EventProvider.LoadEvent(name, null);
			}
			return @event;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000176E0 File Offset: 0x000158E0
		public void PlayEvent(string evt)
		{
			Log.Info("Playing event");
			Log.Info(evt);
			if (!this.EventCanPlay(evt))
			{
				Log.Info("Cannot play event");
				return;
			}
			Log.Info("About to get event");
			StoryPlayerEvent @event = this.GetEvent(evt);
			Log.Info("About to check event exists");
			Log.Info(@event);
			if (@event == null)
			{
				Log.Info("event is null");
				//Log.Warn(new object[] { "Mapped event does not exist: ", evt });
				return;
			}
			Log.Info("About to close menu");
			Game.Screen.CloseMenu();
			Log.Info("About to start play event");
			this.StoryPlayer.Play(@event);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00017730 File Offset: 0x00015930
		public void PlayOnEnterEvents()
		{
			foreach (string evt in this.curEvents.OnEnter)
			{
				this.PlayEvent(evt);
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00017788 File Offset: 0x00015988
		public void UpdateBannedEvents()
		{
			if (this._bannedEventsUpdateTask == null || this._bannedEventsUpdateTask.IsCompleted)
			{
				this._bannedEventsUpdateTask = Task.Run(new Action(this.UpdateBannedEventsProc));
			}
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000177B8 File Offset: 0x000159B8
		private void UpdateBannedEventsProc()
		{
			this.curEvents.BannedEvents.Clear();
			foreach (string eventName in this.curEvents.EventPrerequisites.Keys)
			{
				Log.Trace(new object[] { "Checking prerequisites for event: ", eventName });
				using (List<LogicStatement>.Enumerator enumerator2 = this.curEvents.EventPrerequisites[eventName].GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Evaluate())
						{
							this.curEvents.BannedEvents.Add(eventName);
						}
					}
				}
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00017894 File Offset: 0x00015A94
		public bool SeenEvent(string eventId)
		{
			return this.GetEventInteractionCount(eventId) > 0;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000178A0 File Offset: 0x00015AA0
		public int GetEventInteractionCount(string eventId)
		{
			if (Game.State.Interactions.ContainsKey(eventId))
			{
				return Game.State.Interactions[eventId];
			}
			return 0;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x000178C6 File Offset: 0x00015AC6
		public void AddToEventInteractionCount(string eventId)
		{
			Game.State.Interactions[eventId] = this.GetEventInteractionCount(eventId) + 1;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000178E1 File Offset: 0x00015AE1
		public void AddTrigger(string nodeName, string eventName, StoryPlayerEventTrigger trigger, Direction direction)
		{
			this.curEvents.Mappings[nodeName] = new StoryPlayerMapping(nodeName, eventName, trigger, direction);
			this.LoadEventIntoBundle(eventName, this.curEvents);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001790B File Offset: 0x00015B0B
		public void AddActionTrigger(string nodeName, string eventName)
		{
			this.curEvents.Mappings[nodeName] = new StoryPlayerMapping(nodeName, eventName);
			this.LoadEventIntoBundle(eventName, this.curEvents);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00017932 File Offset: 0x00015B32
		public void AddContactTrigger(string nodeName, string eventName)
		{
			this.curEvents.Mappings[nodeName] = new StoryPlayerMapping(nodeName, eventName, StoryPlayerEventTrigger.Touch);
			this.LoadEventIntoBundle(eventName, this.curEvents);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001795C File Offset: 0x00015B5C
		public void AddItemTrigger(string itemId, string nodeName, string eventName)
		{
			StoryPlayerItemMapping itemMapping = new StoryPlayerItemMapping();
			itemMapping.Object = nodeName;
			itemMapping.Event = eventName;
			itemMapping.Item = itemId;
			this.curEvents.ItemObjectMappings.Add(itemMapping);
			this.LoadEventIntoBundle(eventName, this.curEvents);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000179A2 File Offset: 0x00015BA2
		public void AddItemRoomTrigger(string itemId, string eventName)
		{
			this.curEvents.ItemMappings[itemId] = eventName;
			this.LoadEventIntoBundle(eventName, this.curEvents);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x000179C3 File Offset: 0x00015BC3
		public void AddOnEnterTrigger(string eventName)
		{
			this.curEvents.OnEnter.Add(eventName);
			this.LoadEventIntoBundle(eventName, this.curEvents);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000179E3 File Offset: 0x00015BE3
		public void PreloadEvent(string eventName)
		{
			this.LoadEventIntoBundle(eventName, this.curEvents);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000179F2 File Offset: 0x00015BF2
		public void ApplyTranslationOverrides()
		{
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000179F4 File Offset: 0x00015BF4
		private EventManager.EventBundle CreateEventBundle()
		{
			return new EventManager.EventBundle {
				Events = new Dictionary<string, StoryPlayerEvent>(),
				Mappings = new Dictionary<string, StoryPlayerMapping>(),
				ItemMappings = new Dictionary<string, string>(),
				ItemObjectMappings = new List<StoryPlayerItemMapping>(),
				OnEnter = new List<string>(),
				EventPrerequisites = new Dictionary<string, List<LogicStatement>>(),
				BannedEvents = new HashSet<string>()
			};
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00017A54 File Offset: 0x00015C54
		private EventManager.EventBundle LoadEventBundle(EventManager.EventBundle eventBundle, string room)
		{
			eventBundle.Room = room;
			foreach (StoryPlayerEvent @event in this.EventProvider.GetEventsForRoom(room))
			{
				this.LoadEventIntoBundle(@event, eventBundle);
			}
			return eventBundle;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00017A90 File Offset: 0x00015C90
		private void LoadEventIntoBundle(string name, EventManager.EventBundle eventBundle)
		{
			if (eventBundle.Events.ContainsKey(name))
			{
				return;
			}
			Log.Trace(new object[] { "Searching for event: ", name, ". Curent room: ", eventBundle.Room });
			string roomName = eventBundle.Room;
			if (name.Contains("."))
			{
				string[] array = name.Split('.', StringSplitOptions.None);
				roomName = array[0];
				name = array[1];
			}
			StoryPlayerEvent @event;
			if (this.EventExists(roomName, name))
			{
				@event = this.EventProvider.LoadEvent(name, roomName);
			}
			else
			{
				if (!this.GlobalEventExists(name))
				{
					Log.Warn(new object[] { "Attempted to load an event that doesn't exist: ", name });
					return;
				}
				@event = this.EventProvider.LoadEvent(name, null);
			}
			this.LoadEventIntoBundle(@event, eventBundle);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00017B4C File Offset: 0x00015D4C
		private void LoadEventIntoBundle(StoryPlayerEvent @event, EventManager.EventBundle eventBundle)
		{
			if (@event.Room == eventBundle.Room)
			{
				eventBundle.Events[@event.Name] = @event;
			}
			else
			{
				eventBundle.Events[@event.Id] = @event;
			}
			this.LoadPrerequisites(@event, eventBundle);
			this.LoadRelatedEvents(@event, eventBundle);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00017BA4 File Offset: 0x00015DA4
		private void LoadPrerequisites(StoryPlayerEvent @event, EventManager.EventBundle eventBundle)
		{
			foreach (StoryPlayerCommand block in @event.Dialogue)
			{
				if (!(block is StoryPlayerLabelCommand))
				{
					StoryPlayerIfCommand ifBlock = block as StoryPlayerIfCommand;
					if (ifBlock == null || (ifBlock.Jump.Type != StoryPlayerFlowCommand.FlowType.End && ifBlock.Else.Type != StoryPlayerFlowCommand.FlowType.End))
					{
						break;
					}
					Log.Trace(new object[] { "Found prerequisite for event: ", @event.Name });
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

		// Token: 0x06000607 RID: 1543 RVA: 0x00017CA4 File Offset: 0x00015EA4
		private void LoadRelatedEvents(StoryPlayerEvent @event, EventManager.EventBundle eventBundle)
		{
			StoryPlayerCommand[] dialogue = @event.Dialogue;
			for (int i = 0; i < dialogue.Length; i++)
			{
				StoryPlayerEventCommand eventBlock = dialogue[i] as StoryPlayerEventCommand;
				if (eventBlock != null && (eventBlock.Operation == StoryPlayerEventCommand.EventOperation.Call || eventBlock.Operation == StoryPlayerEventCommand.EventOperation.Queue))
				{
					Log.Trace(new object[] { "Found event dependency: ", eventBlock.Value });
					this.LoadEventIntoBundle(eventBlock.Value, eventBundle);
				}
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00017D0D File Offset: 0x00015F0D
		public bool EventExists(string roomName, string eventName)
		{
			return this.EventProvider.EventExists(roomName + "." + eventName);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00017D26 File Offset: 0x00015F26
		public bool GlobalEventExists(string eventName)
		{
			return this.EventProvider.EventExists("Global." + eventName);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00017D3E File Offset: 0x00015F3E
		private StoryPlayerMapping GetMapping(string objectId)
		{
			if (this.curEvents.Mappings.ContainsKey(objectId) && this.curEvents.Mappings[objectId].Trigger == StoryPlayerEventTrigger.Action)
			{
				return this.curEvents.Mappings[objectId];
			}
			return null;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00017D80 File Offset: 0x00015F80
		private string GetInstantMapping(string objectId)
		{
			if (this.curEvents.Mappings.ContainsKey(objectId) && this.curEvents.Mappings[objectId].Trigger == StoryPlayerEventTrigger.Touch)
			{
				return this.curEvents.Mappings[objectId].Event;
			}
			return null;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00017DD4 File Offset: 0x00015FD4
		private string GetItemMapping(string itemId)
		{
			if (this.curEvents.ItemMappings.ContainsKey(itemId))
			{
				return this.curEvents.ItemMappings[itemId];
			}
			IItem item = Game.Items.Get(itemId);
			if (item.Event != null)
			{
				return item.Event;
			}
			return null;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00017E24 File Offset: 0x00016024
		private string GetItemObjectMapping(string itemId, string objectId)
		{
			foreach (StoryPlayerItemMapping mapping in this.curEvents.ItemObjectMappings)
			{
				if (mapping.Item == itemId && mapping.Object == objectId)
				{
					return mapping.Event;
				}
			}
			return null;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00017EA0 File Offset: 0x000160A0
		private bool EventCanPlay(string eventName)
		{
			EventManager.EventBundle eventBundle = this.curEvents;
			bool? flag;
			if (eventBundle == null)
			{
				flag = null;
			}
			else
			{
				HashSet<string> bannedEvents = eventBundle.BannedEvents;
				flag = ((bannedEvents != null) ? new bool?(bannedEvents.Contains(eventName)) : null);
			}
			bool? flag2 = flag;
			bool banned = flag2.GetValueOrDefault();
			Log.Trace(new object[] { "Event [", eventName, "] is banned? ", banned });
			return !banned;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00017F16 File Offset: 0x00016116
		public void ClearStoryPlayer()
		{
			this.StoryPlayer.DeleteIfValid();
			this.StoryPlayer = null;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00017F2A File Offset: 0x0001612A
		public void InitStoryPlayer()
		{
			this.StoryPlayer.DeleteIfValid();
			this.StoryPlayer = GDUtil.Instance<StoryPlayer>("res://resources/nodes/common/StoryPlayer.tscn");
			Game.Screen.AddToLayer(IScreenManager.Layer.StoryPlayer, this.StoryPlayer);
		}

		// Token: 0x040004D9 RID: 1241
		private Lazy<IEventProvider> _lazyEventProvider = new Lazy<IEventProvider>(() => Injector.Get<IEventProvider>());

		// Token: 0x040004DA RID: 1242
		private EventManager.EventBundle curEvents;

		// Token: 0x040004DB RID: 1243
		private Task _bannedEventsUpdateTask;

		// Token: 0x0200024B RID: 587
		public class EventBundle
		{
			// Token: 0x04000D02 RID: 3330
			public string Room;

			// Token: 0x04000D03 RID: 3331
			public Dictionary<string, StoryPlayerEvent> Events;

			// Token: 0x04000D04 RID: 3332
			public Dictionary<string, StoryPlayerMapping> Mappings;

			// Token: 0x04000D05 RID: 3333
			public Dictionary<string, string> ItemMappings;

			// Token: 0x04000D06 RID: 3334
			public List<StoryPlayerItemMapping> ItemObjectMappings;

			// Token: 0x04000D07 RID: 3335
			public List<string> OnEnter;

			// Token: 0x04000D08 RID: 3336
			public Dictionary<string, List<LogicStatement>> EventPrerequisites;

			// Token: 0x04000D09 RID: 3337
			public HashSet<string> BannedEvents;
		}
	}
}
