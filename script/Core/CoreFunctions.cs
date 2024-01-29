using System.Diagnostics;
using Godot;
using LacieEngine.API;
using LacieEngine.UI;

namespace LacieEngine.Core
{
	[Injectable]
	public class CoreFunctions : ICoreFunctions
	{
		public GameState State { get; private set; }

		public PersistentState PersistentState { get; private set; }

		public Stopwatch PlaytimeStopwatch { get; private set; }

		public void InitPersistentState()
		{
			PersistentState = PersistentState.Load();
		}

		private void InitGame(string saveFile = null)
		{
			State = new GameState();
			if (!saveFile.IsNullOrEmpty())
			{
				GameState.Load(saveFile);
			}
			Game.Events.InitStoryPlayer();
			foreach (ITranslatable item in Injector.GetAll<ITranslatable>())
			{
				item.ApplyTranslationOverrides();
			}
			foreach (IStateOverridable item2 in Injector.GetAll<IStateOverridable>())
			{
				item2.ApplyStateOverrides();
			}
			PlaytimeStopwatch = new Stopwatch();
			PlaytimeStopwatch.Start();
		}

		private void ClearAllGameContent(bool keepState = false)
		{
			Game.Room.Clean();
			Game.Screen.Clean();
			Game.Events.Clean();
			Game.Characters.Clean();
			Game.Items.Clean();
			if (!keepState)
			{
				State = null;
				PlaytimeStopwatch = null;
			}
			Game.Memory.Clean();
		}

		public void Quit()
		{
			Game.Tree.Notification(1006);
		}

		public void PauseGame()
		{
			if (!Game.Tree.Paused)
			{
				Game.Screen.AddToLayer(IScreenManager.Layer.Screen, new PauseScreen());
				Game.Tree.Paused = true;
			}
		}

		public async void StartGameFromEvent(string eventId)
		{
			Game.InputProcessor = Inputs.Processor.None;
			Game.Audio.StopBgm();
			Game.Audio.StopAmbiance();
			await Game.Screen.ShowLoadingScreen();
			ClearAllGameContent();
			InitGame();
			Game.Events.PlayEvent(eventId);
		}

		public async void StartGameFromRoom(string room, string point, Vector2 pos, string dir)
		{
			Game.InputProcessor = Inputs.Processor.None;
			Game.Audio.StopBgm();
			Game.Audio.StopAmbiance();
			await Game.Screen.ShowLoadingScreen();
			ClearAllGameContent();
			InitGame();
			Game.Room.ChangeRoom(room, point, pos, dir);
		}

		public async void StartGameFromSave(string saveFile)
		{
			Game.InputProcessor = Inputs.Processor.None;
			Game.Audio.StopBgm();
			Game.Audio.StopAmbiance();
			await Game.Screen.ShowLoadingScreen();
			ClearAllGameContent();
			InitGame(saveFile);
			Game.State.LocationStr = null;
			Game.State.LocationImg = null;
			if (!Game.State.Event.IsNullOrEmpty())
			{
				string eventId = Game.State.Event;
				Game.State.Event = null;
				Game.State.EventLabel = null;
				Game.Events.PlayEvent(eventId);
			}
			else
			{
				Game.Room.ChangeRoom(Game.State.Room, null, Game.State.Position, Game.State.Direction);
			}
		}

		public void SwitchToScreen(PackedScene scene)
		{
			SwitchToScreen(scene, keepState: false);
		}

		public async void SwitchToScreen(PackedScene scene, bool keepState)
		{
			Game.InputProcessor = Inputs.Processor.None;
			await Game.Screen.FadeToBlack();
			ClearAllGameContent(keepState);
			Node screen = scene.Instance();
			screen.InjectNodes();
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, screen);
			await Game.Screen.FadeFromBlack();
			Game.InputProcessor = Inputs.Processor.Menu;
		}

		public void OpenScreen(PackedScene scene)
		{
			Game.InputProcessor = Inputs.Processor.None;
			Node screen = scene.Instance();
			screen.InjectNodes();
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, screen);
			Game.InputProcessor = Inputs.Processor.Menu;
		}

		public void AssignInputProcessor()
		{
			if (Game.StoryPlayer.Running)
			{
				Game.InputProcessor = Inputs.Processor.StoryPlayer;
			}
			else if (Game.Minigames.Running)
			{
				Game.InputProcessor = Inputs.Processor.Minigame;
			}
			else if (Game.Player != null)
			{
				Game.InputProcessor = Inputs.Processor.Player;
			}
			else
			{
				Game.InputProcessor = Inputs.Processor.Menu;
			}
		}
	}
}
