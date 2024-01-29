namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IAchievementManager : IModule
	{
		bool IsValid(string achievementId);

		void Unlock(string achievementId);
	}
}
