using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	[Tool]
	[ExportType(icon = "image")]
	public class AnimatedTextureRect : SlicedTextureRect
	{
		private float _timeAccumulator;

		private float _frameDuration;

		private bool _playing;

		private int _animationFrame;

		private int[] _framesToUse;

		private int[] _animationFrames = new int[0];

		[Export(PropertyHint.Range, "0.1,300")]
		public float FPS { get; set; } = 4f;


		[Export(PropertyHint.None, "")]
		public bool Playing { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public int[] AnimationFrames
		{
			get
			{
				return _animationFrames;
			}
			set
			{
				_animationFrames = value;
				UpdateTextureSlices();
			}
		}

		public AnimatedTextureRect()
		{
			if (Engine.EditorHint)
			{
				Playing = false;
			}
		}

		public override void _Process(float delta)
		{
			if (!_playing || !Playing || !IsVisibleInTree())
			{
				return;
			}
			_timeAccumulator += delta;
			if (_timeAccumulator >= _frameDuration)
			{
				_timeAccumulator -= _frameDuration;
				if (++_animationFrame >= _framesToUse.Length)
				{
					_animationFrame = 0;
				}
				base.Frame = _framesToUse[_animationFrame];
			}
		}

		protected override void UpdateTextureSlices()
		{
			_playing = false;
			base.UpdateTextureSlices();
			if (!ready)
			{
				return;
			}
			if (AnimationFrames.IsNullOrEmpty())
			{
				_framesToUse = new int[base.Hframes * base.Vframes];
				for (int i = 0; i < _framesToUse.Length; i++)
				{
					_framesToUse[i] = i;
				}
			}
			else
			{
				_framesToUse = AnimationFrames;
			}
			_animationFrame = 0;
			_frameDuration = 1f / FPS;
			base.Frame = _framesToUse[_animationFrame];
			_playing = true;
		}
	}
}
