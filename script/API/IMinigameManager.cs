using Godot;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IMinigameManager
	{
		bool Running { get; }

		Node Node { get; }

		void Start(string file);

		void End(string @event);

		void End();

		bool Function(string function);
	}
}
