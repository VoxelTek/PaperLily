namespace LacieEngine.Core
{
	public class PAchievement
	{
		public string Id { get; private set; }

		private PAchievement(string achievementId)
		{
			Id = achievementId;
		}

		public static implicit operator PAchievement(string achievementId)
		{
			return new PAchievement(achievementId);
		}

		public void Unlock()
		{
			Game.Achievements.Unlock(Id);
		}
	}
}
