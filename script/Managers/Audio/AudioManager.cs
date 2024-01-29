using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Audio
{
	[Injectable]
	public class AudioManager : Node, IAudioManager, IModule
	{
		public const float DEFAULT_PLAYBACK_VOLUME = 1f;

		public const float DEFAULT_FADEOUT_TIME = 1.5f;

		public const float DEFAULT_VOLUMECHANGE_TIME = 0.5f;

		public const decimal MAX_VOLUME = 0.90m;

		private const float TEXT_BEEP_FREQUENCY = 0.06f;

		private AudioPlayer _bgmPlayer;

		private AudioPlayer _ambiancePlayer;

		private SamplePlayer _sfxPlayer;

		private FixedSamplePlayer _textBeepPlayer;

		private SamplePlayer _systemSfxPlayer;

		public AudioStream CurrentBgm => _bgmPlayer.CurrentTrack;

		public event Action VolumeChanged;

		public event Action BgmStopped;

		public AudioManager()
		{
			base.Name = "AudioManager";
		}

		public void Init()
		{
			AudioStreamPlayer nBGM = GDUtil.MakeNode<AudioStreamPlayer>("BGM");
			AudioStreamPlayer nBGM2 = GDUtil.MakeNode<AudioStreamPlayer>("BGM2");
			AudioStreamPlayer nAmbianceSFX = GDUtil.MakeNode<AudioStreamPlayer>("AmbianceSFX");
			AudioStreamPlayer nAmbianceSFX2 = GDUtil.MakeNode<AudioStreamPlayer>("AmbianceSFX2");
			AudioStreamPlayer nSFX1 = GDUtil.MakeNode<AudioStreamPlayer>("SFX1");
			AudioStreamPlayer nSFX2 = GDUtil.MakeNode<AudioStreamPlayer>("SFX2");
			AudioStreamPlayer nSFX3 = GDUtil.MakeNode<AudioStreamPlayer>("SFX3");
			AudioStreamPlayer nTextBeepSFX = GDUtil.MakeNode<AudioStreamPlayer>("TextBeepSFX");
			AudioStreamPlayer nSystemSfx = GDUtil.MakeNode<AudioStreamPlayer>("SystemSFX");
			_bgmPlayer = new AudioPlayer(nBGM, nBGM2);
			_ambiancePlayer = new AudioPlayer(nAmbianceSFX, nAmbianceSFX2);
			_sfxPlayer = new SamplePlayer(new AudioStreamPlayer[3] { nSFX1, nSFX2, nSFX3 });
			_textBeepPlayer = new FixedSamplePlayer(nTextBeepSFX, GD.Load<AudioStream>("res://assets/sfx/ui_text.ogg"));
			_textBeepPlayer.Frequency = 0.06f;
			_systemSfxPlayer = new SamplePlayer(new AudioStreamPlayer[1] { nSystemSfx });
			AddChild(nBGM);
			AddChild(nBGM2);
			AddChild(nAmbianceSFX);
			AddChild(nAmbianceSFX2);
			AddChild(nSFX1);
			AddChild(nSFX2);
			AddChild(nSFX3);
			AddChild(nTextBeepSFX);
			AddChild(nSystemSfx);
			Game.Root.AddChild(this);
		}

		public void PlayBgm(AudioStream track, float volume, bool crossFade)
		{
			_bgmPlayer.Play(track, volume, crossFade);
		}

		public void PlayBgm(AudioStream track, float volume)
		{
			PlayBgm(track, volume, crossFade: false);
		}

		public void PlayBgm(AudioStream track)
		{
			PlayBgm(track, 1f);
		}

		public void ChangeBgmVolume(float volume, float time)
		{
			_bgmPlayer.ChangeVolume(volume, time);
		}

		public void ChangeBgmVolume(float volume)
		{
			ChangeBgmVolume(volume, 0.5f);
		}

		public void StopBgm()
		{
			_bgmPlayer.Stop();
			this.BgmStopped?.Invoke();
		}

		public void StopBgm(float time)
		{
			_bgmPlayer.Stop(time);
			this.BgmStopped?.Invoke();
		}

		public void PlayAmbiance(AudioStream track, float volume)
		{
			_ambiancePlayer.Play(track, volume);
		}

		public void PlayAmbiance(AudioStream track)
		{
			PlayAmbiance(track, 1f);
		}

		public void PlayAmbiance(string path)
		{
			PlayAmbiance(GD.Load<AudioStream>(path));
		}

		public void ChangeAmbianceVolume(float volume, float time)
		{
			_ambiancePlayer.ChangeVolume(volume, time);
		}

		public void ChangeAmbianceVolume(float volume)
		{
			ChangeAmbianceVolume(volume, 0.5f);
		}

		public void StopAmbiance()
		{
			_ambiancePlayer.Stop();
		}

		public void StopAmbiance(float time)
		{
			_ambiancePlayer.Stop(time);
		}

		public void PlaySfx(AudioStream track, float volume)
		{
			_sfxPlayer.Play(track, volume);
		}

		public void PlaySfx(AudioStream track)
		{
			PlaySfx(track, 1f);
		}

		public void PlaySfx(string path, float volume)
		{
			PlaySfx(GD.Load<AudioStream>(path), volume);
		}

		public void PlaySfx(string path)
		{
			PlaySfx(path, 1f);
		}

		public void StopSfx()
		{
			_sfxPlayer.Stop();
		}

		public void PlayTextBeepSound()
		{
			_textBeepPlayer.Play();
		}

		public void PlaySystemSound(string path)
		{
			_systemSfxPlayer.Play(GD.Load<AudioStream>(path));
		}

		public void UpdateVolume()
		{
			_bgmPlayer.MaxVolume = CalculateMaxVolume(Game.Settings.VolumeBgm);
			_ambiancePlayer.MaxVolume = CalculateMaxVolume(Game.Settings.VolumeSfx);
			_sfxPlayer.MaxVolume = CalculateMaxVolume(Game.Settings.VolumeSfx);
			_textBeepPlayer.MaxVolume = CalculateMaxVolume(Game.Settings.VolumeText);
			_systemSfxPlayer.MaxVolume = CalculateMaxVolume(Game.Settings.VolumeSystem);
			this.VolumeChanged?.Invoke();
		}

		private float CalculateMaxVolume(decimal value)
		{
			return (float)((decimal)((!Game.Settings.MuteAudio) ? 1 : 0) * Game.Settings.VolumeMaster * 0.90m * value);
		}
	}
}
