// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.ActionEnabler
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
  public class ActionEnabler : Godot.Node, IAction, IToggleable
  {
    [Export(PropertyHint.None, "")]
    public bool Enabled { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public NodePath Node { get; set; } = new NodePath();

    [Export(PropertyHint.None, "")]
    public bool Enable { get; set; } = true;

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
      switch (this.GetNode(this.Node))
      {
        case IToggleable toggleable:
          toggleable.Enabled = this.Enable;
          break;
        case CollisionShape2D collisionShape2D:
          collisionShape2D.Disabled = !this.Enable;
          break;
        case CollisionPolygon2D collisionPolygon2D:
          collisionPolygon2D.Disabled = !this.Enable;
          break;
      }
    }
  }
}
