using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class VolumeMasterSetting : Setting<decimal>
	{
		private const decimal Max = 1m;

		private const decimal Min = 0m;

		private const decimal Step = 0.05m;

		private decimal value;

		public VolumeMasterSetting()
		{
			base.Name = "system.settings.volume.master";
			value = Game.Settings.VolumeMaster;
		}

		public override string ValueLabel()
		{
			return (int)(value * 100m) + "%";
		}

		public override void Decrement()
		{
			value -= 0.05m;
			if (value < 0m)
			{
				value = default(decimal);
			}
		}

		public override void Increment()
		{
			value += 0.05m;
			if (value > 1m)
			{
				value = 1m;
			}
		}

		public override void Apply()
		{
			Game.Settings.SetVolumeMaster(value);
		}
	}
}
