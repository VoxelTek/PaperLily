// Decompiled with JetBrains decompiler
// Type: LacieEngine.Animation.ModulateAnimation
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Animation
{
  public class ModulateAnimation : LacieAnimation
  {
    private Color _initialColor;
    private Color _finalColor;
    private CanvasItem _node;
    private float _trackR;
    private float _trackG;
    private float _trackB;
    private float _trackA;

    public ModulateAnimation(
      CanvasItem node,
      float duration,
      Color initialColor,
      Color finalColor)
    {
      this.Node = (Node) (this._node = node);
      this.Duration = duration;
      this._initialColor = initialColor;
      this._finalColor = finalColor;
    }

    public override void InitialCalculations()
    {
      this._trackR = this._finalColor.r - this._initialColor.r;
      this._trackG = this._finalColor.g - this._initialColor.g;
      this._trackB = this._finalColor.b - this._initialColor.b;
      this._trackA = this._finalColor.a - this._initialColor.a;
    }

    public override void UpdateToFirstFrame() => this._node.Modulate = this._initialColor;

    public override void UpdateToCurrentFrame()
    {
      float num = this.Elapsed / this.Duration;
      this._node.Modulate = new Color(this._initialColor.r + this._trackR * num, this._initialColor.g + this._trackG * num, this._initialColor.b + this._trackB * num, this._initialColor.a + this._trackA * num);
    }

    public override void UpdateToFinalFrame() => this._node.Modulate = this._finalColor;
  }
}
