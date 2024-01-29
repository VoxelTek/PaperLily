using System;
using System.Collections.Generic;
using Godot;

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
			Date = DateTime.Now;
			Playtime = "0";
			Party = new List<string>();
			Items = new List<StateInventoryEntry>();
			Interactions = new Dictionary<string, int>();
			Variables = new Dictionary<string, string>();
			CurrentObjectives = new List<string>();
			CompletedObjectives = new List<string>();
			FailedObjectives = new List<string>();
			SeenTutorials = new List<string>();
			OverrideCharacterNames = new Dictionary<string, string>();
			OverrideItemNames = new Dictionary<string, string>();
			OverrideItemDescriptions = new Dictionary<string, string>();
			Overrides = new Dictionary<string, string>();
		}

		public static void Save(string saveName, bool quiet = false)
		{
			Log.Info("Saving game in ", saveName);
			if (!quiet)
			{
				Game.Screen.ShowSaving();
			}
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
			string path = "user://save/";
			GDUtil.EnsureFolderExists(path);
			if (SaveExists(saveName))
			{
				new Directory().Copy(path + saveName + ".sav", path + saveName + ".bak");
			}
			GDUtil.SaveJsonFile(Game.State, path + saveName + ".sav", Game.Settings.SaveKey);
			Game.PlaytimeStopwatch.Restart();
			Log.Info("Game Saved!");
		}

		public static void Load(string saveName)
		{
			try
			{
				Log.Info("Loading game from ", saveName);
				if (!SaveExists(saveName))
				{
					Log.Error("Save file doesn't exist: ", saveName);
					return;
				}
				GameState gs = GDUtil.ReadJsonFile<GameState>("user://save/" + saveName + ".sav", Game.Settings.SaveKey);
				Game.State.Date = gs.Date;
				Game.State.Playtime = gs.Playtime;
				Game.State.LocationStr = gs.LocationStr;
				Game.State.LocationImg = gs.LocationImg;
				Game.State.Party = gs.Party;
				Game.State.Event = gs.Event;
				Game.State.EventLabel = gs.EventLabel;
				Game.State.Room = gs.Room;
				Game.State.Position = gs.Position;
				Game.State.Direction = gs.Direction;
				Game.State.Items = gs.Items;
				Game.State.Interactions = gs.Interactions;
				Game.State.Variables = gs.Variables;
				Game.State.CurrentObjectives = gs.CurrentObjectives;
				Game.State.CompletedObjectives = gs.CompletedObjectives;
				Game.State.FailedObjectives = gs.FailedObjectives;
				Game.State.SeenTutorials = gs.SeenTutorials;
				Game.State.OverrideCharacterNames = gs.OverrideCharacterNames;
				Game.State.OverrideItemNames = gs.OverrideItemNames;
				Game.State.OverrideItemDescriptions = gs.OverrideItemDescriptions;
				Game.State.OverrideBgm = gs.OverrideBgm;
				Game.State.Overrides = gs.Overrides;
				Log.Info("Game Loaded!");
			}
			catch (Exception e)
			{
				Log.Error("Failed to load save ", saveName, " : ", e.Message);
				OS.Alert("Failed to load save data (" + saveName + ".sav) - It may be corrupted! The game will now probably crash.");
			}
		}

		public static bool SaveExists(string saveName)
		{
			return GDUtil.FileExists("user://save/" + saveName + ".sav");
		}

		public static bool AnySaveExists()
		{
			return GDUtil.ListFilesInPath("user://save/", ".sav").Count > 0;
		}
	}
}
