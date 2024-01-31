// Decompiled with JetBrains decompiler
// Type: LacieEngine.Animation.FadeInAnimation
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Animation
{
  public class FadeInAnimation : FadeAnimation
  {
    private CanvasItem _node;

    public FadeInAnimation(CanvasItem node, float duration)
    {
      this.Node = (Node) (this._node = node);
      this.Duration = duration;
    }

    public override void UpdateToFirstFrame()
    {
      this._node.Modulate = FadeAnimation.HiddenColor;
      this._node.Visible = true;
    }

    public override void UpdateToCurrentFrame()
    {
      this._node.Modulate = new Color(1f, 1f, 1f, this.Elapsed / this.Duration);
    }

    public override void UpdateToFinalFrame() => this._node.Modulate = FadeAnimation.VisibleColor;
  }
}
