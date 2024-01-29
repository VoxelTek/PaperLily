using System;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "cat")]
	public class SimpleAnimatedSpriteV2 : Sprite, IAnimation2D
	{
		private const float DEFAULT_FRAME_DURATION = 0.1f;

		private float _timeAccumulator;

		private bool _canPlay;

		private int _animationFrame;

		private float _frameDuration;

		private bool _playing;

		private string _frameDurationsStr = "";

		private float[] _frameDurations = new float[0];

		[Export(PropertyHint.None, "")]
		public bool Playing
		{
			get
			{
				return _playing;
			}
			set
			{
				_playing = value;
				UpdateValues();
			}
		}

		[Export(PropertyHint.None, "")]
		public bool Loop { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public bool Autostart { get; set; } = true;


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
				Playing = true;
			}
		}

		public override void _Process(float delta)
		{
			if (!_canPlay || !_playing || !IsVisibleInTree())
			{
				return;
			}
			_timeAccumulator += delta;
			_frameDuration = _frameDurations[_animationFrame];
			while (_timeAccumulator >= _frameDuration)
			{
				_timeAccumulator -= _frameDuration;
				if (++_animationFrame >= base.Hframes * base.Vframes)
				{
					_animationFrame = 0;
					if (!Loop)
					{
						_playing = false;
						break;
					}
				}
				_frameDuration = _frameDurations[_animationFrame];
				base.Frame = _animationFrame;
			}
		}

		public void Play()
		{
			Playing = true;
		}

		public void Stop()
		{
			Playing = false;
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
			if (_frameDurations.Length != base.Hframes * base.Vframes)
			{
				float[] _frameDurationsFix = new float[base.Hframes * base.Vframes];
				Array.Fill(_frameDurationsFix, 0.1f);
				for (int i = 0; i < _frameDurations.Length && i < _frameDurationsFix.Length; i++)
				{
					_frameDurationsFix[i] = _frameDurations[i];
				}
				_frameDurations = _frameDurationsFix;
			}
			base.Frame = (_animationFrame = 0);
			_canPlay = true;
		}
	}
}
