// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.ObjectiveVisibility2D
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType]
  public class ObjectiveVisibility2D : Godot.Node, IAction, IToggleable
  {
    [Export(PropertyHint.None, "")]
    public bool Enabled { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public string Objective { get; set; }

    [Export(PropertyHint.None, "")]
    public ObjectiveVisibility2D.ObjectiveStatus Status { get; set; }

    [Export(PropertyHint.None, "")]
    public NodePath Node { get; set; } = new NodePath();

    [Export(PropertyHint.None, "")]
    public bool Invert { get; set; }

    public override void _Ready()
    {
      if (Engine.EditorHint)
        return;
      Game.Room.RegisteredRoomUpdates.Add((IAction) this);
    }

    public void Execute()
    {
      if (!this.Enabled || this.Node.IsNullOrEmpty())
        return;
      Godot.Node node = this.GetNode(this.Node);
      bool flag1;
      switch (this.Status)
      {
        case ObjectiveVisibility2D.ObjectiveStatus.Done:
          flag1 = Game.Objectives.IsObjectiveCompleted(this.Objective);
          break;
        case ObjectiveVisibility2D.ObjectiveStatus.InProgress:
          flag1 = Game.Objectives.IsObjectiveInProgress(this.Objective);
          break;
        case ObjectiveVisibility2D.ObjectiveStatus.Failed:
          flag1 = Game.Objectives.IsObjectiveFailed(this.Objective);
          break;
        default:
          throw new NotImplementedException();
      }
      bool flag2 = flag1;
      if (this.Invert)
        flag2 = !flag2;
      if (!(node is CanvasItem canvasItem))
        return;
      canvasItem.Visible = flag2;
    }

    public enum ObjectiveStatus
    {
      Done,
      InProgress,
      Failed,
    }
  }
}
