// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.PlayerSidescroller
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Nodes
{
  public class PlayerSidescroller : Player
  {
    protected override void PlayerMotion(out Vector2 inputMotion, out Vector2 actualMotion)
    {
      inputMotion = this.TwoDirectionalMotion();
      actualMotion = Vector2.Zero;
      if (this.NewDirection != Direction.None)
        actualMotion = this.MoveAndSlide(inputMotion);
      else
        this.MoveAndCollide(inputMotion);
    }
  }
}
