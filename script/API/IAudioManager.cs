using System;
using Godot;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IAudioManager : IModule
	{
		AudioStream CurrentBgm { get; }

		event Action VolumeChanged;

		event Action BgmStopped;

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
