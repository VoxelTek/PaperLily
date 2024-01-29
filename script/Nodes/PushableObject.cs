using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	[ExportType(icon = "trolley")]
	public class PushableObject : KinematicBody2D, IActionObject
	{
		private Vector2 _playerRelativePosition;

		private Direction _pushDirection;

		private CollisionShape2D _tempCollisionShape;

		[Export(PropertyHint.None, "")]
		public string PositionVariable { get; set; }

		public PushableObject()
		{
			base.CollisionLayer = 17u;
			base.CollisionMask = 3u;
		}

		public override void _Ready()
		{
			if (!PositionVariable.IsNullOrEmpty() && !Game.Variables.GetVariable(PositionVariable).IsNullOrEmpty())
			{
				base.Position = GDUtil.StringToVector2(Game.Variables.GetVariable(PositionVariable));
			}
			SetPhysicsProcess(enable: false);
		}

		public void Activate()
		{
			_pushDirection = Game.Player.SpriteDirection;
			Game.InputProcessor = Inputs.Processor.PlayerInObject;
			Game.Player.SetPlayerState(CharacterState.InObject);
			Game.Player.CollisionEnabled = false;
			_playerRelativePosition = Game.Player.Node.GlobalPosition - base.GlobalPosition;
			Game.Player.SpriteState = "push";
			Game.Player.SpriteDirection = _pushDirection;
			RectangleShape2D shape = new RectangleShape2D();
			shape.Extents = new Vector2(11.5f, 4f) + new Vector2(1f, 1f);
			_tempCollisionShape = new CollisionShape2D();
			_tempCollisionShape.Shape = shape;
			AddChild(_tempCollisionShape);
			_tempCollisionShape.GlobalPosition = Game.Player.Node.GlobalPosition + new Vector2(0f, -4f);
			SetProcessInput(enable: true);
			SetPhysicsProcess(enable: true);
		}

		public void Deactivate()
		{
			Game.InputProcessor = Inputs.Processor.Player;
			Game.Player.SetPlayerState(CharacterState.Standing);
			_tempCollisionShape.DeleteIfValid();
			_tempCollisionShape = null;
			Game.Player.CollisionEnabled = true;
			SetProcessInput(enable: false);
			SetPhysicsProcess(enable: false);
			this.PixelSnap();
			if (!PositionVariable.IsNullOrEmpty())
			{
				Game.Variables.SetVariable(PositionVariable, base.Position.x + "," + base.Position.y);
			}
		}

		public override void _Input(InputEvent @event)
		{
			if (Inputs.HandleSingle(@event, Inputs.Processor.PlayerInObject, "input_action"))
			{
				Deactivate();
			}
		}

		public override void _PhysicsProcess(float delta)
		{
			Direction dir = Inputs.GetDirectionFromInput(Direction.None, Inputs.Processor.PlayerInObject);
			if (dir == Direction.None || (_pushDirection.HasX() && !dir.HasX()) || (_pushDirection.HasY() && !dir.HasY()))
			{
				Game.Player.SpriteState = "push_stand";
				Game.Player.SpriteDirection = _pushDirection;
				return;
			}
			dir = (_pushDirection.HasX() ? dir.FlattenX() : dir.FlattenY());
			Game.Player.SpriteState = "push";
			Game.Player.SpriteDirection = _pushDirection;
			Vector2 motion = dir.ToVector() * 30f;
			Vector2 actualMotion = MoveAndSlide(motion);
			this.PixelSnap(actualMotion.x == 0f, actualMotion.y == 0f);
			Game.Player.Node.Position = base.GlobalPosition + _playerRelativePosition;
		}
	}
}
