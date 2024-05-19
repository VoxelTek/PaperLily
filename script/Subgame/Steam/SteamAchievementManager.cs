// Decompiled with JetBrains decompiler
// Type: LacieEngine.Achievements.SteamAchievementManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;
using Steamworks;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Achievements
{
  [Injectable(priority = 1, condition = "steam")]
  public class SteamAchievementManager : IAchievementManager, IModule
  {
	private List<string> _achievements;

	public void Init()
	{
	  this._achievements = new List<string>();
	  foreach (string filename in GDUtil.ListFilesInPath("res://definitions/achievements/", (string) null, ".json", true, false))
	  {
		foreach (AchievementDTO.Achievement achievement in GDUtil.ReadJsonFile<AchievementDTO>(filename).Achievements)
		  this._achievements.Add(achievement.Id);
	  }
	}

	public bool IsValid(string achievementId) => this._achievements.Contains(achievementId);

	public void Unlock(string achievementId)
	{
	  if (!this.IsValid(achievementId))
	  {
		Log.Warn((object) "Achievement is not defined: ", (object) achievementId);
	  }
	  else
	  {
		Log.Info((object) "Unlocking achievement: ", (object) achievementId);
		if (!SteamUserStats.SetAchievement(achievementId))
		  Log.Error((object) "Failed to unlock achievement: ", (object) achievementId);
		SteamUserStats.StoreStats();
	  }
	}
  }
}
