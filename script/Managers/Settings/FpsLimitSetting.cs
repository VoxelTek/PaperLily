using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class FpsLimitSetting : Setting<string>
	{
		public class FpsAgent : Node
		{
			private const float Tick = 1f;

			private float _elapsed;

			private int _lastValue = -1;

			public void Init()
			{
				base.Name = "FpsAgent";
				Game.Root.AddChild(this);
				AdjustFps();
			}

			public override void _Process(float delta)
			{
				_elapsed += delta;
				if (!(_elapsed < 1f))
				{
					_elapsed = 0f;
					AdjustFps();
				}
			}

			private void AdjustFps()
			{
				int hz = GetTargetRefreshRate();
				if (Instance.Value == "gsync")
				{
					hz -= 2;
				}
				if (_lastValue != hz)
				{
					Engine.TargetFps = (_lastValue = hz);
					Engine.IterationsPerSecond = Math.Min(hz, 60);
				}
			}
		}

		public const string FPS_AUTO = "auto";

		public const string FPS_VSYNC = "vsync";

		public const string FPS_VSYNC_COMPOSITOR = "vsync-comp";

		public const string FPS_GSYNC = "gsync";

		public const string FPS_UNLOCKED = "unlocked";

		private const int MAX_PHYSICS_FPS = 60;

		private static readonly string[] Options = new string[17]
		{
			"auto", "vsync", "vsync-comp", "gsync", "unlocked", "30", "40", "50", "60", "75",
			"90", "100", "120", "144", "155", "165", "240"
		};

		private static readonly Lazy<FpsLimitSetting> _lazyInstance = new Lazy<FpsLimitSetting>(() => new FpsLimitSetting());

		private FpsAgent nFpsAgent;

		private int selection;

		public static FpsLimitSetting Instance => _lazyInstance.Value;

		private FpsLimitSetting()
		{
			base.Name = "system.settings.fpslimit";
			base.Path = "lacie_engine/video/fps_limit";
			base.Value = ReadValue();
			selection = -1;
			for (int i = 0; i < Options.Length; i++)
			{
				if (Options[i] == base.Value)
				{
					selection = i;
				}
			}
		}

		public override string ValueLabel()
		{
			return base.Value switch
			{
				"auto" => "system.common.auto", 
				"gsync" => "system.settings.gsync", 
				"vsync" => "system.settings.vsync", 
				"vsync-comp" => "system.settings.compvsync", 
				"unlocked" => "system.settings.fpsunlocked", 
				_ => base.Value, 
			};
		}

		public override void Decrement()
		{
			selection--;
			if (selection < 0)
			{
				selection = Options.Length - 1;
			}
			base.Value = Options[selection];
		}

		public override void Increment()
		{
			selection++;
			if (selection >= Options.Length)
			{
				selection = 0;
			}
			base.Value = Options[selection];
		}

		public override void Apply()
		{
			switch (base.Value)
			{
			case "auto":
				OS.VsyncEnabled = false;
				OS.VsyncViaCompositor = false;
				EnableFpsAgent();
				break;
			case "gsync":
				OS.VsyncEnabled = true;
				OS.VsyncViaCompositor = false;
				EnableFpsAgent();
				break;
			case "vsync":
				DisableFpsAgent();
				OS.VsyncEnabled = true;
				OS.VsyncViaCompositor = false;
				Engine.TargetFps = 0;
				Engine.IterationsPerSecond = Math.Min(GetTargetRefreshRate(), 60);
				break;
			case "vsync-comp":
				DisableFpsAgent();
				OS.VsyncEnabled = true;
				OS.VsyncViaCompositor = true;
				Engine.TargetFps = 0;
				Engine.IterationsPerSecond = Math.Min(GetTargetRefreshRate(), 60);
				break;
			case "unlocked":
				DisableFpsAgent();
				OS.VsyncEnabled = false;
				OS.VsyncViaCompositor = false;
				Engine.TargetFps = 0;
				Engine.IterationsPerSecond = 60;
				break;
			default:
				DisableFpsAgent();
				OS.VsyncEnabled = false;
				OS.VsyncViaCompositor = false;
				Engine.TargetFps = int.Parse(base.Value);
				Engine.IterationsPerSecond = Math.Min(int.Parse(base.Value), 60);
				break;
			}
		}

		public static int GetTargetRefreshRate()
		{
			if (OS.GetScreenRefreshRate() == -1f)
			{
				return 60;
			}
			return (int)Math.Ceiling(OS.GetScreenRefreshRate());
		}

		private void EnableFpsAgent()
		{
			if (!nFpsAgent.IsValid())
			{
				nFpsAgent = new FpsAgent();
				nFpsAgent.Init();
			}
		}

		private void DisableFpsAgent()
		{
			nFpsAgent.DeleteIfValid();
			nFpsAgent = null;
		}
	}
}
