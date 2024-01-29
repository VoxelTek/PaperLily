using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class HideCursorSetting : Setting<bool>
	{
		private bool value;

		public HideCursorSetting()
		{
			base.Name = "system.settings.hidecursor";
			value = Game.Settings.HideCursor;
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
			Game.Settings.SetCursorHidden(value);
		}
	}
}
