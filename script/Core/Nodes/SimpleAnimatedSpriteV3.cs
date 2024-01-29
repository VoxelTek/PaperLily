using System;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "cat")]
	public class SimpleAnimatedSpriteV3 : Sprite, IAnimation2D
	{
		private float _timeAccumulator;

		private bool _canPlay;

		private int _animationFrame;

		private float _frameDuration;

		private int[] _framesToUse;

		private float _fps = 4f;

		private bool _playing;

		private string _animationFramesStr = "";

		private int[] _animationFrames;

		private string _frameDurationsStr = "";

		private float[] _frameDurations = new float[0];

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
				UpdateFrameDurations(_frameDurationsStr);
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
					UpdateValues();
					_playing = value;
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

		[Export(PropertyHint.MultilineText, "")]
		public string FrameDurations
		{
			get
			{
				return _frameDurationsStr;
			}
			set
			{
				UpdateFrameDurations(value);
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
			_frameDuration = _frameDurations[_animationFrame];
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

		private void UpdateFrameDurations(string value)
		{
			_frameDurationsStr = value;
			if (!value.IsNullOrEmpty())
			{
				_frameDurations = value.SplitFloats(',');
			}
			else
			{
				_frameDurations = new float[0];
			}
			for (int i = 0; i < _frameDurations.Length; i++)
			{
				_frameDurations[i] /= 1000f;
			}
			UpdateValues();
		}

		private void UpdateValues()
		{
			_canPlay = false;
			if (_animationFrames.IsNullOrEmpty())
			{
				_framesToUse = new int[base.Hframes * base.Vframes];
				for (int j = 0; j < _framesToUse.Length; j++)
				{
					_framesToUse[j] = j;
				}
			}
			else
			{
				_framesToUse = _animationFrames;
			}
			if (_frameDurations.Length != _framesToUse.Length)
			{
				float[] _frameDurationsFix = new float[_framesToUse.Length];
				Array.Fill(_frameDurationsFix, 1f / _fps);
				for (int i = 0; i < _frameDurations.Length && i < _frameDurationsFix.Length; i++)
				{
					_frameDurationsFix[i] = _frameDurations[i];
				}
				_frameDurations = _frameDurationsFix;
			}
			_animationFrame = 0;
			_timeAccumulator = 0f;
			base.Frame = _framesToUse[_animationFrame];
			_canPlay = true;
		}
	}
}
