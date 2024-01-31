// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayer
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
  public class StoryPlayer : Control
  {
    private static readonly Vector2 NamePlateSize = new Vector2(0.0f, 15f);
    private static readonly Vector2 NamePlateTextPosition = new Vector2(0.0f, -4f);
    private static readonly Vector2 NameSeparatorPosition = new Vector2(-15f, 9f);
    private static readonly Vector2 ContinueIndicatorSize = new Vector2(30f, 30f);
    private static readonly Vector2 ContinueIndicatorMargin = new Vector2(20f, 30f);
    public Control nFrame;
    public DialogueFrameNode nDialogueFrame;
    public Control nChoices;
    public Control nItemPopup;
    public AnimatedTextureRect nContinueIndicator2;
    public Control nCharLeft;
    public Control nCharRight;
    public TextureRect nCharLeftMain;
    public TextureRect nCharRightMain;
    public TextureRect nCharLeftAlt;
    public TextureRect nCharRightAlt;
    public Control nCg;
    public Control nCgAlt;
    public Control nTransitionOverlay;
    public ColorRect nFadeOverlay;
    public ColorRect nFlashOverlay;
    private StoryPlayerCommand currentBlock;
    private int currentIndentLv;
    private int nextStepNumber;
    public bool Event;
    private System.Collections.Generic.Queue<StoryPlayerEvent> callQueue;
    private Stack<System.Collections.Generic.Queue<StoryPlayerEvent>> callStackQueue;
    private Stack<StoryPlayerEvent> callStack;
    private Stack<int> callStackStepNum;
    public UiDisplayManager UI;
    public TextDisplayManager Text;
    public CharacterDisplayManager Characters;
    public ChoicesDisplayManager Choices;
    public CgDisplayManager CG;
    private const float _skipInterval = 0.08f;
    private float _skipAccumulator;
    private bool _skipWait;
    private static readonly string[] HandledInputs = new string[3]
    {
      "input_action",
      "input_up",
      "input_down"
    };

    public StoryPlayerEvent Dialogue { get; private set; }

    public bool Running { get; private set; }

    public bool AdvanceDisabled { get; set; }

    public override void _Ready()
    {
      this.Visible = false;
      this.nFrame = this.GetNode((NodePath) "Frame") as Control;
      this.nChoices = this.GetNode((NodePath) "Choices") as Control;
      this.nItemPopup = this.GetNode((NodePath) "ItemPopup") as Control;
      this.nCharLeft = this.GetNode((NodePath) "CharLeft") as Control;
      this.nCharRight = this.GetNode((NodePath) "CharRight") as Control;
      this.nCharLeftMain = this.GetNode((NodePath) "CharLeft/CharLeftMain") as TextureRect;
      this.nCharRightMain = this.GetNode((NodePath) "CharRight/CharRightMain") as TextureRect;
      this.nCharLeftAlt = this.GetNode((NodePath) "CharLeft/CharLeftAlt") as TextureRect;
      this.nCharRightAlt = this.GetNode((NodePath) "CharRight/CharRightAlt") as TextureRect;
      this.nCg = this.GetNode((NodePath) "CG") as Control;
      this.nCgAlt = this.GetNode((NodePath) "CGAlt") as Control;
      this.nTransitionOverlay = this.GetNode((NodePath) "TransitionOverlay") as Control;
      this.nFadeOverlay = this.GetNode((NodePath) "FadeOverlay") as ColorRect;
      this.nFlashOverlay = this.GetNode((NodePath) "FlashOverlay") as ColorRect;
      this.nDialogueFrame = Game.Settings.UiProvider.MakeDialogueBox();
      this.nFrame.AddChild((Node) this.nDialogueFrame);
      this.nContinueIndicator2 = this.MakeContinueIndicator();
      this.nContinueIndicator2.Visible = false;
      this.AddChild((Node) this.nContinueIndicator2);
      this.UI = new UiDisplayManager(this);
      this.Characters = new CharacterDisplayManager(this);
      this.Text = new TextDisplayManager(this);
      this.Choices = new ChoicesDisplayManager(this, this.nChoices);
      this.CG = new CgDisplayManager(this);
      this.AddChild((Node) this.Text);
    }

    private AnimatedTextureRect MakeContinueIndicator()
    {
      AnimatedTextureRect control = GDUtil.MakeNode<AnimatedTextureRect>("ContinueIndicator");
      control.Texture = GD.Load<Texture>("res://assets/img/ui/continue_indicator.png");
      control.Hframes = 3;
      control.FPS = 3f;
      control.AnimationFrames = new int[4]{ 0, 1, 2, 1 };
      control.Expand = true;
      control.RectMinSize = LacieEngine.StoryPlayer.StoryPlayer.ContinueIndicatorSize;
      control.SetAnchorsGrowFrom(1f, 1f, Direction.UpLeft);
      return control;
    }

    public override void _Input(InputEvent @event)
    {
      if (this.Dialogue == null)
        return;
      string str = Inputs.Handle(@event, Inputs.Processor.StoryPlayer, Inputs.AllUi);
      if (str == null)
        return;
      if (this.Choices.Visible)
      {
        this.Choices.HandleInput(@event);
      }
      else
      {
        if (!(str == "input_action") || this.AdvanceDisabled)
          return;
        this.Next();
      }
    }

    public override void _Process(float delta) => this.CheckSkipButton(delta);

    public void Play(StoryPlayerEvent evt)
    {
      if (this.Running)
      {
        this.Queue(evt);
      }
      else
      {
        this.Running = true;
        Log.Debug((object) "StoryPlayer starting: ", (object) evt.Id);
        this.callQueue = new System.Collections.Generic.Queue<StoryPlayerEvent>();
        this.callStack = new Stack<StoryPlayerEvent>();
        this.callStackStepNum = new Stack<int>();
        this.callStackQueue = new Stack<System.Collections.Generic.Queue<StoryPlayerEvent>>();
        Game.InputProcessor = Inputs.Processor.StoryPlayer;
        this.Text.Reset();
        this.Characters.Reset();
        this.CG.Reset();
        this.Visible = true;
        this.EmitSignal("DialogueStarted");
        this.Initiate(evt);
      }
    }

    public void EndDialogue()
    {
      if (this.Dialogue.AffectsPersistent)
        PersistentState.Save();
      if (this.callQueue.Count > 0)
        this.Initiate(this.callQueue.Dequeue());
      else if (this.callStack.Count > 0)
      {
        this.Dialogue = this.callStack.Pop();
        this.nextStepNumber = this.callStackStepNum.Pop();
        this.callQueue = this.callStackQueue.Pop();
        this.Next();
      }
      else
      {
        this.Running = false;
        this.Dialogue = (StoryPlayerEvent) null;
        this.UI.HideDialogueBox();
        this.Characters.HideAllCharacters();
        if (Game.InputProcessor == Inputs.Processor.StoryPlayer)
          Game.Core.AssignInputProcessor();
        Game.Room.UpdateRoomState();
        Game.Objectives.ShowNotification();
        this.EmitSignal("DialogueEnded");
        Log.Debug((object) "StoryPlayer has ended");
      }
    }

    public void ForceEndDialogue()
    {
      this.callQueue.Clear();
      this.callStack.Clear();
      this.EndDialogue();
    }

    public void CallEvent(string evt)
    {
      StoryPlayerEvent @event = Game.Events.GetEvent(evt);
      if (@event != null)
      {
        this.callStack.Push(this.Dialogue);
        this.callStackStepNum.Push(this.nextStepNumber);
        this.callStackQueue.Push(this.callQueue);
        this.callQueue = new System.Collections.Generic.Queue<StoryPlayerEvent>();
        this.Initiate(@event);
      }
      else
      {
        Log.Warn((object) "Attempt to call a non existing event: ", (object) evt);
        this.Next();
      }
    }

    public void Queue(StoryPlayerEvent @event)
    {
      Log.Debug((object) "Queuing event ", (object) @event.Id);
      this.callQueue.Enqueue(@event);
    }

    public void AddToCallQueue(string evt)
    {
      StoryPlayerEvent storyPlayerEvent = Game.Events.GetEvent(evt);
      if (storyPlayerEvent != null)
        this.callQueue.Enqueue(storyPlayerEvent);
      else
        Log.Warn((object) "Attempt to queue a non existing event: ", (object) evt);
    }

    private void Initiate(StoryPlayerEvent @event)
    {
      this.Dialogue = @event;
      this.nextStepNumber = 0;
      this.First();
    }

    private void First()
    {
      if (this.Dialogue.ShouldTrackRepeats)
        Game.Events.AddToEventInteractionCount(this.Dialogue.Id);
      this.UpdateDialogue();
    }

    public void Next()
    {
      if (this.Dialogue == null)
        return;
      this.AdvanceDisabled = true;
      if (this.Text.IsTextNode && !this.Text.IsShowingFullText)
        this.Text.AttemptShowFullText();
      else if (this.Text.IsTimedText)
      {
        this.HideAllUI();
      }
      else
      {
        if (this.Choices.HasChoices)
          return;
        this.nItemPopup.Clear();
        this.nItemPopup.Visible = false;
        if (this.nextStepNumber == -1)
        {
          this.EndDialogue();
        }
        else
        {
          this.nDialogueFrame.DialogueText.BbcodeText = "";
          this.nContinueIndicator2.Visible = false;
          this.Text.Cleanup();
          this.UpdateDialogue();
        }
      }
    }

    public void NextBypassTimed()
    {
      this.Text.IsTimedText = false;
      this.Next();
    }

    public void UpdateDialogue()
    {
      StoryPlayerCommand storyPlayerCommand = this.Dialogue.Dialogue[this.nextStepNumber];
      this.currentBlock = storyPlayerCommand;
      if (storyPlayerCommand.IndentLv != -1)
        this.currentIndentLv = storyPlayerCommand.IndentLv;
      if (!storyPlayerCommand.RawCommand.IsNullOrEmpty())
        Log.Debug((object) "Cmd: ", (object) storyPlayerCommand.RawCommand);
      storyPlayerCommand.Execute(this);
      this.Text.UpdateDialogueText();
    }

    public void HideAllUI()
    {
      this.Characters.HideAllCharacters();
      this.UI.HideDialogueBox();
    }

    public void SetNextBlockEnd() => this.nextStepNumber = -1;

    public void SetNextBlockContinue()
    {
      ++this.nextStepNumber;
      this.CheckCursorAndIndent();
    }

    public void SetNextBlockSkip()
    {
      this.nextStepNumber += 2;
      this.CheckCursorAndIndent();
    }

    private void CheckCursorAndIndent()
    {
      if (this.nextStepNumber >= this.Dialogue.Dialogue.Length)
      {
        this.SetNextBlockEnd();
      }
      else
      {
        if (this.Dialogue.Dialogue[this.nextStepNumber].IndentLv <= this.currentIndentLv)
          return;
        this.SetNextBlockContinue();
      }
    }

    public void SetNextBlockToLabel(string label)
    {
      for (int index = 0; index < this.Dialogue.Dialogue.Length; ++index)
      {
        if (this.Dialogue.Dialogue[index] is StoryPlayerLabelCommand playerLabelCommand && playerLabelCommand.Label == label)
        {
          this.nextStepNumber = index;
          return;
        }
      }
      Log.Error((object) "Label not found in dialogue: ", (object) label);
    }

    public void SetNextBlockToLine(int line)
    {
      if (this.nextStepNumber >= this.Dialogue.Dialogue.Length)
      {
        Log.Warn((object) "Goto Line: Index out of bounds: ", (object) line);
        this.SetNextBlockEnd();
      }
      this.nextStepNumber = line;
      this.currentIndentLv = this.Dialogue.Dialogue[line].IndentLv;
    }

    public void IncreaseIndent() => ++this.currentIndentLv;

    public async void NextAfterSeconds(float timer)
    {
      await DrkieUtil.DelaySeconds((double) timer);
      Game.StoryPlayer.CallDeferred("NextBypassTimed");
    }

    private void CheckSkipButton(float delta)
    {
      if (!Game.Settings.SkipEnabled)
        return;
      if (this._skipWait)
      {
        this._skipAccumulator += delta;
        if ((double) this._skipAccumulator >= 0.079999998211860657)
          this._skipWait = false;
      }
      if (Game.InputProcessor != Inputs.Processor.StoryPlayer || this.Choices.Visible || this.AdvanceDisabled || this._skipWait)
        return;
      if (Inputs.IsActionPressed("input_cancel"))
        this.Next();
      this._skipAccumulator = 0.0f;
      this._skipWait = true;
    }

    [Signal]
    public delegate void DialogueStarted();

    [Signal]
    public delegate void DialogueEnded();
  }
}
