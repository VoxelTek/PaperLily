using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using LacieEngine.Core;

namespace LacieEngine.Settings
{
	public class SettingsManager
	{
		private System.Collections.Generic.Dictionary<Type, SettingParent> _settings;

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

		public decimal VolumeMaster { get; private set; }

		public decimal VolumeBgm { get; private set; }

		public decimal VolumeSfx { get; private set; }

		public decimal VolumeSystem { get; private set; }

		public decimal VolumeText { get; private set; }

		public bool MuteAudio { get; private set; }

		public decimal Brightness { get; private set; }

		public decimal Contrast { get; private set; }

		public decimal Saturation { get; private set; }

		public decimal Gamma { get; private set; }

		public string Renderer { get; private set; }

		public int FpsLimit => GetSettingValue<int, FpsLimitSetting>();

		public int ResolutionScalePixel => GetSettingValue<int, ResolutionScaleSetting>();

		public bool BatterySaver { get; private set; }

		public bool HideCursor { get; private set; }

		public bool ObjectiveNotifications { get; private set; }

		public string InputProfile { get; private set; }

		public bool InputProfileCustom { get; private set; }

		public bool SkipEnabled { get; private set; }

		public bool InputAutoSwitch { get; private set; }

		public float JoystickDeadzone => GetSettingValue<float, DeadzoneSetting>();

		public string TranslationSelected { get; private set; }

		public bool TranslationExtraEnabled => GetSettingValue<bool, TranslationExtraEnabledSetting>();

		public void LoadSettings(bool fullReload = false)
		{
			_settings = new System.Collections.Generic.Dictionary<Type, SettingParent>();
			_settings[typeof(FpsLimitSetting)] = FpsLimitSetting.Instance;
			_settings[typeof(ResolutionScaleSetting)] = ResolutionScaleSetting.Instance;
			_settings[typeof(DeadzoneSetting)] = DeadzoneSetting.Instance;
			_settings[typeof(TranslationExtraEnabledSetting)] = TranslationExtraEnabledSetting.Instance;
			Log.Info("Loading settings...");
			if (fullReload)
			{
				loadedCoreSettings = false;
			}
			LoadCoreSettings();
			ResolutionX = GDUtil.ReadSetting<int>("display/window/size/test_width");
			ResolutionY = GDUtil.ReadSetting<int>("display/window/size/test_height");
			FullScreen = GDUtil.ReadSetting<bool>("display/window/size/fullscreen");
			VolumeMaster = (decimal)GDUtil.ReadSetting<float>("lacie_engine/audio/volume_master");
			VolumeBgm = (decimal)GDUtil.ReadSetting<float>("lacie_engine/audio/volume_bgm");
			VolumeSfx = (decimal)GDUtil.ReadSetting<float>("lacie_engine/audio/volume_sfx");
			VolumeSystem = (decimal)GDUtil.ReadSetting<float>("lacie_engine/audio/volume_system");
			VolumeText = (decimal)GDUtil.ReadSetting<float>("lacie_engine/audio/volume_text");
			MuteAudio = GDUtil.ReadSetting<bool>("lacie_engine/audio/mute");
			Brightness = (decimal)GDUtil.ReadSetting<float>("lacie_engine/video/brightness");
			Contrast = (decimal)GDUtil.ReadSetting<float>("lacie_engine/video/contrast");
			Saturation = (decimal)GDUtil.ReadSetting<float>("lacie_engine/video/saturation");
			Gamma = (decimal)GDUtil.ReadSetting<float>("lacie_engine/video/gamma");
			Renderer = GDUtil.ReadSetting<string>("rendering/quality/driver/driver_name");
			BatterySaver = GDUtil.ReadSetting<bool>("application/run/low_processor_mode");
			HideCursor = GDUtil.ReadSetting<bool>("lacie_engine/input/hide_cursor");
			ObjectiveNotifications = GDUtil.ReadSetting<bool>("lacie_engine/game/objective_notifications");
			InputProfile = GDUtil.ReadSetting<string>("lacie_engine/input/profile");
			InputProfileCustom = GDUtil.ReadSetting<bool>("lacie_engine/input/profile_custom");
			SkipEnabled = GDUtil.ReadSetting<bool>("lacie_engine/game/skip_enabled");
			InputAutoSwitch = GDUtil.ReadSetting<bool>("lacie_engine/input/auto_switch");
			TranslationSelected = GDUtil.ReadSetting<string>("lacie_engine/core/translation_selected");
			unsaved = new System.Collections.Generic.Dictionary<string, object>();
		}

		public T GetSettingValue<T, U>()
		{
			return ((Setting<T>)_settings[typeof(U)]).ReadValue();
		}

		public Setting<T> GetSetting<T, U>()
		{
			return (Setting<T>)_settings[typeof(U)];
		}

		public void SaveSettings()
		{
			if (unsaved.ContainsKey("lacie_engine/input/profile"))
			{
				string profile = (string)unsaved["lacie_engine/input/profile"];
				if (Inputs.Profiles[profile].Type == LacieEngine.Core.InputProfile.InputType.Keyboard)
				{
					unsaved["lacie_engine/input/default_profile_kb"] = profile;
					DefaultInputProfileKeyboard = profile;
				}
				else if (Inputs.Profiles[profile].Type == LacieEngine.Core.InputProfile.InputType.Controller)
				{
					unsaved["lacie_engine/input/default_profile_pad"] = profile;
					DefaultInputProfileController = profile;
				}
			}
			if (!GDUtil.FileExists(SettingsFilename))
			{
				File file = new File();
				file.Open(SettingsFilename, File.ModeFlags.Write);
				file.Close();
			}
			ConfigFile configFile = new ConfigFile();
			configFile.Load(SettingsFilename);
			foreach (string setting in unsaved.Keys)
			{
				ProjectSettings.SetSetting(setting, unsaved[setting]);
				string section = setting.Substring(0, setting.IndexOf("/"));
				string text = setting;
				int num = setting.IndexOf("/") + 1;
				string key = text.Substring(num, text.Length - num);
				configFile.SetValue(section, key, unsaved[setting]);
			}
			configFile.Save(SettingsFilename);
			unsaved.Clear();
			configFile = new ConfigFile();
			configFile.Load(SettingsFilename);
			foreach (SettingParent value in _settings.Values)
			{
				value.WriteValue(configFile);
			}
			configFile.Save(SettingsFilename);
			if (InputProfileCustom)
			{
				InputMapper.SaveCustomProfile();
			}
		}

		public void ApplyAll()
		{
			Log.Info("Applying settings...");
			OS.WindowFullscreen = FullScreen;
			if (!FullScreen)
			{
				OS.WindowSize = new Vector2(ResolutionX, ResolutionY);
			}
			Game.Audio.UpdateVolume();
			Game.Screen.UpdateShaders();
			OS.LowProcessorUsageMode = BatterySaver;
			InputMapper.SetProfile(InputProfile);
			if (HideCursor)
			{
				Input.MouseMode = Input.MouseModeEnum.Hidden;
			}
			else
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
			foreach (SettingParent value in _settings.Values)
			{
				value.Apply();
			}
		}

		public void RevertSettings()
		{
			LoadSettings();
			ApplyAll();
		}

		public void SetFullScreen(bool fullScreen)
		{
			FullScreen = fullScreen;
			unsaved["display/window/size/fullscreen"] = FullScreen;
			OS.WindowFullscreen = FullScreen;
		}

		public void SetResolution(Vector2 resolution)
		{
			ResolutionX = (int)resolution.x;
			ResolutionY = (int)resolution.y;
			unsaved["display/window/size/test_width"] = ResolutionX;
			unsaved["display/window/size/test_height"] = ResolutionY;
			if (!FullScreen)
			{
				OS.WindowSize = resolution;
			}
		}

		public void SetVolumeMaster(decimal volume)
		{
			if (!(VolumeMaster == volume))
			{
				VolumeMaster = volume;
				unsaved["lacie_engine/audio/volume_master"] = decimal.ToSingle(VolumeMaster);
				Game.Audio.UpdateVolume();
			}
		}

		public void SetVolumeBgm(decimal volume)
		{
			if (!(VolumeBgm == volume))
			{
				VolumeBgm = volume;
				unsaved["lacie_engine/audio/volume_bgm"] = decimal.ToSingle(VolumeBgm);
				Game.Audio.UpdateVolume();
			}
		}

		public void SetVolumeSfx(decimal volume)
		{
			if (!(VolumeSfx == volume))
			{
				VolumeSfx = volume;
				unsaved["lacie_engine/audio/volume_sfx"] = decimal.ToSingle(VolumeSfx);
				Game.Audio.UpdateVolume();
			}
		}

		public void SetVolumeSystem(decimal volume)
		{
			if (!(VolumeSystem == volume))
			{
				VolumeSystem = volume;
				unsaved["lacie_engine/audio/volume_system"] = decimal.ToSingle(VolumeSystem);
				Game.Audio.UpdateVolume();
			}
		}

		public void SetVolumeText(decimal volume)
		{
			if (!(VolumeText == volume))
			{
				VolumeText = volume;
				unsaved["lacie_engine/audio/volume_text"] = decimal.ToSingle(VolumeText);
				Game.Audio.UpdateVolume();
			}
		}

		public void SetMuteAudio(bool mute)
		{
			if (MuteAudio != mute)
			{
				MuteAudio = mute;
				unsaved["lacie_engine/audio/mute"] = MuteAudio;
				Game.Audio.UpdateVolume();
			}
		}

		public void SetBrightness(decimal value)
		{
			if (!(Brightness == value))
			{
				Brightness = value;
				unsaved["lacie_engine/video/brightness"] = decimal.ToSingle(Brightness);
				Game.Screen.UpdateShaders();
			}
		}

		public void SetContrast(decimal value)
		{
			if (!(Contrast == value))
			{
				Contrast = value;
				unsaved["lacie_engine/video/contrast"] = decimal.ToSingle(Contrast);
				Game.Screen.UpdateShaders();
			}
		}

		public void SetSaturation(decimal value)
		{
			if (!(Saturation == value))
			{
				Saturation = value;
				unsaved["lacie_engine/video/saturation"] = decimal.ToSingle(Saturation);
				Game.Screen.UpdateShaders();
			}
		}

		public void SetGamma(decimal value)
		{
			if (!(Gamma == value))
			{
				Gamma = value;
				unsaved["lacie_engine/video/gamma"] = decimal.ToSingle(Gamma);
				Game.Screen.UpdateShaders();
			}
		}

		public void SetRenderer(string value)
		{
			if (!(Renderer == value))
			{
				Renderer = value;
				unsaved["rendering/quality/driver/driver_name"] = Renderer;
			}
		}

		public void SetBatterySaver(bool value)
		{
			if (BatterySaver != value)
			{
				BatterySaver = value;
				unsaved["application/run/low_processor_mode"] = BatterySaver;
			}
		}

		public void SetCursorHidden(bool value)
		{
			if (HideCursor != value)
			{
				HideCursor = value;
				unsaved["lacie_engine/input/hide_cursor"] = HideCursor;
				if (HideCursor)
				{
					Input.MouseMode = Input.MouseModeEnum.Hidden;
				}
				else
				{
					Input.MouseMode = Input.MouseModeEnum.Visible;
				}
			}
		}

		public void SetObjectiveNotifications(bool value)
		{
			if (ObjectiveNotifications != value)
			{
				ObjectiveNotifications = value;
				unsaved["lacie_engine/game/objective_notifications"] = ObjectiveNotifications;
			}
		}

		public void SetInputAutoSwitch(bool autoSwitch)
		{
			if (InputAutoSwitch != autoSwitch)
			{
				InputAutoSwitch = autoSwitch;
				unsaved["lacie_engine/input/auto_switch"] = InputAutoSwitch;
			}
		}

		public void SetInputProfile(string value)
		{
			if (!(InputProfile == value))
			{
				InputProfile = value;
				unsaved["lacie_engine/input/profile"] = InputProfile;
				SetInputProfileCustom(value == "custom");
			}
		}

		public void SetInputProfileCustom(bool value)
		{
			if (InputProfileCustom != value)
			{
				InputProfileCustom = value;
				unsaved["lacie_engine/input/profile_custom"] = InputProfileCustom;
			}
		}

		public void SetSkipEnabled(bool value)
		{
			if (SkipEnabled != value)
			{
				SkipEnabled = value;
				unsaved["lacie_engine/game/skip_enabled"] = SkipEnabled;
			}
		}

		public void SetTranslationLocale(string value)
		{
			if (!(TranslationSelected == value))
			{
				TranslationSelected = value;
				unsaved["lacie_engine/core/translation_selected"] = TranslationSelected;
			}
		}

		internal void SetTranslationRemaps(System.Collections.Generic.Dictionary<string, string[]> value)
		{
			TranslationRemaps = value;
			unsaved["locale/translation_remaps"] = TranslationRemaps;
		}

		private void LoadCoreSettings()
		{
			if (!loadedCoreSettings)
			{
				ProductName = GDUtil.ReadSetting<string>("lacie_engine/core/game_name");
				ProductVersion = GDUtil.ReadSetting<string>("lacie_engine/core/game_version");
				if (ProductVersion == "0.0.0")
				{
					ProductVersion = DateTime.Now.ToString("yyyyMMdd");
				}
				EngineVersion = GDUtil.ReadSetting<string>("lacie_engine/core/engine_version");
				ProductCopyright = GDUtil.ReadSetting<string>("lacie_engine/core/game_copyright");
				SettingsFilename = GDUtil.ReadSetting<string>("application/config/project_settings_override");
				TranslationBaseLocale = GDUtil.ReadSetting<string>("lacie_engine/core/translation_base_locale");
				TranslationBaseName = GDUtil.ReadSetting<string>("lacie_engine/core/translation_base_name");
				TranslationRemaps = GetTranslationRemapsSetting();
				WebsiteEnabled = GDUtil.ReadSetting<bool>("lacie_engine/core/website_enabled");
				WebsiteLink = GDUtil.ReadSetting<string>("lacie_engine/core/website_link");
				WebsiteCaption = GDUtil.ReadSetting<string>("lacie_engine/core/website_caption");
				DefaultInputProfile = GDUtil.ReadSetting<string>("lacie_engine/input/default_profile");
				DefaultInputProfileKeyboard = GDUtil.ReadSetting<string>("lacie_engine/input/default_profile_kb");
				DefaultInputProfileController = GDUtil.ReadSetting<string>("lacie_engine/input/default_profile_pad");
				SaveKey = GDUtil.ReadSetting<string>("lacie_engine/core/save_key");
				LsbKey = GDUtil.ReadSetting<string>("lacie_engine/core/lsb_key");
				NewGameEvent = GDUtil.ReadSetting<string>("lacie_engine/core/new_game_event");
				string sceneProviderPath = GDUtil.ReadSetting<string>("lacie_engine/core/scene_provider");
				SceneProvider = GD.Load<SceneProvider>(sceneProviderPath);
				string uiProviderPath = GDUtil.ReadSetting<string>("lacie_engine/core/ui_provider");
				UiProvider = GD.Load<UiLookAndFeelProvider>(uiProviderPath);
				DefaultCharacter = GDUtil.ReadSetting<string>("lacie_engine/core/default_character");
				SteamBuild = GDUtil.ReadSetting<bool>("lacie_engine/platform/steam");
				SteamAppId = GDUtil.ReadSetting<int>("lacie_engine/platform/steam_app_id");
				BaseResolution = GDUtil.ReadSetting<Vector2>("lacie_engine/video/base_resolution");
				DebugRoom = GDUtil.ReadSetting<string>("lacie_engine/debug/debug_room");
				DebugQuickstartOn = GDUtil.ReadSetting<bool>("lacie_engine/debug/quickstart/enabled");
				DebugQuickstartRoom = GDUtil.ReadSetting<string>("lacie_engine/debug/quickstart/room");
				DebugQuickstartPoint = GDUtil.ReadSetting<string>("lacie_engine/debug/quickstart/point");
				loadedCoreSettings = true;
			}
		}

		private System.Collections.Generic.Dictionary<string, string[]> GetTranslationRemapsSetting()
		{
			System.Collections.Generic.Dictionary<string, string[]> csharpDict = new System.Collections.Generic.Dictionary<string, string[]>();
			try
			{
				Dictionary godotDict = (Dictionary)ProjectSettings.GetSetting("locale/translation_remaps");
				foreach (string i in godotDict.Keys)
				{
					csharpDict[i] = (string[])godotDict[i];
				}
				return csharpDict;
			}
			catch (Exception e)
			{
				Log.Error("Failed to load Translation Remaps from settings: ", e.Message);
				return csharpDict;
			}
		}
	}
}
