using Godot;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	public class NPCWalker : WalkingCharacter
	{
		public NPCWalker()
		{
		}

		public NPCWalker(string characterName)
			: this()
		{
			base.CharacterId = characterName;
			base.DefaultDirection = Direction.Down;
		}

		public NPCWalker(string characterName, string defaultDirection)
			: this()
		{
			base.CharacterId = characterName;
			base.DefaultDirection = Direction.FromString(defaultDirection);
		}

		public override void _EnterTree()
		{
			base.Name = base.CharacterId.ToPascalCase();
			CreateCharacterSprite();
			Game.Room.RegisteredNPCs[base.CharacterId] = this;
		}

		protected override void _CharacterReady()
		{
			base.SpriteState = "stand";
			TurnToDefault();
			Ready = true;
		}

		public override void _PhysicsProcess(float delta)
		{
			if (Ready)
			{
				if (AutoMovement)
				{
					NewDirection = AutoDirection;
				}
				UpdateState();
				Vector2 motion = EightDirectionalMotion();
				MoveAndSlide(motion);
				if (AutoMovement)
				{
					CheckIfAutoMovementOver();
				}
				PlayAnimations();
			}
		}

		public override Vector2 GetMovementWithFloorModifier(Vector2 movement)
		{
			return movement;
		}
	}
}
