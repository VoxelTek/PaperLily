using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class GameMenu : SimpleVerticalMenu
	{
		private Control _submenuContainer;

		private Control _openContainer;

		public GameMenu(Control submenuContainer, IMenuEntryContainer container)
		{
			base.Container = container;
			base.Entries = new List<IMenuEntry>();
			base.Entries.Add(new SimpleMenuEntry("system.menu.inventory", delegate
			{
				Inventory();
			}, this));
			base.Entries.Add(new SimpleMenuEntry("system.menu.objectives", delegate
			{
				Objectives();
			}, this));
			base.Entries.Add(new SimpleMenuEntry("system.menu.settings", delegate
			{
				Settings();
			}, this));
			base.Entries.Add(new SimpleMenuEntry("system.menu.load", delegate
			{
				Load();
			}, this));
			base.Entries.Add(new SimpleMenuEntry("system.menu.quit", delegate
			{
				Quit();
			}, this));
			_submenuContainer = submenuContainer;
		}

		public void Settings()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			IngameSettingsMenuContainer container = new IngameSettingsMenuContainer();
			container.OnClose = delegate
			{
				Close();
			};
			_submenuContainer.AddChild(container);
			_openContainer = container;
		}

		public void Load()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			LoadMenuContainer container = new LoadMenuContainer();
			container.OnClose = delegate
			{
				Close();
			};
			_submenuContainer.AddChild(container);
			_openContainer = container;
		}

		public void Inventory()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			InventoryMenuContainer container = new InventoryMenuContainer();
			container.OnClose = delegate
			{
				Close();
			};
			_submenuContainer.AddChild(container);
			_openContainer = container;
		}

		public void Objectives()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			ObjectivesMenuContainer container = new ObjectivesMenuContainer();
			container.OnClose = delegate
			{
				Close();
			};
			_submenuContainer.AddChild(container);
			_openContainer = container;
		}

		public void Quit()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			QuitMenuContainer container = new QuitMenuContainer();
			container.OnClose = delegate
			{
				Close();
			};
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, container);
			_openContainer = container;
		}

		public void Close()
		{
			_openContainer.Delete();
			_openContainer = null;
		}
	}
}
