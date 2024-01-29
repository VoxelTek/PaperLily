using System.Threading.Tasks;
using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;

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
			get
			{
				return _maxVolume;
			}
			set
			{
				_maxVolume = value;
				if (nCurrentPlayer.Playing)
				{
					nCurrentPlayer.VolumeDb = GD.Linear2Db(currentVolume * value);
				}
			}
		}

		public AudioStream CurrentTrack => currentTrack;

		public AudioPlayer(AudioStreamPlayer player1, AudioStreamPlayer player2)
		{
			nCurrentPlayer = player1;
			nAltPlayer = player2;
			nextFadeDuration = 1.5f;
		}

		public void Play(AudioStream track, float volume = 1f, bool crossFade = false)
		{
			nextTrack = track;
			nextVolume = volume;
			_crossFade = crossFade;
			UpdateAudio();
		}

		private async Task SwitchDynamic(AudioStream track, float volume)
		{
			if (!nCurrentPlayer.Playing)
			{
				nCurrentPlayer.VolumeDb = GD.Linear2Db(_maxVolume * volume);
				nCurrentPlayer.Stream = track;
				nCurrentPlayer.Play();
				return;
			}
			nAltPlayer.Playing = false;
			nAltPlayer.Stream = track;
			nAltPlayer.Playing = true;
			nAltPlayer.Seek(nCurrentPlayer.GetPlaybackPosition());
			Game.Animations.Play(new AudioVolumeFadeInAnimation(nAltPlayer, volume * _maxVolume, 3f));
			Game.Animations.Play(new AudioVolumeFadeOutAnimation(nCurrentPlayer, 3f));
			await DrkieUtil.DelaySeconds(3.0);
			AudioStreamPlayer audioStreamPlayer = nCurrentPlayer;
			AudioStreamPlayer audioStreamPlayer2 = nAltPlayer;
			nAltPlayer = audioStreamPlayer;
			nCurrentPlayer = audioStreamPlayer2;
		}

		public void Stop(float time = 1.5f)
		{
			nextTrack = null;
			nextVolume = 1f;
			nextFadeDuration = time;
			UpdateAudio();
		}

		public void ChangeVolume(float volume, float time = 0.5f)
		{
			nextVolume = volume;
			nextFadeDuration = time;
			UpdateAudio();
		}

		private async void UpdateAudio()
		{
			if (!updating && (nextTrack != currentTrack || nextVolume != currentVolume))
			{
				updating = true;
				if (nextTrack == currentTrack && nextVolume != currentVolume)
				{
					Log.Debug("Audio: Change volume.");
					LacieAnimation volumeChangeAnimation = new AudioVolumeFadeAnimation(nCurrentPlayer, nextFadeDuration, currentVolume * _maxVolume, nextVolume * _maxVolume);
					Game.Animations.Play(volumeChangeAnimation);
					await volumeChangeAnimation.WaitUntilFinished();
					currentVolume = nextVolume;
				}
				else if (_crossFade && IsDynamicSwitch(currentTrack, nextTrack))
				{
					Log.Debug("Audio: Dynamic switch.");
					AudioStream nextTrackAsync = nextTrack;
					float nextVolumeAsync = nextVolume;
					await SwitchDynamic(nextTrack, nextVolume);
					currentTrack = nextTrackAsync;
					currentVolume = nextVolumeAsync;
				}
				else if (nextTrack != currentTrack && currentTrack != null)
				{
					Log.Debug("Audio: Stop track.");
					await FadeOut(nextFadeDuration);
					nextFadeDuration = 1.5f;
					nCurrentPlayer.Stop();
					currentTrack = null;
					currentVolume = 1f;
				}
				else if (nextTrack != currentTrack && nextTrack != null)
				{
					Log.Debug("Audio: Play track.");
					currentTrack = nextTrack;
					currentVolume = nextVolume;
					nCurrentPlayer.VolumeDb = GD.Linear2Db(_maxVolume * nextVolume);
					nCurrentPlayer.Stream = nextTrack;
					nCurrentPlayer.Play();
				}
				updating = false;
				UpdateAudio();
			}
		}

		private async Task FadeOut(float time)
		{
			LacieAnimation animation = new AudioVolumeFadeOutAnimation(nCurrentPlayer, time);
			Game.Animations.Play(animation);
			await animation.WaitUntilFinished();
		}

		private bool IsDynamicSwitch(AudioStream track1, AudioStream track2)
		{
			if (track1 == null || track2 == null)
			{
				return false;
			}
			string resourcePath = track1.ResourcePath;
			int num = track1.ResourcePath.LastIndexOf("/") + 1;
			string track1Name = resourcePath.Substring(num, resourcePath.Length - num).StripSuffix(".ogg");
			resourcePath = track2.ResourcePath;
			num = track2.ResourcePath.LastIndexOf("/") + 1;
			string track2Name = resourcePath.Substring(num, resourcePath.Length - num).StripSuffix(".ogg");
			return track1Name.Substring(0, track1Name.LastIndexOf("_")) == track2Name.Substring(0, track2Name.LastIndexOf("_"));
		}
	}
}
