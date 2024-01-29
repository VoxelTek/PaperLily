using Godot;

namespace LacieEngine.Core
{
	public class InputMapper
	{
		public static readonly uint[] Blacklist = new uint[20]
		{
			16777244u, 16777245u, 16777246u, 16777247u, 16777248u, 16777249u, 16777250u, 16777251u, 16777252u, 16777253u,
			16777254u, 16777255u, 16777226u, 16777260u, 16777261u, 16777239u, 16777262u, 16777243u, 16777225u, 134217728u
		};

		public static InputProfile CurrentProfile { get; private set; }

		public static void SetProfile(string profileName)
		{
			if (!Inputs.Profiles.ContainsKey(Game.Settings.DefaultInputProfile))
			{
				Log.Error("FATAL ERROR: Default input profile is missing! (", profileName, ")");
				return;
			}
			if (!Inputs.Profiles.ContainsKey(profileName))
			{
				Log.Error("Invalid input profile ", profileName, " - reverting to default!");
				Game.Settings.SetInputProfile(Game.Settings.DefaultInputProfile);
				SetProfile(Game.Settings.DefaultInputProfile);
				return;
			}
			Log.Info("Switching input profile to ", profileName);
			ClearAllMappings();
			InputProfile profile = Inputs.Profiles[profileName];
			RegisterAllActions(profile);
			if (profile.Type == InputProfile.InputType.Keyboard)
			{
				RegisterAllActions(Inputs.Profiles[Game.Settings.DefaultInputProfileController]);
			}
			else if (profile.Type == InputProfile.InputType.Controller)
			{
				RegisterAllActions(Inputs.Profiles[Game.Settings.DefaultInputProfileKeyboard]);
			}
			RegisterAllActions(Inputs.SystemKeys);
			if (OS.IsDebugBuild())
			{
				RegisterAllActions(Inputs.DebugKeys);
			}
			foreach (string action in InputMap.GetActions())
			{
				InputMap.ActionSetDeadzone(action, Game.Settings.JoystickDeadzone);
			}
			CurrentProfile = profile;
		}

		public static void CloneIntoCustomProfile(string profileName)
		{
			InputProfile profile = new InputProfile(Inputs.Profiles[profileName]);
			profile.Name = "custom";
			profile.Caption = "system.settings.input.profile.custom";
			Inputs.Profiles["custom"] = profile;
		}

		public static void SaveCustomProfile()
		{
			GDUtil.SaveJsonFile(new InputProfileDTO(Inputs.Profiles["custom"])
			{
				Order = null,
				Captions = null
			}, "user://bindings.json");
		}

		private static void RegisterAllActions(InputProfile profile)
		{
			foreach (InputProfile.Mapping mapping in profile.Mappings)
			{
				InputMap.ActionAddEvent(mapping.Action, mapping.Event);
			}
		}

		private static void ClearAllMappings()
		{
			foreach (string action in InputMap.GetActions())
			{
				InputMap.ActionEraseEvents(action);
			}
		}
	}
}
