using System.Collections.Generic;
using LacieEngine.API;
using LacieEngine.Core;
using Steamworks;

namespace LacieEngine.Achievements
{
	[Injectable(priority = 1, condition = "steam")]
	public class SteamAchievementManager : IAchievementManager, IModule
	{
		private List<string> _achievements;

		public void Init()
		{
			_achievements = new List<string>();
			foreach (string item in GDUtil.ListFilesInPath("res://definitions/achievements/", null, ".json"))
			{
				foreach (AchievementDTO.Achievement achievement in GDUtil.ReadJsonFile<AchievementDTO>(item).Achievements)
				{
					_achievements.Add(achievement.Id);
				}
			}
		}

		public bool IsValid(string achievementId)
		{
			return _achievements.Contains(achievementId);
		}

		public void Unlock(string achievementId)
		{
			if (!IsValid(achievementId))
			{
				Log.Warn("Achievement is not defined: ", achievementId);
				return;
			}
			Log.Info("Unlocking achievement: ", achievementId);
			if (!SteamUserStats.SetAchievement(achievementId))
			{
				Log.Error("Failed to unlock achievement: ", achievementId);
			}
			SteamUserStats.StoreStats();
		}
	}
}
