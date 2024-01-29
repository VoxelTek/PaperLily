using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class SkipEnabledSetting : Setting<bool>
	{
		private bool value;

		public SkipEnabledSetting()
		{
			base.Name = "system.settings.game.skipenabled";
			value = Game.Settings.SkipEnabled;
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
			Game.Settings.SetSkipEnabled(value);
		}
	}
}
