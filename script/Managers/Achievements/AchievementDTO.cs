using System.Collections.Generic;

namespace LacieEngine.Achievements
{
	internal class AchievementDTO
	{
		public class Achievement
		{
			public string Id;

			public string Description;
		}

		public List<Achievement> Achievements;
	}
}
