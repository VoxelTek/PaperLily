using System;
using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerFadeCommand : StoryPlayerCommand
	{
		public enum FadeType
		{
			FadeIn,
			FadeOut,
			Flash
		}

		private const float DEFAULT_FADE_DURATION = 0.25f;

		private readonly Color DEFAULT_FADE_COLOR = Colors.Black;

		public FadeType Type { get; set; }

		public float? Time { get; set; }

		public string Color { get; set; }

		public bool ContinueImmediately { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			ColorRect overlay = ((Type == FadeType.Flash) ? storyPlayer.nFlashOverlay : storyPlayer.nFadeOverlay);
			float time = Time ?? 0.25f;
			overlay.Color = ((!Color.IsNullOrEmpty()) ? GDUtil.StringToColor(Color) : DEFAULT_FADE_COLOR);
			storyPlayer.Characters.HideAllCharacters();
			storyPlayer.UI.HideDialogueBox();
			Game.Animations.StopAnimations(overlay);
			if (Type == FadeType.FadeOut)
			{
				ShowOverlay(overlay, time);
			}
			else if (Type == FadeType.FadeIn)
			{
				HideOverlay(overlay, time);
			}
			else if (Type == FadeType.Flash)
			{
				Flash(overlay, time);
			}
			storyPlayer.SetNextBlockContinue();
			if (ContinueImmediately || time <= 0f)
			{
				storyPlayer.Next();
			}
			else
			{
				storyPlayer.NextAfterSeconds(time);
			}
		}

		private void ShowOverlay(Control control, float time)
		{
			if (control.Modulate.a != 1f)
			{
				if (time > 0f)
				{
					Game.Animations.Play(new FadeInAnimation(control, time));
				}
				else
				{
					control.Modulate = Colors.White;
				}
			}
		}

		private void HideOverlay(Control control, float time)
		{
			if (control.Modulate.a != 0f)
			{
				if (time > 0f)
				{
					Game.Animations.Play(new FadeOutAnimation(control, time));
				}
				else
				{
					control.Modulate = Colors.Transparent;
				}
			}
		}

		private void Flash(Control control, float time)
		{
			control.Modulate = Colors.White;
			HideOverlay(control, time);
		}
	}
}
