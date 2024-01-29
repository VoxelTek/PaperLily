using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerAudioCommand : StoryPlayerCommand
	{
		public enum AudioType
		{
			Bgm,
			Sfx,
			Ambiance
		}

		public enum AudioOperation
		{
			Play,
			Stop,
			Volume,
			Override,
			RemoveOverride
		}

		public AudioType Type { get; set; }

		public AudioOperation Operation { get; set; }

		public string Track { get; set; }

		public float? Time { get; set; }

		public float Volume { get; set; } = 1f;


		public bool CrossFade { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			if (Type == AudioType.Bgm)
			{
				if (Operation == AudioOperation.Play)
				{
					Game.Audio.PlayBgm(GD.Load<AudioStream>(Track), Volume, CrossFade);
				}
				else if (Operation == AudioOperation.Stop)
				{
					if (Time.HasValue)
					{
						Game.Audio.StopBgm(Time.Value);
					}
					else
					{
						Game.Audio.StopBgm();
					}
				}
				else if (Operation == AudioOperation.Volume)
				{
					if (Time.HasValue)
					{
						Game.Audio.ChangeBgmVolume(Volume, Time.Value);
					}
					else
					{
						Game.Audio.ChangeBgmVolume(Volume);
					}
				}
				else if (Operation == AudioOperation.Override)
				{
					Game.State.OverrideBgm = Track;
				}
				else if (Operation == AudioOperation.RemoveOverride)
				{
					Game.State.OverrideBgm = null;
				}
			}
			else if (Type == AudioType.Ambiance)
			{
				if (Operation == AudioOperation.Play)
				{
					Game.Audio.PlayAmbiance(GD.Load<AudioStream>(Track), Volume);
				}
				else if (Operation == AudioOperation.Stop)
				{
					if (Time.HasValue)
					{
						Game.Audio.StopAmbiance(Time.Value);
					}
					else
					{
						Game.Audio.StopAmbiance();
					}
				}
				else if (Operation == AudioOperation.Volume)
				{
					if (Time.HasValue)
					{
						Game.Audio.ChangeAmbianceVolume(Volume, Time.Value);
					}
					else
					{
						Game.Audio.ChangeAmbianceVolume(Volume);
					}
				}
			}
			else if (Type == AudioType.Sfx)
			{
				Game.Audio.PlaySfx(Track, Volume);
			}
			storyPlayer.SetNextBlockContinue();
			storyPlayer.Next();
		}

		public override IList<string> GetDependencies()
		{
			if (!Track.IsNullOrEmpty() && Track != "res://assets/bgm/silent.ogg")
			{
				return new List<string>(1) { Track };
			}
			return Array.Empty<string>();
		}
	}
}
