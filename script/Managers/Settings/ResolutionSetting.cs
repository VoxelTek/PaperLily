using Godot;
using LacieEngine.Core;

namespace LacieEngine.Settings
{
	public class ResolutionSetting : Setting<Vector2>
	{
		private static readonly Vector2[] StandardResolutions = new Vector2[6]
		{
			new Vector2(640f, 360f),
			new Vector2(1280f, 720f),
			new Vector2(1920f, 1080f),
			new Vector2(2560f, 1440f),
			new Vector2(3200f, 1800f),
			new Vector2(3840f, 2160f)
		};

		private Vector2 value;

		public ResolutionSetting()
		{
			base.Name = "system.settings.resolution";
			ApplyCurrentResolution();
		}

		public override string ValueLabel()
		{
			Vector2 resolution = Game.Root.Size;
			return resolution.x + "x" + resolution.y;
		}

		public override void Decrement()
		{
			Vector2 resolution = Game.Root.Size;
			value = StandardResolutions[0];
			Vector2[] standardResolutions = StandardResolutions;
			foreach (Vector2 res in standardResolutions)
			{
				if (!(res >= resolution))
				{
					value = res;
					continue;
				}
				break;
			}
		}

		public override void Increment()
		{
			Vector2 resolution = Game.Root.Size;
			Vector2[] standardResolutions = StandardResolutions;
			for (int i = 0; i < standardResolutions.Length && !((value = standardResolutions[i]) > resolution); i++)
			{
			}
		}

		public override void Apply()
		{
			Game.Settings.SetFullScreen(fullScreen: false);
			Game.Settings.SetResolution(value);
		}

		public void ApplyCurrentResolution()
		{
			value = OS.WindowSize;
			Game.Settings.SetFullScreen(OS.WindowFullscreen);
			Game.Settings.SetResolution(OS.WindowSize);
		}
	}
}
