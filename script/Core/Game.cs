// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.Game
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Rooms;
using LacieEngine.Settings;
using System.Diagnostics;

#nullable disable
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

    public static IGameCamera Camera => Game.Room.Camera;

    public static Inputs.Processor InputProcessor { get; set; }

    public static PersistentState PersistentState => Game.Core.PersistentState;

    public static IPlayer Player => Game.Room.Player;

    public static GameRoom CurrentRoom => Game.Room.CurrentRoom;

    public static Stopwatch PlaytimeStopwatch => Game.Core.PlaytimeStopwatch;

    public static Viewport Root { get; private set; }

    public static SettingsManager Settings { get; private set; }

    public static GameState State => Game.Core.State;

    public static LacieEngine.StoryPlayer.StoryPlayer StoryPlayer => Game.Events.StoryPlayer;

    public static SceneTree Tree { get; private set; }

    public Game(SettingsManager settings, SceneTree tree)
    {
      Game.Settings = settings;
      Game.Tree = tree;
      Game.Root = tree.Root;
      Game.Screen = Injector.Get<IScreenManager>();
      Game.Memory = Injector.Get<IMemoryManager>();
      Game.Events = Injector.Get<IEventManager>();
      Game.Items = Injector.Get<IItemManager>();
      Game.Variables = Injector.Get<IVariableManager>();
      Game.Minigames = Injector.Get<IMinigameManager>();
      Game.Objectives = Injector.Get<IObjectiveManager>();
      Game.Characters = Injector.Get<ICharacterManager>();
      Game.Achievements = Injector.Get<IAchievementManager>();
      Game.Room = Injector.Get<IGameRoomManager>();
      Game.Animations = Injector.Get<IAnimationManager>();
      Game.Language = Injector.Get<ILanguageManager>();
      Game.Audio = Injector.Get<IAudioManager>();
      Game.Input = Injector.Get<IInputManager>();
      Game.Core = Injector.Get<ICoreFunctions>();
    }
  }
}
