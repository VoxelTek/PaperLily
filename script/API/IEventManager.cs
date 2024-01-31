// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.IEventManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using LacieEngine.StoryPlayer;

#nullable disable
namespace LacieEngine.API
{
  [InjectableInterface(unique = true)]
  public interface IEventManager : IModule
  {
    LacieEngine.StoryPlayer.StoryPlayer StoryPlayer { get; }

    void ClearMappings();

    void LoadMappings(string room);

    bool Exists(string eventId);

    bool HasMapping(string objectId, Direction direction);

    bool HasMapping(string objectId);

    bool HasInstantMapping(string objectId);

    bool HasItemMapping(string itemId);

    bool HasItemObjectMapping(string itemId, string objectId);

    void ExecuteMapping(string objectId);

    void ExecuteInstantMapping(string objectId);

    void ExecuteItemMapping(string itemId);

    void ExecuteItemObjectMapping(string itemId, string objectId);

    StoryPlayerEvent GetEvent(string name);

    void PlayEvent(string evt);

    void PlayOnEnterEvents();

    void UpdateBannedEvents();

    bool SeenEvent(string eventId);

    int GetEventInteractionCount(string eventId);

    void AddToEventInteractionCount(string eventId);

    void AddTrigger(
      string nodeName,
      string eventName,
      StoryPlayerEventTrigger trigger,
      Direction direction);

    void AddActionTrigger(string nodeName, string eventName);

    void AddContactTrigger(string nodeName, string eventName);

    void AddItemTrigger(string itemId, string nodeName, string eventName);

    void AddItemRoomTrigger(string itemId, string eventName);

    void AddOnEnterTrigger(string eventName);

    void InitStoryPlayer();

    void PreloadEvent(string eventName);
  }
}
