using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1GardenExitRitual : GameRoom
	{
		public class CameraPanUpAnimation : LacieAnimation
		{
			private Node2D _node;

			private Vector2 _from;

			private Vector2 _to;

			private Vector2 _track;

			public CameraPanUpAnimation(Node2D node, Vector2 from, Vector2 to, float duration)
			{
				base.Node = (_node = node);
				base.Duration = duration;
				_from = from;
				_to = to;
			}

			public override void InitialCalculations()
			{
				_track = _to - _from;
			}

			public override void UpdateToFirstFrame()
			{
				_node.Position = _from;
			}

			public override void UpdateToCurrentFrame()
			{
				float progress = base.Elapsed / base.Duration;
				progress = progress * progress * progress;
				_node.Position = _from + _track * progress;
			}

			public override void UpdateToFinalFrame()
			{
				_node.Position = _to;
			}
		}

		[GetNode("Main/VaseNoFlower")]
		private Node2D nVaseNoFlower;

		[GetNode("Main/VaseFlower")]
		private Node2D nVaseFlower;

		[GetNode("Main/VaseNoFlower/Paperlily")]
		private Node2D nPaperLily;

		[GetNode("Main/DoorGroup/Door")]
		private Sprite nDoor;

		[GetNode("Main/DoorGroup/White")]
		private Node2D nDoorWhite;

		private PVar varTookFlower = "ch1.garden_took_flower";

		private PVar varPlacedFlower = "ch1.garden_placed_flower";

		public override void _UpdateRoom()
		{
			bool placedFlower = varPlacedFlower.Value;
			nVaseNoFlower.Visible = varTookFlower.Value;
			nVaseFlower.Visible = !varTookFlower.Value;
			nDoor.Frame = (placedFlower ? 2 : 0);
			nDoorWhite.Visible = placedFlower;
			nPaperLily.Visible = placedFlower;
		}

		public void DespawnLacie()
		{
			Game.Animations.Play(new FadeOutAnimation(Game.Room.RegisteredNPCs["lacie"], 1f));
		}

		public void CameraPanUp()
		{
			Game.Animations.Play(new CameraPanUpAnimation(Game.Camera.Node, Game.Camera.Node.Position, Game.Camera.Node.Position + Vector2.Up * 1000f, 2f));
		}
	}
}
