// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerMapping
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;

#nullable disable
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

    public StoryPlayerMapping(
      string nodeName,
      string eventName,
      StoryPlayerEventTrigger trigger,
      Direction direction)
    {
      this.Node = nodeName;
      this.Event = eventName;
      this.Trigger = trigger;
      this.Direction = direction;
    }

    public bool IsDirectionValid(Direction direction)
    {
      return this.Direction == Direction.None || ((int) (byte) this.Direction & (int) (byte) direction) == (int) (byte) direction;
    }
  }
}
