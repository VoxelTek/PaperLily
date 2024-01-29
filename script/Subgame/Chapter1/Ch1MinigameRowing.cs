using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Minigames;

namespace LacieEngine.Subgame.Chapter1
{
	public class Ch1MinigameRowing : Control, IMinigame
	{
		private static readonly string[] HandledInputs = new string[4] { "input_left", "input_up", "input_right", "input_down" };

		private static readonly Color NextValidKey = new Color("#77b359");

		private static readonly Color WrongKey = new Color("#b35959");

		private static readonly Color InvalidKey = new Color(1f, 1f, 1f, 0.3f);

		private Random Random = new Random();

		private const float MovementImpulseX = 1.5f;

		private const float MovementImpulseY = 2f;

		private const float MovementAtenuation = 0.01f;

		private const float SideLimit = 50f;

		private const string LeftArr = "res://assets/img/ui/input/keyboard_arrow_left.png";

		private const string UpArr = "res://assets/img/ui/input/keyboard_arrow_up.png";

		private const string RightArr = "res://assets/img/ui/input/keyboard_arrow_right.png";

		private const string DownArr = "res://assets/img/ui/input/keyboard_arrow_down.png";

		[GetNode("Main/Boat")]
		private Area2D nBoat;

		[GetNode("TilesWater")]
		private Node2D nWater;

		[GetNode("Stats")]
		private Label nStats;

		[GetNode("ScrollingLayer")]
		private Node2D nScrollingLayer;

		[GetNode("HBoxContainer/LeftContainer/VBoxContainer/LeftInputs")]
		private Control nLeftInputs;

		[GetNode("HBoxContainer/ForwardContainer/VBoxContainer/ForwardInputs")]
		private Control nForwardInputs;

		[GetNode("HBoxContainer/RightContainer/VBoxContainer/RightInputs")]
		private Control nRightInputs;

		private Dictionary<Direction, Texture> _directionTextures;

		private float _time;

		private float _impulseX;

		private float _impulseY;

		private bool _inputsGenerated;

		private bool _readingInputs;

		private Dictionary<Direction, Direction[]> _inputSequences;

		private Direction _inputChosen = Direction.None;

		private int _inputIndex;

		private float _obstacleGeneartionTimer = 3f;

		public override void _Input(InputEvent @event)
		{
			string input = Inputs.Handle(Inputs.Processor.Minigame, HandledInputs);
			if (input != null)
			{
				SendInput(input);
			}
		}

		public override void _Ready()
		{
			UpdateInputs();
			_readingInputs = true;
			nBoat.Connect("area_entered", this, "CollideWithRock");
		}

		public override void _Process(float delta)
		{
			_time += _impulseY * delta;
			_obstacleGeneartionTimer -= delta;
			if (_obstacleGeneartionTimer <= 0f)
			{
				Sprite sprite = new Sprite();
				sprite.Texture = GD.Load<Texture>("res://assets/sprite/ch1/forest/floor_rock2.png");
				Area2D obstacle = new Area2D();
				obstacle.Monitoring = false;
				obstacle.CollisionLayer = 1u;
				obstacle.CollisionMask = 0u;
				obstacle.Position = new Vector2(Random.Next(0, 640), -200f);
				obstacle.AddChild(sprite);
				obstacle.AddChild(GDUtil.MakeCollisionRect(new Vector2(20f, 10f)));
				nScrollingLayer.AddChild(obstacle);
				_obstacleGeneartionTimer = 3f;
			}
			UpdateWaterShader();
			nBoat.Position += new Vector2(_impulseX, 0f);
			foreach (Node2D child in nScrollingLayer.GetChildren())
			{
				child.Position += new Vector2(0f, _impulseY / 3f);
			}
			if (nBoat.Position.x <= 50f)
			{
				nBoat.Position = new Vector2(50f, nBoat.Position.y);
				_impulseX = 0f;
			}
			if (nBoat.Position.x >= 590f)
			{
				nBoat.Position = new Vector2(590f, nBoat.Position.y);
				_impulseX = 0f;
			}
			_impulseX = ((Math.Abs(_impulseX) < 0.01f) ? 0f : Mathf.Lerp(_impulseX, 0f, 0.01f));
			_impulseY = ((Math.Abs(_impulseY) < 1.01f) ? 1f : Mathf.Lerp(_impulseY, 1f, 0.01f));
			UpdateStats();
		}

		private void UpdateWaterShader()
		{
			(nWater.Material as ShaderMaterial).SetShaderParam("time", _time);
		}

		private void UpdateInputs()
		{
			if (!_inputsGenerated)
			{
				_inputSequences = new Dictionary<Direction, Direction[]>();
				_inputSequences[Direction.Left] = new Direction[3];
				_inputSequences[Direction.Left][0] = Direction.Left;
				_inputSequences[Direction.Left][1] = RandomInput();
				_inputSequences[Direction.Left][2] = RandomInput();
				_inputSequences[Direction.Up] = new Direction[4];
				_inputSequences[Direction.Up][0] = Direction.Up;
				_inputSequences[Direction.Up][1] = RandomInput();
				_inputSequences[Direction.Up][2] = RandomInput();
				_inputSequences[Direction.Up][3] = RandomInput();
				_inputSequences[Direction.Right] = new Direction[3];
				_inputSequences[Direction.Right][0] = Direction.Right;
				_inputSequences[Direction.Right][1] = RandomInput();
				_inputSequences[Direction.Right][2] = RandomInput();
				_inputsGenerated = true;
				nLeftInputs.Clear();
				Direction[] array = _inputSequences[Direction.Left];
				foreach (Direction d in array)
				{
					nLeftInputs.AddChild(MakeInputIndicator(d));
				}
				nForwardInputs.Clear();
				array = _inputSequences[Direction.Up];
				foreach (Direction d2 in array)
				{
					nForwardInputs.AddChild(MakeInputIndicator(d2));
				}
				nRightInputs.Clear();
				array = _inputSequences[Direction.Right];
				foreach (Direction d3 in array)
				{
					nRightInputs.AddChild(MakeInputIndicator(d3));
				}
				nLeftInputs.Modulate = Colors.White;
				nForwardInputs.Modulate = Colors.White;
				nRightInputs.Modulate = Colors.White;
				nLeftInputs.GetChild<Control>(0).Modulate = NextValidKey;
				nForwardInputs.GetChild<Control>(0).Modulate = NextValidKey;
				nRightInputs.GetChild<Control>(0).Modulate = NextValidKey;
			}
			if (_inputChosen != Direction.None)
			{
				nLeftInputs.Modulate = InvalidKey;
				nForwardInputs.Modulate = InvalidKey;
				nRightInputs.Modulate = InvalidKey;
				nLeftInputs.GetChild<Control>(0).Modulate = Colors.White;
				nForwardInputs.GetChild<Control>(0).Modulate = Colors.White;
				nRightInputs.GetChild<Control>(0).Modulate = Colors.White;
				Control selected = _inputChosen.ToEnum() switch
				{
					Direction.Enum.Left => nLeftInputs, 
					Direction.Enum.Up => nForwardInputs, 
					Direction.Enum.Right => nRightInputs, 
					_ => throw new NotImplementedException(), 
				};
				selected.Modulate = Colors.White;
				for (int i = 0; i < _inputIndex; i++)
				{
					selected.GetChild<Control>(i).Modulate = InvalidKey;
				}
				selected.GetChild<Control>(_inputIndex).Modulate = NextValidKey;
			}
		}

		private Control MakeInputIndicator(Direction direction)
		{
			TextureRect textureRect = GDUtil.MakeNode<TextureRect>("Indicator");
			textureRect.Texture = _directionTextures[direction];
			textureRect.Expand = true;
			textureRect.RectMinSize = new Vector2(20f, 20f);
			return textureRect;
		}

		private void SendInput(string input)
		{
			if (!_readingInputs || !_inputsGenerated)
			{
				return;
			}
			if (_inputChosen == Direction.None)
			{
				if (!(input == "input_down"))
				{
					_inputChosen = Direction.FromInput(input);
					_inputIndex = 1;
					UpdateInputs();
				}
			}
			else if (_inputSequences[_inputChosen][_inputIndex] == Direction.FromInput(input))
			{
				_inputIndex++;
				if (_inputIndex >= _inputSequences[_inputChosen].Length)
				{
					SendImpulse(_inputChosen);
					ResetInputs();
				}
				else
				{
					UpdateInputs();
				}
			}
			else
			{
				WrongInpupt();
			}
		}

		private async void WrongInpupt()
		{
			_readingInputs = false;
			(_inputChosen.ToEnum() switch
			{
				Direction.Enum.Left => nLeftInputs, 
				Direction.Enum.Up => nForwardInputs, 
				Direction.Enum.Right => nRightInputs, 
				_ => throw new NotImplementedException(), 
			}).GetChild<Control>(_inputIndex).Modulate = WrongKey;
			await DrkieUtil.DelaySeconds(0.5);
			ResetInputs();
			_readingInputs = true;
		}

		private void ResetInputs()
		{
			_inputSequences = null;
			_inputChosen = Direction.None;
			_inputIndex = 0;
			_inputsGenerated = false;
			UpdateInputs();
		}

		private void SendImpulse(Direction direction)
		{
			if (direction == Direction.Left)
			{
				_impulseX -= 1.5f;
			}
			else if (direction == Direction.Right)
			{
				_impulseX += 1.5f;
			}
			else if (direction == Direction.Up)
			{
				_impulseY += 2f;
			}
		}

		private void UpdateStats()
		{
			nStats.Text = "Speed: " + Math.Round(_impulseY, 2).ToString("0.00") + "km/h\nDistance: " + Math.Round(_time, 2).ToString("0.00") + "m";
		}

		private Direction RandomInput()
		{
			double d = Random.NextDouble();
			if (d < 0.25)
			{
				return Direction.Left;
			}
			if (d < 0.5)
			{
				return Direction.Up;
			}
			if (d < 0.75)
			{
				return Direction.Right;
			}
			return Direction.Down;
		}

		public void Init()
		{
			_directionTextures = new Dictionary<Direction, Texture>();
			_directionTextures[Direction.Left] = GD.Load<Texture>("res://assets/img/ui/input/keyboard_arrow_left.png");
			_directionTextures[Direction.Up] = GD.Load<Texture>("res://assets/img/ui/input/keyboard_arrow_up.png");
			_directionTextures[Direction.Right] = GD.Load<Texture>("res://assets/img/ui/input/keyboard_arrow_right.png");
			_directionTextures[Direction.Down] = GD.Load<Texture>("res://assets/img/ui/input/keyboard_arrow_down.png");
		}

		public void CollideWithRock(Area2D area)
		{
			area.Delete();
			nBoat.Position = new Vector2(320f, nBoat.Position.y);
			_impulseX = 0f;
			_impulseY -= 1f;
			ResetInputs();
		}
	}
}
