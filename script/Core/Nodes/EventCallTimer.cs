// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.EventCallTimer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System;

#nullable disable
namespace LacieEngine.Core
{
  [ExportType(icon = "stopwatch")]
  public class EventCallTimer : SimpleTimer
  {
    [Export(PropertyHint.None, "")]
    public string Event { get; set; }

    public override void _Ready()
    {
      this.Callback = (Action) (() => ((PEvent) this.Event).Play());
      base._Ready();
    }
  }
}
