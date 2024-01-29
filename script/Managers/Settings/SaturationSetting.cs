using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class SaturationSetting : Setting<decimal>
	{
		private const decimal Max = 2m;

		private const decimal Min = 0m;

		private const decimal Step = 0.01m;

		private decimal value;

		public SaturationSetting()
		{
			base.Name = "system.settings.saturation";
			value = Game.Settings.Saturation;
		}

		public override string ValueLabel()
		{
			return (int)(value * 100m) + "%";
		}

		public override void Decrement()
		{
			value -= 0.01m;
			if (value < 0m)
			{
				value = default(decimal);
			}
		}

		public override void Increment()
		{
			value += 0.01m;
			if (value > 2m)
			{
				value = 2m;
			}
		}

		public override void Apply()
		{
			Game.Settings.SetSaturation(value);
		}
	}
}
