using System.Threading.Tasks;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IMemoryManager : IModule
	{
		void Cache(string path);

		void SystemCache(string path);

		Task WaitForCompletion();
	}
}
