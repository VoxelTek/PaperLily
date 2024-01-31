// Decompiled with JetBrains decompiler
// Type: LacieEngine.Animation.SlideOutAnimation
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Animation
{
  public abstract class SlideOutAnimation : FadeAnimation
  {
    public const float DefaultDuration = 0.15f;
    protected Control _node;
    protected Vector2 _from;
    protected Vector2 _to;
    protected Vector2 _track;
    protected Vector2? pFrom;
    protected float? _pDisplacement;

    public SlideOutAnimation(Control node, float? displacement = null, Vector2? from = null, float duration = 0.15f)
    {
      this.Node = (Node) (this._node = node);
      this.Duration = duration;
      this.pFrom = from;
      this._pDisplacement = displacement;
    }

    public override void UpdateToFirstFrame()
    {
      this._node.Modulate = FadeAnimation.VisibleColor;
      this._node.RectPosition = this._from;
    }

    public override void UpdateToCurrentFrame()
    {
      float num = DrkieUtil.EaseOutQuad(this.Elapsed / this.Duration);
      this._node.Modulate = new Color(1f, 1f, 1f, 1f - num);
      this._node.RectPosition = this._from + this._track * num;
    }

    public override void UpdateToFinalFrame()
    {
      this._node.Modulate = FadeAnimation.HiddenColor;
      this._node.RectPosition = this._to;
    }
  }
}
