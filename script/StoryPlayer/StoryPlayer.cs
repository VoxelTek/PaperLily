using System.Collections.Generic;
using Godot;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.StoryPlayer
{
	public class StoryPlayer : Control
	{
		[Signal]
		public delegate void DialogueStarted();

		[Signal]
		public delegate void DialogueEnded();

		private static readonly Vector2 NamePlateSize = new Vector2(0f, 15f);

		private static readonly Vector2 NamePlateTextPosition = new Vector2(0f, -4f);

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

		private Queue<StoryPlayerEvent> callQueue;

		private Stack<Queue<StoryPlayerEvent>> callStackQueue;

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

		private static readonly string[] HandledInputs = new string[3] { "input_action", "input_up", "input_down" };

		public StoryPlayerEvent Dialogue { get; private set; }

		public bool Running { get; private set; }

		public bool AdvanceDisabled { get; set; }

		public override void _Ready()
		{
			base.Visible = false;
			nFrame = GetNode("Frame") as Control;
			nChoices = GetNode("Choices") as Control;
			nItemPopup = GetNode("ItemPopup") as Control;
			nCharLeft = GetNode("CharLeft") as Control;
			nCharRight = GetNode("CharRight") as Control;
			nCharLeftMain = GetNode("CharLeft/CharLeftMain") as TextureRect;
			nCharRightMain = GetNode("CharRight/CharRightMain") as TextureRect;
			nCharLeftAlt = GetNode("CharLeft/CharLeftAlt") as TextureRect;
			nCharRightAlt = GetNode("CharRight/CharRightAlt") as TextureRect;
			nCg = GetNode("CG") as Control;
			nCgAlt = GetNode("CGAlt") as Control;
			nTransitionOverlay = GetNode("TransitionOverlay") as Control;
			nFadeOverlay = GetNode("FadeOverlay") as ColorRect;
			nFlashOverlay = GetNode("FlashOverlay") as ColorRect;
			nDialogueFrame = Game.Settings.UiProvider.MakeDialogueBox();
			nFrame.AddChild(nDialogueFrame);
			nContinueIndicator2 = MakeContinueIndicator();
			nContinueIndicator2.Visible = false;
			AddChild(nContinueIndicator2);
			UI = new UiDisplayManager(this);
			Characters = new CharacterDisplayManager(this);
			Text = new TextDisplayManager(this);
			Choices = new ChoicesDisplayManager(this, nChoices);
			CG = new CgDisplayManager(this);
			AddChild(Text);
		}

		private AnimatedTextureRect MakeContinueIndicator()
		{
			AnimatedTextureRect continueIndicator = GDUtil.MakeNode<AnimatedTextureRect>("ContinueIndicator");
			continueIndicator.Texture = GD.Load<Texture>("res://assets/img/ui/continue_indicator.png");
			continueIndicator.Hframes = 3;
			continueIndicator.FPS = 3f;
			continueIndicator.AnimationFrames = new int[4] { 0, 1, 2, 1 };
			continueIndicator.Expand = true;
			continueIndicator.RectMinSize = ContinueIndicatorSize;
			continueIndicator.SetAnchorsGrowFrom(1f, 1f, Direction.UpLeft);
			return continueIndicator;
		}

		public override void _Input(InputEvent @event)
		{
			if (Dialogue == null)
			{
				return;
			}
			string input = Inputs.Handle(@event, Inputs.Processor.StoryPlayer, Inputs.AllUi);
			if (input != null)
			{
				if (Choices.Visible)
				{
					Choices.HandleInput(@event);
				}
				else if (input == "input_action" && !AdvanceDisabled)
				{
					Next();
				}
			}
		}

		public override void _Process(float delta)
		{
			CheckSkipButton(delta);
		}

		public void Play(StoryPlayerEvent evt)
		{
			if (Running)
			{
				Queue(evt);
				return;
			}
			Running = true;
			Log.Debug("StoryPlayer starting: ", evt.Id);
			callQueue = new Queue<StoryPlayerEvent>();
			callStack = new Stack<StoryPlayerEvent>();
			callStackStepNum = new Stack<int>();
			callStackQueue = new Stack<Queue<StoryPlayerEvent>>();
			Game.InputProcessor = Inputs.Processor.StoryPlayer;
			Text.Reset();
			Characters.Reset();
			CG.Reset();
			base.Visible = true;
			EmitSignal("DialogueStarted");
			Initiate(evt);
		}

		public void EndDialogue()
		{
			if (Dialogue.AffectsPersistent)
			{
				PersistentState.Save();
			}
			if (callQueue.Count > 0)
			{
				StoryPlayerEvent evt = callQueue.Dequeue();
				Initiate(evt);
				return;
			}
			if (callStack.Count > 0)
			{
				Dialogue = callStack.Pop();
				nextStepNumber = callStackStepNum.Pop();
				callQueue = callStackQueue.Pop();
				Next();
				return;
			}
			Running = false;
			Dialogue = null;
			UI.HideDialogueBox();
			Characters.HideAllCharacters();
			if (Game.InputProcessor == Inputs.Processor.StoryPlayer)
			{
				Game.Core.AssignInputProcessor();
			}
			Game.Room.UpdateRoomState();
			Game.Objectives.ShowNotification();
			EmitSignal("DialogueEnded");
			Log.Debug("StoryPlayer has ended");
		}

		public void ForceEndDialogue()
		{
			callQueue.Clear();
			callStack.Clear();
			EndDialogue();
		}

		public void CallEvent(string evt)
		{
			StoryPlayerEvent @event = Game.Events.GetEvent(evt);
			if (@event != null)
			{
				callStack.Push(Dialogue);
				callStackStepNum.Push(nextStepNumber);
				callStackQueue.Push(callQueue);
				callQueue = new Queue<StoryPlayerEvent>();
				Initiate(@event);
			}
			else
			{
				Log.Warn("Attempt to call a non existing event: ", evt);
				Next();
			}
		}

		public void Queue(StoryPlayerEvent @event)
		{
			Log.Debug("Queuing event ", @event.Id);
			callQueue.Enqueue(@event);
		}

		public void AddToCallQueue(string evt)
		{
			StoryPlayerEvent @event = Game.Events.GetEvent(evt);
			if (@event != null)
			{
				callQueue.Enqueue(@event);
				return;
			}
			Log.Warn("Attempt to queue a non existing event: ", evt);
		}

		private void Initiate(StoryPlayerEvent @event)
		{
			Dialogue = @event;
			nextStepNumber = 0;
			First();
		}

		private void First()
		{
			if (Dialogue.ShouldTrackRepeats)
			{
				Game.Events.AddToEventInteractionCount(Dialogue.Id);
			}
			UpdateDialogue();
		}

		public void Next()
		{
			if (Dialogue == null)
			{
				return;
			}
			AdvanceDisabled = true;
			if (Text.IsTextNode && !Text.IsShowingFullText)
			{
				Text.AttemptShowFullText();
			}
			else if (Text.IsTimedText)
			{
				HideAllUI();
			}
			else if (!Choices.HasChoices)
			{
				nItemPopup.Clear();
				nItemPopup.Visible = false;
				if (nextStepNumber == -1)
				{
					EndDialogue();
					return;
				}
				nDialogueFrame.DialogueText.BbcodeText = "";
				nContinueIndicator2.Visible = false;
				Text.Cleanup();
				UpdateDialogue();
			}
		}

		public void NextBypassTimed()
		{
			Text.IsTimedText = false;
			Next();
		}

		public void UpdateDialogue()
		{
			StoryPlayerCommand block = (currentBlock = Dialogue.Dialogue[nextStepNumber]);
			if (block.IndentLv != -1)
			{
				currentIndentLv = block.IndentLv;
			}
			if (!block.RawCommand.IsNullOrEmpty())
			{
				Log.Debug("Cmd: ", block.RawCommand);
			}
			block.Execute(this);
			Text.UpdateDialogueText();
		}

		public void HideAllUI()
		{
			Characters.HideAllCharacters();
			UI.HideDialogueBox();
		}

		public void SetNextBlockEnd()
		{
			nextStepNumber = -1;
		}

		public void SetNextBlockContinue()
		{
			nextStepNumber++;
			CheckCursorAndIndent();
		}

		public void SetNextBlockSkip()
		{
			nextStepNumber += 2;
			CheckCursorAndIndent();
		}

		private void CheckCursorAndIndent()
		{
			if (nextStepNumber >= Dialogue.Dialogue.Length)
			{
				SetNextBlockEnd();
			}
			else if (Dialogue.Dialogue[nextStepNumber].IndentLv > currentIndentLv)
			{
				SetNextBlockContinue();
			}
		}

		public void SetNextBlockToLabel(string label)
		{
			for (int i = 0; i < Dialogue.Dialogue.Length; i++)
			{
				if (Dialogue.Dialogue[i] is StoryPlayerLabelCommand lblBlock && lblBlock.Label == label)
				{
					nextStepNumber = i;
					return;
				}
			}
			Log.Error("Label not found in dialogue: ", label);
		}

		public void SetNextBlockToLine(int line)
		{
			if (nextStepNumber >= Dialogue.Dialogue.Length)
			{
				Log.Warn("Goto Line: Index out of bounds: ", line);
				SetNextBlockEnd();
			}
			nextStepNumber = line;
			currentIndentLv = Dialogue.Dialogue[line].IndentLv;
		}

		public void IncreaseIndent()
		{
			currentIndentLv++;
		}

		public async void NextAfterSeconds(float timer)
		{
			await DrkieUtil.DelaySeconds(timer);
			Game.StoryPlayer.CallDeferred("NextBypassTimed");
		}

		private void CheckSkipButton(float delta)
		{
			if (!Game.Settings.SkipEnabled)
			{
				return;
			}
			if (_skipWait)
			{
				_skipAccumulator += delta;
				if (_skipAccumulator >= 0.08f)
				{
					_skipWait = false;
				}
			}
			if (Game.InputProcessor == Inputs.Processor.StoryPlayer && !Choices.Visible && !AdvanceDisabled && !_skipWait)
			{
				if (Inputs.IsActionPressed("input_cancel"))
				{
					Next();
				}
				_skipAccumulator = 0f;
				_skipWait = true;
			}
		}
	}
}
