using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class MuteAudioSetting : Setting<bool>
	{
		private bool value;

		public MuteAudioSetting()
		{
			base.Name = "system.settings.volume.mute";
			value = Game.Settings.MuteAudio;
		}

		public override string ValueLabel()
		{
			if (!value)
			{
				return "system.common.no";
			}
			return "system.common.yes";
		}

		public override void Decrement()
		{
			value = !value;
		}

		public override void Increment()
		{
			value = !value;
		}

		public override void Apply()
		{
			Game.Settings.SetMuteAudio(value);
		}
	}
}
