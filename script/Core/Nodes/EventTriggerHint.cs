// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.EventTriggerHint
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType(icon = "bolt")]
  public abstract class EventTriggerHint : Node, IEventTriggerNode
  {
    [Export(PropertyHint.None, "")]
    public string Event { get; set; } = "";

    public string EffectiveEventName => !this.Event.IsNullOrEmpty() ? this.Event : this.Name;
  }
}
