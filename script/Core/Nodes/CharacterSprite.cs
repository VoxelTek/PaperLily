using System.Collections.Generic;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "face")]
	public class CharacterSprite : SimpleAnimatedSprite
	{
		public const string STATE_STAND = "stand";

		public const string STATE_WALK = "walk";

		public const string STATE_RUN = "run";

		public const string STATE_SNEAK = "sneak";

		public const string STATE_IDLE = "idle";

		public const string STATE_SIT = "sit";

		public const string STATE_PUSH = "push";

		public const string STATE_PUSH_STAND = "push_stand";

		private float _baseFps;

		private float _fpsMultiplier = 1f;

		private string _state;

		private Direction _direction = LacieEngine.Core.Direction.None;

		private Dictionary<string, string> _stateOverrides = new Dictionary<string, string>();

		private Dictionary<string, Texture> _textureOverrides = new Dictionary<string, Texture>();

		[Export(PropertyHint.None, "")]
		public string CharacterId { get; set; }

		[Export(PropertyHint.None, "")]
		public string State
		{
			get
			{
				return _state;
			}
			set
			{
				SetState(value);
			}
		}

		[Export(PropertyHint.None, "")]
		public Direction.Enum Direction
		{
			get
			{
				return _direction;
			}
			set
			{
				SetDirection(value);
			}
		}

		public float FpsMultiplier
		{
			get
			{
				return _fpsMultiplier;
			}
			set
			{
				SetFpsMultiplier(value);
			}
		}

		public override void _Ready()
		{
			if (!Engine.EditorHint)
			{
				Direction preEnterDirection = _direction;
				if (_state != null)
				{
					UpdateState();
				}
				if (preEnterDirection != LacieEngine.Core.Direction.None)
				{
					_direction = preEnterDirection;
					UpdateDirection();
				}
				base.LightMask = 2;
			}
		}

		public void OverrideTextureForState(string state, Texture texture)
		{
			if (texture != null)
			{
				_textureOverrides[state] = texture;
			}
			if (_state == state)
			{
				base.Texture = texture;
			}
		}

		public void OverrideStateWithState(string oldState, string newState)
		{
			_stateOverrides[oldState] = newState;
			if (_state == oldState)
			{
				UpdateState();
			}
		}

		public void ClearOverrides()
		{
			_textureOverrides.Clear();
			_stateOverrides.Clear();
		}

		private void SetState(string state)
		{
			if (!IsInsideTree())
			{
				_state = state;
			}
			else if (!(_state == state))
			{
				_state = state;
				UpdateState();
			}
		}

		private void SetDirection(Direction direction)
		{
			if (!IsInsideTree())
			{
				_direction = direction;
			}
			else if (!(_direction == direction))
			{
				_direction = direction;
				UpdateDirection();
			}
		}

		private void SetFpsMultiplier(float fpsMultiplier)
		{
			if (_fpsMultiplier != fpsMultiplier)
			{
				_fpsMultiplier = fpsMultiplier;
				base.FPS = _baseFps * _fpsMultiplier;
			}
		}

		private void UpdateState()
		{
			if (!Engine.EditorHint)
			{
				string state = _state;
				if (_stateOverrides.ContainsKey(state))
				{
					state = _stateOverrides[state];
				}
				Game.Characters.Get(CharacterId).UpdateSpriteState(this, state);
				if (_textureOverrides.ContainsKey(state))
				{
					base.Texture = _textureOverrides[state];
				}
				_baseFps = base.FPS;
				_fpsMultiplier = 1f;
			}
		}

		private void UpdateDirection()
		{
			if (!Engine.EditorHint)
			{
				string state = _state;
				if (_stateOverrides.ContainsKey(state))
				{
					state = _stateOverrides[state];
				}
				Game.Characters.Get(CharacterId).UpdateSpriteDirection(this, _direction, state);
			}
		}
	}
}
