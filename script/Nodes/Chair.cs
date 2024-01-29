using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	[Tool]
	[ExportType(icon = "chair")]
	public class Chair : Node2D, IEditorArea
	{
		public class ActualSitSurface : StaticBody2D, IActionObject
		{
			public const int StandUpDistance = 10;

			private Chair _surface;

			private CharacterSprite _playerSittingSprite;

			private Vector2 _playerOriginalPosition;

			public ActualSitSurface(Chair surface)
			{
				_surface = surface;
			}

			public override void _Ready()
			{
				SetProcessInput(enable: false);
			}

			public void Activate()
			{
				Game.InputProcessor = Inputs.Processor.PlayerInObject;
				Game.Player.Node.Visible = false;
				Game.Player.CollisionEnabled = false;
				Direction surfaceDirection = _surface.Direction;
				_playerOriginalPosition = Game.Player.Node.GlobalPosition;
				Game.Player.SetPlayerState(CharacterState.InObject);
				_playerSittingSprite = new CharacterSprite();
				_playerSittingSprite.Frame = 0;
				if (_surface.RelatedNode.IsNullOrEmpty())
				{
					AddChild(_playerSittingSprite);
				}
				else
				{
					_surface.GetNode(_surface.RelatedNode).AddChild(_playerSittingSprite);
				}
				_playerSittingSprite.CharacterId = Game.State.Party[0];
				_playerSittingSprite.State = "sit";
				_playerSittingSprite.Direction = surfaceDirection;
				_playerSittingSprite.ShowBehindParent = _surface.ShowPlayerBehindObject;
				_playerSittingSprite.GlobalPosition = _surface.GlobalPosition + _surface.PlayerOffset;
				if (surfaceDirection.HasY() && _surface.Wide)
				{
					float minPos2 = base.GlobalPosition.x - _surface.Area.x / 2f + (float)_surface.WideMargin;
					float maxPos2 = base.GlobalPosition.x + _surface.Area.x / 2f - (float)_surface.WideMargin;
					_playerOriginalPosition.x = Math.Max(minPos2, _playerOriginalPosition.x);
					_playerOriginalPosition.x = Math.Min(maxPos2, _playerOriginalPosition.x);
					_playerSittingSprite.Position += new Vector2(_playerOriginalPosition.x - base.GlobalPosition.x, 0f);
				}
				else if (surfaceDirection.HasX() && _surface.Wide)
				{
					float minPos = base.GlobalPosition.y - _surface.Area.y / 2f + (float)_surface.WideMargin;
					float maxPos = base.GlobalPosition.y + _surface.Area.y / 2f - (float)_surface.WideMargin;
					_playerOriginalPosition.y = Math.Max(minPos, _playerOriginalPosition.y);
					_playerOriginalPosition.y = Math.Min(maxPos, _playerOriginalPosition.y);
					_playerSittingSprite.Position += new Vector2(0f, _playerOriginalPosition.y - base.GlobalPosition.y);
				}
				Game.Player.Node.GlobalPosition = _playerSittingSprite.GlobalPosition;
				SetProcessInput(enable: true);
			}

			public void Deactivate()
			{
				_playerSittingSprite.Delete();
				_playerSittingSprite = null;
				Game.Player.Node.Visible = true;
				Game.Player.SetDirection(_surface.Direction);
				Vector2 newPlayerGlobalPosition = base.GlobalPosition;
				Direction surfaceDirection = _surface.Direction;
				if (surfaceDirection.HasX())
				{
					int newX2 = (int)(base.GlobalPosition.x + surfaceDirection.ToVector().x * (_surface.Area.x / 2f + 10f));
					int newY2 = (int)(_surface.Wide ? ((float)(int)_playerOriginalPosition.y) : base.GlobalPosition.y);
					newPlayerGlobalPosition = new Vector2(newX2, newY2);
				}
				else if (surfaceDirection.HasY())
				{
					int newX = (int)(_surface.Wide ? _playerOriginalPosition.x : base.GlobalPosition.x);
					int newY = (int)(base.GlobalPosition.y + surfaceDirection.ToVector().y * (_surface.Area.y / 2f + 10f));
					newPlayerGlobalPosition = new Vector2(newX, newY);
				}
				Game.Player.Node.GlobalPosition = newPlayerGlobalPosition;
				Game.Player.CollisionEnabled = true;
				Game.InputProcessor = Inputs.Processor.Player;
				SetProcessInput(enable: false);
			}

			public bool IsValidForDirection(Direction direction)
			{
				return _surface.Direction.ToDirection().GetOpposite() == direction;
			}

			public override void _Input(InputEvent @event)
			{
				switch (Inputs.Handle(@event, Inputs.Processor.PlayerInObject, HandledInputs))
				{
				case "input_action":
					Deactivate();
					break;
				case "input_left":
					if (_surface.Direction == LacieEngine.Core.Direction.Left)
					{
						Deactivate();
					}
					break;
				case "input_up":
					if (_surface.Direction == LacieEngine.Core.Direction.Up)
					{
						Deactivate();
					}
					break;
				case "input_right":
					if (_surface.Direction == LacieEngine.Core.Direction.Right)
					{
						Deactivate();
					}
					break;
				case "input_down":
					if (_surface.Direction == LacieEngine.Core.Direction.Down)
					{
						Deactivate();
					}
					break;
				}
			}
		}

		private static readonly string[] HandledInputs = new string[5] { "input_action", "input_left", "input_up", "input_right", "input_down" };

		[Export(PropertyHint.None, "")]
		public Vector2 PlayerOffset = new Vector2(0f, 0f);

		[Export(PropertyHint.None, "")]
		public Direction.Enum Direction = LacieEngine.Core.Direction.Enum.Down;

		[Export(PropertyHint.None, "")]
		public Vector2 Area { get; set; } = new Vector2(32f, 32f);


		[Export(PropertyHint.None, "")]
		public bool Wide { get; set; }

		[Export(PropertyHint.None, "")]
		public int WideMargin { get; set; } = 8;


		[Export(PropertyHint.None, "")]
		public NodePath RelatedNode { get; private set; } = new NodePath();


		[Export(PropertyHint.None, "")]
		public bool ShowPlayerBehindObject { get; set; }

		public override void _EnterTree()
		{
			if (!Engine.EditorHint)
			{
				ActualSitSurface _collisionNode = new ActualSitSurface(this);
				_collisionNode.CollisionLayer = 1u;
				_collisionNode.CollisionMask = 0u;
				AddChild(_collisionNode);
				CollisionShape2D _shapeNode = new CollisionShape2D();
				RectangleShape2D rect = new RectangleShape2D();
				rect.Extents = Area / 2f;
				_shapeNode.Shape = rect;
				_shapeNode.Position = this.GetPixelPerfectOffset();
				_collisionNode.AddChild(_shapeNode);
			}
		}

		void IEditorArea.Update()
		{
			Update();
		}
	}
}
