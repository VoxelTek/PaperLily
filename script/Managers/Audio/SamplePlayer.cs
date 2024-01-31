// Decompiled with JetBrains decompiler
// Type: LacieEngine.Audio.SamplePlayer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Audio
{
  public class SamplePlayer
  {
    private AudioStreamPlayer[] nChannels;
    private int _nextChannel;

    public float MaxVolume { get; set; }

    public SamplePlayer(AudioStreamPlayer[] channels) => this.nChannels = channels;

    public void Play(AudioStream track, float volume = 1f)
    {
      this.nChannels[this._nextChannel].Stream = track;
      this.nChannels[this._nextChannel].VolumeDb = GD.Linear2Db(this.MaxVolume * volume);
      this.nChannels[this._nextChannel].Play();
      if (++this._nextChannel < this.nChannels.Length)
        return;
      this._nextChannel = 0;
    }

    public void Stop()
    {
      foreach (AudioStreamPlayer nChannel in this.nChannels)
        nChannel.Stop();
    }

    public bool IsPlaying(int channel) => this.nChannels[channel].Playing;
  }
}
