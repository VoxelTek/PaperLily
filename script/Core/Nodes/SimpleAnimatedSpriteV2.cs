// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.SimpleAnimatedSpriteV2
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System;

#nullable disable
namespace LacieEngine.Core
{
  [Tool]
  [ExportType(icon = "cat")]
  public class SimpleAnimatedSpriteV2 : Sprite, IAnimation2D
  {
    private const float DEFAULT_FRAME_DURATION = 0.1f;
    private float _timeAccumulator;
    private bool _canPlay;
    private int _animationFrame;
    private float _frameDuration;
    private bool _playing;
    private string _frameDurationsStr = "";
    private float[] _frameDurations = new float[0];

    [Export(PropertyHint.None, "")]
    public bool Playing
    {
      get => this._playing;
      set
      {
        this._playing = value;
        this.UpdateValues();
      }
    }

    [Export(PropertyHint.None, "")]
    public bool Loop { get; set; } = true;

    [Export(PropertyHint.None, "")]
    public bool Autostart { get; set; } = true;

    [Export(PropertyHint.MultilineText, "")]
    public string FrameDurations
    {
      get => this._frameDurationsStr;
      set => this.UpdateFrameDurations(value);
    }

    public override void _EnterTree()
    {
      if (!this.Autostart || Engine.EditorHint)
        return;
      this.Playing = true;
    }

    public override void _Process(float delta)
    {
      if (!this._canPlay || !this._playing || !this.IsVisibleInTree())
        return;
      this._timeAccumulator += delta;
      this._frameDuration = this._frameDurations[this._animationFrame];
      while ((double) this._timeAccumulator >= (double) this._frameDuration)
      {
        this._timeAccumulator -= this._frameDuration;
        if (++this._animationFrame >= this.Hframes * this.Vframes)
        {
          this._animationFrame = 0;
          if (!this.Loop)
          {
            this._playing = false;
            break;
          }
        }
        this._frameDuration = this._frameDurations[this._animationFrame];
        this.Frame = this._animationFrame;
      }
    }

    public void Play() => this.Playing = true;

    public void Stop() => this.Playing = false;

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
      if (this._frameDurations.Length != this.Hframes * this.Vframes)
      {
        float[] array = new float[this.Hframes * this.Vframes];
        Array.Fill<float>(array, 0.1f);
        for (int index = 0; index < this._frameDurations.Length && index < array.Length; ++index)
          array[index] = this._frameDurations[index];
        this._frameDurations = array;
      }
      this.Frame = this._animationFrame = 0;
      this._canPlay = true;
    }
  }
}
