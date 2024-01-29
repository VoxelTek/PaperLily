using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.API
{
	public interface IPlayer : IPhysicsInterpolated
	{
		bool Exists
		{
			get
			{
				if (Node != null)
				{
					return Node.IsValid();
				}
				return false;
			}
		}

		bool SneakingEnabled { get; set; }

		KinematicBody2D Node { get; }

		string SpriteState { get; set; }

		Direction SpriteDirection { get; set; }

		bool CollisionEnabled { get; set; }

		void SetLight(Light2D light);

		Light2D GetLight();

		Vector2 GetCenter();

		EventTrigger GetInteractingNode();

		EventTrigger GetItemInteractingNode(string itemId);

		Direction GetDirection();

		void SetDirection(Direction dir);

		void DisableRunning();

		void EnableRunning();

		CharacterState GetPlayerState();

		void SetPlayerState(CharacterState state);

		void Turn(Direction direction);
	}
}
