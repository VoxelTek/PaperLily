using Godot;

namespace LacieEngine.Audio
{
	public class SamplePlayer
	{
		private AudioStreamPlayer[] nChannels;

		private int _nextChannel;

		public float MaxVolume { get; set; }

		public SamplePlayer(AudioStreamPlayer[] channels)
		{
			nChannels = channels;
		}

		public void Play(AudioStream track, float volume = 1f)
		{
			nChannels[_nextChannel].Stream = track;
			nChannels[_nextChannel].VolumeDb = GD.Linear2Db(MaxVolume * volume);
			nChannels[_nextChannel].Play();
			if (++_nextChannel >= nChannels.Length)
			{
				_nextChannel = 0;
			}
		}

		public void Stop()
		{
			AudioStreamPlayer[] array = nChannels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Stop();
			}
		}

		public bool IsPlaying(int channel)
		{
			return nChannels[channel].Playing;
		}
	}
}
