// Decompiled with JetBrains decompiler
// Type: LacieEngine.Animation.PanAnimationNode2D
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Animation
{
  public class PanAnimationNode2D : LacieAnimation
  {
    private Node2D _node;
    private Vector2 _from;
    private Vector2 _to;
    private Vector2 _track;

    public PanAnimationNode2D(Node2D node, Vector2 from, Vector2 to, float duration)
    {
      this.Node = (Node) (this._node = node);
      this.Duration = duration;
      this._from = from;
      this._to = to;
    }

    public override void InitialCalculations() => this._track = this._to - this._from;

    public override void UpdateToFirstFrame() => this._node.Position = this._from;

    public override void UpdateToCurrentFrame()
    {
      this._node.Position = this._from + this._track * (this.Elapsed / this.Duration);
    }

    public override void UpdateToFinalFrame() => this._node.Position = this._to;
  }
}
