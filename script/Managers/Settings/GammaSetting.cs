using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class GammaSetting : Setting<decimal>
	{
		private const decimal Max = 1m;

		private const decimal Min = -1m;

		private const decimal Step = 0.01m;

		private decimal value;

		public GammaSetting()
		{
			base.Name = "system.settings.gamma";
			value = Game.Settings.Gamma;
		}

		public override string ValueLabel()
		{
			if (value <= 0m)
			{
				return value.ToString("0.00");
			}
			return "+" + value.ToString("0.00");
		}

		public override void Decrement()
		{
			value -= 0.01m;
			if (value < -1m)
			{
				value = -1m;
			}
		}

		public override void Increment()
		{
			value += 0.01m;
			if (value > 1m)
			{
				value = 1m;
			}
		}

		public override void Apply()
		{
			Game.Settings.SetGamma(value);
		}
	}
}
