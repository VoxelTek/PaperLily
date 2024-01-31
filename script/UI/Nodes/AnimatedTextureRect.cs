// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.AnimatedTextureRect
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
  [Tool]
  [ExportType(icon = "image")]
  public class AnimatedTextureRect : SlicedTextureRect
  {
    private float _timeAccumulator;
    private float _frameDuration;
    private bool _playing;
    private int _animationFrame;
    private int[] _framesToUse;
    private int[] _animationFrames = new int[0];

    [Export(PropertyHint.Range, "0.1,300")]
    public float FPS { get; set; } = 4f;

    [Export(PropertyHint.None, "")]
    public bool Playing { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public int[] AnimationFrames
    {
      get => this._animationFrames;
      set
      {
        this._animationFrames = value;
        this.UpdateTextureSlices();
      }
    }

    public AnimatedTextureRect()
    {
      if (!Engine.EditorHint)
        return;
      this.Playing = false;
    }

    public override void _Process(float delta)
    {
      if (!this._playing || !this.Playing || !this.IsVisibleInTree())
        return;
      this._timeAccumulator += delta;
      if ((double) this._timeAccumulator < (double) this._frameDuration)
        return;
      this._timeAccumulator -= this._frameDuration;
      if (++this._animationFrame >= this._framesToUse.Length)
        this._animationFrame = 0;
      this.Frame = this._framesToUse[this._animationFrame];
    }

    protected override void UpdateTextureSlices()
    {
      this._playing = false;
      base.UpdateTextureSlices();
      if (!this.ready)
        return;
      if (((ICollection<int>) this.AnimationFrames).IsNullOrEmpty<int>())
      {
        this._framesToUse = new int[this.Hframes * this.Vframes];
        for (int index = 0; index < this._framesToUse.Length; ++index)
          this._framesToUse[index] = index;
      }
      else
        this._framesToUse = this.AnimationFrames;
      this._animationFrame = 0;
      this._frameDuration = 1f / this.FPS;
      this.Frame = this._framesToUse[this._animationFrame];
      this._playing = true;
    }
  }
}
