namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IPlatformInitializer
	{
		void Init();
	}
}
