using Godot;

namespace LacieEngine.Core
{
	public interface IFloorMovementModifier
	{
		Vector2 GetModifiedMovement(Vector2 movement);
	}
}
