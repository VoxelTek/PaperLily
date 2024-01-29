using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class SimpleScreen : Control
	{
		private const float DEFAULT_WAIT_TIME = 4f;

		[Export(PropertyHint.None, "")]
		public float WaitTime = 4f;

		[Export(PropertyHint.None, "")]
		public bool NoSkip;

		[Export(PropertyHint.None, "")]
		public bool KeepState;

		[Export(PropertyHint.None, "")]
		public PackedScene NextScreen;

		private string[] HandledInputs = new string[1] { "input_action" };

		private Timer nTimer;

		public override void _EnterTree()
		{
			if (WaitTime > 0f)
			{
				nTimer = GDUtil.MakeNode<Timer>("Timer");
				nTimer.WaitTime = WaitTime;
				nTimer.OneShot = true;
				nTimer.Connect("timeout", this, "GoToNextScreen");
				AddChild(nTimer);
			}
		}

		public override void _Ready()
		{
			if (WaitTime > 0f)
			{
				nTimer.Start();
			}
		}

		public override void _Input(InputEvent @event)
		{
			if (!NoSkip && Inputs.Handle(@event, Inputs.Processor.Menu, HandledInputs) == "input_action")
			{
				nTimer?.Stop();
				GoToNextScreen();
			}
		}

		public virtual void GoToNextScreen()
		{
			if (NextScreen == null)
			{
				Log.Error("Next screen not set in ", base.Name);
			}
			else
			{
				Game.Core.SwitchToScreen(NextScreen, KeepState);
			}
		}
	}
}
