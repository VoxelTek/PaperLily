// Decompiled with JetBrains decompiler
// Type: LacieEngine.Audio.AudioPlayer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.Audio
{
  public class AudioPlayer
  {
    private AudioStreamPlayer nCurrentPlayer;
    private AudioStreamPlayer nAltPlayer;
    private float nextFadeDuration;
    private AudioStream nextTrack;
    private AudioStream currentTrack;
    private float nextVolume;
    private float currentVolume = 1f;
    private bool updating;
    private float _maxVolume;
    private bool _crossFade;

    public float MaxVolume
    {
      get => this._maxVolume;
      set
      {
        this._maxVolume = value;
        if (!this.nCurrentPlayer.Playing)
          return;
        this.nCurrentPlayer.VolumeDb = GD.Linear2Db(this.currentVolume * value);
      }
    }

    public AudioStream CurrentTrack => this.currentTrack;

    public AudioPlayer(AudioStreamPlayer player1, AudioStreamPlayer player2)
    {
      this.nCurrentPlayer = player1;
      this.nAltPlayer = player2;
      this.nextFadeDuration = 1.5f;
    }

    public void Play(AudioStream track, float volume = 1f, bool crossFade = false)
    {
      this.nextTrack = track;
      this.nextVolume = volume;
      this._crossFade = crossFade;
      this.UpdateAudio();
    }

    private async Task SwitchDynamic(AudioStream track, float volume)
    {
      if (!this.nCurrentPlayer.Playing)
      {
        this.nCurrentPlayer.VolumeDb = GD.Linear2Db(this._maxVolume * volume);
        this.nCurrentPlayer.Stream = track;
        this.nCurrentPlayer.Play();
      }
      else
      {
        this.nAltPlayer.Playing = false;
        this.nAltPlayer.Stream = track;
        this.nAltPlayer.Playing = true;
        this.nAltPlayer.Seek(this.nCurrentPlayer.GetPlaybackPosition());
        Game.Animations.Play((LacieAnimation) new AudioVolumeFadeInAnimation(this.nAltPlayer, volume * this._maxVolume, 3f));
        Game.Animations.Play((LacieAnimation) new AudioVolumeFadeOutAnimation(this.nCurrentPlayer, 3f));
        await DrkieUtil.DelaySeconds(3.0);
        AudioStreamPlayer nCurrentPlayer = this.nCurrentPlayer;
        AudioStreamPlayer nAltPlayer = this.nAltPlayer;
        this.nAltPlayer = nCurrentPlayer;
        this.nCurrentPlayer = nAltPlayer;
      }
    }

    public void Stop(float time = 1.5f)
    {
      this.nextTrack = (AudioStream) null;
      this.nextVolume = 1f;
      this.nextFadeDuration = time;
      this.UpdateAudio();
    }

    public void ChangeVolume(float volume, float time = 0.5f)
    {
      this.nextVolume = volume;
      this.nextFadeDuration = time;
      this.UpdateAudio();
    }

    private async void UpdateAudio()
    {
      if (this.updating || this.nextTrack == this.currentTrack && (double) this.nextVolume == (double) this.currentVolume)
        return;
      this.updating = true;
      if (this.nextTrack == this.currentTrack && (double) this.nextVolume != (double) this.currentVolume)
      {
        Log.Debug((object) "Audio: Change volume.");
        LacieAnimation animation = (LacieAnimation) new AudioVolumeFadeAnimation(this.nCurrentPlayer, this.nextFadeDuration, this.currentVolume * this._maxVolume, this.nextVolume * this._maxVolume);
        Game.Animations.Play(animation);
        await animation.WaitUntilFinished();
        this.currentVolume = this.nextVolume;
      }
      else if (this._crossFade && this.IsDynamicSwitch(this.currentTrack, this.nextTrack))
      {
        Log.Debug((object) "Audio: Dynamic switch.");
        AudioStream nextTrackAsync = this.nextTrack;
        float nextVolumeAsync = this.nextVolume;
        await this.SwitchDynamic(this.nextTrack, this.nextVolume);
        this.currentTrack = nextTrackAsync;
        this.currentVolume = nextVolumeAsync;
        nextTrackAsync = (AudioStream) null;
      }
      else if (this.nextTrack != this.currentTrack && this.currentTrack != null)
      {
        Log.Debug((object) "Audio: Stop track.");
        await this.FadeOut(this.nextFadeDuration);
        this.nextFadeDuration = 1.5f;
        this.nCurrentPlayer.Stop();
        this.currentTrack = (AudioStream) null;
        this.currentVolume = 1f;
      }
      else if (this.nextTrack != this.currentTrack && this.nextTrack != null)
      {
        Log.Debug((object) "Audio: Play track.");
        this.currentTrack = this.nextTrack;
        this.currentVolume = this.nextVolume;
        this.nCurrentPlayer.VolumeDb = GD.Linear2Db(this._maxVolume * this.nextVolume);
        this.nCurrentPlayer.Stream = this.nextTrack;
        this.nCurrentPlayer.Play();
      }
      this.updating = false;
      this.UpdateAudio();
    }

    private async Task FadeOut(float time)
    {
      LacieAnimation animation = (LacieAnimation) new AudioVolumeFadeOutAnimation(this.nCurrentPlayer, time);
      Game.Animations.Play(animation);
      await animation.WaitUntilFinished();
    }

    private bool IsDynamicSwitch(AudioStream track1, AudioStream track2)
    {
      if (track1 == null || track2 == null)
        return false;
      string resourcePath1 = track1.ResourcePath;
      int startIndex1 = track1.ResourcePath.LastIndexOf("/") + 1;
      string str1 = resourcePath1.Substring(startIndex1, resourcePath1.Length - startIndex1).StripSuffix(".ogg");
      string resourcePath2 = track2.ResourcePath;
      int startIndex2 = track2.ResourcePath.LastIndexOf("/") + 1;
      string str2 = resourcePath2.Substring(startIndex2, resourcePath2.Length - startIndex2).StripSuffix(".ogg");
      return str1.Substring(0, str1.LastIndexOf("_")) == str2.Substring(0, str2.LastIndexOf("_"));
    }
  }
}
