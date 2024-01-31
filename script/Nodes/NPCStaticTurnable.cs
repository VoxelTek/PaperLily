// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.NPCStaticTurnable
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Nodes
{
  public class NPCStaticTurnable : StaticBody2D, ITurnable
  {
    [Export(PropertyHint.None, "")]
    public Direction.Enum DefaultDirection;
    private Sprite nSprite;

    [Export(PropertyHint.None, "")]
    public bool TurningEnabled { get; set; } = true;

    public override void _Ready()
    {
      Game.Room.RegisteredNPCs[this.Name] = (Node2D) this;
      this.nSprite = (Sprite) this.GetNode((NodePath) "Sprite");
      this.TurnToDefault();
    }

    public void Turn(Direction direction)
    {
      if (!this.TurningEnabled)
        return;
      switch (direction.ToEnum())
      {
        case Direction.Enum.Left:
          this.nSprite.Frame = 0;
          break;
        case Direction.Enum.Up:
          this.nSprite.Frame = 1;
          break;
        case Direction.Enum.Right:
          this.nSprite.Frame = 2;
          break;
        case Direction.Enum.Down:
          this.nSprite.Frame = 3;
          break;
      }
    }

    public void TurnToDefault()
    {
      if (!this.TurningEnabled)
        return;
      this.Turn((Direction) this.DefaultDirection);
    }
  }
}
