using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class BrightnessSetting : Setting<decimal>
	{
		private const decimal Max = 2m;

		private const decimal Min = 0.1m;

		private const decimal Step = 0.01m;

		private decimal value;

		public BrightnessSetting()
		{
			base.Name = "system.settings.brightness";
			value = Game.Settings.Brightness;
		}

		public override string ValueLabel()
		{
			return (int)(value * 100m) + "%";
		}

		public override void Decrement()
		{
			value -= 0.01m;
			if (value < 0.1m)
			{
				value = 0.1m;
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
			Game.Settings.SetBrightness(value);
		}
	}
}
