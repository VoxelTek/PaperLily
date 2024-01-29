using Godot;
using LacieEngine.API;
using LacieEngine.Nodes;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType]
	public class ActionPlayerSpeed : Node, IAction, IToggleable
	{
		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public float Multiplier { get; set; } = 1f;


		public override void _Ready()
		{
			if (!Engine.EditorHint)
			{
				Game.Room.RegisteredRoomActions[base.Name] = this;
			}
		}

		public void Execute()
		{
			if (Enabled && Game.Player != null && Game.Player.Node.IsValid() && Game.Player.Node is WalkingCharacter walkingCharacter)
			{
				walkingCharacter.MoveSpeedMultiplier = Multiplier;
			}
		}
	}
}
