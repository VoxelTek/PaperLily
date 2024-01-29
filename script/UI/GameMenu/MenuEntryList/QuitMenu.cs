using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class QuitMenu : SimpleVerticalMenu
	{
		public QuitMenu(IMenuEntryContainer container)
		{
			base.Container = container;
			base.Entries = new List<IMenuEntry>();
			if (OS.IsDebugBuild())
			{
				base.Entries.Add(new SimpleMenuEntry("system.menu.quit.debugroom", delegate
				{
					GoToDebugRoom();
				}, this));
			}
			base.Entries.Add(new SimpleMenuEntry("system.menu.quit.mainmenu", delegate
			{
				QuitToTitle();
			}, this));
			base.Entries.Add(new SimpleMenuEntry("system.menu.quit.desktop", delegate
			{
				QuitToDesktop();
			}, this));
		}

		private void GoToDebugRoom()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			AreYouSureContainer areYouSure = new AreYouSureContainer();
			areYouSure.OnYes = delegate
			{
				Game.Core.StartGameFromRoom(Game.Settings.DebugRoom, null, Vector2.Zero, "down");
			};
			areYouSure.OnClose = delegate
			{
				areYouSure.Delete();
			};
			areYouSure.Text = "system.menu.quit.areyousure";
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, areYouSure);
		}

		private void QuitToTitle()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			AreYouSureContainer areYouSure = new AreYouSureContainer();
			areYouSure.OnYes = delegate
			{
				Game.Core.SwitchToScreen(Game.Settings.SceneProvider.TitleScreen);
			};
			areYouSure.OnClose = delegate
			{
				areYouSure.Delete();
			};
			areYouSure.Text = "system.menu.quit.areyousure";
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, areYouSure);
		}

		private void QuitToDesktop()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
			AreYouSureContainer areYouSure = new AreYouSureContainer();
			areYouSure.OnYes = delegate
			{
				Game.Tree.Quit();
			};
			areYouSure.OnClose = delegate
			{
				areYouSure.Delete();
			};
			areYouSure.Text = "system.menu.quit.areyousure";
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, areYouSure);
		}
	}
}
