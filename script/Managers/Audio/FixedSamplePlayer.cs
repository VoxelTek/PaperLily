// Decompiled with JetBrains decompiler
// Type: LacieEngine.Audio.FixedSamplePlayer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Audio
{
  public class FixedSamplePlayer : Node
  {
    private const float DEFAULT_FREQUENCY = 0.06f;
    private float _waitTime;
    private bool _playing;
    private AudioStreamPlayer nPlayer;
    private float _maxVolume;

    public float Frequency { get; set; } = 0.06f;

    public float MaxVolume
    {
      get => this._maxVolume;
      set
      {
        this._maxVolume = value;
        this.nPlayer.VolumeDb = GD.Linear2Db(value);
      }
    }

    private FixedSamplePlayer()
    {
    }

    public FixedSamplePlayer(AudioStreamPlayer player, AudioStream track)
    {
      this.Name = nameof (FixedSamplePlayer);
      this.nPlayer = player;
      this.nPlayer.Stream = track;
      Game.Root.AddChild((Node) this);
      this.SetProcess(false);
    }

    public override void _Process(float delta)
    {
      this._waitTime -= delta;
      if (this._playing && (double) this._waitTime <= 0.0)
      {
        this.nPlayer.Play();
        this._waitTime = this.Frequency;
        this._playing = false;
      }
      else
      {
        if ((double) this._waitTime > 0.0)
          return;
        this._waitTime = 0.0f;
        this.SetProcess(false);
      }
    }

    public void Play()
    {
      this._playing = true;
      this.SetProcess(true);
    }
  }
}
