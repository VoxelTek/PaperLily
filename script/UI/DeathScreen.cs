using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class DeathScreen : Control
	{
		[GetNode("MenuContainer")]
		private Control nMenuContainer;

		public override void _Ready()
		{
			ShowMenu();
		}

		public async void ShowMenu()
		{
			await DrkieUtil.DelaySeconds(1.0);
			SimpleChoicesMenuContainer retryMenu = new SimpleChoicesMenuContainer(new List<IMenuEntry>
			{
				new SimpleMenuEntry(Game.Language.GetCaption("system.screen.death.retry"), Retry, null),
				new SimpleMenuEntry(Game.Language.GetCaption("system.screen.death.giveup"), GoToTitleScreen, null)
			});
			nMenuContainer.AddChild(retryMenu);
			Game.InputProcessor = Inputs.Processor.Menu;
		}

		public void Retry()
		{
			if (GameState.SaveExists("retrysave"))
			{
				Game.InputProcessor = Inputs.Processor.None;
				Game.Core.StartGameFromSave("retrysave");
			}
			else
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
			}
		}

		public void GoToTitleScreen()
		{
			Game.Core.SwitchToScreen(Game.Settings.SceneProvider.TitleScreen);
		}
	}
}
