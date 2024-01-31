// Decompiled with JetBrains decompiler
// Type: LacieEngine.Modules.Tutorials.StoryPlayerTutorialCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.StoryPlayer;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Modules.Tutorials
{
  [Serializable]
  public class StoryPlayerTutorialCommand : StoryPlayerCommand
  {
    [Inject]
    [NonSerialized]
    private ITutorialManager Tutorials;

    public string Value { get; set; }

    public string SaveEventLabel { get; set; }

    public string SaveLocation { get; set; }

    public string SaveImage { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      if (!Game.State.SeenTutorials.Contains(this.Value))
      {
        storyPlayer.Characters.HideAllCharacters();
        storyPlayer.UI.HideDialogueBox();
        this.Tutorials.ShowTutorial(this.Value);
        this.Tutorials.TutorialEnded += new Action(this.ResumeAfterTutorial);
        storyPlayer.SetNextBlockContinue();
      }
      else
      {
        storyPlayer.SetNextBlockContinue();
        storyPlayer.Next();
      }
    }

    public override void Load() => this.Tutorials.LoadResourcesForTutorial(this.Value);

    public override IList<string> GetDependencies() => this.Tutorials.GetDependencies(this.Value);

    private void ResumeAfterTutorial()
    {
      this.Tutorials.TutorialEnded -= new Action(this.ResumeAfterTutorial);
      Game.InputProcessor = Inputs.Processor.StoryPlayer;
      Game.StoryPlayer.Next();
    }
  }
}
