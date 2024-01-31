// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.SimpleAnimatedSpriteV3
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType(icon = "cat")]
  public class SimpleAnimatedSpriteV3 : Sprite, IAnimation2D
  {
    private float _timeAccumulator;
    private bool _canPlay;
    private int _animationFrame;
    private float _frameDuration;
    private int[] _framesToUse;
    private float _fps = 4f;
    private bool _playing;
    private string _animationFramesStr = "";
    private int[] _animationFrames;
    private string _frameDurationsStr = "";
    private float[] _frameDurations = new float[0];

    [Export(PropertyHint.Range, "0.1,300")]
    public float FPS
    {
      get => this._fps;
      set
      {
        this._fps = value;
        this.UpdateFrameDurations(this._frameDurationsStr);
      }
    }

    [Export(PropertyHint.None, "")]
    public bool Playing
    {
      get => this._playing;
      set
      {
        if (this._playing == value)
          return;
        this.UpdateValues();
        this._playing = value;
      }
    }

    [Export(PropertyHint.None, "")]
    public bool Loop { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public int LoopFrame { get; set; }

    [Export(PropertyHint.None, "")]
    public bool Autostart { get; set; } = true;

    [Export(PropertyHint.MultilineText, "")]
    public string AnimationFrames
    {
      get => this._animationFramesStr;
      set => this.UpdateAnimationFrames(value);
    }

    [Export(PropertyHint.MultilineText, "")]
    public string FrameDurations
    {
      get => this._frameDurationsStr;
      set => this.UpdateFrameDurations(value);
    }

    public override void _EnterTree()
    {
      if (this.Autostart && !Engine.EditorHint)
        this._playing = true;
      this.UpdateValues();
    }

    public override void _Process(float delta)
    {
      if (!this._canPlay || !this._playing || !this.IsVisibleInTree())
        return;
      this._timeAccumulator += delta;
      this._frameDuration = this._frameDurations[this._animationFrame];
      if ((double) this._timeAccumulator < (double) this._frameDuration)
        return;
      this._timeAccumulator -= this._frameDuration;
      if (++this._animationFrame >= this._framesToUse.Length)
      {
        this._animationFrame = this.LoopFrame;
        if (!this.Loop)
        {
          this._animationFrame = 0;
          this._playing = false;
          return;
        }
      }
      this.Frame = this._framesToUse[this._animationFrame];
    }

    public void Play() => this.Playing = true;

    public void Stop() => this.Playing = false;

    private void UpdateAnimationFrames(string value)
    {
      this._animationFramesStr = value;
      this._animationFrames = value.IsNullOrEmpty() ? (int[]) null : value.SplitInts(',');
      this.UpdateValues();
    }

    private void UpdateFrameDurations(string value)
    {
      this._frameDurationsStr = value;
      this._frameDurations = value.IsNullOrEmpty() ? new float[0] : value.SplitFloats(',');
      for (int index = 0; index < this._frameDurations.Length; ++index)
        this._frameDurations[index] = this._frameDurations[index] / 1000f;
      this.UpdateValues();
    }

    private void UpdateValues()
    {
      this._canPlay = false;
      if (((ICollection<int>) this._animationFrames).IsNullOrEmpty<int>())
      {
        this._framesToUse = new int[this.Hframes * this.Vframes];
        for (int index = 0; index < this._framesToUse.Length; ++index)
          this._framesToUse[index] = index;
      }
      else
        this._framesToUse = this._animationFrames;
      if (this._frameDurations.Length != this._framesToUse.Length)
      {
        float[] array = new float[this._framesToUse.Length];
        Array.Fill<float>(array, 1f / this._fps);
        for (int index = 0; index < this._frameDurations.Length && index < array.Length; ++index)
          array[index] = this._frameDurations[index];
        this._frameDurations = array;
      }
      this._animationFrame = 0;
      this._timeAccumulator = 0.0f;
      this.Frame = this._framesToUse[this._animationFrame];
      this._canPlay = true;
    }
  }
}
