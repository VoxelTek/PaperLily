// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.GameState
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Core
{
  public class GameState
  {
    public string Version { get; set; }

    public DateTime Date { get; set; }

    public string Playtime { get; set; }

    public string LocationStr { get; set; }

    public string LocationImg { get; set; }

    public List<string> Party { get; set; }

    public string Event { get; set; }

    public string EventLabel { get; set; }

    public string Room { get; set; }

    public Vector2 Position { get; set; }

    public string Direction { get; set; }

    public List<StateInventoryEntry> Items { get; set; }

    public Dictionary<string, int> Interactions { get; set; }

    public Dictionary<string, string> Variables { get; set; }

    public List<string> CurrentObjectives { get; private set; }

    public List<string> CompletedObjectives { get; private set; }

    public List<string> FailedObjectives { get; private set; }

    public List<string> SeenTutorials { get; private set; }

    public Dictionary<string, string> OverrideCharacterNames { get; set; }

    public Dictionary<string, string> OverrideItemNames { get; set; }

    public Dictionary<string, string> OverrideItemDescriptions { get; set; }

    public string OverrideBgm { get; set; }

    public bool MenuDisabled { get; set; }

    public Dictionary<string, string> Overrides { get; set; }

    public GameState()
    {
      this.Date = DateTime.Now;
      this.Playtime = "0";
      this.Party = new List<string>();
      this.Items = new List<StateInventoryEntry>();
      this.Interactions = new Dictionary<string, int>();
      this.Variables = new Dictionary<string, string>();
      this.CurrentObjectives = new List<string>();
      this.CompletedObjectives = new List<string>();
      this.FailedObjectives = new List<string>();
      this.SeenTutorials = new List<string>();
      this.OverrideCharacterNames = new Dictionary<string, string>();
      this.OverrideItemNames = new Dictionary<string, string>();
      this.OverrideItemDescriptions = new Dictionary<string, string>();
      this.Overrides = new Dictionary<string, string>();
    }

    public static void Save(string saveName, bool quiet = false)
    {
      Log.Info((object) "Saving game in ", (object) saveName);
      if (!quiet)
        Game.Screen.ShowSaving();
      Game.State.Date = DateTime.Now;
      Game.State.Version = Game.Settings.EngineVersion;
      Game.State.Playtime = (long.Parse(Game.State.Playtime) + Game.PlaytimeStopwatch.ElapsedMilliseconds).ToString();
      if (Game.Player != null)
      {
        Game.State.Position = Game.Player.Node.Position;
        Game.State.Direction = Game.Player.GetDirection().ToString();
      }
      else
      {
        Game.State.Position = Vector2.Zero;
        Game.State.Direction = "down";
      }
      string folder = "user://save/";
      GDUtil.EnsureFolderExists(folder);
      if (GameState.SaveExists(saveName))
      {
        int num = (int) new Directory().Copy(folder + saveName + ".sav", folder + saveName + ".bak");
      }
      GDUtil.SaveJsonFile((object) Game.State, folder + saveName + ".sav", Game.Settings.SaveKey);
      Game.PlaytimeStopwatch.Restart();
      Log.Info((object) "Game Saved!");
    }

    public static void Load(string saveName)
    {
      try
      {
        Log.Info((object) "Loading game from ", (object) saveName);
        if (!GameState.SaveExists(saveName))
        {
          Log.Error((object) "Save file doesn't exist: ", (object) saveName);
        }
        else
        {
          GameState gameState = GDUtil.ReadJsonFile<GameState>("user://save/" + saveName + ".sav", Game.Settings.SaveKey);
          Game.State.Date = gameState.Date;
          Game.State.Playtime = gameState.Playtime;
          Game.State.LocationStr = gameState.LocationStr;
          Game.State.LocationImg = gameState.LocationImg;
          Game.State.Party = gameState.Party;
          Game.State.Event = gameState.Event;
          Game.State.EventLabel = gameState.EventLabel;
          Game.State.Room = gameState.Room;
          Game.State.Position = gameState.Position;
          Game.State.Direction = gameState.Direction;
          Game.State.Items = gameState.Items;
          Game.State.Interactions = gameState.Interactions;
          Game.State.Variables = gameState.Variables;
          Game.State.CurrentObjectives = gameState.CurrentObjectives;
          Game.State.CompletedObjectives = gameState.CompletedObjectives;
          Game.State.FailedObjectives = gameState.FailedObjectives;
          Game.State.SeenTutorials = gameState.SeenTutorials;
          Game.State.OverrideCharacterNames = gameState.OverrideCharacterNames;
          Game.State.OverrideItemNames = gameState.OverrideItemNames;
          Game.State.OverrideItemDescriptions = gameState.OverrideItemDescriptions;
          Game.State.OverrideBgm = gameState.OverrideBgm;
          Game.State.Overrides = gameState.Overrides;
          Log.Info((object) "Game Loaded!");
        }
      }
      catch (Exception ex)
      {
        Log.Error((object) "Failed to load save ", (object) saveName, (object) " : ", (object) ex.Message);
        OS.Alert("Failed to load save data (" + saveName + ".sav) - It may be corrupted! The game will now probably crash.");
      }
    }

    public static bool SaveExists(string saveName)
    {
      return GDUtil.FileExists("user://save/" + saveName + ".sav");
    }

    public static bool AnySaveExists() => GDUtil.ListFilesInPath("user://save/", ".sav").Count > 0;
  }
}
