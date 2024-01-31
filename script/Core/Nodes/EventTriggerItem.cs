// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.EventTriggerItem
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
  public class EventTriggerItem : EventTriggerHint
  {
    [Export(PropertyHint.MultilineText, "")]
    public string Items { get; set; } = "";

    [Export(PropertyHint.MultilineText, "")]
    public string Nodes { get; set; } = "";

    public override void _EnterTree()
    {
      if (Engine.EditorHint || Game.Room.Cutscene)
        return;
      foreach (string splitLine1 in this.Items.SplitLines())
      {
        foreach (string splitLine2 in this.Nodes.SplitLines())
          Game.Events.AddItemTrigger(splitLine1, splitLine2, this.EffectiveEventName);
      }
    }
  }
}
