// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.Constants
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Core
{
  public class Constants
  {
    public static class Files
    {
      public const string QuickSave = "quicksave";
      public const string AutoSave = "autosave";
      public const string RetrySave = "retrysave";
      public const string SlotSave = "slot";
      public const string PersistentSave = "persistent";
      public const string CustomInputProfile = "bindings.json";
      public const string FaceImage = "face.png";
    }

    public static class Extensions
    {
      public const string Scene = ".tscn";
      public const string Resource = ".tres";
      public const string ResourceCompiled = ".converted.res";
      public const string AssetCompiled = ".import";
      public const string Json = ".json";
      public const string JsonComments = ".jsonc";
      public const string LacieStoryFile = ".lsf";
      public const string LacieStoryBinary = ".lsb";
      public const string Captions = ".csv";
      public const string Csv = ".csv";
      public const string Pot = ".pot";
      public const string Po = ".po";
      public const string Image = ".png";
      public const string Bgm = ".ogg";
      public const string Sfx = ".ogg";
      public const string AmbianceSfx = ".ogg";
      public const string Save = ".sav";
      public const string Backup = ".bak";
      public const string Pack = ".pck";
      public const string Config = ".cfg";
      public const string Script = ".cs";
    }

    public static class Paths
    {
      public const string Bgm = "res://assets/bgm/";
      public const string Busts = "res://assets/img/bust/";
      public const string CGs = "res://assets/img/cg/";
      public const string TutorialImages = "res://assets/img/tutorial/";
      public const string UIImages = "res://assets/img/ui/";
      public const string InputImages = "res://assets/img/ui/input/";
      public const string SaveImages = "res://assets/img/ui/save/";
      public const string CharacterFaceImages = "res://assets/img/ui/chr_face/";
      public const string Sfx = "res://assets/sfx/";
      public const string CommonSprites = "res://assets/sprite/common/";
      public const string CharacterSprites = "res://assets/sprite/common/character/";
      public const string ItemSprites = "res://assets/sprite/common/item/";
      public const string MinigameSprites = "res://assets/sprite/common/minigame/";
      public const string MinigameRoutedSprites = "res://assets/sprite/{0}/minigame/";
      public const string AmbianceSfx = "res://assets/sfx/";
      public const string Achievements = "res://definitions/achievements/";
      public const string Captions = "res://definitions/captions/";
      public const string Translations = "res://definitions/translations/";
      public const string Characters = "res://definitions/characters/";
      public const string Configs = "res://definitions/config/";
      public const string Events = "res://definitions/events/";
      public const string Locale = "res://definitions/locale/";
      public const string GlobalEvents = "res://definitions/events/Global/";
      public const string InputProfiles = "res://definitions/inputprofile/";
      public const string Items = "res://definitions/items/";
      public const string Objectives = "res://definitions/objectives/";
      public const string Variables = "res://definitions/variables/";
      public const string Animations = "res://resources/animation/";
      public const string Fonts = "res://resources/font/";
      public const string LightingPresets = "res://resources/lighting/";
      public const string Materials = "res://resources/material/";
      public const string Minigames = "res://resources/minigame/";
      public const string Nodes = "res://resources/nodes/common/";
      public const string Bubbles = "res://resources/nodes/common/bubble/";
      public const string CGNodes = "res://resources/nodes/cg/";
      public const string MinigameRoutedNodes = "res://resources/nodes/{0}/minigame/";
      public const string RoomNodes = "res://resources/nodes/rooms/";
      public const string Screens = "res://resources/screen/";
      public const string Tutorials = "res://resources/tutorials/";
      public const string Script = "res://script/";
      public const string ExternalPack = "/pack/";
      public const string Screenshots = "user://screenshot/";
      public const string Saves = "user://save/";
      public const string Root = "res://";
      public const string UserInputProfiles = "user://";
    }

    public static class Scenes
    {
      public const string Player = "res://resources/nodes/common/Player.tscn";
      public const string PlayerSidescroller = "res://resources/nodes/common/PlayerSidescroller.tscn";
      public const string StoryPlayer = "res://resources/nodes/common/StoryPlayer.tscn";
      public const string VoidRoom = "res://resources/nodes/rooms/Void.tscn";
    }

    public static class Resources
    {
      public const string WhiteTexture = "res://assets/sprite/common/white.png";
      public const string RoundLightTexture = "res://assets/sprite/common/light01.png";
      public const string RoundLightTexture2 = "res://assets/sprite/common/light04.png";
      public const string PixelPerfectMaterial = "res://resources/material/pixel_perfect.tres";
      public const string PixelLayerMaterial = "res://resources/material/pixel_layer.tres";
      public const string MirrorReflectionMaterial = "res://resources/material/mirror_reflection.tres";
      public const string LightOnlyMaterial = "res://resources/material/light_only.tres";
      public const string UnshadedMaterial = "res://resources/material/unshaded.tres";
      public const string ImageCorrectionMaterial = "res://resources/material/image_correction.tres";
      public const string AnimationShake = "res://resources/material/animation_shake.tres";
      public const string AnimationShakeUnshaded = "res://resources/material/animation_shake_unshaded.tres";
      public const string BlurMaterial = "res://resources/material/screen_blur.tres";
      public const string BasicLighting = "res://resources/lighting/basic.tres";

      public static class Textures
      {
        public const string FrameMenu2 = "res://assets/img/ui/frame_menu_2.png";
        public const string FrameMenu3 = "res://assets/img/ui/frame_menu_3.png";
        public const string FrameMenu2Mask = "res://assets/img/ui/frame_menu_2_mask.png";
        public const string FrameMenu2NoGradient = "res://assets/img/ui/frame_menu_2b.png";
        public const string FrameMenu2NoGradientMask = "res://assets/img/ui/frame_menu_2bc_mask.png";
        public const string FrameInfobox = "res://assets/img/ui/frame_infobox.png";
        public const string FrameInfoboxMask = "res://assets/img/ui/frame_menu_2_bg_mask.png";
        public const string FrameLegacy = "res://assets/img/ui/frame_legacy.png";
        public const string ContinueIndicator = "res://assets/img/ui/continue_indicator.png";
        public const string ItemIconBg = "res://assets/img/ui/item_icon_bg.png";
        public const string ItemIconBgSelected = "res://assets/img/ui/item_icon_bg_selected.png";
        public const string ItemIconBgEmpty = "res://assets/img/ui/item_icon_bg_empty.png";
        public const string ItemIconBgEmptySelected = "res://assets/img/ui/item_icon_bg_empty_selected.png";
        public const string DividerSmall = "res://assets/img/ui/divider_sm.png";
        public const string DividerMedium = "res://assets/img/ui/divider_md.png";
        public const string DividerLarge = "res://assets/img/ui/divider_lg.png";
        public const string BulletPoint = "res://assets/img/ui/bullet_point.png";
        public const string FrameMenuBgDecor = "res://assets/img/ui/menu_bg_decor.png";
        public const string LoadingIndicator = "res://assets/img/ui/loading.png";
        public const string SavingIndicator = "res://assets/img/ui/saving.png";
        public const string SaveSlot = "res://assets/img/ui/save_slot.png";
        public const string SaveSlotSelected = "res://assets/img/ui/save_slot_selected.png";
        public const string SaveSlotEmpty = "res://assets/img/ui/save_slot_empty.png";
        public const string SaveSlotEmptySelected = "res://assets/img/ui/save_slot_empty_selected.png";
        public const string SaveDecor = "res://assets/img/ui/save_decor.png";
        public const string SaveDefaultLocationImg = "res://assets/img/ui/save/unknown.png";
        public const string CharacterFaceDefaultImage = "res://assets/img/ui/chr_face/unknown.png";
        public const string Cursor = "res://assets/img/ui/cursor.png";
        public const string Arrow = "res://assets/img/ui/arrow_up.png";
      }

      public static class Sfx
      {
        public const string Bad = "res://assets/sfx/ui_bad.ogg";
        public const string ItemGet = "res://assets/sfx/ui_item.ogg";
        public const string Navigation = "res://assets/sfx/ui_navigation.ogg";
        public const string Back = "res://assets/sfx/ui_navigation2.ogg";
        public const string Objective = "res://assets/sfx/ui_objective.ogg";
        public const string Select = "res://assets/sfx/ui_start.ogg";
        public const string TextScroll = "res://assets/sfx/ui_text.ogg";
      }

      public static class MinigameTextures
      {
        public const string TimingBarBgLeft = "res://assets/sprite/common/minigame/timingbar/bg_left.png";
        public const string TimingBarBgProgress = "res://assets/sprite/common/minigame/timingbar/bg_progress.png";
        public const string TimingBarBgStretch = "res://assets/sprite/common/minigame/timingbar/bg_stretch.png";
        public const string TimingBarBgRight = "res://assets/sprite/common/minigame/timingbar/bg_right.png";
        public const string TimingBarPlayer = "res://assets/sprite/common/minigame/timingbar/player.png";
        public const string TimingBarAttempt = "res://assets/sprite/common/minigame/timingbar/attempt.png";
        public const string TimingBarAttemptUsed = "res://assets/sprite/common/minigame/timingbar/attempt_used.png";
      }

      public static class MinigameSfx
      {
        public const string Fanfare = "res://assets/sfx/minigame_fanfare.ogg";
        public const string BlockSlide = "res://assets/sfx/minigame_block_slide.ogg";
      }

      public static class Fonts
      {
        public const string Default = "res://resources/font/default.tres";
        public const string DefaultLarge = "res://resources/font/default_large.tres";
        public const string DefaultSmall = "res://resources/font/default_small.tres";
        public const string Title = "res://resources/font/default_title.tres";
        public const string InventoryStackSize = "res://resources/font/inventory_stack_size.tres";
        public const string ImageValign = "res://resources/font/image_valign.tres";
      }
    }

    public static class Colors
    {
      public static readonly Color DefaultTextColor = new Color(0.93f, 0.87f, 0.87f);
      public static readonly Color Passthrough = new Color(1f, 0.0f, 1f, 0.42f);
      public static readonly Color SpecialEvent = new Color(0.0f, 1f, 0.0f, 0.42f);
    }

    public static class Settings
    {
      public const string GameName = "lacie_engine/core/game_name";
      public const string EngineVersion = "lacie_engine/core/engine_version";
      public const string GameVersion = "lacie_engine/core/game_version";
      public const string GameCopyright = "lacie_engine/core/game_copyright";
      public const string TranslationSelected = "lacie_engine/core/translation_selected";
      public const string TranslationExtraEnabled = "lacie_engine/core/translation_extra_enabled";
      public const string TranslationBaseLocale = "lacie_engine/core/translation_base_locale";
      public const string TranslationBaseName = "lacie_engine/core/translation_base_name";
      public const string TranslationRemaps = "locale/translation_remaps";
      public const string WebsiteEnabled = "lacie_engine/core/website_enabled";
      public const string WebsiteLink = "lacie_engine/core/website_link";
      public const string WebsiteCaption = "lacie_engine/core/website_caption";
      public const string SaveKey = "lacie_engine/core/save_key";
      public const string LsbKey = "lacie_engine/core/lsb_key";
      public const string NewGameEvent = "lacie_engine/core/new_game_event";
      public const string SceneProvider = "lacie_engine/core/scene_provider";
      public const string UiProvider = "lacie_engine/core/ui_provider";
      public const string DefaultCharacter = "lacie_engine/core/default_character";
      public const string SteamBuild = "lacie_engine/platform/steam";
      public const string SteamAppId = "lacie_engine/platform/steam_app_id";
      public const string BatterySaver = "application/run/low_processor_mode";
      public const string SettingsFilename = "application/config/project_settings_override";
      public const string ResolutionX = "display/window/size/test_width";
      public const string ResolutionY = "display/window/size/test_height";
      public const string FullScreen = "display/window/size/fullscreen";
      public const string VolumeMaster = "lacie_engine/audio/volume_master";
      public const string VolumeBgm = "lacie_engine/audio/volume_bgm";
      public const string VolumeSfx = "lacie_engine/audio/volume_sfx";
      public const string VolumeSystem = "lacie_engine/audio/volume_system";
      public const string VolumeText = "lacie_engine/audio/volume_text";
      public const string MuteAudio = "lacie_engine/audio/mute";
      public const string Brightness = "lacie_engine/video/brightness";
      public const string Contrast = "lacie_engine/video/contrast";
      public const string Saturation = "lacie_engine/video/saturation";
      public const string Gamma = "lacie_engine/video/gamma";
      public const string BaseResolution = "lacie_engine/video/base_resolution";
      public const string ResolutionScalePixel = "lacie_engine/video/resolution_scale_pixel";
      public const string Renderer = "rendering/quality/driver/driver_name";
      public const string FpsLimit = "lacie_engine/video/fps_limit";
      public const string HideCursor = "lacie_engine/input/hide_cursor";
      public const string ObjectiveNotifications = "lacie_engine/game/objective_notifications";
      public const string InputProfile = "lacie_engine/input/profile";
      public const string InputProfileCustom = "lacie_engine/input/profile_custom";
      public const string DefaultInputProfile = "lacie_engine/input/default_profile";
      public const string DefaultInputProfileKeyboard = "lacie_engine/input/default_profile_kb";
      public const string DefaultInputProfileController = "lacie_engine/input/default_profile_pad";
      public const string InputAutoSwitch = "lacie_engine/input/auto_switch";
      public const string SkipEnabled = "lacie_engine/game/skip_enabled";
      public const string ShowSkipOnDeath = "lacie_engine/game/show_skip_option";
      public const string JoystickDeadzone = "lacie_engine/input/joystick_deadzone";
      public const string DebugRoom = "lacie_engine/debug/debug_room";
      public const string DebugQuickstartOn = "lacie_engine/debug/quickstart/enabled";
      public const string DebugQuickstartRoom = "lacie_engine/debug/quickstart/room";
      public const string DebugQuickstartPoint = "lacie_engine/debug/quickstart/point";
    }

    public static class Captions
    {
      public static class Common
      {
        public const string Ok = "system.common.ok";
        public const string Yes = "system.common.yes";
        public const string No = "system.common.no";
        public const string On = "system.common.on";
        public const string Off = "system.common.off";
        public const string Auto = "system.common.auto";
        public const string Next = "system.common.next";
        public const string Back = "system.common.back";
        public const string Apply = "system.common.apply";
        public const string Cancel = "system.common.cancel";
        public const string Done = "system.common.done";
        public const string Loading = "system.common.loading";
        public const string Key = "system.common.key";
        public const string Button = "system.common.button";
        public const string Axis = "system.common.axis";
        public const string Keyboard = "system.common.keyboard";
        public const string Controller = "system.common.controller";
      }

      public static class Settings
      {
        public const string BatterySaver = "system.settings.batterysaver";
        public const string Brightness = "system.settings.brightness";
        public const string Contrast = "system.settings.contrast";
        public const string FpsLimit = "system.settings.fpslimit";
        public const string FpsUnlocked = "system.settings.fpsunlocked";
        public const string Vsync = "system.settings.vsync";
        public const string VsyncComp = "system.settings.compvsync";
        public const string Gsync = "system.settings.gsync";
        public const string FullScreen = "system.settings.fullscreen";
        public const string ResolutionScalePixel = "system.settings.resolutionscale";
        public const string Gamma = "system.settings.gamma";
        public const string Renderer = "system.settings.renderer";
        public const string Gles2 = "system.settings.renderer.gles2";
        public const string Gles3 = "system.settings.renderer.gles3";
        public const string Resolution = "system.settings.resolution";
        public const string Saturation = "system.settings.saturation";
        public const string VolumeMaster = "system.settings.volume.master";
        public const string VolumeBgm = "system.settings.volume.bgm";
        public const string VolumeSfx = "system.settings.volume.sfx";
        public const string VolumeSystem = "system.settings.volume.system";
        public const string VolumeText = "system.settings.volume.text";
        public const string MuteAudio = "system.settings.volume.mute";
        public const string HideCursor = "system.settings.hidecursor";
        public const string ObjectiveNotifications = "system.settings.objectivenotifications";
        public const string Audio = "system.settings.audio";
        public const string Video = "system.settings.video";
        public const string Input = "system.settings.input";
        public const string InputError = "system.settings.input.error";
        public const string Game = "system.settings.game";
        public const string InputProfile = "system.settings.input.profile";
        public const string InputProfileCaption = "system.settings.input.profile.caption";
        public const string InputProfileCustom = "system.settings.input.profile.custom";
        public const string InputPressKey = "system.settings.input.presskey";
        public const string InputPressButton = "system.settings.input.pressbutton";
        public const string InputAutoSwitch = "system.settings.input.autoswitch";
        public const string Deadzone = "system.settings.input.deadzone";
        public const string Advanced = "system.settings.advanced";
        public const string RevertAll = "system.settings.revertall";
        public const string NotRecommended = "system.settings.notrecommended";
        public const string RestartRequired = "system.settings.restartrequired";
        public const string Current = "system.settings.current";
        public const string SkipEnabled = "system.settings.game.skipenabled";
        public const string ShowSkipOnDeath = "system.settings.game.showskipoption";
        public const string SelectLanguage = "system.settings.game.language.select";
        public const string ExtraLanguages = "system.settings.game.language.extra";
        public const string SelectExtraLanguage = "system.settings.game.language.select.extra";
        public const string ExtraLanguagesWarning = "system.settings.game.language.extra.warning";
        public const string Language = "system.settings.game.language";
      }

      public static class Menu
      {
        public const string NewGame = "system.menu.newgame";
        public const string Continue = "system.menu.continue";
        public const string Website = "system.menu.website";
        public const string WebsiteItchio = "system.menu.website.itchio";
        public const string WebsiteTranslator = "system.menu.website.translator";
        public const string Save = "system.menu.save";
        public const string SaveGame = "system.menu.save.game";
        public const string SaveSlot = "system.menu.save.slot";
        public const string SaveSlotEmpty = "system.menu.save.slot.empty";
        public const string SaveCorrupted = "system.menu.save.corrupted";
        public const string SaveObsolete = "system.menu.save.obsolete";
        public const string SaveLocationUnknown = "system.menu.save.location.unknown";
        public const string SaveOverwriteAreYouSure = "system.menu.save.overwrite.areyousure";
        public const string Load = "system.menu.load";
        public const string LoadGame = "system.menu.load.game";
        public const string Quit = "system.menu.quit";
        public const string DebugRoom = "system.menu.debugroom";
        public const string Settings = "system.menu.settings";
        public const string Inventory = "system.menu.inventory";
        public const string Status = "system.menu.status";
        public const string Objectives = "system.menu.objectives";
        public const string ObjectivesEmpty = "system.menu.objectives.empty";
        public const string MainMenu = "system.menu.mainmenu";
        public const string Info = "system.menu.info";
        public const string QuitToDebugRoom = "system.menu.quit.debugroom";
        public const string QuitToTitle = "system.menu.quit.mainmenu";
        public const string QuitToDesktop = "system.menu.quit.desktop";
        public const string QuitAreYouSure = "system.menu.quit.areyousure";
        public const string Paused = "system.menu.paused";
        public const string StartupSettingsInfo = "system.menu.startup.settings.info";
        public const string Currency = "system.menu.currency";
        public const string Retry = "system.screen.death.retry";
        public const string GiveUp = "system.screen.death.giveup";
      }

      public static class Objectives
      {
        public const string Updated = "system.objectives.updated";
        public const string Done = "system.objectives.done";
      }

      public static class Minigames
      {
        public const string BoxPuzzleInfo = "system.minigames.boxpuzzle.info";
        public const string PointAndClickInfo = "system.minigames.pointandclick.info";
      }

      public static class StoryPlayer
      {
        public const string ItemGet = "system.storyplayer.item.get";
      }

      public static class Tutorials
      {
        public const string Next = "system.tutorials.next";
        public const string Okay = "system.tutorials.okay";
      }

      public static class Actions
      {
        public const string Up = "system.actions.up";
        public const string Down = "system.actions.down";
        public const string Left = "system.actions.left";
        public const string Right = "system.actions.right";
        public const string Action = "system.actions.action";
        public const string Cancel = "system.actions.cancel";
        public const string Run = "system.actions.run";
        public const string Special = "system.actions.special";
        public const string Menu = "system.actions.menu";
      }
    }

    public static class Other
    {
      public const string GlobalEvents = "Global";
      public const string EventCaptionsPrefix = "events_";
      public const string InputProfileCustom = "custom";
      public const string InputTypeKeyboard = "keyboard";
      public const string InputTypeController = "controller";
      public const string AudioNone = "res://assets/bgm/silent.ogg";
      public const string PersistentVarPrefix = "persistent.";
    }

    public static class PhysicsLayers
    {
      public const uint None = 0;
      public const uint Interactables = 1;
      public const uint Walls = 2;
      public const uint Player = 4;
      public const uint Passthrough = 8;
      public const uint Pushables = 16;
    }

    public static class RenderLayers
    {
      public const int None = 0;
      public const int Environment = 1;
      public const int Characters = 2;
      public const int Mirrors = 4;
      public const int GuiTransparency = 524288;
    }
  }
}
