// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.ActionPositionPlayerAlign
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
  public class ActionPositionPlayerAlign : Godot.Node, IAction, IToggleable
  {
    [Export(PropertyHint.None, "")]
    public bool Enabled { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public NodePath Node { get; set; } = new NodePath();

    [Export(PropertyHint.None, "")]
    public bool AlignX { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public bool AlignY { get; set; } = true;

    public override void _Ready()
    {
      if (Engine.EditorHint)
        return;
      Game.Room.RegisteredRoomActions[this.Name] = (IAction) this;
    }

    public void Execute()
    {
      if (!this.Enabled || this.Node.IsNullOrEmpty())
        return;
      if (Game.Player == null || !Game.Player.Node.IsValid())
      {
        Log.Warn((object) "[", (object) nameof (ActionPositionPlayerAlign), (object) "::", (object) this.Name, (object) "] Player is not present!");
      }
      else
      {
        if (!(this.GetNode(this.Node) is Node2D node))
          return;
        if (this.AlignX)
          node.Position = node.Position.ReplaceX(Game.Player.Node.Position.x);
        if (!this.AlignY)
          return;
        node.Position = node.Position.ReplaceY(Game.Player.Node.Position.y);
      }
    }
  }
}
