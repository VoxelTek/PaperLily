using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class PauseScreen : CenterContainer
	{
		private static readonly string[] ProcessedInputs = new string[1] { "input_menu" };

		public PauseScreen()
		{
			base.PauseMode = PauseModeEnum.Process;
		}

		public override void _EnterTree()
		{
			ColorRect darkener = UIUtil.CreateDarkenerOverlay();
			SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			Control frame = UIUtil.CreateInfoBox("system.menu.paused");
			AddChild(darkener);
			AddChild(frame);
		}

		public override void _Process(float delta)
		{
			if (!GetTree().Paused)
			{
				this.Delete();
			}
		}

		public override void _Input(InputEvent @event)
		{
			string input = Inputs.Handle(@event, Inputs.Processor.Any, ProcessedInputs);
			if (input != null && input == "input_menu")
			{
				GetTree().Paused = false;
			}
		}
	}
}
