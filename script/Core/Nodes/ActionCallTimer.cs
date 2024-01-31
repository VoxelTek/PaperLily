// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.ActionCallTimer
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
  public class ActionCallTimer : SimpleTimer
  {
    [Export(PropertyHint.None, "")]
    public NodePath Node { get; set; } = new NodePath();

    public override void _Ready()
    {
      if (Engine.EditorHint)
        return;
      this.Callback = (Action) (() =>
      {
        if (this.Node.IsNullOrEmpty() || !(this.GetNode(this.Node) is IAction node2))
          return;
        node2.Execute();
      });
      base._Ready();
    }
  }
}
