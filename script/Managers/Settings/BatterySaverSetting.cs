using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class BatterySaverSetting : Setting<bool>
	{
		private bool value;

		public BatterySaverSetting()
		{
			base.Name = "system.settings.batterysaver";
			value = Game.Settings.BatterySaver;
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
			Game.Settings.SetBatterySaver(value);
		}
	}
}
