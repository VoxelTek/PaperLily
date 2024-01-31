// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.IAudioManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;

#nullable disable
namespace LacieEngine.API
{
  [InjectableInterface(unique = true)]
  public interface IAudioManager : IModule
  {
    event Action VolumeChanged;

    event Action BgmStopped;

    AudioStream CurrentBgm { get; }

    void PlayBgm(AudioStream track, float volume, bool crossFade);

    void PlayBgm(AudioStream track, float volume);

    void PlayBgm(AudioStream track);

    void StopBgm();

    void StopBgm(float time);

    void ChangeBgmVolume(float volume);

    void ChangeBgmVolume(float volume, float time);

    void PlayAmbiance(AudioStream track, float volume);

    void PlayAmbiance(AudioStream track);

    void PlayAmbiance(string path);

    void StopAmbiance();

    void StopAmbiance(float time);

    void ChangeAmbianceVolume(float volume);

    void ChangeAmbianceVolume(float volume, float time);

    void PlaySfx(AudioStream track, float volume);

    void PlaySfx(AudioStream track);

    void PlaySfx(string path, float volume);

    void PlaySfx(string path);

    void StopSfx();

    void PlayTextBeepSound();

    void PlaySystemSound(string path);

    void UpdateVolume();
  }
}
