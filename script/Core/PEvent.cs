// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.PEvent
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

#nullable disable
namespace LacieEngine.Core
{
  public class PEvent
  {
    public string Id { get; private set; }

    private PEvent(string eventId) => this.Id = eventId;

    public static implicit operator PEvent(string eventId) => new PEvent(eventId);

    public void Play() => Game.Events.PlayEvent(this.Id);
  }
}
