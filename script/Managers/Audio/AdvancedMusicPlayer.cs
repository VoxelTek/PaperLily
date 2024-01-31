// Decompiled with JetBrains decompiler
// Type: LacieEngine.Audio.AdvancedMusicPlayer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Audio
{
  public class AdvancedMusicPlayer : Node
  {
    public const string NODE_NAME = "AdvancedMusicPlayer";
    private AudioStreamPlayer nCurrentPlayer;
    private AudioStreamPlayer nAltPlayer;
    private float _maxVolume;
    private AdvancedMusicPlayer.AudioInformation _current;
    private System.Collections.Generic.Queue<AdvancedMusicPlayer.AudioInformation> _queue = new System.Collections.Generic.Queue<AdvancedMusicPlayer.AudioInformation>();
    private bool _playing;

    public bool Loop { get; set; }

    public bool Playing => this._playing;

    public bool Empty => this._queue.IsEmpty<AdvancedMusicPlayer.AudioInformation>();

    public override void _EnterTree()
    {
      this.Name = nameof (AdvancedMusicPlayer);
      this.nCurrentPlayer = new AudioStreamPlayer();
      this.AddChild((Node) this.nCurrentPlayer);
      this.nAltPlayer = new AudioStreamPlayer();
      this.AddChild((Node) this.nAltPlayer);
      this.UpdateMaxVolume();
      Game.Audio.VolumeChanged += new Action(this.UpdateMaxVolume);
      Log.Debug((object) "[AdvancedMusicPlayer] Created.");
    }

    public override void _ExitTree()
    {
      Game.Audio.VolumeChanged -= new Action(this.UpdateMaxVolume);
    }

    public override void _Process(float delta)
    {
      if (this._playing && !this._queue.IsEmpty<AdvancedMusicPlayer.AudioInformation>() && (double) this.nCurrentPlayer.GetPlaybackPosition() >= (double) this._current.RightAttchPoint - (double) this._queue.Peek().LeftAttachPoint)
      {
        Log.Debug((object) "[AdvancedMusicPlayer] Switching Track...");
        this._current = this._queue.Dequeue();
        this.nAltPlayer.Stream = this._current.Stream;
        this.nAltPlayer.VolumeDb = GD.Linear2Db(this._maxVolume);
        this.nAltPlayer.Playing = true;
        this.SwapChannels();
      }
      else if (this._playing && this.Loop && this._queue.IsEmpty<AdvancedMusicPlayer.AudioInformation>() && (double) this.nCurrentPlayer.GetPlaybackPosition() >= (double) this._current.RightAttchPoint - (double) this._current.LeftAttachPoint)
      {
        Log.Debug((object) "[AdvancedMusicPlayer] Looping Track...");
        this.nAltPlayer.Stream = this._current.Stream;
        this.nAltPlayer.VolumeDb = GD.Linear2Db(this._maxVolume);
        this.nAltPlayer.Playing = true;
        this.SwapChannels();
      }
      if (this.nCurrentPlayer.Playing || this.nAltPlayer.Playing)
        return;
      this._playing = false;
    }

    public void Play(AdvancedMusicPlayer.AudioInformation audio)
    {
      if (this.nCurrentPlayer.Playing && audio.Stream == this._current.Stream)
        return;
      Log.Debug((object) "[AdvancedMusicPlayer] Starting Track...");
      this.Stop();
      this._current = audio;
      this.nCurrentPlayer.Stream = audio.Stream;
      this.nCurrentPlayer.VolumeDb = GD.Linear2Db(this._maxVolume);
      this.nCurrentPlayer.Play();
      this._playing = true;
    }

    public void Stop(float time = 1.5f)
    {
      Log.Debug((object) "[AdvancedMusicPlayer] Stopping...");
      this._playing = false;
      this.Loop = false;
      this._queue.Clear();
      this.StopPlayer(this.nCurrentPlayer, time);
      this.StopPlayer(this.nAltPlayer, time);
    }

    public void Queue(AdvancedMusicPlayer.AudioInformation audio)
    {
      Log.Debug((object) "[AdvancedMusicPlayer] Queuing...");
      if (!this._playing)
        this.Play(audio);
      else
        this._queue.Enqueue(audio);
    }

    public void ClearQueue()
    {
      Log.Debug((object) "[AdvancedMusicPlayer] Clearing Queue...");
      this._queue.Clear();
    }

    public void UpdateMaxVolume()
    {
      Decimal linear = (Decimal) (!Game.Settings.MuteAudio ? 1 : 0) * Game.Settings.VolumeMaster * 0.90M * Game.Settings.VolumeBgm;
      this._maxVolume = (float) linear;
      if (!this.nCurrentPlayer.Playing)
        return;
      this.nCurrentPlayer.VolumeDb = GD.Linear2Db((float) linear);
    }

    private async void StopPlayer(AudioStreamPlayer player, float time)
    {
      if (!player.Playing)
        return;
      LacieAnimation animation = (LacieAnimation) new AudioVolumeFadeOutAnimation(this.nCurrentPlayer, time);
      Game.Animations.Play(animation);
      await animation.WaitUntilFinished();
      player.Stop();
      this.SwapChannels();
    }

    private void SwapChannels()
    {
      AudioStreamPlayer nCurrentPlayer = this.nCurrentPlayer;
      AudioStreamPlayer nAltPlayer = this.nAltPlayer;
      this.nAltPlayer = nCurrentPlayer;
      this.nCurrentPlayer = nAltPlayer;
    }

    public class AudioInformation
    {
      public AudioStream Stream;
      public float LeftAttachPoint;
      public float RightAttchPoint;
    }
  }
}
