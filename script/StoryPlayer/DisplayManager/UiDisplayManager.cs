using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.StoryPlayer
{
	public class UiDisplayManager
	{
		public enum FramePosition
		{
			None,
			Bottom,
			Center
		}

		public enum FrameType
		{
			Normal,
			NoBackground,
			Blur
		}

		private const int DistanceFromBottom = 10;

		private static readonly Color HiddenColor = UIUtil.Transparent;

		private static readonly float AnimationDisplacement = 50f;

		private FramePosition currentFramePos;

		private FrameType currentFrameType;

		private Vector2 visiblePos;

		private StoryPlayer storyPlayer;

		private Material blurMat;

		public bool Visible { get; private set; }

		public UiDisplayManager(StoryPlayer storyPlayer)
		{
			this.storyPlayer = storyPlayer;
			blurMat = GD.Load<Material>("res://resources/material/screen_blur.tres");
			HideUi();
		}

		public void SetFramePosition(FramePosition pos = FramePosition.Bottom)
		{
			if (currentFramePos != pos)
			{
				currentFramePos = pos;
				switch (pos)
				{
				case FramePosition.Bottom:
					visiblePos = new Vector2((Game.Settings.BaseResolution.x - storyPlayer.nFrame.RectSize.x) / 2f, Game.Settings.BaseResolution.y - storyPlayer.nFrame.RectSize.y - 10f);
					break;
				case FramePosition.Center:
					visiblePos = new Vector2((Game.Settings.BaseResolution.x - storyPlayer.nFrame.RectSize.x) / 2f, (Game.Settings.BaseResolution.y - storyPlayer.nFrame.RectSize.y - 10f) / 2f);
					break;
				}
				HideUi();
			}
		}

		public void SetFrameType(FrameType type = FrameType.Normal)
		{
			if (currentFrameType != type)
			{
				switch (type)
				{
				case FrameType.Normal:
					storyPlayer.nDialogueFrame.Background.Modulate = Colors.White;
					storyPlayer.nDialogueFrame.Background.Material = null;
					storyPlayer.nDialogueFrame.NameSeparator.Modulate = Colors.White;
					storyPlayer.nDialogueFrame.ContinueIndicator.SetAnchorsAndMarginsPreset(Control.LayoutPreset.BottomRight);
					currentFrameType = FrameType.Normal;
					break;
				case FrameType.NoBackground:
					storyPlayer.nDialogueFrame.Background.Modulate = Colors.Transparent;
					storyPlayer.nDialogueFrame.Background.Material = null;
					storyPlayer.nDialogueFrame.NameSeparator.Modulate = Colors.Transparent;
					storyPlayer.nDialogueFrame.ContinueIndicator.SetAnchorsAndMarginsPreset(Control.LayoutPreset.CenterBottom);
					currentFrameType = FrameType.NoBackground;
					break;
				case FrameType.Blur:
					storyPlayer.nDialogueFrame.Background.Modulate = new Color("#606060");
					storyPlayer.nDialogueFrame.Background.Material = blurMat;
					storyPlayer.nDialogueFrame.NameSeparator.Modulate = Colors.Transparent;
					storyPlayer.nDialogueFrame.ContinueIndicator.SetAnchorsAndMarginsPreset(Control.LayoutPreset.CenterBottom);
					currentFrameType = FrameType.Blur;
					break;
				}
			}
		}

		public void ShowDialogueBox()
		{
			AnimateUiIn();
			Visible = true;
		}

		public void HideDialogueBox()
		{
			storyPlayer.nDialogueFrame.ContinueIndicator.Visible = false;
			AnimateUiOut();
			Visible = false;
		}

		private void AnimateUiIn()
		{
			if (!Visible)
			{
				Game.Animations.Play(new SlideInBottomAnimation(storyPlayer.nFrame, AnimationDisplacement, visiblePos));
			}
		}

		private void AnimateUiOut()
		{
			if (Visible)
			{
				Game.Animations.Play(new SlideOutBottomAnimation(storyPlayer.nFrame, AnimationDisplacement, visiblePos));
			}
		}

		private void HideUi()
		{
			Game.Animations.StopAnimations(storyPlayer.nFrame);
			storyPlayer.nFrame.Modulate = HiddenColor;
			Visible = false;
		}
	}
}
