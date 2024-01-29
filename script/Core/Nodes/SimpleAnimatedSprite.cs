using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "cat")]
	public class SimpleAnimatedSprite : Sprite, IAnimation2D
	{
		private float _timeAccumulator;

		private float _frameDuration;

		private bool _canPlay;

		private int _animationFrame;

		private int[] _framesToUse;

		private float _fps = 4f;

		private bool _playing;

		private string _animationFramesStr = "";

		private int[] _animationFrames;

		[Export(PropertyHint.Range, "0.1,300")]
		public float FPS
		{
			get
			{
				return _fps;
			}
			set
			{
				_fps = value;
				UpdateValues();
			}
		}

		[Export(PropertyHint.None, "")]
		public bool Playing
		{
			get
			{
				return _playing;
			}
			set
			{
				if (_playing != value)
				{
					_playing = value;
					UpdateValues();
				}
			}
		}

		[Export(PropertyHint.None, "")]
		public bool Loop { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public int LoopFrame { get; set; }

		[Export(PropertyHint.None, "")]
		public bool Autostart { get; set; } = true;


		[Export(PropertyHint.MultilineText, "")]
		public string AnimationFrames
		{
			get
			{
				return _animationFramesStr;
			}
			set
			{
				UpdateAnimationFrames(value);
			}
		}

		public override void _EnterTree()
		{
			if (Autostart && !Engine.EditorHint)
			{
				_playing = true;
			}
			UpdateValues();
		}

		public override void _Process(float delta)
		{
			if (!_canPlay || !_playing || !IsVisibleInTree())
			{
				return;
			}
			_timeAccumulator += delta;
			if (!(_timeAccumulator >= _frameDuration))
			{
				return;
			}
			_timeAccumulator -= _frameDuration;
			if (++_animationFrame >= _framesToUse.Length)
			{
				_animationFrame = LoopFrame;
				if (!Loop)
				{
					_animationFrame = 0;
					_playing = false;
					return;
				}
			}
			base.Frame = _framesToUse[_animationFrame];
		}

		public void Play()
		{
			Playing = true;
		}

		public void Stop()
		{
			Playing = false;
		}

		private void UpdateAnimationFrames(string value)
		{
			_animationFramesStr = value;
			if (!value.IsNullOrEmpty())
			{
				_animationFrames = value.SplitInts(',');
			}
			else
			{
				_animationFrames = null;
			}
			UpdateValues();
		}

		private void UpdateValues()
		{
			_canPlay = false;
			if (_animationFrames.IsNullOrEmpty())
			{
				_framesToUse = new int[base.Hframes * base.Vframes];
				for (int i = 0; i < _framesToUse.Length; i++)
				{
					_framesToUse[i] = i;
				}
			}
			else
			{
				_framesToUse = _animationFrames;
			}
			_animationFrame = 0;
			_frameDuration = 1f / _fps;
			base.Frame = _framesToUse[_animationFrame];
			_canPlay = true;
		}
	}
}
