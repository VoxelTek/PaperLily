using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class ContrastSetting : Setting<decimal>
	{
		private const decimal Max = 1.5m;

		private const decimal Min = 0.5m;

		private const decimal Step = 0.01m;

		private decimal value;

		public ContrastSetting()
		{
			base.Name = "system.settings.contrast";
			value = Game.Settings.Contrast;
		}

		public override string ValueLabel()
		{
			return (int)(value * 100m) + "%";
		}

		public override void Decrement()
		{
			value -= 0.01m;
			if (value < 0.5m)
			{
				value = 0.5m;
			}
		}

		public override void Increment()
		{
			value += 0.01m;
			if (value > 1.5m)
			{
				value = 1.5m;
			}
		}

		public override void Apply()
		{
			Game.Settings.SetContrast(value);
		}
	}
}
