// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.LsbEventProvider
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Injectable]
  public class LsbEventProvider : IEventProvider, IModule, ITranslatable
  {
    private Dictionary<string, StoryPlayerEvent[]> _events;
    private ISet<string> _eventIds;

    public void Init()
    {
      Log.Info((object) "Loading events...");
      Dictionary<string, List<StoryPlayerEvent>> dictionary = new Dictionary<string, List<StoryPlayerEvent>>();
      this._eventIds = (ISet<string>) new HashSet<string>();
      foreach (string filename in GDUtil.ListFilesInPath("res://definitions/events/", ".lsb"))
      {
        foreach (StoryPlayerEvent @event in GDUtil.ReadBinaryFile(filename, Game.Settings.LsbKey) as StoryPlayerEvent[])
        {
          this.ProcessEvent(@event);
          if (!dictionary.ContainsKey(@event.Room))
            dictionary[@event.Room] = new List<StoryPlayerEvent>();
          dictionary[@event.Room].Add(@event);
          this._eventIds.Add(@event.Id);
        }
      }
      this._events = new Dictionary<string, StoryPlayerEvent[]>();
      foreach (string key in dictionary.Keys)
        this._events[key] = dictionary[key].ToArray();
    }

    public StoryPlayerEvent[] GetEventsForRoom(string room)
    {
      return this._events.ContainsKey(room) ? this._events[room] : Array.Empty<StoryPlayerEvent>();
    }

    public StoryPlayerEvent LoadEvent(string name, string room = null)
    {
      foreach (StoryPlayerEvent storyPlayerEvent in this._events[room.IsNullOrEmpty() ? "Global" : room])
      {
        if (storyPlayerEvent.Name == name)
          return storyPlayerEvent;
      }
      Log.Error((object) "Event not found: ", (object) room, (object) ".", (object) name);
      return (StoryPlayerEvent) null;
    }

    public bool EventExists(string eventId) => this._eventIds.Contains(eventId);

    public StoryPlayerEvent LoadEventFromPath(string _)
    {
      throw new NotImplementedException("LSB Event Provider should not be called from plugin!");
    }

    public void ApplyTranslationOverrides()
    {
      foreach (string key in this._events.Keys)
      {
        foreach (StoryPlayerEvent @event in this._events[key])
          this.OverrideCaptions(@event);
      }
    }

    private void ProcessEvent(StoryPlayerEvent @event)
    {
      foreach (StoryPlayerCommand storyPlayerCommand in @event.Dialogue)
      {
        Injector.Resolve((object) storyPlayerCommand);
        this.OverrideCaptions(@event);
        storyPlayerCommand.Event = @event;
      }
    }

    private void OverrideCaptions(StoryPlayerEvent @event)
    {
      if (!Game.Language.TranslationEnabled)
        return;
      ICaptionSet storyCaptions = Game.Language.GetStoryCaptions(@event.Room);
      if (storyCaptions == null)
        return;
      foreach (StoryPlayerCommand storyPlayerCommand in @event.Dialogue)
        storyPlayerCommand.OverrideCaptions(storyCaptions);
    }
  }
}
