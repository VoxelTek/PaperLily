// Decompiled with JetBrains decompiler
// Type: LacieEngine.Audio.AudioManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Audio
{
  [Injectable]
  public class AudioManager : Node, IAudioManager, IModule
  {
    public const float DEFAULT_PLAYBACK_VOLUME = 1f;
    public const float DEFAULT_FADEOUT_TIME = 1.5f;
    public const float DEFAULT_VOLUMECHANGE_TIME = 0.5f;
    public const Decimal MAX_VOLUME = 0.90M;
    private const float TEXT_BEEP_FREQUENCY = 0.06f;
    private AudioPlayer _bgmPlayer;
    private AudioPlayer _ambiancePlayer;
    private SamplePlayer _sfxPlayer;
    private FixedSamplePlayer _textBeepPlayer;
    private SamplePlayer _systemSfxPlayer;

    public event Action VolumeChanged;

    public event Action BgmStopped;

    public AudioStream CurrentBgm => this._bgmPlayer.CurrentTrack;

    public AudioManager() => this.Name = nameof (AudioManager);

    public void Init()
    {
      AudioStreamPlayer player1_1 = GDUtil.MakeNode<AudioStreamPlayer>("BGM");
      AudioStreamPlayer player2_1 = GDUtil.MakeNode<AudioStreamPlayer>("BGM2");
      AudioStreamPlayer player1_2 = GDUtil.MakeNode<AudioStreamPlayer>("AmbianceSFX");
      AudioStreamPlayer player2_2 = GDUtil.MakeNode<AudioStreamPlayer>("AmbianceSFX2");
      AudioStreamPlayer audioStreamPlayer1 = GDUtil.MakeNode<AudioStreamPlayer>("SFX1");
      AudioStreamPlayer audioStreamPlayer2 = GDUtil.MakeNode<AudioStreamPlayer>("SFX2");
      AudioStreamPlayer audioStreamPlayer3 = GDUtil.MakeNode<AudioStreamPlayer>("SFX3");
      AudioStreamPlayer player = GDUtil.MakeNode<AudioStreamPlayer>("TextBeepSFX");
      AudioStreamPlayer audioStreamPlayer4 = GDUtil.MakeNode<AudioStreamPlayer>("SystemSFX");
      this._bgmPlayer = new AudioPlayer(player1_1, player2_1);
      this._ambiancePlayer = new AudioPlayer(player1_2, player2_2);
      this._sfxPlayer = new SamplePlayer(new AudioStreamPlayer[3]
      {
        audioStreamPlayer1,
        audioStreamPlayer2,
        audioStreamPlayer3
      });
      this._textBeepPlayer = new FixedSamplePlayer(player, GD.Load<AudioStream>("res://assets/sfx/ui_text.ogg"));
      this._textBeepPlayer.Frequency = 0.06f;
      this._systemSfxPlayer = new SamplePlayer(new AudioStreamPlayer[1]
      {
        audioStreamPlayer4
      });
      this.AddChild((Node) player1_1);
      this.AddChild((Node) player2_1);
      this.AddChild((Node) player1_2);
      this.AddChild((Node) player2_2);
      this.AddChild((Node) audioStreamPlayer1);
      this.AddChild((Node) audioStreamPlayer2);
      this.AddChild((Node) audioStreamPlayer3);
      this.AddChild((Node) player);
      this.AddChild((Node) audioStreamPlayer4);
      Game.Root.AddChild((Node) this);
    }

    public void PlayBgm(AudioStream track, float volume, bool crossFade)
    {
      this._bgmPlayer.Play(track, volume, crossFade);
    }

    public void PlayBgm(AudioStream track, float volume) => this.PlayBgm(track, volume, false);

    public void PlayBgm(AudioStream track) => this.PlayBgm(track, 1f);

    public void ChangeBgmVolume(float volume, float time)
    {
      this._bgmPlayer.ChangeVolume(volume, time);
    }

    public void ChangeBgmVolume(float volume) => this.ChangeBgmVolume(volume, 0.5f);

    public void StopBgm()
    {
      this._bgmPlayer.Stop();
      Action bgmStopped = this.BgmStopped;
      if (bgmStopped == null)
        return;
      bgmStopped();
    }

    public void StopBgm(float time)
    {
      this._bgmPlayer.Stop(time);
      Action bgmStopped = this.BgmStopped;
      if (bgmStopped == null)
        return;
      bgmStopped();
    }

    public void PlayAmbiance(AudioStream track, float volume)
    {
      this._ambiancePlayer.Play(track, volume);
    }

    public void PlayAmbiance(AudioStream track) => this.PlayAmbiance(track, 1f);

    public void PlayAmbiance(string path) => this.PlayAmbiance(GD.Load<AudioStream>(path));

    public void ChangeAmbianceVolume(float volume, float time)
    {
      this._ambiancePlayer.ChangeVolume(volume, time);
    }

    public void ChangeAmbianceVolume(float volume) => this.ChangeAmbianceVolume(volume, 0.5f);

    public void StopAmbiance() => this._ambiancePlayer.Stop();

    public void StopAmbiance(float time) => this._ambiancePlayer.Stop(time);

    public void PlaySfx(AudioStream track, float volume) => this._sfxPlayer.Play(track, volume);

    public void PlaySfx(AudioStream track) => this.PlaySfx(track, 1f);

    public void PlaySfx(string path, float volume)
    {
      this.PlaySfx(GD.Load<AudioStream>(path), volume);
    }

    public void PlaySfx(string path) => this.PlaySfx(path, 1f);

    public void StopSfx() => this._sfxPlayer.Stop();

    public void PlayTextBeepSound() => this._textBeepPlayer.Play();

    public void PlaySystemSound(string path)
    {
      this._systemSfxPlayer.Play(GD.Load<AudioStream>(path));
    }

    public void UpdateVolume()
    {
      this._bgmPlayer.MaxVolume = this.CalculateMaxVolume(Game.Settings.VolumeBgm);
      this._ambiancePlayer.MaxVolume = this.CalculateMaxVolume(Game.Settings.VolumeSfx);
      this._sfxPlayer.MaxVolume = this.CalculateMaxVolume(Game.Settings.VolumeSfx);
      this._textBeepPlayer.MaxVolume = this.CalculateMaxVolume(Game.Settings.VolumeText);
      this._systemSfxPlayer.MaxVolume = this.CalculateMaxVolume(Game.Settings.VolumeSystem);
      Action volumeChanged = this.VolumeChanged;
      if (volumeChanged == null)
        return;
      volumeChanged();
    }

    private float CalculateMaxVolume(Decimal value)
    {
      return (float) ((Decimal) (!Game.Settings.MuteAudio ? 1 : 0) * Game.Settings.VolumeMaster * 0.90M * value);
    }
  }
}
