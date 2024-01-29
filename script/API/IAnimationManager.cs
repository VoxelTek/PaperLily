using Godot;
using LacieEngine.Animation;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IAnimationManager : IModule
	{
		void Play(LacieAnimation animation);

		void PlayDelayed(LacieAnimation animation, float delay);

		void StopAnimations(Node node);

		void StopAnimations<T>(Node node) where T : LacieAnimation;
	}
}
