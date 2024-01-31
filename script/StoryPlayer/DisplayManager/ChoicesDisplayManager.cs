// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.ChoicesDisplayManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.UI;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  public class ChoicesDisplayManager
  {
    private Control nChoices;
    private Control nChoiceList;
    private LacieEngine.StoryPlayer.StoryPlayer storyPlayer;
    private List<string> choiceLabels;
    private List<int> choiceJumps;
    private int currentChoice;

    public bool HasChoices => this.choiceJumps.Count > 0;

    public bool Visible => this.nChoices.Visible;

    public ChoicesDisplayManager(LacieEngine.StoryPlayer.StoryPlayer storyPlayer, Control nChoices)
    {
      this.storyPlayer = storyPlayer;
      this.nChoices = nChoices;
      nChoices.Visible = false;
      this.choiceLabels = new List<string>();
      this.choiceJumps = new List<int>();
    }

    public void Show()
    {
      Frame frame = Game.Settings.UiProvider.MakeChoicesFrame();
      this.nChoiceList = UIUtil.CreateOptionList(this.choiceLabels);
      frame.AddToFrame((Node) this.nChoiceList);
      this.nChoices.AddChild((Node) frame);
      this.currentChoice = -1;
      this.nChoices.Visible = true;
    }

    public void ClearAndHide()
    {
      this.choiceLabels.Clear();
      this.choiceJumps.Clear();
      foreach (Node child in this.nChoices.GetChildren())
        child.Delete();
      this.nChoices.Visible = false;
    }

    public void AddChoice(string choiceText, int line)
    {
      this.choiceLabels.Add(choiceText);
      this.choiceJumps.Add(line);
    }

    public void HandleInput(InputEvent @event)
    {
      if (@event.IsActionPressed("input_up"))
      {
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
        if (--this.currentChoice <= -1)
          this.currentChoice = this.choiceJumps.Count - 1;
        UIUtil.HighlightItem(this.nChoiceList, this.currentChoice);
      }
      else if (@event.IsActionPressed("input_down"))
      {
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
        if (++this.currentChoice >= this.choiceJumps.Count)
          this.currentChoice = 0;
        UIUtil.HighlightItem(this.nChoiceList, this.currentChoice);
      }
      else
      {
        if (!@event.IsActionPressed("input_action") || this.currentChoice < 0)
          return;
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
        this.storyPlayer.SetNextBlockToLine(this.choiceJumps[this.currentChoice]);
        this.ClearAndHide();
        this.storyPlayer.Next();
      }
    }
  }
}
