using System.Diagnostics;
using Godot;
using LacieEngine.API;
using LacieEngine.Rooms;
using LacieEngine.Settings;
using LacieEngine.StoryPlayer;

namespace LacieEngine.Core
{
	public class Game
	{
		public static IAchievementManager Achievements { get; private set; }

		public static IAnimationManager Animations { get; private set; }

		public static IAudioManager Audio { get; private set; }

		public static ICharacterManager Characters { get; private set; }

		public static ICoreFunctions Core { get; private set; }

		public static IEventManager Events { get; private set; }

		public static IInputManager Input { get; private set; }

		public static IItemManager Items { get; private set; }

		public static ILanguageManager Language { get; private set; }

		public static IMemoryManager Memory { get; private set; }

		public static IMinigameManager Minigames { get; private set; }

		public static IObjectiveManager Objectives { get; private set; }

		public static IGameRoomManager Room { get; private set; }

		public static IScreenManager Screen { get; private set; }

		public static IVariableManager Variables { get; private set; }

		public static IGameCamera Camera => Room.Camera;

		public static Inputs.Processor InputProcessor { get; set; }

		public static PersistentState PersistentState => Core.PersistentState;

		public static IPlayer Player => Room.Player;

		public static GameRoom CurrentRoom => Room.CurrentRoom;

		public static Stopwatch PlaytimeStopwatch => Core.PlaytimeStopwatch;

		public static Viewport Root { get; private set; }

		public static SettingsManager Settings { get; private set; }

		public static GameState State => Core.State;

		public static LacieEngine.StoryPlayer.StoryPlayer StoryPlayer => Events.StoryPlayer;

		public static SceneTree Tree { get; private set; }

		public Game(SettingsManager settings, SceneTree tree)
		{
			Settings = settings;
			Tree = tree;
			Root = tree.Root;
			Screen = Injector.Get<IScreenManager>();
			Memory = Injector.Get<IMemoryManager>();
			Events = Injector.Get<IEventManager>();
			Items = Injector.Get<IItemManager>();
			Variables = Injector.Get<IVariableManager>();
			Minigames = Injector.Get<IMinigameManager>();
			Objectives = Injector.Get<IObjectiveManager>();
			Characters = Injector.Get<ICharacterManager>();
			Achievements = Injector.Get<IAchievementManager>();
			Room = Injector.Get<IGameRoomManager>();
			Animations = Injector.Get<IAnimationManager>();
			Language = Injector.Get<ILanguageManager>();
			Audio = Injector.Get<IAudioManager>();
			Input = Injector.Get<IInputManager>();
			Core = Injector.Get<ICoreFunctions>();
		}
	}
}
