using Godot;
using LacieEngine.Core;

namespace LacieEngine.Audio
{
	public class FixedSamplePlayer : Node
	{
		private const float DEFAULT_FREQUENCY = 0.06f;

		private float _waitTime;

		private bool _playing;

		private AudioStreamPlayer nPlayer;

		private float _maxVolume;

		public float Frequency { get; set; } = 0.06f;


		public float MaxVolume
		{
			get
			{
				return _maxVolume;
			}
			set
			{
				_maxVolume = value;
				nPlayer.VolumeDb = GD.Linear2Db(value);
			}
		}

		private FixedSamplePlayer()
		{
		}

		public FixedSamplePlayer(AudioStreamPlayer player, AudioStream track)
		{
			base.Name = "FixedSamplePlayer";
			nPlayer = player;
			nPlayer.Stream = track;
			Game.Root.AddChild(this);
			SetProcess(enable: false);
		}

		public override void _Process(float delta)
		{
			_waitTime -= delta;
			if (_playing && _waitTime <= 0f)
			{
				nPlayer.Play();
				_waitTime = Frequency;
				_playing = false;
			}
			else if (_waitTime <= 0f)
			{
				_waitTime = 0f;
				SetProcess(enable: false);
			}
		}

		public void Play()
		{
			_playing = true;
			SetProcess(enable: true);
		}
	}
}
