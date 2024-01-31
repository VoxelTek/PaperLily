// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.UiDisplayManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  public class UiDisplayManager
  {
    private const int DistanceFromBottom = 10;
    private static readonly Color HiddenColor = UIUtil.Transparent;
    private static readonly float AnimationDisplacement = 50f;
    private UiDisplayManager.FramePosition currentFramePos;
    private UiDisplayManager.FrameType currentFrameType;
    private Vector2 visiblePos;
    private LacieEngine.StoryPlayer.StoryPlayer storyPlayer;
    private Material blurMat;

    public bool Visible { get; private set; }

    public UiDisplayManager(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      this.storyPlayer = storyPlayer;
      this.blurMat = GD.Load<Material>("res://resources/material/screen_blur.tres");
      this.HideUi();
    }

    public void SetFramePosition(UiDisplayManager.FramePosition pos = UiDisplayManager.FramePosition.Bottom)
    {
      if (this.currentFramePos == pos)
        return;
      this.currentFramePos = pos;
      switch (pos)
      {
        case UiDisplayManager.FramePosition.Bottom:
          this.visiblePos = new Vector2((float) (((double) Game.Settings.BaseResolution.x - (double) this.storyPlayer.nFrame.RectSize.x) / 2.0), (float) ((double) Game.Settings.BaseResolution.y - (double) this.storyPlayer.nFrame.RectSize.y - 10.0));
          break;
        case UiDisplayManager.FramePosition.Center:
          this.visiblePos = new Vector2((float) (((double) Game.Settings.BaseResolution.x - (double) this.storyPlayer.nFrame.RectSize.x) / 2.0), (float) (((double) Game.Settings.BaseResolution.y - (double) this.storyPlayer.nFrame.RectSize.y - 10.0) / 2.0));
          break;
      }
      this.HideUi();
    }

    public void SetFrameType(UiDisplayManager.FrameType type = UiDisplayManager.FrameType.Normal)
    {
      if (this.currentFrameType == type)
        return;
      switch (type)
      {
        case UiDisplayManager.FrameType.Normal:
          this.storyPlayer.nDialogueFrame.Background.Modulate = Godot.Colors.White;
          this.storyPlayer.nDialogueFrame.Background.Material = (Material) null;
          this.storyPlayer.nDialogueFrame.NameSeparator.Modulate = Godot.Colors.White;
          this.storyPlayer.nDialogueFrame.ContinueIndicator.SetAnchorsAndMarginsPreset(Control.LayoutPreset.BottomRight);
          this.currentFrameType = UiDisplayManager.FrameType.Normal;
          break;
        case UiDisplayManager.FrameType.NoBackground:
          this.storyPlayer.nDialogueFrame.Background.Modulate = Godot.Colors.Transparent;
          this.storyPlayer.nDialogueFrame.Background.Material = (Material) null;
          this.storyPlayer.nDialogueFrame.NameSeparator.Modulate = Godot.Colors.Transparent;
          this.storyPlayer.nDialogueFrame.ContinueIndicator.SetAnchorsAndMarginsPreset(Control.LayoutPreset.CenterBottom);
          this.currentFrameType = UiDisplayManager.FrameType.NoBackground;
          break;
        case UiDisplayManager.FrameType.Blur:
          this.storyPlayer.nDialogueFrame.Background.Modulate = new Color("#606060");
          this.storyPlayer.nDialogueFrame.Background.Material = this.blurMat;
          this.storyPlayer.nDialogueFrame.NameSeparator.Modulate = Godot.Colors.Transparent;
          this.storyPlayer.nDialogueFrame.ContinueIndicator.SetAnchorsAndMarginsPreset(Control.LayoutPreset.CenterBottom);
          this.currentFrameType = UiDisplayManager.FrameType.Blur;
          break;
      }
    }

    public void ShowDialogueBox()
    {
      this.AnimateUiIn();
      this.Visible = true;
    }

    public void HideDialogueBox()
    {
      this.storyPlayer.nDialogueFrame.ContinueIndicator.Visible = false;
      this.AnimateUiOut();
      this.Visible = false;
    }

    private void AnimateUiIn()
    {
      if (this.Visible)
        return;
      Game.Animations.Play((LacieAnimation) new SlideInBottomAnimation(this.storyPlayer.nFrame, new float?(UiDisplayManager.AnimationDisplacement), new Vector2?(this.visiblePos)));
    }

    private void AnimateUiOut()
    {
      if (!this.Visible)
        return;
      Game.Animations.Play((LacieAnimation) new SlideOutBottomAnimation(this.storyPlayer.nFrame, new float?(UiDisplayManager.AnimationDisplacement), new Vector2?(this.visiblePos)));
    }

    private void HideUi()
    {
      Game.Animations.StopAnimations((Node) this.storyPlayer.nFrame);
      this.storyPlayer.nFrame.Modulate = UiDisplayManager.HiddenColor;
      this.Visible = false;
    }

    public enum FramePosition
    {
      None,
      Bottom,
      Center,
    }

    public enum FrameType
    {
      Normal,
      NoBackground,
      Blur,
    }
  }
}
