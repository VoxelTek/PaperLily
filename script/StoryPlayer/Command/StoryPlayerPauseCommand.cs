// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerPauseCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerPauseCommand : StoryPlayerCommand
  {
    public StoryPlayerPauseCommand.PauseType Type { get; set; }

    public float Time { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      if (this.Type == StoryPlayerPauseCommand.PauseType.Wait)
      {
        storyPlayer.UI.HideDialogueBox();
        storyPlayer.nContinueIndicator2.Visible = true;
        storyPlayer.SetNextBlockContinue();
        storyPlayer.AdvanceDisabled = false;
      }
      else
      {
        if (this.Type != StoryPlayerPauseCommand.PauseType.Pause)
          return;
        storyPlayer.SetNextBlockContinue();
        storyPlayer.NextAfterSeconds(this.Time);
      }
    }

    public enum PauseType
    {
      Pause,
      Wait,
    }
  }
}
