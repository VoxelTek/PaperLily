using Godot;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	public class NPCStaticTurnable : StaticBody2D, ITurnable
	{
		[Export(PropertyHint.None, "")]
		public Direction.Enum DefaultDirection;

		private Sprite nSprite;

		[Export(PropertyHint.None, "")]
		public bool TurningEnabled { get; set; } = true;


		public override void _Ready()
		{
			Game.Room.RegisteredNPCs[base.Name] = this;
			nSprite = (Sprite)GetNode("Sprite");
			TurnToDefault();
		}

		public void Turn(Direction direction)
		{
			if (TurningEnabled)
			{
				switch (direction.ToEnum())
				{
				case Direction.Enum.Left:
					nSprite.Frame = 0;
					break;
				case Direction.Enum.Up:
					nSprite.Frame = 1;
					break;
				case Direction.Enum.Right:
					nSprite.Frame = 2;
					break;
				case Direction.Enum.Down:
					nSprite.Frame = 3;
					break;
				}
			}
		}

		public void TurnToDefault()
		{
			if (TurningEnabled)
			{
				Turn(DefaultDirection);
			}
		}
	}
}
