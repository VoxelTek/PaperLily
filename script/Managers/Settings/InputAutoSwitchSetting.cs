using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class InputAutoSwitchSetting : Setting<bool>
	{
		private bool value;

		public InputAutoSwitchSetting()
		{
			base.Name = "system.settings.input.autoswitch";
			value = Game.Settings.InputAutoSwitch;
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
			Game.Settings.SetInputAutoSwitch(value);
		}
	}
}
