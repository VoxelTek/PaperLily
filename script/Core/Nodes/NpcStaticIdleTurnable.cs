using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "person")]
	public class NpcStaticIdleTurnable : Node2D, ITurnable, IIdle, ICharaWithStates
	{
		private CharacterSprite nSprite;

		[Export(PropertyHint.None, "")]
		public string CharacterId { get; set; }

		[Export(PropertyHint.None, "")]
		public Direction.Enum DefaultDirection { get; set; } = LacieEngine.Core.Direction.Down;


		[Export(PropertyHint.None, "")]
		public bool TurningEnabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public string TurnSpriteState { get; set; } = "stand";


		[Export(PropertyHint.None, "")]
		public string IdleSpriteState { get; set; } = "idle";


		public Direction.Enum Direction
		{
			get
			{
				return nSprite.Direction;
			}
			set
			{
				Turn(value);
			}
		}

		public string SpriteState
		{
			get
			{
				return nSprite.State;
			}
			set
			{
				nSprite.State = value;
			}
		}

		public override void _EnterTree()
		{
			if (!Engine.EditorHint && Validate())
			{
				nSprite = this.EnsureNode<CharacterSprite>("Sprite");
				nSprite.CharacterId = CharacterId;
				nSprite.State = IdleSpriteState;
				nSprite.Direction = DefaultDirection;
				Game.Room.RegisteredNPCs[CharacterId] = this;
			}
		}

		public void Turn(Direction direction)
		{
			if (TurningEnabled)
			{
				nSprite.State = TurnSpriteState;
				nSprite.Direction = direction;
			}
		}

		public void TurnToDefault()
		{
			StartIdleAnimation();
		}

		public void StartIdleAnimation()
		{
			nSprite.State = IdleSpriteState;
		}

		public void StopIdleAnimation()
		{
			nSprite.State = TurnSpriteState;
			nSprite.Direction = DefaultDirection;
		}

		private bool Validate()
		{
			if (CharacterId.IsNullOrEmpty())
			{
				Log.Error("Character ID not specified!");
				return false;
			}
			return true;
		}
	}
}
