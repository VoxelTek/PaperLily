using Godot;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.Minigames
{
	internal class AlphanumericInput : MarginContainer, IMinigame
	{
		private static readonly string[] HandledInputs = new string[7] { "input_action", "input_cancel", "input_up", "input_down", "input_left", "input_right", "debug_key" };

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
			string text = Inputs.Handle(@event, Inputs.Processor.Minigame, HandledInputs);
			if (text == null)
			{
				return;
			}
			switch (text.Length)
			{
			case 12:
				switch (text[6])
				{
				case 'a':
					if (text == "input_action")
					{
						CheckAnswer();
					}
					break;
				case 'c':
					if (text == "input_cancel")
					{
						Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
						Game.Minigames.End(OnFailureEvent);
					}
					break;
				}
				break;
			case 10:
				switch (text[6])
				{
				case 'l':
					if (text == "input_left")
					{
						_currentSelection--;
						if (_currentSelection < 0)
						{
							_currentSelection = Digits - 1;
						}
						UpdateSelection();
						Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
					}
					break;
				case 'd':
					if (text == "input_down")
					{
						RotateDown(_currentSelection);
						Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
					}
					break;
				}
				break;
			case 11:
				if (text == "input_right")
				{
					_currentSelection++;
					if (_currentSelection >= Digits)
					{
						_currentSelection = 0;
					}
					UpdateSelection();
					Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				}
				break;
			case 8:
				if (text == "input_up")
				{
					RotateUp(_currentSelection);
					Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
				}
				break;
			case 9:
				if (text == "debug_key")
				{
					Game.Audio.PlaySystemSound("res://assets/sfx/minigame_fanfare.ogg");
					Game.Minigames.End(OnSuccessEvent);
				}
				break;
			}
		}

		public void Init()
		{
			_validDigits = new string[Characters.Length];
			for (int i = 0; i < Characters.Length; i++)
			{
				_validDigits[i] = Characters[i].ToString();
			}
			_correctAnswer = (VariableAnswer ? Game.Variables.GetVariable(CorrectAnswer) : CorrectAnswer);
			Frame frame = new InfoBoxFrame();
			HBoxContainer hBoxContainer = new HBoxContainer();
			hBoxContainer.SetSeparation(0);
			_labels = new Label[Digits];
			_labelBgs = new ColorRect[Digits];
			for (int j = 0; j < Digits; j++)
			{
				MarginContainer opContainer = GDUtil.MakeNode<MarginContainer>("Option" + j);
				ColorRect opBg = GDUtil.MakeNode<ColorRect>("Bg");
				opBg.Color = UIUtil.Transparent;
				Label label = GDUtil.MakeNode<Label>("Digit" + j);
				label.Text = _validDigits[0];
				label.Align = Label.AlignEnum.Center;
				label.RectMinSize = new Vector2(15f, 0f);
				label.SetDefaultFontAndColor();
				_labels[j] = label;
				_labelBgs[j] = opBg;
				opContainer.AddChild(opBg);
				opContainer.AddChild(label);
				hBoxContainer.AddChild(opContainer);
			}
			frame.AddToFrame(hBoxContainer);
			UpdateSelection();
			AddChild(frame);
		}

		private void RotateUp(int position)
		{
			for (int i = 0; i < _validDigits.Length; i++)
			{
				if (_labels[position].Text == _validDigits[i])
				{
					AdjustDigitAtPosition(position, i + 1);
					break;
				}
			}
		}

		private void RotateDown(int position)
		{
			for (int i = 0; i < _validDigits.Length; i++)
			{
				if (_labels[position].Text == _validDigits[i])
				{
					AdjustDigitAtPosition(position, i - 1);
					break;
				}
			}
		}

		private void AdjustDigitAtPosition(int position, int newDigitIndex)
		{
			if (newDigitIndex >= _validDigits.Length)
			{
				_labels[position].Text = _validDigits[0];
			}
			else if (newDigitIndex < 0)
			{
				_labels[position].Text = _validDigits[^1];
			}
			else
			{
				_labels[position].Text = _validDigits[newDigitIndex];
			}
		}

		private void UpdateSelection()
		{
			ColorRect[] labelBgs = _labelBgs;
			for (int i = 0; i < labelBgs.Length; i++)
			{
				labelBgs[i].Color = UIUtil.Transparent;
			}
			_labelBgs[_currentSelection].Color = UIUtil.SelectionColor;
		}

		private void CheckAnswer()
		{
			string answer = "";
			Label[] labels = _labels;
			foreach (Label label in labels)
			{
				answer += label.Text;
			}
			if (answer == _correctAnswer)
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/minigame_fanfare.ogg");
				Game.Minigames.End(OnSuccessEvent);
			}
			else
			{
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
				Game.Minigames.End(OnFailureEvent);
			}
		}
	}
}
