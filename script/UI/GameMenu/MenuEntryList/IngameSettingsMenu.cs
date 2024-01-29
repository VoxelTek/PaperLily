using System.Collections.Generic;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Settings;

namespace LacieEngine.UI
{
	public class IngameSettingsMenu : SimpleVerticalMenu
	{
		public IngameSettingsMenu(IMenuEntryList parentMenu, IMenuEntryContainer container)
		{
			base.Parent = parentMenu;
			base.Container = container;
			base.Entries = new List<IMenuEntry>();
			SubMenuEntry gameOptions = new SubMenuEntry("system.settings.game", this, container);
			gameOptions.AddEntry(new SettingMenuEntry<SkipEnabledSetting, bool>(gameOptions));
			gameOptions.AddEntry(new SettingMenuEntry<ShowObjectivesNotificationsSetting, bool>(gameOptions));
			gameOptions.AddEntry(new BackMenuEntry("system.common.back", gameOptions));
			SubMenuEntry audioOptions = new SubMenuEntry("system.settings.audio", this, container);
			audioOptions.AddEntry(new SettingMenuEntry<VolumeMasterSetting, decimal>(audioOptions));
			audioOptions.AddEntry(new SettingMenuEntry<VolumeBgmSetting, decimal>(audioOptions));
			audioOptions.AddEntry(new SettingMenuEntry<VolumeSfxSetting, decimal>(audioOptions));
			audioOptions.AddEntry(new SettingMenuEntry<VolumeSystemSetting, decimal>(audioOptions));
			audioOptions.AddEntry(new SettingMenuEntry<VolumeTextSetting, decimal>(audioOptions));
			audioOptions.AddEntry(new SettingMenuEntry<MuteAudioSetting, bool>(audioOptions));
			audioOptions.AddEntry(new BackMenuEntry("system.common.back", audioOptions));
			SubMenuEntry videoOptions = new SubMenuEntry("system.settings.video", this, container);
			videoOptions.AddEntry(new FullScreenMenuEntry(videoOptions));
			videoOptions.AddEntry(new ResolutionMenuEntry(videoOptions));
			videoOptions.AddEntry(new SettingMenuEntry2<ResolutionScaleSetting, int>(videoOptions));
			videoOptions.AddEntry(new SettingMenuEntry2<FpsLimitSetting, string>(videoOptions));
			videoOptions.AddEntry(new SettingMenuEntry<BrightnessSetting, decimal>(videoOptions));
			videoOptions.AddEntry(new SettingMenuEntry<ContrastSetting, decimal>(videoOptions));
			videoOptions.AddEntry(new SettingMenuEntry<GammaSetting, decimal>(videoOptions));
			videoOptions.AddEntry(new SettingMenuEntry<HideCursorSetting, bool>(videoOptions));
			videoOptions.AddEntry(new BackMenuEntry("system.common.back", videoOptions));
			SubMenuEntry inputOptions = new SubMenuEntry("system.settings.input", this, container);
			inputOptions.AddEntry(new SettingMenuEntry<InputTypeSetting, string>(inputOptions));
			inputOptions.AddEntry(new SettingMenuEntry2<DeadzoneSetting, float>(inputOptions));
			inputOptions.AddEntry(new SettingMenuEntry<InputAutoSwitchSetting, bool>(inputOptions));
			inputOptions.AddEntry(new InputBindingMenuEntry(inputOptions, "system.actions.up", "input_up"));
			inputOptions.AddEntry(new InputBindingMenuEntry(inputOptions, "system.actions.down", "input_down"));
			inputOptions.AddEntry(new InputBindingMenuEntry(inputOptions, "system.actions.left", "input_left"));
			inputOptions.AddEntry(new InputBindingMenuEntry(inputOptions, "system.actions.right", "input_right"));
			inputOptions.AddEntry(new InputBindingMenuEntry(inputOptions, "system.actions.action", "input_action"));
			inputOptions.AddEntry(new InputBindingMenuEntry(inputOptions, "system.actions.cancel", "input_cancel"));
			inputOptions.AddEntry(new InputBindingMenuEntry(inputOptions, "system.actions.run", "input_run"));
			inputOptions.AddEntry(new InputBindingMenuEntry(inputOptions, "system.actions.special", "input_special"));
			inputOptions.AddEntry(new InputBindingMenuEntry(inputOptions, "system.actions.menu", "input_menu"));
			inputOptions.AddEntry(new BackMenuEntry("system.common.back", inputOptions));
			base.Entries.Add(gameOptions);
			base.Entries.Add(audioOptions);
			base.Entries.Add(videoOptions);
			base.Entries.Add(inputOptions);
		}

		public override void Back()
		{
			if (Game.Settings.InputProfileCustom && !Inputs.Profiles["custom"].IsComplete())
			{
				Log.Info("Input profile not complete!");
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
				MessageContainer errorMsg = new MessageContainer();
				errorMsg.OnClose = delegate
				{
					errorMsg.Delete();
				};
				errorMsg.Text = "system.settings.input.error";
				Game.Screen.AddToLayer(IScreenManager.Layer.Screen, errorMsg);
			}
			else
			{
				Game.Settings.SaveSettings();
				Game.Settings.RevertSettings();
				if (base.OnBack != null)
				{
					base.OnBack();
				}
				else if (base.Parent != null)
				{
					base.Parent.Root();
				}
			}
		}
	}
}
