using System;
using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class ResolutionScaleSetting : Setting<int>
	{
		private static readonly Lazy<ResolutionScaleSetting> _lazyInstance = new Lazy<ResolutionScaleSetting>(() => new ResolutionScaleSetting());

		private static readonly int[] Options = new int[6] { 1, 2, 3, 4, 6, 8 };

		private int selection;

		public static ResolutionScaleSetting Instance => _lazyInstance.Value;

		private ResolutionScaleSetting()
		{
			base.Name = "system.settings.resolutionscale";
			base.Path = "lacie_engine/video/resolution_scale_pixel";
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
			return base.Value + "x";
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
			Game.Screen?.RefreshPixelScaling();
			Game.Room?.RefreshPixelScaling();
			Game.Camera?.RefreshZoom();
		}
	}
}
