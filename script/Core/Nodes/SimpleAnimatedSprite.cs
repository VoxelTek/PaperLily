// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.SimpleAnimatedSprite
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType(icon = "cat")]
  public class SimpleAnimatedSprite : Sprite, IAnimation2D
  {
    private float _timeAccumulator;
    private float _frameDuration;
    private bool _canPlay;
    private int _animationFrame;
    private int[] _framesToUse;
    private float _fps = 4f;
    private bool _playing;
    private string _animationFramesStr = "";
    private int[] _animationFrames;

    [Export(PropertyHint.Range, "0.1,300")]
    public float FPS
    {
      get => this._fps;
      set
      {
        this._fps = value;
        this.UpdateValues();
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
        this._playing = value;
        this.UpdateValues();
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
      this._animationFrame = 0;
      this._frameDuration = 1f / this._fps;
      this.Frame = this._framesToUse[this._animationFrame];
      this._canPlay = true;
    }
  }
}
