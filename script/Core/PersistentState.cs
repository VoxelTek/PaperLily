using System;
using System.Collections.Generic;
using Godot;

namespace LacieEngine.Core
{
	public class PersistentState
	{
		public Dictionary<string, string> Variables { get; set; }

		public PersistentState()
		{
			Variables = new Dictionary<string, string>();
		}

		public static void Save()
		{
			Log.Info("Saving persistent state...");
			string saveName = "persistent";
			string path = "user://save/";
			GDUtil.EnsureFolderExists(path);
			if (GameState.SaveExists(saveName))
			{
				new Directory().Copy(path + saveName + ".sav", path + saveName + ".bak");
			}
			GDUtil.SaveJsonFile(Game.PersistentState, path + saveName + ".sav", Game.Settings.SaveKey);
			Log.Info("Persistent state saved!");
		}

		public static PersistentState Load()
		{
			try
			{
				Log.Info("Loading persistent state...");
				string saveName = "persistent";
				if (!GameState.SaveExists(saveName))
				{
					return new PersistentState();
				}
				PersistentState ps = GDUtil.ReadJsonFile<PersistentState>("user://save/" + saveName + ".sav", Game.Settings.SaveKey);
				Log.Info("Persistent state loaded!");
				return ps;
			}
			catch (Exception e)
			{
				Log.Error("Failed to load persistent save : ", e.Message);
				OS.Alert("Failed to load master data (persistent.sav) - It may be corrupted! The game will continue but part of your save data could be lost.");
				return new PersistentState();
			}
		}
	}
}
