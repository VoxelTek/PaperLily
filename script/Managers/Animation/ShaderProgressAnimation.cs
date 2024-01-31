// Decompiled with JetBrains decompiler
// Type: LacieEngine.Animation.ShaderProgressAnimation
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Animation
{
  public class ShaderProgressAnimation : LacieAnimation
  {
    private CanvasItem _node;
    private bool _invert;

    public ShaderProgressAnimation(CanvasItem node, float duration, bool invert = false)
    {
      this.Node = (Node) (this._node = node);
      this.Duration = duration;
      this._invert = invert;
    }

    public override void UpdateToFirstFrame()
    {
      ((ShaderMaterial) this._node.Material).SetShaderParam("progress", (object) (this._invert ? 1 : 0));
    }

    public override void UpdateToCurrentFrame()
    {
      float num = this.Elapsed / this.Duration;
      if (this._invert)
        num = 1f - num;
      ((ShaderMaterial) this._node.Material).SetShaderParam("progress", (object) num);
    }

    public override void UpdateToFinalFrame()
    {
      ((ShaderMaterial) this._node.Material).SetShaderParam("progress", (object) (!this._invert ? 1 : 0));
    }
  }
}
