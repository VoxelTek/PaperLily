using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityElevator : GameRoom
	{
		[GetNode("Background/WallIndicator/B1F")]
		private Node2D nB1FLight;

		[GetNode("Background/WallIndicator/B2F")]
		private Node2D nB2FLight;

		[GetNode("Background/WallIndicator/B3F")]
		private Node2D nB3FLight;

		[GetNode("Background/WallIndicator/B4F")]
		private Node2D nB4FLight;

		private PVar varElevatorOn = "ch1.facility_elevator_on";

		private PVar varElevatorLocation = "ch1.facility_elevator_location";

		public bool Riding;

		private const float CAMERA_SHAKE_FPS = 10f;

		private Random random = new Random();

		private Vector2 _cameraOffset = Vector2.Zero;

		private Vector2 _direction = Direction.Down.ToVector();

		private float _elapsed;

		public override void _UpdateRoom()
		{
			if ((bool)varElevatorOn.Value)
			{
				int location = varElevatorLocation.Value;
				nB1FLight.Visible = location == 1;
				nB2FLight.Visible = location == 2;
				nB3FLight.Visible = location == 3;
				nB4FLight.Visible = location == 4;
			}
		}

		public override void _RoomProcess(float delta)
		{
			if (Riding)
			{
				ProcessShake(delta);
			}
		}

		public void StartElevator()
		{
			Riding = true;
		}

		public void StopElevator()
		{
			Riding = false;
		}

		private void ProcessShake(float delta)
		{
			_elapsed += delta;
			if (_elapsed >= 0.1f)
			{
				_elapsed = 0f;
				Vector2 movementDirection = _direction;
				Vector2 newOffset = _cameraOffset + movementDirection;
				if (Math.Abs(newOffset.x) > 1f || Math.Abs(newOffset.y) > 1f)
				{
					movementDirection = (_direction = random.NextDirection().ToVector());
					newOffset = _cameraOffset + movementDirection;
				}
				if (Math.Abs(newOffset.x) > 1f)
				{
					movementDirection.x = 0f;
				}
				if (Math.Abs(newOffset.y) > 1f)
				{
					movementDirection.y = 0f;
				}
				_cameraOffset = newOffset;
				if (movementDirection == Vector2.Zero)
				{
					_cameraOffset = Vector2.Zero;
					_direction = random.NextDirection().ToVector();
				}
				Game.Camera.Node.Offset = _cameraOffset.Round();
			}
		}
	}
}
