// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.DynamicOccluderRD
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Nodes
{
  internal class DynamicOccluderRD : LightOccluder2D
  {
    private Position2D nCenter;

    public override void _Ready() => this.nCenter = this.GetNode((NodePath) "Center") as Position2D;

    public override void _Process(float delta)
    {
      if ((double) Game.Player.GetCenter().x > (double) this.nCenter.GlobalPosition.x && (double) Game.Player.GetCenter().y > (double) this.nCenter.GlobalPosition.y)
        this.Visible = false;
      else
        this.Visible = true;
    }
  }
}
