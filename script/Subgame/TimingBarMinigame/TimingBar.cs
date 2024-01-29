using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.Minigames
{
	internal class TimingBar : MarginContainer, IMinigame
	{
		private const int TimingBarHeight = 59;

		private const int PlayerBasicOffset = 54;

		private const int GoodBarOffsetY = 15;

		private const int GoodBarHeight = 29;

		private const int ProgressBarOffsetX = 7;

		private const int ProgressBarOffsetY = 8;

		private const int AttemptsSeparation = -5;

		private const int AttemptsOffsetX = 10;

		private const int AttemptsOffsetY = -20;

		private const string TextureBgLeft = "res://assets/sprite/common/minigame/timingbar/bg_left.png";

		private const string TextureBgProgress = "res://assets/sprite/common/minigame/timingbar/bg_progress.png";

		private const string TextureBgStretch = "res://assets/sprite/common/minigame/timingbar/bg_stretch.png";

		private const string TextureBgRight = "res://assets/sprite/common/minigame/timingbar/bg_right.png";

		private const string TexturePlayer = "res://assets/sprite/common/minigame/timingbar/player.png";

		private const string TextureAttempt = "res://assets/sprite/common/minigame/timingbar/attempt.png";

		private const string TextureAttemptUsed = "res://assets/sprite/common/minigame/timingbar/attempt_used.png";

		private static readonly Color GoodBarColor = new Color(0.478f, 0.882f, 0.51f);

		private static readonly string[] HandledInputs = new string[2] { "input_action", "debug_key" };

		private TextureProgress nProgress;

		private ColorRect nGoodBar;

		private TextureRect nPlayer;

		private Control nBar;

		private TextureRect[] attemptIndicators;

		private int rangeOffset;

		private int arrowOffset;

		private double timer;

		private int currentSegment;

		private float hitZoneFrom;

		private float hitZoneTo;

		private float speed;

		private int attemptsLeft;

		private Random random = new Random();

		[Export(PropertyHint.None, "")]
		public string OnSuccessEvent { get; private set; }

		[Export(PropertyHint.None, "")]
		public string OnFailureEvent { get; private set; }

		[Export(PropertyHint.None, "")]
		public Texture TexIcon { get; private set; }

		[Export(PropertyHint.None, "")]
		public int Attempts { get; private set; } = 3;


		[Export(PropertyHint.None, "")]
		public int Range { get; private set; } = 300;


		[Export(PropertyHint.None, "")]
		public TimingBarSegment[] Segments { get; private set; }

		public override void _Process(float delta)
		{
			timer += delta;
			double newPos = 54.0 + Math.Cos(timer * (double)speed) * -1.0 * (double)rangeOffset + (double)rangeOffset + (double)arrowOffset;
			nPlayer.RectPosition = new Vector2((float)newPos, 0f);
		}

		public override void _Input(InputEvent @event)
		{
			string text = Inputs.Handle(@event, Inputs.Processor.Minigame, HandledInputs);
			if (!(text == "input_action"))
			{
				if (text == "debug_key")
				{
					Game.InputProcessor = Inputs.Processor.None;
					Game.Minigames.End(OnSuccessEvent);
				}
				return;
			}
			float position = nPlayer.RectPosition.x - 54f + (float)arrowOffset;
			if (position >= hitZoneFrom && position <= hitZoneTo)
			{
				currentSegment++;
				nProgress.Value = (float)(currentSegment - 1) / (float)Segments.Length * 100f;
				if (currentSegment > Segments.Length)
				{
					Game.InputProcessor = Inputs.Processor.None;
					EndMinigameOnSuccess();
				}
				else
				{
					Game.Audio.PlaySystemSound("res://assets/sfx/ui_objective.ogg");
					MakeNextSegment();
				}
			}
			else
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
				attemptsLeft--;
				attemptIndicators[attemptsLeft].Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/attempt_used.png") as Texture;
				if (attemptsLeft <= 0)
				{
					Game.InputProcessor = Inputs.Processor.None;
					Game.Minigames.End(OnFailureEvent);
				}
				else
				{
					timer = 0.0;
				}
			}
		}

		public void Init()
		{
			HBoxContainer bg = new HBoxContainer();
			bg.Name = "Background";
			bg.Set("custom_constants/separation", 0);
			MarginContainer leftContainer = new MarginContainer();
			leftContainer.Name = "LeftContainer";
			bg.AddChild(leftContainer);
			nBar = new Control();
			nBar.Name = "Bar";
			nBar.RectMinSize = new Vector2(Range, 0f);
			bg.AddChild(nBar);
			TextureRect leftTex = new TextureRect();
			leftTex.Name = "LeftTexture";
			leftTex.Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/bg_left.png") as Texture;
			leftContainer.AddChild(leftTex);
			nProgress = new TextureProgress();
			nProgress.Name = "Progress";
			nProgress.FillMode = 3;
			nProgress.TextureProgress_ = GD.Load("res://assets/sprite/common/minigame/timingbar/bg_progress.png") as Texture;
			MarginContainer progressContainer = new MarginContainer();
			progressContainer.AddChild(nProgress);
			progressContainer.Set("custom_constants/margin_left", 7);
			progressContainer.Set("custom_constants/margin_top", 8);
			leftContainer.AddChild(progressContainer);
			TextureRect stretchTex = new TextureRect();
			stretchTex.Name = "StretchTexture";
			stretchTex.Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/bg_stretch.png") as Texture;
			stretchTex.RectMinSize = new Vector2(Range, 59f);
			stretchTex.StretchMode = TextureRect.StretchModeEnum.Tile;
			nBar.AddChild(stretchTex);
			TextureRect rightText = new TextureRect();
			rightText.Name = "RightTexture";
			rightText.Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/bg_right.png") as Texture;
			bg.AddChild(rightText);
			TextureRect icon = new TextureRect();
			icon.Name = "Icon";
			icon.Texture = TexIcon;
			leftContainer.AddChild(icon);
			Control attemptsContainer = new Control();
			attemptsContainer.Name = "AttemptsContainer";
			HBoxContainer attempts = new HBoxContainer();
			attempts.Name = "Attempts";
			attempts.Alignment = BoxContainer.AlignMode.End;
			attempts.Set("custom_constants/separation", -5);
			attemptIndicators = new TextureRect[Attempts];
			for (int i = 0; i < Attempts; i++)
			{
				attemptIndicators[i] = new TextureRect();
				attemptIndicators[i].Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/attempt.png") as Texture;
				attempts.AddChild(attemptIndicators[i]);
			}
			int attemptsPosX = 54 + Range + 7 - (int)(attemptIndicators[0].Texture.GetSize().x * (float)Attempts) - -5 * (Attempts - 1) + 10;
			attempts.RectPosition = new Vector2(attemptsPosX, -20f);
			attemptsContainer.AddChild(attempts);
			nPlayer = new TextureRect();
			nPlayer.Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/player.png") as Texture;
			arrowOffset = -(int)(nPlayer.RectMinSize.x / 2f);
			rangeOffset = Range / 2;
			attemptsLeft = Attempts;
			currentSegment = 1;
			AddChild(bg);
			AddChild(attemptsContainer);
			AddChild(nPlayer);
			MakeNextSegment();
		}

		public async void EndMinigameOnSuccess()
		{
			Game.Audio.PlaySystemSound("res://assets/sfx/minigame_fanfare.ogg");
			await GDUtil.DelayOneFrame();
			Game.Minigames.End(OnSuccessEvent);
		}

		private void MakeNextSegment()
		{
			if (nGoodBar != null)
			{
				nGoodBar.Delete();
			}
			TimingBarSegment segment = Segments[currentSegment - 1];
			nGoodBar = new ColorRect();
			nGoodBar.RectMinSize = new Vector2(segment.Range, 29f);
			int offset = (segment.RandomOffset ? RandomSegmentPosition(segment) : segment.Offset);
			nGoodBar.RectPosition = new Vector2(Range / 2 - segment.Range / 2 + offset, 15f);
			nGoodBar.Color = GoodBarColor;
			nBar.AddChild(nGoodBar);
			timer = 0.0;
			speed = segment.Speed;
			hitZoneFrom = nGoodBar.RectPosition.x;
			hitZoneTo = nGoodBar.RectPosition.x + nGoodBar.RectSize.x;
		}

		private int RandomSegmentPosition(TimingBarSegment segment)
		{
			int min = segment.Range / 2;
			int max = Range - min + 1;
			return random.Next(min, max) - Range / 2;
		}
	}
}
