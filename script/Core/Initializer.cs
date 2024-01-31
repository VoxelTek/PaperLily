using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using LacieEngine.API;
using LacieEngine.Settings;
using LacieEngine.UI;

namespace LacieEngine.Core
{
	// Token: 0x02000184 RID: 388
	public class Initializer : Node
	{
		// Token: 0x06000DE8 RID: 3560 RVA: 0x00035794 File Offset: 0x00033994
		public override void _Ready()
		{
			Log.Info(new object[] { "Initializing Lacie Engine..." });
			System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
			base.CallDeferred("Init", Array.Empty<object>());
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x000357E8 File Offset: 0x000339E8
		public void Init()
		{
			Injector.Init();
			Log.Init();
			Log.Info(new object[] { "Dependency injector initialized!" });
			TranslationServer.SetLocale(ProjectSettings.GetSetting("lacie_engine/core/translation_base_locale") as string);
			string packPath = OS.GetExecutablePath().GetBaseDir() + "/pack/";
			List<string> list = GDUtil.ListFilesInPath(packPath, null, ".pck", false, false);
			list.Sort();
			foreach (string filename in list)
			{
				Log.Info(new object[] { "Loading PCK file: ", filename });
				ProjectSettings.LoadResourcePack(packPath + filename, true, 0);
			}
			foreach (string filename2 in GDUtil.ListFilesInPath("res://definitions/config/", ".cfg"))
			{
				Log.Debug(new object[] { "Processing external settings: ", filename2 });
				ConfigFile configFile = new ConfigFile();
				configFile.Load(filename2);
				foreach (string section in configFile.GetSections())
				{
					foreach (string key in configFile.GetSectionKeys(section))
					{
						ProjectSettings.SetSetting(section + "/" + key, configFile.GetValue(section, key, null));
					}
				}
			}
			VisualServer.SetDefaultClearColor(Colors.Black);
			SettingsManager settings = new SettingsManager();
			settings.LoadSettings(false);
			if (OS.IsDebugBuild())
			{
				OS.SetWindowTitle(settings.ProductName + " v" + settings.ProductVersion);
			}
			else
			{
				OS.SetWindowTitle(settings.ProductName);
			}
			new Game(settings, base.GetTree());
			Log.Info(new object[] { "Initializing main screen..." });
			Injector.Get<IScreenManager>().Init();
			this.PerformLoading();
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00035A00 File Offset: 0x00033C00
		private async void PerformLoading()
		{
			await GDUtil.DelayOneFrame();
			await Game.Screen.ShowLoadingScreenInstantly();
			Task task = Task.Run(delegate {
				this.LoadingProc();
			});
			while (!task.IsCompleted)
			{
				await GDUtil.DelayOneFrame();
			}
			await Game.Screen.HideLoadingScreen();
			Log.Info(new object[]
			{
				Game.Settings.ProductName,
				" ",
				Game.Settings.ProductVersion,
				", Game start!"
			});
			if (Game.Language.GetAvailableLanguages().Count > 1 && Game.Settings.TranslationSelected.IsNullOrEmpty())
			{
				Initializer.ShowLanguageSelection();
			}
			else
			{
				Initializer.ShowFirstScreen();
			}
			this.Delete();
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00035A38 File Offset: 0x00033C38
		private void LoadingProc()
		{
			try
			{
				Injector.Get<IPlatformInitializer>().Init();
				Game.Memory.Init();
				this.SystemPreload();
				Inputs.Init();
				Game.Core.InitPersistentState();
				Log.Info(new object[] { "Initializing modules..." });
				Injector.GetAll<IModule>().ForEach(delegate (IModule module) {
					module.Init();
				});
				Log.Info(new object[] { "Modules initialized!" });
				Log.Info(new object[] { "Initializing language..." });
				Game.Language.LoadLanguage(Game.Settings.TranslationSelected);
				Game.Settings.ApplyAll();
			}
			catch (Exception ex)
			{
				Log.Exception(ex, new object[] { "An error occurred while initializing the game." });
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00035B18 File Offset: 0x00033D18
		private void SystemPreload()
		{
			Log.Info(new object[] { "Loading fonts..." });
			foreach (string filename in GDUtil.ListFilesInPath("res://resources/font/", ".tres"))
			{
				Game.Memory.SystemCache(filename);
			}
			Log.Info(new object[] { "Loading materials..." });
			foreach (string filename2 in GDUtil.ListFilesInPath("res://resources/material/", ".tres"))
			{
				Game.Memory.SystemCache(filename2);
			}
			Log.Info(new object[] { "Loading graphics..." });
			foreach (string filename3 in GDUtil.ListFilesInPath("res://assets/img/ui/", ".png"))
			{
				Game.Memory.SystemCache(filename3);
			}
			foreach (string filename4 in GDUtil.ListFilesInPath("res://assets/img/ui/input/", ".png"))
			{
				Game.Memory.SystemCache(filename4);
			}
			Log.Info(new object[] { "Loading sounds..." });
			foreach (string filename5 in GDUtil.ListFilesInPath("res://assets/sfx/", "ui_", ".ogg", true, false))
			{
				Game.Memory.SystemCache(filename5);
			}
			Log.Info(new object[] { "Loading animations..." });
			foreach (string filename6 in GDUtil.ListFilesInPath("res://resources/animation/", ".tres"))
			{
				Game.Memory.SystemCache(filename6);
			}
			Log.Info(new object[] { "Loading system nodes..." });
			Game.Memory.SystemCache("res://resources/nodes/common/Player.tscn");
			Game.Memory.SystemCache("res://resources/nodes/common/PlayerSidescroller.tscn");
			Game.Memory.SystemCache("res://resources/nodes/common/StoryPlayer.tscn");
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00035DB0 File Offset: 0x00033FB0
		private static void ShowLanguageSelection()
		{
			Game.Settings.SetTranslationLocale(Game.Settings.TranslationBaseLocale);
			TitleLanguageMenuContainer languageSelector = GDUtil.MakeNode<TitleLanguageMenuContainer>("LanguageSelector");
			languageSelector.OnClose = delegate {
				Initializer.ShowFirstScreen();
			};
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, languageSelector);
			languageSelector.Menu.ResetSelection();
			Game.InputProcessor = Inputs.Processor.Menu;
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00035E20 File Offset: 0x00034020
		private static void ShowFirstScreen()
		{
			if (OS.IsDebugBuild() && Game.Settings.DebugQuickstartOn)
			{
				Game.Core.StartGameFromRoom(Game.Settings.DebugQuickstartRoom.IsNullOrEmpty() ? Game.Settings.DebugRoom : Game.Settings.DebugQuickstartRoom, Game.Settings.DebugQuickstartPoint.IsNullOrEmpty() ? null : Game.Settings.DebugQuickstartPoint, Vector2.Zero, null);
				return;
			}
			Game.Core.SwitchToScreen(Game.Settings.SceneProvider.FirstScreen);
		}
	}
}
