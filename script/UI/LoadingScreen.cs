using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class LoadingScreen : Control
	{
		private AnimatedTextureRect nLoadingIndicator;

		public bool Instant { get; set; }

		public override void _EnterTree()
		{
			nLoadingIndicator = GetNode<AnimatedTextureRect>("LoadingIndicatorContainer/LoadingIndicator");
		}

		public override void _Ready()
		{
			ShowLoadingIndicator();
		}

		public async void ShowLoadingIndicator()
		{
			if (!Instant)
			{
				await DrkieUtil.DelaySeconds(0.5);
			}
			if (nLoadingIndicator.IsValid())
			{
				nLoadingIndicator.Visible = true;
				nLoadingIndicator.Frame = 0;
				nLoadingIndicator.Playing = true;
			}
		}
	}
}
