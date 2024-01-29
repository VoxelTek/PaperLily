using Godot;
using LacieEngine.Core;

namespace LacieEngine.Settings
{
	public class FullScreenSetting : Setting<bool>
	{
		private bool value;

		public FullScreenSetting()
		{
			base.Name = "system.settings.fullscreen";
			value = OS.WindowFullscreen;
		}

		public override string ValueLabel()
		{
			if (!OS.WindowFullscreen)
			{
				return "system.common.no";
			}
			return "system.common.yes";
		}

		public override void Decrement()
		{
			value = !OS.WindowFullscreen;
		}

		public override void Increment()
		{
			value = !OS.WindowFullscreen;
		}

		public override void Apply()
		{
			Game.Settings.SetFullScreen(value);
		}
	}
}
