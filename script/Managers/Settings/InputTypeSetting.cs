using System.Collections.Generic;
using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class InputTypeSetting : Setting<string>
	{
		private readonly List<string> Options;

		private string value;

		private int selection;

		public InputTypeSetting()
		{
			base.Name = "system.settings.input.profile";
			Options = new List<string>();
			List<InputProfile> list = new List<InputProfile>(Inputs.Profiles.Values);
			list.Sort();
			foreach (InputProfile profile in list)
			{
				if (!profile.Name.StartsWith("_"))
				{
					Options.Add(profile.Name);
				}
			}
			value = Game.Settings.InputProfile;
			for (int i = 0; i < Options.Count; i++)
			{
				if (Options[i] == value)
				{
					selection = i;
				}
			}
		}

		public override string ValueLabel()
		{
			string type = ((Inputs.Profiles[Game.Settings.InputProfile].Type == InputProfile.InputType.Keyboard) ? "system.common.keyboard" : "system.common.controller");
			return Game.Language.GetFormattedCaption("system.settings.input.profile.caption", Game.Language.GetCaption(type), Game.Language.GetCaption(Inputs.Profiles[Game.Settings.InputProfile].Caption));
		}

		public override void Decrement()
		{
			selection--;
			if (selection < 0)
			{
				selection = Options.Count - 1;
			}
			value = Options[selection];
		}

		public override void Increment()
		{
			selection++;
			if (selection >= Options.Count)
			{
				selection = 0;
			}
			value = Options[selection];
		}

		public override void Apply()
		{
			Game.Settings.SetInputProfile(value);
		}
	}
}
