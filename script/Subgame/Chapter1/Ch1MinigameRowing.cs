// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1MinigameRowing
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Minigames;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class Ch1MinigameRowing : Control, IMinigame
  {
    private static readonly string[] HandledInputs = new string[4]
    {
      "input_left",
      "input_up",
      "input_right",
      "input_down"
    };
    private static readonly Color NextValidKey = new Color("#77b359");
    private static readonly Color WrongKey = new Color("#b35959");
    private static readonly Color InvalidKey = new Color(1f, 1f, 1f, 0.3f);
    private Random Random = new Random();
    private const float MovementImpulseX = 1.5f;
    private const float MovementImpulseY = 2f;
    private const float MovementAtenuation = 0.01f;
    private const float SideLimit = 50f;
    private const string LeftArr = "res://assets/img/ui/input/keyboard_arrow_left.png";
    private const string UpArr = "res://assets/img/ui/input/keyboard_arrow_up.png";
    private const string RightArr = "res://assets/img/ui/input/keyboard_arrow_right.png";
    private const string DownArr = "res://assets/img/ui/input/keyboard_arrow_down.png";
    [LacieEngine.API.GetNode("Main/Boat")]
    private Area2D nBoat;
    [LacieEngine.API.GetNode("TilesWater")]
    private Node2D nWater;
    [LacieEngine.API.GetNode("Stats")]
    private Label nStats;
    [LacieEngine.API.GetNode("ScrollingLayer")]
    private Node2D nScrollingLayer;
    [LacieEngine.API.GetNode("HBoxContainer/LeftContainer/VBoxContainer/LeftInputs")]
    private Control nLeftInputs;
    [LacieEngine.API.GetNode("HBoxContainer/ForwardContainer/VBoxContainer/ForwardInputs")]
    private Control nForwardInputs;
    [LacieEngine.API.GetNode("HBoxContainer/RightContainer/VBoxContainer/RightInputs")]
    private Control nRightInputs;
    private Dictionary<Direction, Texture> _directionTextures;
    private float _time;
    private float _impulseX;
    private float _impulseY;
    private bool _inputsGenerated;
    private bool _readingInputs;
    private Dictionary<Direction, Direction[]> _inputSequences;
    private Direction _inputChosen = Direction.None;
    private int _inputIndex;
    private float _obstacleGeneartionTimer = 3f;

    public override void _Input(InputEvent @event)
    {
      string input = Inputs.Handle(Inputs.Processor.Minigame, Ch1MinigameRowing.HandledInputs);
      if (input == null)
        return;
      this.SendInput(input);
    }

    public override void _Ready()
    {
      this.UpdateInputs();
      this._readingInputs = true;
      int num = (int) this.nBoat.Connect("area_entered", (Godot.Object) this, "CollideWithRock");
    }

    public override void _Process(float delta)
    {
      this._time += this._impulseY * delta;
      this._obstacleGeneartionTimer -= delta;
      if ((double) this._obstacleGeneartionTimer <= 0.0)
      {
        Sprite sprite = new Sprite();
        sprite.Texture = GD.Load<Texture>("res://assets/sprite/ch1/forest/floor_rock2.png");
        Area2D area2D = new Area2D();
        area2D.Monitoring = false;
        area2D.CollisionLayer = 1U;
        area2D.CollisionMask = 0U;
        area2D.Position = new Vector2((float) this.Random.Next(0, 640), -200f);
        area2D.AddChild((Node) sprite);
        area2D.AddChild((Node) GDUtil.MakeCollisionRect(new Vector2(20f, 10f)));
        this.nScrollingLayer.AddChild((Node) area2D);
        this._obstacleGeneartionTimer = 3f;
      }
      this.UpdateWaterShader();
      Area2D nBoat = this.nBoat;
      nBoat.Position = nBoat.Position + new Vector2(this._impulseX, 0.0f);
      foreach (Node2D child in this.nScrollingLayer.GetChildren())
        child.Position += new Vector2(0.0f, this._impulseY / 3f);
      if ((double) this.nBoat.Position.x <= 50.0)
      {
        this.nBoat.Position = new Vector2(50f, this.nBoat.Position.y);
        this._impulseX = 0.0f;
      }
      if ((double) this.nBoat.Position.x >= 590.0)
      {
        this.nBoat.Position = new Vector2(590f, this.nBoat.Position.y);
        this._impulseX = 0.0f;
      }
      this._impulseX = (double) Math.Abs(this._impulseX) < 0.0099999997764825821 ? 0.0f : Mathf.Lerp(this._impulseX, 0.0f, 0.01f);
      this._impulseY = (double) Math.Abs(this._impulseY) < 1.0099999904632568 ? 1f : Mathf.Lerp(this._impulseY, 1f, 0.01f);
      this.UpdateStats();
    }

    private void UpdateWaterShader()
    {
      (this.nWater.Material as ShaderMaterial).SetShaderParam("time", (object) this._time);
    }

    private void UpdateInputs()
    {
      if (!this._inputsGenerated)
      {
        this._inputSequences = new Dictionary<Direction, Direction[]>();
        this._inputSequences[Direction.Left] = new Direction[3];
        this._inputSequences[Direction.Left][0] = Direction.Left;
        this._inputSequences[Direction.Left][1] = this.RandomInput();
        this._inputSequences[Direction.Left][2] = this.RandomInput();
        this._inputSequences[Direction.Up] = new Direction[4];
        this._inputSequences[Direction.Up][0] = Direction.Up;
        this._inputSequences[Direction.Up][1] = this.RandomInput();
        this._inputSequences[Direction.Up][2] = this.RandomInput();
        this._inputSequences[Direction.Up][3] = this.RandomInput();
        this._inputSequences[Direction.Right] = new Direction[3];
        this._inputSequences[Direction.Right][0] = Direction.Right;
        this._inputSequences[Direction.Right][1] = this.RandomInput();
        this._inputSequences[Direction.Right][2] = this.RandomInput();
        this._inputsGenerated = true;
        this.nLeftInputs.Clear();
        foreach (Direction direction in this._inputSequences[Direction.Left])
          this.nLeftInputs.AddChild((Node) this.MakeInputIndicator(direction));
        this.nForwardInputs.Clear();
        foreach (Direction direction in this._inputSequences[Direction.Up])
          this.nForwardInputs.AddChild((Node) this.MakeInputIndicator(direction));
        this.nRightInputs.Clear();
        foreach (Direction direction in this._inputSequences[Direction.Right])
          this.nRightInputs.AddChild((Node) this.MakeInputIndicator(direction));
        this.nLeftInputs.Modulate = Godot.Colors.White;
        this.nForwardInputs.Modulate = Godot.Colors.White;
        this.nRightInputs.Modulate = Godot.Colors.White;
        this.nLeftInputs.GetChild<Control>(0).Modulate = Ch1MinigameRowing.NextValidKey;
        this.nForwardInputs.GetChild<Control>(0).Modulate = Ch1MinigameRowing.NextValidKey;
        this.nRightInputs.GetChild<Control>(0).Modulate = Ch1MinigameRowing.NextValidKey;
      }
      if (!(this._inputChosen != Direction.None))
        return;
      this.nLeftInputs.Modulate = Ch1MinigameRowing.InvalidKey;
      this.nForwardInputs.Modulate = Ch1MinigameRowing.InvalidKey;
      this.nRightInputs.Modulate = Ch1MinigameRowing.InvalidKey;
      this.nLeftInputs.GetChild<Control>(0).Modulate = Godot.Colors.White;
      this.nForwardInputs.GetChild<Control>(0).Modulate = Godot.Colors.White;
      this.nRightInputs.GetChild<Control>(0).Modulate = Godot.Colors.White;
      Control control1;
      switch (this._inputChosen.ToEnum())
      {
        case Direction.Enum.Left:
          control1 = this.nLeftInputs;
          break;
        case Direction.Enum.Up:
          control1 = this.nForwardInputs;
          break;
        case Direction.Enum.Right:
          control1 = this.nRightInputs;
          break;
        default:
          throw new NotImplementedException();
      }
      Control control2 = control1;
      control2.Modulate = Godot.Colors.White;
      for (int idx = 0; idx < this._inputIndex; ++idx)
        control2.GetChild<Control>(idx).Modulate = Ch1MinigameRowing.InvalidKey;
      control2.GetChild<Control>(this._inputIndex).Modulate = Ch1MinigameRowing.NextValidKey;
    }

    private Control MakeInputIndicator(Direction direction)
    {
      TextureRect textureRect = GDUtil.MakeNode<TextureRect>("Indicator");
      textureRect.Texture = this._directionTextures[direction];
      textureRect.Expand = true;
      textureRect.RectMinSize = new Vector2(20f, 20f);
      return (Control) textureRect;
    }

    private void SendInput(string input)
    {
      if (!this._readingInputs || !this._inputsGenerated)
        return;
      if (this._inputChosen == Direction.None)
      {
        if (input == "input_down")
          return;
        this._inputChosen = Direction.FromInput(input);
        this._inputIndex = 1;
        this.UpdateInputs();
      }
      else if (this._inputSequences[this._inputChosen][this._inputIndex] == Direction.FromInput(input))
      {
        ++this._inputIndex;
        if (this._inputIndex >= this._inputSequences[this._inputChosen].Length)
        {
          this.SendImpulse(this._inputChosen);
          this.ResetInputs();
        }
        else
          this.UpdateInputs();
      }
      else
        this.WrongInpupt();
    }

    private async void WrongInpupt()
    {
      this._readingInputs = false;
      Control control;
      switch (this._inputChosen.ToEnum())
      {
        case Direction.Enum.Left:
          control = this.nLeftInputs;
          break;
        case Direction.Enum.Up:
          control = this.nForwardInputs;
          break;
        case Direction.Enum.Right:
          control = this.nRightInputs;
          break;
        default:
          throw new NotImplementedException();
      }
      control.GetChild<Control>(this._inputIndex).Modulate = Ch1MinigameRowing.WrongKey;
      await DrkieUtil.DelaySeconds(0.5);
      this.ResetInputs();
      this._readingInputs = true;
    }

    private void ResetInputs()
    {
      this._inputSequences = (Dictionary<Direction, Direction[]>) null;
      this._inputChosen = Direction.None;
      this._inputIndex = 0;
      this._inputsGenerated = false;
      this.UpdateInputs();
    }

    private void SendImpulse(Direction direction)
    {
      if (direction == Direction.Left)
        this._impulseX -= 1.5f;
      else if (direction == Direction.Right)
      {
        this._impulseX += 1.5f;
      }
      else
      {
        if (!(direction == Direction.Up))
          return;
        this._impulseY += 2f;
      }
    }

    private void UpdateStats()
    {
      this.nStats.Text = "Speed: " + Math.Round((double) this._impulseY, 2).ToString("0.00") + "km/h\nDistance: " + Math.Round((double) this._time, 2).ToString("0.00") + "m";
    }

    private Direction RandomInput()
    {
      double num = this.Random.NextDouble();
      if (num < 0.25)
        return Direction.Left;
      if (num < 0.5)
        return Direction.Up;
      return num < 0.75 ? Direction.Right : Direction.Down;
    }

    public void Init()
    {
      this._directionTextures = new Dictionary<Direction, Texture>();
      this._directionTextures[Direction.Left] = GD.Load<Texture>("res://assets/img/ui/input/keyboard_arrow_left.png");
      this._directionTextures[Direction.Up] = GD.Load<Texture>("res://assets/img/ui/input/keyboard_arrow_up.png");
      this._directionTextures[Direction.Right] = GD.Load<Texture>("res://assets/img/ui/input/keyboard_arrow_right.png");
      this._directionTextures[Direction.Down] = GD.Load<Texture>("res://assets/img/ui/input/keyboard_arrow_down.png");
    }

    public void CollideWithRock(Area2D area)
    {
      area.Delete();
      this.nBoat.Position = new Vector2(320f, this.nBoat.Position.y);
      this._impulseX = 0.0f;
      --this._impulseY;
      this.ResetInputs();
    }
  }
}
