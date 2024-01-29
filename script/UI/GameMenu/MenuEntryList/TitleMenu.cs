using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class TitleMenu : SimpleVerticalMenu
	{
		private TitleSettingsMenuContainer nSettingsContainer;

		private TitleLoadMenuContainer nLoadContainer;

		public TitleMenu(IMenuEntryContainer container)
		{
			base.Container = container;
		}

		public void Continue()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			base.Container.Control.Visible = false;
			nLoadContainer = GDUtil.MakeNode<TitleLoadMenuContainer>("LoadMenuContainer");
			nLoadContainer.OnClose = delegate
			{
				CloseLoad();
			};
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, nLoadContainer);
			nLoadContainer.Menu.ResetSelection();
		}

		public void NewGame()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			Game.Core.StartGameFromEvent(Game.Settings.NewGameEvent);
		}

		public void DebugRoom()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			Game.Core.StartGameFromRoom(Game.Settings.DebugRoom, null, Vector2.Zero, "down");
		}

		public void Settings()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			base.Container.Control.Visible = false;
			nSettingsContainer = GDUtil.MakeNode<TitleSettingsMenuContainer>("SettingsContainer");
			nSettingsContainer.OnClose = delegate
			{
				CloseSettings();
			};
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, nSettingsContainer);
		}

		public void CloseSettings()
		{
			nSettingsContainer.Delete();
			nSettingsContainer = null;
			base.Container.Control.Visible = true;
			Root();
			if (base.Container is TitleScreen title)
			{
				title.UpdateExtraInfo();
			}
		}

		public void CloseLoad()
		{
			nLoadContainer.Delete();
			nLoadContainer = null;
			base.Container.Control.Visible = true;
			Root();
		}

		public void HomePage()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			OS.ShellOpen(Game.Settings.WebsiteLink);
		}

		public void TranslatorPage()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			OS.ShellOpen(Game.Language.GetTranslatorWebsite());
		}

		public void Quit()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
			Game.InputProcessor = Inputs.Processor.None;
			Game.Tree.Quit();
		}

		public override Control DrawContent()
		{
			base.Entries = new List<IMenuEntry>();
			if (GameState.AnySaveExists())
			{
				base.Entries.Add(new SimpleMenuEntry("system.menu.continue", delegate
				{
					Continue();
				}, this));
			}
			base.Entries.Add(new SimpleMenuEntry("system.menu.newgame", delegate
			{
				NewGame();
			}, this));
			if (OS.IsDebugBuild())
			{
				base.Entries.Add(new SimpleMenuEntry("system.menu.debugroom", delegate
				{
					DebugRoom();
				}, this));
			}
			base.Entries.Add(new SimpleMenuEntry("system.menu.settings", delegate
			{
				Settings();
			}, this));
			if (Game.Settings.WebsiteEnabled)
			{
				base.Entries.Add(new SimpleMenuEntry(Game.Settings.WebsiteCaption, delegate
				{
					HomePage();
				}, this));
			}
			if (!Game.Language.GetTranslatorWebsite().IsNullOrEmpty())
			{
				base.Entries.Add(new SimpleMenuEntry("system.menu.website.translator", delegate
				{
					TranslatorPage();
				}, this));
			}
			base.Entries.Add(new SimpleMenuEntry("system.menu.quit", delegate
			{
				Quit();
			}, this));
			return UIUtil.CreateVerticalEntryList(base.Entries, out _selectBgs, 2);
		}
	}
}
