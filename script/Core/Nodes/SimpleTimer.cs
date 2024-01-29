using System;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[ExportType(icon = "stopwatch")]
	public class SimpleTimer : Node
	{
		private bool _started;

		private float _curTimeLeft;

		[Export(PropertyHint.None, "")]
		public float WaitTime { get; set; }

		[Export(PropertyHint.None, "")]
		public bool OneShot { get; set; }

		[Export(PropertyHint.None, "")]
		public bool Autostart { get; set; }

		public Action Callback { get; set; } = delegate
		{
			Log.Warn("No callback specified for timer!");
		};


		public override void _Ready()
		{
			if (Autostart)
			{
				Start();
			}
		}

		public override void _Process(float delta)
		{
			if (!_started)
			{
				return;
			}
			_curTimeLeft -= delta;
			if (_curTimeLeft <= 0f)
			{
				Callback();
				_curTimeLeft = WaitTime;
				if (OneShot)
				{
					_started = false;
				}
			}
		}

		public void Start()
		{
			_curTimeLeft = WaitTime;
			_started = true;
		}

		public void Stop()
		{
			_started = false;
			_curTimeLeft = WaitTime;
		}
	}
}
