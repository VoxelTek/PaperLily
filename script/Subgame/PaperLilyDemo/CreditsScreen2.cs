using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.Rooms
{
	public class CreditsScreen2 : SimpleScreen
	{
		public override void _Ready()
		{
			base._Ready();
			SetProcessInput(enable: false);
			Timers();
		}

		private async void Timers()
		{
			await this.DelaySeconds(1.3);
			Game.Animations.Play(new PanAnimationControl(this, Vector2.Zero, Vector2.Up * (base.RectSize.y - 360f), 20.5f));
			await this.DelaySeconds(25.0);
			GoToNextScreen();
		}
	}
}
