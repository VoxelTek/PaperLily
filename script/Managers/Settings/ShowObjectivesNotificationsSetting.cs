using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class ShowObjectivesNotificationsSetting : Setting<bool>
	{
		private bool value;

		public ShowObjectivesNotificationsSetting()
		{
			base.Name = "system.settings.objectivenotifications";
			value = Game.Settings.ObjectiveNotifications;
		}

		public override string ValueLabel()
		{
			if (!value)
			{
				return "system.common.off";
			}
			return "system.common.on";
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
			Game.Settings.SetObjectiveNotifications(value);
		}
	}
}
