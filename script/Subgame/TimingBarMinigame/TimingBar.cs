// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.TimingBar
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
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
    private static readonly string[] HandledInputs = new string[2]
    {
      "input_action",
      "debug_key"
    };
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
      this.timer += (double) delta;
      this.nPlayer.RectPosition = new Vector2((float) (54.0 + Math.Cos(this.timer * (double) this.speed) * -1.0 * (double) this.rangeOffset) + (float) this.rangeOffset + (float) this.arrowOffset, 0.0f);
    }

    public override void _Input(InputEvent @event)
    {
      switch (Inputs.Handle(@event, Inputs.Processor.Minigame, TimingBar.HandledInputs))
      {
        case "input_action":
          float num = this.nPlayer.RectPosition.x - 54f + (float) this.arrowOffset;
          if ((double) num >= (double) this.hitZoneFrom && (double) num <= (double) this.hitZoneTo)
          {
            ++this.currentSegment;
            this.nProgress.Value = (double) (this.currentSegment - 1) / (double) this.Segments.Length * 100.0;
            if (this.currentSegment > this.Segments.Length)
            {
              Game.InputProcessor = Inputs.Processor.None;
              this.EndMinigameOnSuccess();
              break;
            }
            Game.Audio.PlaySystemSound("res://assets/sfx/ui_objective.ogg");
            this.MakeNextSegment();
            break;
          }
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
          --this.attemptsLeft;
          this.attemptIndicators[this.attemptsLeft].Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/attempt_used.png") as Texture;
          if (this.attemptsLeft <= 0)
          {
            Game.InputProcessor = Inputs.Processor.None;
            Game.Minigames.End(this.OnFailureEvent);
            break;
          }
          this.timer = 0.0;
          break;
        case "debug_key":
          Game.InputProcessor = Inputs.Processor.None;
          Game.Minigames.End(this.OnSuccessEvent);
          break;
      }
    }

    public void Init()
    {
      HBoxContainer hboxContainer1 = new HBoxContainer();
      hboxContainer1.Name = "Background";
      hboxContainer1.Set("custom_constants/separation", (object) 0);
      MarginContainer marginContainer1 = new MarginContainer();
      marginContainer1.Name = "LeftContainer";
      hboxContainer1.AddChild((Node) marginContainer1);
      this.nBar = new Control();
      this.nBar.Name = "Bar";
      this.nBar.RectMinSize = new Vector2((float) this.Range, 0.0f);
      hboxContainer1.AddChild((Node) this.nBar);
      TextureRect textureRect1 = new TextureRect();
      textureRect1.Name = "LeftTexture";
      textureRect1.Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/bg_left.png") as Texture;
      marginContainer1.AddChild((Node) textureRect1);
      this.nProgress = new TextureProgress();
      this.nProgress.Name = "Progress";
      this.nProgress.FillMode = 3;
      this.nProgress.TextureProgress_ = GD.Load("res://assets/sprite/common/minigame/timingbar/bg_progress.png") as Texture;
      MarginContainer marginContainer2 = new MarginContainer();
      marginContainer2.AddChild((Node) this.nProgress);
      marginContainer2.Set("custom_constants/margin_left", (object) 7);
      marginContainer2.Set("custom_constants/margin_top", (object) 8);
      marginContainer1.AddChild((Node) marginContainer2);
      TextureRect textureRect2 = new TextureRect();
      textureRect2.Name = "StretchTexture";
      textureRect2.Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/bg_stretch.png") as Texture;
      textureRect2.RectMinSize = new Vector2((float) this.Range, 59f);
      textureRect2.StretchMode = TextureRect.StretchModeEnum.Tile;
      this.nBar.AddChild((Node) textureRect2);
      TextureRect textureRect3 = new TextureRect();
      textureRect3.Name = "RightTexture";
      textureRect3.Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/bg_right.png") as Texture;
      hboxContainer1.AddChild((Node) textureRect3);
      TextureRect textureRect4 = new TextureRect();
      textureRect4.Name = "Icon";
      textureRect4.Texture = this.TexIcon;
      marginContainer1.AddChild((Node) textureRect4);
      Control control = new Control();
      control.Name = "AttemptsContainer";
      HBoxContainer hboxContainer2 = new HBoxContainer();
      hboxContainer2.Name = "Attempts";
      hboxContainer2.Alignment = BoxContainer.AlignMode.End;
      hboxContainer2.Set("custom_constants/separation", (object) -5);
      this.attemptIndicators = new TextureRect[this.Attempts];
      for (int index = 0; index < this.Attempts; ++index)
      {
        this.attemptIndicators[index] = new TextureRect();
        this.attemptIndicators[index].Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/attempt.png") as Texture;
        hboxContainer2.AddChild((Node) this.attemptIndicators[index]);
      }
      int x = 54 + this.Range + 7 - (int) ((double) this.attemptIndicators[0].Texture.GetSize().x * (double) this.Attempts) - -5 * (this.Attempts - 1) + 10;
      hboxContainer2.RectPosition = new Vector2((float) x, -20f);
      control.AddChild((Node) hboxContainer2);
      this.nPlayer = new TextureRect();
      this.nPlayer.Texture = GD.Load("res://assets/sprite/common/minigame/timingbar/player.png") as Texture;
      this.arrowOffset = -(int) ((double) this.nPlayer.RectMinSize.x / 2.0);
      this.rangeOffset = this.Range / 2;
      this.attemptsLeft = this.Attempts;
      this.currentSegment = 1;
      this.AddChild((Node) hboxContainer1);
      this.AddChild((Node) control);
      this.AddChild((Node) this.nPlayer);
      this.MakeNextSegment();
    }

    public async void EndMinigameOnSuccess()
    {
      Game.Audio.PlaySystemSound("res://assets/sfx/minigame_fanfare.ogg");
      await GDUtil.DelayOneFrame();
      Game.Minigames.End(this.OnSuccessEvent);
    }

    private void MakeNextSegment()
    {
      if (this.nGoodBar != null)
        this.nGoodBar.Delete();
      TimingBarSegment segment = this.Segments[this.currentSegment - 1];
      this.nGoodBar = new ColorRect();
      this.nGoodBar.RectMinSize = new Vector2((float) segment.Range, 29f);
      int num = segment.RandomOffset ? this.RandomSegmentPosition(segment) : segment.Offset;
      this.nGoodBar.RectPosition = new Vector2((float) (this.Range / 2 - segment.Range / 2 + num), 15f);
      this.nGoodBar.Color = TimingBar.GoodBarColor;
      this.nBar.AddChild((Node) this.nGoodBar);
      this.timer = 0.0;
      this.speed = segment.Speed;
      this.hitZoneFrom = this.nGoodBar.RectPosition.x;
      this.hitZoneTo = this.nGoodBar.RectPosition.x + this.nGoodBar.RectSize.x;
    }

    private int RandomSegmentPosition(TimingBarSegment segment)
    {
      int minValue = segment.Range / 2;
      int maxValue = this.Range - minValue + 1;
      return this.random.Next(minValue, maxValue) - this.Range / 2;
    }
  }
}
