using Godot;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	public class PlayerSidescroller : Player
	{
		protected override void PlayerMotion(out Vector2 inputMotion, out Vector2 actualMotion)
		{
			inputMotion = TwoDirectionalMotion();
			actualMotion = Vector2.Zero;
			if (NewDirection != Direction.None)
			{
				actualMotion = MoveAndSlide(inputMotion);
			}
			else
			{
				MoveAndCollide(inputMotion);
			}
		}
	}
}
