using Godot;
using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class RendererSetting : Setting<string>
	{
		private static readonly string[] Options = new string[2] { "GLES2", "GLES3" };

		private string value;

		private bool changed;

		private int selection;

		public override string CaptionLabel()
		{
			if (changed && value != OS.GetCurrentVideoDriver().ToString().ToUpper())
			{
				return "system.settings.renderer";
			}
			return Game.Language.GetCaption("system.settings.renderer") + " (" + Game.Language.GetFormattedCaption("system.settings.current", OS.GetCurrentVideoDriver().ToString().ToUpper()) + ")";
		}

		public RendererSetting()
		{
			changed = false;
			value = Game.Settings.Renderer;
			for (int i = 0; i < Options.Length; i++)
			{
				if (Options[i] == value)
				{
					selection = i;
				}
			}
		}

		public override string ValueLabel()
		{
			string rendererText = ((!(value == "GLES2")) ? "system.settings.renderer.gles3" : "system.settings.renderer.gles2");
			if (changed && value != OS.GetCurrentVideoDriver().ToString().ToUpper())
			{
				return Game.Language.GetCaption(rendererText) + " (" + Game.Language.GetCaption("system.settings.restartrequired") + ")";
			}
			return rendererText;
		}

		public override void Decrement()
		{
			changed = true;
			selection--;
			if (selection < 0)
			{
				selection = Options.Length - 1;
			}
			value = Options[selection];
		}

		public override void Increment()
		{
			changed = true;
			selection++;
			if (selection >= Options.Length)
			{
				selection = 0;
			}
			value = Options[selection];
		}

		public override void Apply()
		{
			Game.Settings.SetRenderer(value);
		}
	}
}
