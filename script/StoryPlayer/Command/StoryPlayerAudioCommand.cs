// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerAudioCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerAudioCommand : StoryPlayerCommand
  {
    public StoryPlayerAudioCommand.AudioType Type { get; set; }

    public StoryPlayerAudioCommand.AudioOperation Operation { get; set; }

    public string Track { get; set; }

    public float? Time { get; set; }

    public float Volume { get; set; } = 1f;

    public bool CrossFade { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      if (this.Type == StoryPlayerAudioCommand.AudioType.Bgm)
      {
        if (this.Operation == StoryPlayerAudioCommand.AudioOperation.Play)
          Game.Audio.PlayBgm(GD.Load<AudioStream>(this.Track), this.Volume, this.CrossFade);
        else if (this.Operation == StoryPlayerAudioCommand.AudioOperation.Stop)
        {
          if (this.Time.HasValue)
            Game.Audio.StopBgm(this.Time.Value);
          else
            Game.Audio.StopBgm();
        }
        else if (this.Operation == StoryPlayerAudioCommand.AudioOperation.Volume)
        {
          if (this.Time.HasValue)
            Game.Audio.ChangeBgmVolume(this.Volume, this.Time.Value);
          else
            Game.Audio.ChangeBgmVolume(this.Volume);
        }
        else if (this.Operation == StoryPlayerAudioCommand.AudioOperation.Override)
          Game.State.OverrideBgm = this.Track;
        else if (this.Operation == StoryPlayerAudioCommand.AudioOperation.RemoveOverride)
          Game.State.OverrideBgm = (string) null;
      }
      else if (this.Type == StoryPlayerAudioCommand.AudioType.Ambiance)
      {
        if (this.Operation == StoryPlayerAudioCommand.AudioOperation.Play)
          Game.Audio.PlayAmbiance(GD.Load<AudioStream>(this.Track), this.Volume);
        else if (this.Operation == StoryPlayerAudioCommand.AudioOperation.Stop)
        {
          if (this.Time.HasValue)
            Game.Audio.StopAmbiance(this.Time.Value);
          else
            Game.Audio.StopAmbiance();
        }
        else if (this.Operation == StoryPlayerAudioCommand.AudioOperation.Volume)
        {
          if (this.Time.HasValue)
            Game.Audio.ChangeAmbianceVolume(this.Volume, this.Time.Value);
          else
            Game.Audio.ChangeAmbianceVolume(this.Volume);
        }
      }
      else if (this.Type == StoryPlayerAudioCommand.AudioType.Sfx)
        Game.Audio.PlaySfx(this.Track, this.Volume);
      storyPlayer.SetNextBlockContinue();
      storyPlayer.Next();
    }

    public override IList<string> GetDependencies()
    {
      if (this.Track.IsNullOrEmpty() || !(this.Track != "res://assets/bgm/silent.ogg"))
        return (IList<string>) Array.Empty<string>();
      return (IList<string>) new List<string>(1)
      {
        this.Track
      };
    }

    public enum AudioType
    {
      Bgm,
      Sfx,
      Ambiance,
    }

    public enum AudioOperation
    {
      Play,
      Stop,
      Volume,
      Override,
      RemoveOverride,
    }
  }
}
