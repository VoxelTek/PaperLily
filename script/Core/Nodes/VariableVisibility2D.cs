// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.VariableVisibility2D
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType]
  public class VariableVisibility2D : Godot.Node, IAction, IToggleable
  {
    [Export(PropertyHint.None, "")]
    public bool Enabled { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public string Variable { get; set; }

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
      bool flag = Game.Variables.GetFlag(this.Variable);
      if (this.Invert)
        flag = !flag;
      if (!(node is CanvasItem canvasItem))
        return;
      canvasItem.Visible = flag;
    }
  }
}
