// Decompiled with JetBrains decompiler
// Type: LacieEngine.Animation.SlideInAnimation
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Animation
{
  public abstract class SlideInAnimation : FadeAnimation
  {
    public const float DefaultDuration = 0.15f;
    protected Control _node;
    protected Vector2 _from;
    protected Vector2 _to;
    protected Vector2 _track;
    protected Vector2? _pTo;
    protected float? _pDisplacement;

    public SlideInAnimation(Control node, float? displacement = null, Vector2? to = null, float duration = 0.15f)
    {
      this.Node = (Node) (this._node = node);
      this.Duration = duration;
      this._pTo = to;
      this._pDisplacement = displacement;
    }

    public override void UpdateToFirstFrame()
    {
      this._node.Modulate = FadeAnimation.HiddenColor;
      this._node.RectPosition = this._from;
    }

    public override void UpdateToCurrentFrame()
    {
      float a = DrkieUtil.EaseOutQuad(this.Elapsed / this.Duration);
      this._node.Modulate = new Color(1f, 1f, 1f, a);
      this._node.RectPosition = this._from + this._track * a;
    }

    public override void UpdateToFinalFrame()
    {
      this._node.Modulate = FadeAnimation.VisibleColor;
      this._node.RectPosition = this._to;
    }
  }
}
