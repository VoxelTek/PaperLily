// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerChoiceCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Translations;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerChoiceCommand : StoryPlayerCommand
  {
    public string Text { get; set; }

    public List<StoryPlayerChoiceCommand.Choice> Choices { get; set; } = new List<StoryPlayerChoiceCommand.Choice>();

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      List<StoryPlayerChoiceCommand.Choice> collection = new List<StoryPlayerChoiceCommand.Choice>();
      foreach (StoryPlayerChoiceCommand.Choice choice in this.Choices)
      {
        if (choice.Condition == null || choice.Condition.Evaluate())
          collection.Add(choice);
      }
      if (collection.IsEmpty<StoryPlayerChoiceCommand.Choice>())
      {
        Log.Trace((object) "Choice block: no choices are valid.");
        storyPlayer.SetNextBlockContinue();
        storyPlayer.Next();
      }
      else if (collection.Count == 1)
      {
        Log.Trace((object) "Choice block: only one choice is valid.");
        storyPlayer.SetNextBlockToLine(collection[0].GotoLine);
        storyPlayer.Next();
      }
      else
      {
        storyPlayer.UI.SetFramePosition();
        storyPlayer.UI.SetFrameType();
        storyPlayer.Text.SetText(this.Text);
        storyPlayer.UI.ShowDialogueBox();
        storyPlayer.Characters.HideAllCharacters();
        storyPlayer.Characters.ShowCharacter((string) null);
        foreach (StoryPlayerChoiceCommand.Choice choice in collection)
          storyPlayer.Choices.AddChoice(choice.ChoiceText, choice.GotoLine);
        storyPlayer.AdvanceDisabled = false;
      }
    }

    public override void FindCaptions(TranslatableMessages captions)
    {
      captions.Add(this.Text, this.ChoiceTextContext());
      foreach (StoryPlayerChoiceCommand.Choice choice in this.Choices)
        captions.Add(choice.ChoiceText, this.ChoiceOptionContext());
    }

    public override void OverrideCaptions(ICaptionSet captions)
    {
      string context1 = this.ChoiceTextContext();
      string context2 = this.ChoiceOptionContext();
      if (captions.ContainsKey(this.Text))
        this.Text = captions.Get(this.Text, context1);
      foreach (StoryPlayerChoiceCommand.Choice choice in this.Choices)
      {
        if (captions.ContainsKey(choice.ChoiceText))
          choice.ChoiceText = captions.Get(choice.ChoiceText, context2);
      }
    }

    private string ChoiceTextContext() => "events." + this.Event.Id + ".choice";

    private string ChoiceOptionContext() => "events." + this.Event.Id + ".choice." + this.Text;

    [Serializable]
    public class Choice
    {
      public string ChoiceText;
      public int GotoLine;
      public LogicStatement Condition;
    }
  }
}
