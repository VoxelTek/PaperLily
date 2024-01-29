using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	[Tool]
	[ExportType(icon = "chair")]
	public class ChairV2 : Node2D, IEditorArea
	{
		public class ActualSitSurface : StaticBody2D, IActionObject
		{
			public const int StandUpDistance = 10;

			private ChairV2 _surface;

			private CharacterSprite _playerSittingSprite;

			private Vector2 _playerOriginalPosition;

			private Direction _playerOriginalDirection;

			private Direction _curSurfaceDirection;

			public ActualSitSurface(ChairV2 surface)
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
				_playerOriginalPosition = Game.Player.Node.GlobalPosition;
				_playerOriginalDirection = Game.Player.SpriteDirection;
				_curSurfaceDirection = _playerOriginalDirection.GetOpposite();
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
				_playerSittingSprite.Direction = _curSurfaceDirection;
				_playerSittingSprite.ShowBehindParent = _surface.ShowPlayerBehindObject;
				Vector2 playerOffset = _playerOriginalDirection.ToEnum() switch
				{
					Direction.Enum.Left => _surface.PlayerOffsetLeft, 
					Direction.Enum.Up => _surface.PlayerOffsetUp, 
					Direction.Enum.Right => _surface.PlayerOffsetRight, 
					Direction.Enum.Down => _surface.PlayerOffsetDown, 
					_ => Vector2.Zero, 
				};
				_playerSittingSprite.GlobalPosition = _surface.GlobalPosition + playerOffset;
				if (_curSurfaceDirection.HasY() && _surface.WideX)
				{
					float minPos2 = base.GlobalPosition.x - _surface.Area.x / 2f + (float)_surface.WideMarginX;
					float maxPos2 = base.GlobalPosition.x + _surface.Area.x / 2f - (float)_surface.WideMarginX;
					_playerOriginalPosition.x = Math.Max(minPos2, _playerOriginalPosition.x);
					_playerOriginalPosition.x = Math.Min(maxPos2, _playerOriginalPosition.x);
					_playerSittingSprite.Position += new Vector2(_playerOriginalPosition.x - base.GlobalPosition.x, 0f);
				}
				else if (_curSurfaceDirection.HasX() && _surface.WideY)
				{
					float minPos = base.GlobalPosition.y - _surface.Area.y / 2f + (float)_surface.WideMarginY;
					float maxPos = base.GlobalPosition.y + _surface.Area.y / 2f - (float)_surface.WideMarginY;
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
				Game.Player.SetDirection(_playerOriginalDirection.GetOpposite());
				Vector2 newPlayerGlobalPosition = base.GlobalPosition;
				if (_curSurfaceDirection.HasY())
				{
					int newX2 = (int)(_surface.WideX ? _playerOriginalPosition.x : base.GlobalPosition.x);
					int newY2 = (int)(base.GlobalPosition.y + _curSurfaceDirection.ToVector().y * (_surface.Area.y / 2f + 10f));
					newPlayerGlobalPosition = new Vector2(newX2, newY2);
				}
				else if (_curSurfaceDirection.HasX())
				{
					int newX = (int)(base.GlobalPosition.x + _curSurfaceDirection.ToVector().x * (_surface.Area.x / 2f + 10f));
					int newY = (int)(_surface.WideY ? ((float)(int)_playerOriginalPosition.y) : base.GlobalPosition.y);
					newPlayerGlobalPosition = new Vector2(newX, newY);
				}
				Game.Player.Node.GlobalPosition = newPlayerGlobalPosition;
				Game.Player.CollisionEnabled = true;
				Game.InputProcessor = Inputs.Processor.Player;
				SetProcessInput(enable: false);
			}

			public bool IsValidForDirection(Direction direction)
			{
				return Direction.FromByte(_surface.Directions).HasDirection(direction);
			}

			public override void _Input(InputEvent @event)
			{
				switch (Inputs.Handle(@event, Inputs.Processor.PlayerInObject, HandledInputs))
				{
				case "input_action":
					Deactivate();
					break;
				case "input_left":
					if (_curSurfaceDirection == Direction.Left)
					{
						Deactivate();
					}
					break;
				case "input_up":
					if (_curSurfaceDirection == Direction.Up)
					{
						Deactivate();
					}
					break;
				case "input_right":
					if (_curSurfaceDirection == Direction.Right)
					{
						Deactivate();
					}
					break;
				case "input_down":
					if (_curSurfaceDirection == Direction.Down)
					{
						Deactivate();
					}
					break;
				}
			}
		}

		private static readonly string[] HandledInputs = new string[5] { "input_action", "input_left", "input_up", "input_right", "input_down" };

		[Export(PropertyHint.None, "")]
		public Vector2 PlayerOffsetLeft = new Vector2(0f, 0f);

		[Export(PropertyHint.None, "")]
		public Vector2 PlayerOffsetUp = new Vector2(0f, 0f);

		[Export(PropertyHint.None, "")]
		public Vector2 PlayerOffsetRight = new Vector2(0f, 0f);

		[Export(PropertyHint.None, "")]
		public Vector2 PlayerOffsetDown = new Vector2(0f, 0f);

		[Export(PropertyHint.None, "")]
		public Vector2 Area { get; set; } = new Vector2(32f, 32f);


		[Export(PropertyHint.Flags, "Left,Up,Right,Down")]
		public byte Directions { get; set; }

		[Export(PropertyHint.None, "")]
		public bool WideX { get; set; }

		[Export(PropertyHint.None, "")]
		public bool WideY { get; set; }

		[Export(PropertyHint.None, "")]
		public int WideMarginX { get; set; } = 8;


		[Export(PropertyHint.None, "")]
		public int WideMarginY { get; set; } = 8;


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
