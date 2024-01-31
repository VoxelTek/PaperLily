// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.ActionPlayerSpeed
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Nodes;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType]
  public class ActionPlayerSpeed : Node, IAction, IToggleable
  {
    [Export(PropertyHint.None, "")]
    public bool Enabled { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public float Multiplier { get; set; } = 1f;

    public override void _Ready()
    {
      if (Engine.EditorHint)
        return;
      Game.Room.RegisteredRoomActions[this.Name] = (IAction) this;
    }

    public void Execute()
    {
      if (!this.Enabled || Game.Player == null || !Game.Player.Node.IsValid() || !(Game.Player.Node is WalkingCharacter node))
        return;
      node.MoveSpeedMultiplier = this.Multiplier;
    }
  }
}
