using Godot;

namespace LacieEngine.API
{
	public interface IPhysicsInterpolated
	{
		Node2D VisualNode { get; }

		void Teleport();

		void Reparent(Node newParent);
	}
}
