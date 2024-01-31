// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Firefly
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class Firefly : Node2D
  {
    [Export(PropertyHint.None, "")]
    public float Speed = 5f;
    [Export(PropertyHint.None, "")]
    public float Spread = 160f;
    private Random random = new Random();
    private Vector2 _direction = Vector2.One;
    private Vector2 _spawnPoint;

    public override void _Ready() => this._spawnPoint = this.Position;

    public override void _Process(float delta)
    {
      this._direction = (double) this.Position.DistanceTo(this._spawnPoint) >= (double) this.Spread ? this.Position.DirectionTo(this._spawnPoint) : this._direction.Rotated((float) (this.random.NextDouble() - 0.5));
      this.Position = this.Position + delta * this.Speed * this._direction;
    }
  }
}
