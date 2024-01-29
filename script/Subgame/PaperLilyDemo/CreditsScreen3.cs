using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.Rooms
{
	public class CreditsScreen3 : SimpleScreen
	{
		[GetNode("Container/Section1")]
		private Control nPart1;

		[GetNode("Container/Section2")]
		private Control nPart2;

		[GetNode("Container/Section3")]
		private Control nPart3;

		[GetNode("Container/Section3/SeeYou")]
		private Control nPart4;

		public override void _Ready()
		{
			base._Ready();
			SetProcessInput(enable: false);
			Timers();
		}

		private async void Timers()
		{
			await this.DelaySeconds(4.5);
			ShowCredits2();
			await this.DelaySeconds(5.0);
			ShowCredits3();
			await this.DelaySeconds(4.0);
			ShowCredits4();
			await this.DelaySeconds(4.2);
			ShowCredits5();
		}

		private async void ShowCredits2()
		{
			Game.Animations.Play(new FadeOutAnimation(nPart1, 1f));
			await this.DelaySeconds(1.0);
			nPart2.Visible = true;
			Game.Animations.Play(new FadeInAnimation(nPart2, 1f));
		}

		private async void ShowCredits3()
		{
			Game.Animations.Play(new FadeOutAnimation(nPart2, 1f));
			await this.DelaySeconds(1.0);
			nPart3.Visible = true;
			Game.Animations.Play(new FadeInAnimation(nPart3, 1f));
		}

		private void ShowCredits4()
		{
			nPart4.Visible = true;
			Game.Animations.Play(new FadeInAnimation(nPart4, 1f));
		}

		private async void ShowCredits5()
		{
			Game.Animations.Play(new FadeOutAnimation(nPart3, 2f));
			await this.DelaySeconds(4.0);
			GoToNextScreen();
		}

		public override void GoToNextScreen()
		{
			Game.State.Party.Clear();
			Game.State.Party.Add("sai");
			Game.State.Event = "ch2_intro_from_demo";
			Game.State.EventLabel = "START";
			Game.State.LocationStr = "system.locations.ch1.event.ending";
			Game.State.LocationImg = "ch2_maze";
			Game.InputProcessor = Inputs.Processor.Menu;
			SaveScreenMenuContainer saveContainer = GDUtil.MakeNode<SaveScreenMenuContainer>("SaveContainer");
			saveContainer.OnClose = delegate
			{
				base.GoToNextScreen();
			};
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, saveContainer);
		}
	}
}
