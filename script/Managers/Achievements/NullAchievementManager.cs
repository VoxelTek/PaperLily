using LacieEngine.API;

namespace LacieEngine.Achievements
{
	[Injectable]
	public class NullAchievementManager : IAchievementManager, IModule
	{
		public bool IsValid(string achievementId)
		{
			return true;
		}

		public void Unlock(string achievementId)
		{
		}
	}
}
