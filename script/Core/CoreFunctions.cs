// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.CoreFunctions
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.UI;
using System.Diagnostics;

#nullable disable
namespace LacieEngine.Core
{
  [Injectable]
  public class CoreFunctions : ICoreFunctions
  {
	public GameState State { get; private set; }

	public PersistentState PersistentState { get; private set; }

	public Stopwatch PlaytimeStopwatch { get; private set; }

	public void InitPersistentState() => this.PersistentState = PersistentState.Load();

	private void InitGame(string saveFile = null)
	{
	  this.State = new GameState();
	  if (!saveFile.IsNullOrEmpty())
		GameState.Load(saveFile);
	  Game.Events.InitStoryPlayer();
	  foreach (ITranslatable translatable in Injector.GetAll<ITranslatable>())
		translatable.ApplyTranslationOverrides();
	  foreach (IStateOverridable stateOverridable in Injector.GetAll<IStateOverridable>())
		stateOverridable.ApplyStateOverrides();
	  this.PlaytimeStopwatch = new Stopwatch();
	  this.PlaytimeStopwatch.Start();
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
		this.State = (GameState) null;
		this.PlaytimeStopwatch = (Stopwatch) null;
	  }
	  Game.Memory.Clean();
	}

	public void Quit() => Game.Tree.Notification(1006);

	public void PauseGame()
	{
	  if (Game.Tree.Paused)
		return;
	  Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) new PauseScreen());
	  Game.Tree.Paused = true;
	}

	public async void StartGameFromEvent(string eventId)
	{
	  Log.Info("Starting game from event");
	  Game.InputProcessor = Inputs.Processor.None;
	  Game.Audio.StopBgm();
	  Game.Audio.StopAmbiance();
	  await Game.Screen.ShowLoadingScreen();
	  this.ClearAllGameContent();
	  this.InitGame();
	  Game.Events.PlayEvent(eventId);
	}

	public async void StartGameFromRoom(string room, string point, Vector2 pos, string dir)
	{
	  Log.Info("Starting game from room");
	  Game.InputProcessor = Inputs.Processor.None;
	  Game.Audio.StopBgm();
	  Game.Audio.StopAmbiance();
	  await Game.Screen.ShowLoadingScreen();
	  this.ClearAllGameContent();
	  this.InitGame();
	  Game.Room.ChangeRoom(room, point, pos, dir);
	}

	public async void StartGameFromSave(string saveFile)
	{
	  Log.Info("Starting game from save");
	  Game.InputProcessor = Inputs.Processor.None;
	  Game.Audio.StopBgm();
	  Game.Audio.StopAmbiance();
	  await Game.Screen.ShowLoadingScreen();
	  this.ClearAllGameContent();
	  this.InitGame(saveFile);
	  Game.State.LocationStr = (string) null;
	  Game.State.LocationImg = (string) null;
	  if (!Game.State.Event.IsNullOrEmpty())
	  {
		string evt = Game.State.Event;
		Game.State.Event = (string) null;
		Game.State.EventLabel = (string) null;
		Game.Events.PlayEvent(evt);
	  }
	  else
		Game.Room.ChangeRoom(Game.State.Room, (string) null, Game.State.Position, Game.State.Direction);
	}

	public void SwitchToScreen(PackedScene scene) => this.SwitchToScreen(scene, false);

	public async void SwitchToScreen(PackedScene scene, bool keepState)
	{
	  Log.Info("Switching to screen");
	  Game.InputProcessor = Inputs.Processor.None;
	  await Game.Screen.FadeToBlack();
	  this.ClearAllGameContent(keepState);
	  Node node = scene.Instance();
	  node.InjectNodes();
	  Game.Screen.AddToLayer(IScreenManager.Layer.Screen, node);
	  await Game.Screen.FadeFromBlack();
	  Game.InputProcessor = Inputs.Processor.Menu;
	}

	public void OpenScreen(PackedScene scene)
	{
	  Game.InputProcessor = Inputs.Processor.None;
	  Node node = scene.Instance();
	  node.InjectNodes();
	  Game.Screen.AddToLayer(IScreenManager.Layer.Screen, node);
	  Game.InputProcessor = Inputs.Processor.Menu;
	}

	public void AssignInputProcessor()
	{
	  if (Game.StoryPlayer.Running)
		Game.InputProcessor = Inputs.Processor.StoryPlayer;
	  else if (Game.Minigames.Running)
		Game.InputProcessor = Inputs.Processor.Minigame;
	  else if (Game.Player != null)
		Game.InputProcessor = Inputs.Processor.Player;
	  else
		Game.InputProcessor = Inputs.Processor.Menu;
	}
  }
}
