using Godot;

namespace LacieEngine.API
{
	public interface IAnimation2D
	{
		Node2D Node => (Node2D)this;

		void Play();

		void Stop();
	}
}
