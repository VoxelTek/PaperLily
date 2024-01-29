using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.Rooms
{
	public class CreditsScreen1 : SimpleScreen
	{
		[GetNode("EndingLabel")]
		private Control nEndingLabel;

		[GetNode("Part1")]
		private Control nPart1;

		[GetNode("Part2")]
		private Control nPart2;

		[GetNode("Part3")]
		private Control nPart3;

		[GetNode("Part4")]
		private Control nPart4;

		private PEvent evtPostCredits = "ch1_postcredits";

		public override void _Ready()
		{
			base._Ready();
			SetProcessInput(enable: false);
			Timers();
		}

		private async void Timers()
		{
			await this.DelaySeconds(2.0);
			ShowEnding();
			await this.DelaySeconds(2.5);
			HideEnding();
			ShowCredits2();
			await this.DelaySeconds(5.0);
			ShowCredits3();
			await this.DelaySeconds(5.5);
			ShowCredits4();
			await this.DelaySeconds(6.0);
			PlayPostCredits();
		}

		private void ShowEnding()
		{
			nEndingLabel.Visible = true;
			Game.Animations.Play(new FadeInAnimation(nEndingLabel, 1f));
		}

		private void HideEnding()
		{
			Game.Animations.Play(new FadeOutAnimation(nEndingLabel, 1f));
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

		private async void ShowCredits4()
		{
			Game.Animations.Play(new FadeOutAnimation(nPart3, 1f));
			await this.DelaySeconds(1.0);
			nPart4.Visible = true;
			Game.Animations.Play(new FadeInAnimation(nPart4, 1f));
		}

		private async void PlayPostCredits()
		{
			await Game.Screen.FadeToBlack();
			this.Delete();
			evtPostCredits.Play();
			Game.Screen.FadeFromBlack();
		}
	}
}
