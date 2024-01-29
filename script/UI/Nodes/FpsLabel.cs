using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class FpsLabel : Label
	{
		public override void _EnterTree()
		{
			base.Name = "FPS";
			this.SetFontColor(Colors.White);
			this.SetFontShadowColor(Colors.Black);
		}

		public override void _Process(float delta)
		{
			base.Text = Engine.GetFramesPerSecond() + " FPS";
		}
	}
}
