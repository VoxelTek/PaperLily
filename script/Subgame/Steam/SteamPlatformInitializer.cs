// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.SteamPlatformInitializer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Nodes;
using Steamworks;
using System;

#nullable disable
namespace LacieEngine.Core
{
  [Injectable(priority = 1, condition = "steam")]
  public class SteamPlatformInitializer : IPlatformInitializer
  {
    public void Init()
    {
      Log.Info((object) "Initializing Steam...");
      Log.Info((object) "Steam App ID: ", (object) Game.Settings.SteamAppId);
      if (SteamAPI.RestartAppIfNecessary((AppId_t) (uint) Game.Settings.SteamAppId))
      {
        Log.Info((object) "Restarting game through steam!");
        Game.Tree.Quit();
      }
      try
      {
        SteamAPI.Init();
        Log.Info((object) "Connected with Steam account: ", (object) SteamFriends.GetPersonaName());
        Game.Root.AddChild((Node) new SteamUpdater());
      }
      catch (Exception ex)
      {
        Log.Error((object) "Failed to initialize Steam: ", (object) ex.Message);
        OS.Alert("Failed to connect with Steam. The game will now exit.", "Error");
        Game.Tree.Quit();
      }
    }
  }
}
