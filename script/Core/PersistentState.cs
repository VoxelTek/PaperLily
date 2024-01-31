// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.PersistentState
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Core
{
  public class PersistentState
  {
    public Dictionary<string, string> Variables { get; set; }

    public PersistentState() => this.Variables = new Dictionary<string, string>();

    public static void Save()
    {
      Log.Info((object) "Saving persistent state...");
      string saveName = "persistent";
      string folder = "user://save/";
      GDUtil.EnsureFolderExists(folder);
      if (GameState.SaveExists(saveName))
      {
        int num = (int) new Directory().Copy(folder + saveName + ".sav", folder + saveName + ".bak");
      }
      GDUtil.SaveJsonFile((object) Game.PersistentState, folder + saveName + ".sav", Game.Settings.SaveKey);
      Log.Info((object) "Persistent state saved!");
    }

    public static PersistentState Load()
    {
      try
      {
        Log.Info((object) "Loading persistent state...");
        string saveName = "persistent";
        if (!GameState.SaveExists(saveName))
          return new PersistentState();
        PersistentState persistentState = GDUtil.ReadJsonFile<PersistentState>("user://save/" + saveName + ".sav", Game.Settings.SaveKey);
        Log.Info((object) "Persistent state loaded!");
        return persistentState;
      }
      catch (Exception ex)
      {
        Log.Error((object) "Failed to load persistent save : ", (object) ex.Message);
        OS.Alert("Failed to load master data (persistent.sav) - It may be corrupted! The game will continue but part of your save data could be lost.");
        return new PersistentState();
      }
    }
  }
}
