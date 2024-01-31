// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.AlphanumericInput
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.UI;

#nullable disable
namespace LacieEngine.Minigames
{
  internal class AlphanumericInput : MarginContainer, IMinigame
  {
    private static readonly string[] HandledInputs = new string[7]
    {
      "input_action",
      "input_cancel",
      "input_up",
      "input_down",
      "input_left",
      "input_right",
      "debug_key"
    };
    private string _correctAnswer;
    private int _currentSelection;
    private string[] _validDigits;
    private ColorRect[] _labelBgs;
    private Label[] _labels;

    [Export(PropertyHint.None, "")]
    public string OnSuccessEvent { get; set; }

    [Export(PropertyHint.None, "")]
    public string OnFailureEvent { get; set; }

    [Export(PropertyHint.None, "")]
    public int Digits { get; set; } = 4;

    [Export(PropertyHint.None, "")]
    public string Characters { get; set; } = "0123456789";

    [Export(PropertyHint.None, "")]
    public string CorrectAnswer { get; set; }

    [Export(PropertyHint.None, "")]
    public bool VariableAnswer { get; set; }

    public override void _Input(InputEvent @event)
    {
      string str = Inputs.Handle(@event, Inputs.Processor.Minigame, AlphanumericInput.HandledInputs);
      if (str == null)
        return;
      switch (str.Length)
      {
        case 8:
          if (!(str == "input_up"))
            break;
          this.RotateUp(this._currentSelection);
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
          break;
        case 9:
          if (!(str == "debug_key"))
            break;
          Game.Audio.PlaySystemSound("res://assets/sfx/minigame_fanfare.ogg");
          Game.Minigames.End(this.OnSuccessEvent);
          break;
        case 10:
          switch (str[6])
          {
            case 'd':
              if (!(str == "input_down"))
                return;
              this.RotateDown(this._currentSelection);
              Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
              return;
            case 'l':
              if (!(str == "input_left"))
                return;
              --this._currentSelection;
              if (this._currentSelection < 0)
                this._currentSelection = this.Digits - 1;
              this.UpdateSelection();
              Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
              return;
            default:
              return;
          }
        case 11:
          if (!(str == "input_right"))
            break;
          ++this._currentSelection;
          if (this._currentSelection >= this.Digits)
            this._currentSelection = 0;
          this.UpdateSelection();
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
          break;
        case 12:
          switch (str[6])
          {
            case 'a':
              if (!(str == "input_action"))
                return;
              this.CheckAnswer();
              return;
            case 'c':
              if (!(str == "input_cancel"))
                return;
              Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
              Game.Minigames.End(this.OnFailureEvent);
              return;
            default:
              return;
          }
      }
    }

    public void Init()
    {
      this._validDigits = new string[this.Characters.Length];
      for (int index = 0; index < this.Characters.Length; ++index)
        this._validDigits[index] = this.Characters[index].ToString();
      this._correctAnswer = this.VariableAnswer ? Game.Variables.GetVariable(this.CorrectAnswer) : this.CorrectAnswer;
      Frame frame = (Frame) new InfoBoxFrame();
      HBoxContainer hboxContainer = new HBoxContainer();
      hboxContainer.SetSeparation(0);
      this._labels = new Label[this.Digits];
      this._labelBgs = new ColorRect[this.Digits];
      for (int index = 0; index < this.Digits; ++index)
      {
        MarginContainer marginContainer = GDUtil.MakeNode<MarginContainer>("Option" + index.ToString());
        ColorRect colorRect = GDUtil.MakeNode<ColorRect>("Bg");
        colorRect.Color = UIUtil.Transparent;
        Label label = GDUtil.MakeNode<Label>("Digit" + index.ToString());
        label.Text = this._validDigits[0];
        label.Align = Label.AlignEnum.Center;
        label.RectMinSize = new Vector2(15f, 0.0f);
        label.SetDefaultFontAndColor();
        this._labels[index] = label;
        this._labelBgs[index] = colorRect;
        marginContainer.AddChild((Node) colorRect);
        marginContainer.AddChild((Node) label);
        hboxContainer.AddChild((Node) marginContainer);
      }
      frame.AddToFrame((Node) hboxContainer);
      this.UpdateSelection();
      this.AddChild((Node) frame);
    }

    private void RotateUp(int position)
    {
      for (int index = 0; index < this._validDigits.Length; ++index)
      {
        if (this._labels[position].Text == this._validDigits[index])
        {
          this.AdjustDigitAtPosition(position, index + 1);
          break;
        }
      }
    }

    private void RotateDown(int position)
    {
      for (int index = 0; index < this._validDigits.Length; ++index)
      {
        if (this._labels[position].Text == this._validDigits[index])
        {
          this.AdjustDigitAtPosition(position, index - 1);
          break;
        }
      }
    }

    private void AdjustDigitAtPosition(int position, int newDigitIndex)
    {
      if (newDigitIndex >= this._validDigits.Length)
        this._labels[position].Text = this._validDigits[0];
      else if (newDigitIndex < 0)
      {
        Label label = this._labels[position];
        string[] validDigits = this._validDigits;
        string str = validDigits[validDigits.Length - 1];
        label.Text = str;
      }
      else
        this._labels[position].Text = this._validDigits[newDigitIndex];
    }

    private void UpdateSelection()
    {
      foreach (ColorRect labelBg in this._labelBgs)
        labelBg.Color = UIUtil.Transparent;
      this._labelBgs[this._currentSelection].Color = UIUtil.SelectionColor;
    }

    private void CheckAnswer()
    {
      string str = "";
      foreach (Label label in this._labels)
        str += label.Text;
      if (str == this._correctAnswer)
      {
        Game.Audio.PlaySystemSound("res://assets/sfx/minigame_fanfare.ogg");
        Game.Minigames.End(this.OnSuccessEvent);
      }
      else
      {
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
        Game.Minigames.End(this.OnFailureEvent);
      }
    }
  }
}
