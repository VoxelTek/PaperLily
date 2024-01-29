using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "person")]
	public class NpcStaticIdle : Node2D, IIdle, ICharaWithStates
	{
		private CharacterSprite nSprite;

		[Export(PropertyHint.None, "")]
		public string CharacterId { get; set; }

		[Export(PropertyHint.None, "")]
		public string IdleSpriteState { get; set; } = "idle";


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
				Game.Room.RegisteredNPCs[CharacterId] = this;
			}
		}

		public void StartIdleAnimation()
		{
			nSprite.Playing = true;
		}

		public void StopIdleAnimation()
		{
			nSprite.Playing = false;
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
