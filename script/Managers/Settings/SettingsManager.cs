// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.SettingsManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using Godot.Collections;
using LacieEngine.Core;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Settings
{
	public class SettingsManager
	{
		private System.Collections.Generic.Dictionary<System.Type, SettingParent> _settings;
		private System.Collections.Generic.Dictionary<string, object> unsaved;
		private bool loadedCoreSettings;

		public string ProductName { get; private set; }

		public string ProductVersion { get; private set; }

		public string ProductCopyright { get; private set; }

		public string EngineVersion { get; private set; }

		public string SettingsFilename { get; private set; }

		public string TranslationBaseLocale { get; private set; }

		public string TranslationBaseName { get; private set; }

		public IDictionary<string, string[]> TranslationRemaps { get; private set; }

		public bool WebsiteEnabled { get; private set; }

		public string WebsiteLink { get; private set; }

		public string WebsiteCaption { get; private set; }

		public string DefaultInputProfile { get; private set; }

		public string DefaultInputProfileKeyboard { get; private set; }

		public string DefaultInputProfileController { get; private set; }

		public string SaveKey { get; private set; }

		public string LsbKey { get; private set; }

		public string NewGameEvent { get; private set; }

		public SceneProvider SceneProvider { get; private set; }

		public UiLookAndFeelProvider UiProvider { get; private set; }

		public string DefaultCharacter { get; private set; }

		public bool SteamBuild { get; private set; }

		public int SteamAppId { get; private set; }

		public Vector2 BaseResolution { get; private set; }

		public string DebugRoom { get; private set; }

		public bool DebugQuickstartOn { get; private set; }

		public string DebugQuickstartRoom { get; private set; }

		public string DebugQuickstartPoint { get; private set; }

		public int ResolutionX { get; private set; }

		public int ResolutionY { get; private set; }

		public bool FullScreen { get; private set; }

		public Decimal VolumeMaster { get; private set; }

		public Decimal VolumeBgm { get; private set; }

		public Decimal VolumeSfx { get; private set; }

		public Decimal VolumeSystem { get; private set; }

		public Decimal VolumeText { get; private set; }

		public bool MuteAudio { get; private set; }

		public Decimal Brightness { get; private set; }

		public Decimal Contrast { get; private set; }

		public Decimal Saturation { get; private set; }

		public Decimal Gamma { get; private set; }

		public string Renderer { get; private set; }

		public int FpsLimit => this.GetSettingValue<int, FpsLimitSetting>();

		public int ResolutionScalePixel => this.GetSettingValue<int, ResolutionScaleSetting>();

		public bool BatterySaver { get; private set; }

		public bool HideCursor { get; private set; }

		public bool ObjectiveNotifications { get; private set; }

		public string InputProfile { get; private set; }

		public bool InputProfileCustom { get; private set; }

		public bool SkipEnabled { get; private set; }

		public bool InputAutoSwitch { get; private set; }

		public float JoystickDeadzone => this.GetSettingValue<float, DeadzoneSetting>();

		public string TranslationSelected { get; private set; }

		public bool TranslationExtraEnabled {
			get => this.GetSettingValue<bool, TranslationExtraEnabledSetting>();
		}

		public bool ShowSkipOnDeath {
			get => this.GetSettingValue<bool, ShowSkipOnDeathSetting>();
		}

		public void LoadSettings(bool fullReload = false)
		{
			this._settings = new System.Collections.Generic.Dictionary<System.Type, SettingParent>();
			this._settings[typeof(FpsLimitSetting)] = (SettingParent)FpsLimitSetting.Instance;
			this._settings[typeof(ResolutionScaleSetting)] = (SettingParent)ResolutionScaleSetting.Instance;
			this._settings[typeof(DeadzoneSetting)] = (SettingParent)DeadzoneSetting.Instance;
			this._settings[typeof(TranslationExtraEnabledSetting)] = (SettingParent)TranslationExtraEnabledSetting.Instance;
			this._settings[typeof(ShowSkipOnDeathSetting)] = (SettingParent)ShowSkipOnDeathSetting.Instance;
			Log.Info((object)"Loading settings...");
			if (fullReload)
				this.loadedCoreSettings = false;
			this.LoadCoreSettings();
			this.ResolutionX = GDUtil.ReadSetting<int>("display/window/size/test_width");
			this.ResolutionY = GDUtil.ReadSetting<int>("display/window/size/test_height");
			this.FullScreen = GDUtil.ReadSetting<bool>("display/window/size/fullscreen");
			this.VolumeMaster = (Decimal)GDUtil.ReadSetting<float>("lacie_engine/audio/volume_master");
			this.VolumeBgm = (Decimal)GDUtil.ReadSetting<float>("lacie_engine/audio/volume_bgm");
			this.VolumeSfx = (Decimal)GDUtil.ReadSetting<float>("lacie_engine/audio/volume_sfx");
			this.VolumeSystem = (Decimal)GDUtil.ReadSetting<float>("lacie_engine/audio/volume_system");
			this.VolumeText = (Decimal)GDUtil.ReadSetting<float>("lacie_engine/audio/volume_text");
			this.MuteAudio = GDUtil.ReadSetting<bool>("lacie_engine/audio/mute");
			this.Brightness = (Decimal)GDUtil.ReadSetting<float>("lacie_engine/video/brightness");
			this.Contrast = (Decimal)GDUtil.ReadSetting<float>("lacie_engine/video/contrast");
			this.Saturation = (Decimal)GDUtil.ReadSetting<float>("lacie_engine/video/saturation");
			this.Gamma = (Decimal)GDUtil.ReadSetting<float>("lacie_engine/video/gamma");
			this.Renderer = GDUtil.ReadSetting<string>("rendering/quality/driver/driver_name");
			this.BatterySaver = GDUtil.ReadSetting<bool>("application/run/low_processor_mode");
			this.HideCursor = GDUtil.ReadSetting<bool>("lacie_engine/input/hide_cursor");
			this.ObjectiveNotifications = GDUtil.ReadSetting<bool>("lacie_engine/game/objective_notifications");
			this.InputProfile = GDUtil.ReadSetting<string>("lacie_engine/input/profile");
			this.InputProfileCustom = GDUtil.ReadSetting<bool>("lacie_engine/input/profile_custom");
			this.SkipEnabled = GDUtil.ReadSetting<bool>("lacie_engine/game/skip_enabled");
			this.InputAutoSwitch = GDUtil.ReadSetting<bool>("lacie_engine/input/auto_switch");
			this.TranslationSelected = GDUtil.ReadSetting<string>("lacie_engine/core/translation_selected");
			this.unsaved = new System.Collections.Generic.Dictionary<string, object>();
		}

		public T GetSettingValue<T, U>() => ((Setting<T>)this._settings[typeof(U)]).ReadValue();

		public Setting<T> GetSetting<T, U>() => (Setting<T>)this._settings[typeof(U)];

		public void SaveSettings()
		{
			if (this.unsaved.ContainsKey("lacie_engine/input/profile"))
			{
				string key = (string)this.unsaved["lacie_engine/input/profile"];
				if (Inputs.Profiles[key].Type == LacieEngine.Core.InputProfile.InputType.Keyboard)
				{
					this.unsaved["lacie_engine/input/default_profile_kb"] = (object)key;
					this.DefaultInputProfileKeyboard = key;
				}
				else if (Inputs.Profiles[key].Type == LacieEngine.Core.InputProfile.InputType.Controller)
				{
					this.unsaved["lacie_engine/input/default_profile_pad"] = (object)key;
					this.DefaultInputProfileController = key;
				}
			}
			if (!GDUtil.FileExists(this.SettingsFilename))
			{
				File file = new File();
				int num = (int)file.Open(this.SettingsFilename, File.ModeFlags.Write);
				file.Close();
			}
			ConfigFile configFile1 = new ConfigFile();
			int num1 = (int)configFile1.Load(this.SettingsFilename);
			foreach (string key1 in this.unsaved.Keys)
			{
				ProjectSettings.SetSetting(key1, this.unsaved[key1]);
				string section = key1.Substring(0, key1.IndexOf("/"));
				string str = key1;
				int startIndex = key1.IndexOf("/") + 1;
				string key2 = str.Substring(startIndex, str.Length - startIndex);
				configFile1.SetValue(section, key2, this.unsaved[key1]);
			}
			int num2 = (int)configFile1.Save(this.SettingsFilename);
			this.unsaved.Clear();
			ConfigFile configFile2 = new ConfigFile();
			int num3 = (int)configFile2.Load(this.SettingsFilename);
			foreach (SettingParent settingParent in this._settings.Values)
				settingParent.WriteValue(configFile2);
			int num4 = (int)configFile2.Save(this.SettingsFilename);
			if (!this.InputProfileCustom)
				return;
			InputMapper.SaveCustomProfile();
		}

		public void ApplyAll()
		{
			Log.Info((object)"Applying settings...");
			OS.WindowFullscreen = this.FullScreen;
			if (!this.FullScreen)
				OS.WindowSize = new Vector2((float)this.ResolutionX, (float)this.ResolutionY);
			Game.Audio.UpdateVolume();
			Game.Screen.UpdateShaders();
			OS.LowProcessorUsageMode = this.BatterySaver;
			InputMapper.SetProfile(this.InputProfile);
			Input.MouseMode = !this.HideCursor ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Hidden;
			foreach (SettingParent settingParent in this._settings.Values)
				settingParent.Apply();
		}

		public void RevertSettings()
		{
			this.LoadSettings();
			this.ApplyAll();
		}

		public void SetFullScreen(bool fullScreen)
		{
			this.FullScreen = fullScreen;
			this.unsaved["display/window/size/fullscreen"] = (object)this.FullScreen;
			OS.WindowFullscreen = this.FullScreen;
		}

		public void SetResolution(Vector2 resolution)
		{
			this.ResolutionX = (int)resolution.x;
			this.ResolutionY = (int)resolution.y;
			this.unsaved["display/window/size/test_width"] = (object)this.ResolutionX;
			this.unsaved["display/window/size/test_height"] = (object)this.ResolutionY;
			if (this.FullScreen)
				return;
			OS.WindowSize = resolution;
		}

		public void SetVolumeMaster(Decimal volume)
		{
			if (this.VolumeMaster == volume)
				return;
			this.VolumeMaster = volume;
			this.unsaved["lacie_engine/audio/volume_master"] = (object)Decimal.ToSingle(this.VolumeMaster);
			Game.Audio.UpdateVolume();
		}

		public void SetVolumeBgm(Decimal volume)
		{
			if (this.VolumeBgm == volume)
				return;
			this.VolumeBgm = volume;
			this.unsaved["lacie_engine/audio/volume_bgm"] = (object)Decimal.ToSingle(this.VolumeBgm);
			Game.Audio.UpdateVolume();
		}

		public void SetVolumeSfx(Decimal volume)
		{
			if (this.VolumeSfx == volume)
				return;
			this.VolumeSfx = volume;
			this.unsaved["lacie_engine/audio/volume_sfx"] = (object)Decimal.ToSingle(this.VolumeSfx);
			Game.Audio.UpdateVolume();
		}

		public void SetVolumeSystem(Decimal volume)
		{
			if (this.VolumeSystem == volume)
				return;
			this.VolumeSystem = volume;
			this.unsaved["lacie_engine/audio/volume_system"] = (object)Decimal.ToSingle(this.VolumeSystem);
			Game.Audio.UpdateVolume();
		}

		public void SetVolumeText(Decimal volume)
		{
			if (this.VolumeText == volume)
				return;
			this.VolumeText = volume;
			this.unsaved["lacie_engine/audio/volume_text"] = (object)Decimal.ToSingle(this.VolumeText);
			Game.Audio.UpdateVolume();
		}

		public void SetMuteAudio(bool mute)
		{
			if (this.MuteAudio == mute)
				return;
			this.MuteAudio = mute;
			this.unsaved["lacie_engine/audio/mute"] = (object)this.MuteAudio;
			Game.Audio.UpdateVolume();
		}

		public void SetBrightness(Decimal value)
		{
			if (this.Brightness == value)
				return;
			this.Brightness = value;
			this.unsaved["lacie_engine/video/brightness"] = (object)Decimal.ToSingle(this.Brightness);
			Game.Screen.UpdateShaders();
		}

		public void SetContrast(Decimal value)
		{
			if (this.Contrast == value)
				return;
			this.Contrast = value;
			this.unsaved["lacie_engine/video/contrast"] = (object)Decimal.ToSingle(this.Contrast);
			Game.Screen.UpdateShaders();
		}

		public void SetSaturation(Decimal value)
		{
			if (this.Saturation == value)
				return;
			this.Saturation = value;
			this.unsaved["lacie_engine/video/saturation"] = (object)Decimal.ToSingle(this.Saturation);
			Game.Screen.UpdateShaders();
		}

		public void SetGamma(Decimal value)
		{
			if (this.Gamma == value)
				return;
			this.Gamma = value;
			this.unsaved["lacie_engine/video/gamma"] = (object)Decimal.ToSingle(this.Gamma);
			Game.Screen.UpdateShaders();
		}

		public void SetRenderer(string value)
		{
			if (this.Renderer == value)
				return;
			this.Renderer = value;
			this.unsaved["rendering/quality/driver/driver_name"] = (object)this.Renderer;
		}

		public void SetBatterySaver(bool value)
		{
			if (this.BatterySaver == value)
				return;
			this.BatterySaver = value;
			this.unsaved["application/run/low_processor_mode"] = (object)this.BatterySaver;
		}

		public void SetCursorHidden(bool value)
		{
			if (this.HideCursor == value)
				return;
			this.HideCursor = value;
			this.unsaved["lacie_engine/input/hide_cursor"] = (object)this.HideCursor;
			if (this.HideCursor)
				Input.MouseMode = Input.MouseModeEnum.Hidden;
			else
				Input.MouseMode = Input.MouseModeEnum.Visible;
		}

		public void SetObjectiveNotifications(bool value)
		{
			if (this.ObjectiveNotifications == value)
				return;
			this.ObjectiveNotifications = value;
			this.unsaved["lacie_engine/game/objective_notifications"] = (object)this.ObjectiveNotifications;
		}

		public void SetInputAutoSwitch(bool autoSwitch)
		{
			if (this.InputAutoSwitch == autoSwitch)
				return;
			this.InputAutoSwitch = autoSwitch;
			this.unsaved["lacie_engine/input/auto_switch"] = (object)this.InputAutoSwitch;
		}

		public void SetInputProfile(string value)
		{
			if (this.InputProfile == value)
				return;
			this.InputProfile = value;
			this.unsaved["lacie_engine/input/profile"] = (object)this.InputProfile;
			this.SetInputProfileCustom(value == "custom");
		}

		public void SetInputProfileCustom(bool value)
		{
			if (this.InputProfileCustom == value)
				return;
			this.InputProfileCustom = value;
			this.unsaved["lacie_engine/input/profile_custom"] = (object)this.InputProfileCustom;
		}

		public void SetSkipEnabled(bool value)
		{
			if (this.SkipEnabled == value)
				return;
			this.SkipEnabled = value;
			this.unsaved["lacie_engine/game/skip_enabled"] = (object)this.SkipEnabled;
		}

		public void SetTranslationLocale(string value)
		{
			if (this.TranslationSelected == value)
				return;
			this.TranslationSelected = value;
			this.unsaved["lacie_engine/core/translation_selected"] = (object)this.TranslationSelected;
		}

		internal void SetTranslationRemaps(System.Collections.Generic.Dictionary<string, string[]> value)
		{
			this.TranslationRemaps = (IDictionary<string, string[]>)value;
			this.unsaved["locale/translation_remaps"] = (object)this.TranslationRemaps;
		}

		private void LoadCoreSettings()
		{
			if (this.loadedCoreSettings)
				return;
			this.ProductName = GDUtil.ReadSetting<string>("lacie_engine/core/game_name");
			this.ProductVersion = GDUtil.ReadSetting<string>("lacie_engine/core/game_version");
			if (this.ProductVersion == "0.0.0")
				this.ProductVersion = DateTime.Now.ToString("yyyyMMdd");
			this.EngineVersion = GDUtil.ReadSetting<string>("lacie_engine/core/engine_version");
			this.ProductCopyright = GDUtil.ReadSetting<string>("lacie_engine/core/game_copyright");
			this.SettingsFilename = GDUtil.ReadSetting<string>("application/config/project_settings_override");
			this.TranslationBaseLocale = GDUtil.ReadSetting<string>("lacie_engine/core/translation_base_locale");
			this.TranslationBaseName = GDUtil.ReadSetting<string>("lacie_engine/core/translation_base_name");
			this.TranslationRemaps = (IDictionary<string, string[]>)this.GetTranslationRemapsSetting();
			this.WebsiteEnabled = GDUtil.ReadSetting<bool>("lacie_engine/core/website_enabled");
			this.WebsiteLink = GDUtil.ReadSetting<string>("lacie_engine/core/website_link");
			this.WebsiteCaption = GDUtil.ReadSetting<string>("lacie_engine/core/website_caption");
			this.DefaultInputProfile = GDUtil.ReadSetting<string>("lacie_engine/input/default_profile");
			this.DefaultInputProfileKeyboard = GDUtil.ReadSetting<string>("lacie_engine/input/default_profile_kb");
			this.DefaultInputProfileController = GDUtil.ReadSetting<string>("lacie_engine/input/default_profile_pad");
			this.SaveKey = GDUtil.ReadSetting<string>("lacie_engine/core/save_key");
			this.LsbKey = GDUtil.ReadSetting<string>("lacie_engine/core/lsb_key");
			this.NewGameEvent = GDUtil.ReadSetting<string>("lacie_engine/core/new_game_event");
			this.SceneProvider = GD.Load<SceneProvider>(GDUtil.ReadSetting<string>("lacie_engine/core/scene_provider"));
			this.UiProvider = GD.Load<UiLookAndFeelProvider>(GDUtil.ReadSetting<string>("lacie_engine/core/ui_provider"));
			this.DefaultCharacter = GDUtil.ReadSetting<string>("lacie_engine/core/default_character");
			this.SteamBuild = GDUtil.ReadSetting<bool>("lacie_engine/platform/steam");
			this.SteamAppId = GDUtil.ReadSetting<int>("lacie_engine/platform/steam_app_id");
			this.BaseResolution = GDUtil.ReadSetting<Vector2>("lacie_engine/video/base_resolution");
			this.DebugRoom = GDUtil.ReadSetting<string>("lacie_engine/debug/debug_room");
			this.DebugQuickstartOn = GDUtil.ReadSetting<bool>("lacie_engine/debug/quickstart/enabled");
			this.DebugQuickstartRoom = GDUtil.ReadSetting<string>("lacie_engine/debug/quickstart/room");
			this.DebugQuickstartPoint = GDUtil.ReadSetting<string>("lacie_engine/debug/quickstart/point");
			this.loadedCoreSettings = true;
		}

		private System.Collections.Generic.Dictionary<string, string[]> GetTranslationRemapsSetting()
		{
			System.Collections.Generic.Dictionary<string, string[]> translationRemapsSetting = new System.Collections.Generic.Dictionary<string, string[]>();
			try
			{
				Dictionary setting = (Dictionary)ProjectSettings.GetSetting("locale/translation_remaps");
				foreach (string key in (IEnumerable)setting.Keys)
					translationRemapsSetting[key] = (string[])setting[(object)key];
			}
			catch (Exception ex)
			{
				Log.Error((object)"Failed to load Translation Remaps from settings: ", (object)ex.Message);
			}
			return translationRemapsSetting;
		}
	}
}
