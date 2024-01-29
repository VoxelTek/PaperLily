using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	public class EventTimer : Node
	{
		private Action _callback;

		private bool _visible = true;

		private Label nLabel;

		private Timer nTimer;

		public bool SelfDestructWhenFinished { get; set; }

		public bool Visible
		{
			get
			{
				if (!nLabel.IsValid())
				{
					return _visible;
				}
				return nLabel.Visible;
			}
			set
			{
				if (nLabel.IsValid())
				{
					nLabel.Visible = value;
				}
				else
				{
					_visible = value;
				}
			}
		}

		public override void _EnterTree()
		{
			base.Name = "Timer";
			nLabel = GDUtil.MakeNode<Label>("TimerLabel");
			nLabel.SetDefaultFontAndColor();
			nLabel.RectPosition = new Vector2(300f, 5f);
			nLabel.Visible = _visible;
			Game.Screen.AddToLayer(IScreenManager.Layer.HUD, nLabel);
		}

		public override void _ExitTree()
		{
			nTimer.DeleteIfValid();
			nLabel.DeleteIfValid();
		}

		public override void _Process(float delta)
		{
			if (nTimer.IsValid())
			{
				nLabel.Text = ((int)(nTimer.TimeLeft / 60f)).ToString().PadLeft(2, '0') + ":" + ((int)(nTimer.TimeLeft % 60f)).ToString().PadLeft(2, '0');
			}
		}

		public void StartTimer(float seconds, Action timeoutCallback)
		{
			nTimer.DeleteIfValid();
			nTimer = GDUtil.MakeNode<Timer>("ActualTimer");
			nTimer.Autostart = true;
			nTimer.WaitTime = seconds;
			nTimer.OneShot = true;
			nTimer.Connect("timeout", this, "Timeout");
			_callback = timeoutCallback;
			AddChild(nTimer);
		}

		public void StopTimer()
		{
			nTimer.DeleteIfValid();
			_callback = null;
		}

		public void PauseTimer()
		{
			if (!nTimer.IsValid())
			{
				Log.Error("Timer needs to be started first!");
			}
			else
			{
				nTimer.Paused = true;
			}
		}

		public void ResumeTimer()
		{
			if (!nTimer.IsValid())
			{
				Log.Error("Timer needs to be started first!");
			}
			else
			{
				nTimer.Paused = false;
			}
		}

		public void IncreaseTime(float seconds)
		{
			if (!nTimer.IsValid())
			{
				Log.Error("Timer needs to be started first!");
			}
			else
			{
				nTimer.Stop();
				nTimer.WaitTime += seconds;
				nTimer.Start();
			}
		}

		public void DecreaseTime(float seconds)
		{
			if (!nTimer.IsValid())
			{
				Log.Error("Timer needs to be started first!");
				return;
			}
			nTimer.Stop();
			if (nTimer.WaitTime - seconds > 0f)
			{
				nTimer.WaitTime -= seconds;
				nTimer.Start();
			}
			else
			{
				Timeout();
			}
		}

		public void Timeout()
		{
			_callback?.Invoke();
			if (SelfDestructWhenFinished)
			{
				this.DeleteIfValid();
			}
		}
	}
}
