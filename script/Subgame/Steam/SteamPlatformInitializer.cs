using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Nodes;
using Steamworks;

namespace LacieEngine.Core
{
	[Injectable(priority = 1, condition = "steam")]
	public class SteamPlatformInitializer : IPlatformInitializer
	{
		public void Init()
		{
			Log.Info("Initializing Steam...");
			Log.Info("Steam App ID: ", Game.Settings.SteamAppId);
			if (SteamAPI.RestartAppIfNecessary((AppId_t)(uint)Game.Settings.SteamAppId))
			{
				Log.Info("Restarting game through steam!");
				Game.Tree.Quit();
			}
			try
			{
				SteamAPI.Init();
				Log.Info("Connected with Steam account: ", SteamFriends.GetPersonaName());
				Game.Root.AddChild(new SteamUpdater());
			}
			catch (Exception e)
			{
				Log.Error("Failed to initialize Steam: ", e.Message);
				OS.Alert("Failed to connect with Steam. The game will now exit.", "Error");
				Game.Tree.Quit();
			}
		}
	}
}
