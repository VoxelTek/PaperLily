using Godot;
using LacieEngine.Core;

namespace LacieEngine.Subgame.Chapter1
{
	public class HomePhoneRinger : Node
	{
		private PVar varPhoneCallRinging = "ch1.home_phone_call_ringing";

		public const float RingerTimer = 3f;

		private float _timer = 1.5f;

		private AudioStream _sfx;

		private float _volume;

		private HomePhoneRinger()
		{
		}

		public HomePhoneRinger(AudioStream sfx, float volume)
		{
			_sfx = sfx;
			_volume = volume;
		}

		public override void _Process(float delta)
		{
			_timer -= delta;
			if (!(_timer > 0f))
			{
				if (!varPhoneCallRinging.Value)
				{
					this.Delete();
					return;
				}
				Game.Audio.PlaySfx(_sfx, _volume);
				_timer = 3f;
			}
		}
	}
}
