using System.Collections.Generic;
using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;

namespace LacieEngine.Audio
{
	public class AdvancedMusicPlayer : Node
	{
		public class AudioInformation
		{
			public AudioStream Stream;

			public float LeftAttachPoint;

			public float RightAttchPoint;
		}

		public const string NODE_NAME = "AdvancedMusicPlayer";

		private AudioStreamPlayer nCurrentPlayer;

		private AudioStreamPlayer nAltPlayer;

		private float _maxVolume;

		private AudioInformation _current;

		private Queue<AudioInformation> _queue = new Queue<AudioInformation>();

		private bool _playing;

		public bool Loop { get; set; }

		public bool Playing => _playing;

		public bool Empty => _queue.IsEmpty();

		public override void _EnterTree()
		{
			base.Name = "AdvancedMusicPlayer";
			nCurrentPlayer = new AudioStreamPlayer();
			AddChild(nCurrentPlayer);
			nAltPlayer = new AudioStreamPlayer();
			AddChild(nAltPlayer);
			UpdateMaxVolume();
			Game.Audio.VolumeChanged += UpdateMaxVolume;
			Log.Debug("[AdvancedMusicPlayer] Created.");
		}

		public override void _ExitTree()
		{
			Game.Audio.VolumeChanged -= UpdateMaxVolume;
		}

		public override void _Process(float delta)
		{
			if (_playing && !_queue.IsEmpty() && nCurrentPlayer.GetPlaybackPosition() >= _current.RightAttchPoint - _queue.Peek().LeftAttachPoint)
			{
				Log.Debug("[AdvancedMusicPlayer] Switching Track...");
				_current = _queue.Dequeue();
				nAltPlayer.Stream = _current.Stream;
				nAltPlayer.VolumeDb = GD.Linear2Db(_maxVolume);
				nAltPlayer.Playing = true;
				SwapChannels();
			}
			else if (_playing && Loop && _queue.IsEmpty() && nCurrentPlayer.GetPlaybackPosition() >= _current.RightAttchPoint - _current.LeftAttachPoint)
			{
				Log.Debug("[AdvancedMusicPlayer] Looping Track...");
				nAltPlayer.Stream = _current.Stream;
				nAltPlayer.VolumeDb = GD.Linear2Db(_maxVolume);
				nAltPlayer.Playing = true;
				SwapChannels();
			}
			if (!nCurrentPlayer.Playing && !nAltPlayer.Playing)
			{
				_playing = false;
			}
		}

		public void Play(AudioInformation audio)
		{
			if (!nCurrentPlayer.Playing || audio.Stream != _current.Stream)
			{
				Log.Debug("[AdvancedMusicPlayer] Starting Track...");
				Stop();
				_current = audio;
				nCurrentPlayer.Stream = audio.Stream;
				nCurrentPlayer.VolumeDb = GD.Linear2Db(_maxVolume);
				nCurrentPlayer.Play();
				_playing = true;
			}
		}

		public void Stop(float time = 1.5f)
		{
			Log.Debug("[AdvancedMusicPlayer] Stopping...");
			_playing = false;
			Loop = false;
			_queue.Clear();
			StopPlayer(nCurrentPlayer, time);
			StopPlayer(nAltPlayer, time);
		}

		public void Queue(AudioInformation audio)
		{
			Log.Debug("[AdvancedMusicPlayer] Queuing...");
			if (!_playing)
			{
				Play(audio);
			}
			else
			{
				_queue.Enqueue(audio);
			}
		}

		public void ClearQueue()
		{
			Log.Debug("[AdvancedMusicPlayer] Clearing Queue...");
			_queue.Clear();
		}

		public void UpdateMaxVolume()
		{
			decimal volume = (decimal)((!Game.Settings.MuteAudio) ? 1 : 0) * Game.Settings.VolumeMaster * 0.90m * Game.Settings.VolumeBgm;
			_maxVolume = (float)volume;
			if (nCurrentPlayer.Playing)
			{
				nCurrentPlayer.VolumeDb = GD.Linear2Db((float)volume);
			}
		}

		private async void StopPlayer(AudioStreamPlayer player, float time)
		{
			if (player.Playing)
			{
				LacieAnimation volumeFadeOut = new AudioVolumeFadeOutAnimation(nCurrentPlayer, time);
				Game.Animations.Play(volumeFadeOut);
				await volumeFadeOut.WaitUntilFinished();
				player.Stop();
				SwapChannels();
			}
		}

		private void SwapChannels()
		{
			AudioStreamPlayer audioStreamPlayer = nCurrentPlayer;
			AudioStreamPlayer audioStreamPlayer2 = nAltPlayer;
			nAltPlayer = audioStreamPlayer;
			nCurrentPlayer = audioStreamPlayer2;
		}
	}
}
