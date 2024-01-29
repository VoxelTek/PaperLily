using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Minigames;
using LacieEngine.UI;

namespace LacieEngine.Subgame.Chapter1
{
	public class Ch1PncPhone : PointAndClick
	{
		[Export(PropertyHint.None, "")]
		public Texture TexDigits;

		[Export(PropertyHint.None, "")]
		public AudioStream[] SfxDialTones;

		[GetNode("Display")]
		protected Control nDigitContainer;

		protected static Dictionary<string, string> CorrectCombinations;

		private const int MaxDigits = 8;

		private string _curCombination = "";

		public override void _Ready()
		{
			base._Ready();
			if (CorrectCombinations == null)
			{
				Log.Error("Phone combinations not initialized!");
			}
		}

		public void Dial0()
		{
			DialNumber("0");
		}

		public void Dial1()
		{
			DialNumber("1");
		}

		public void Dial2()
		{
			DialNumber("2");
		}

		public void Dial3()
		{
			DialNumber("3");
		}

		public void Dial4()
		{
			DialNumber("4");
		}

		public void Dial5()
		{
			DialNumber("5");
		}

		public void Dial6()
		{
			DialNumber("6");
		}

		public void Dial7()
		{
			DialNumber("7");
		}

		public void Dial8()
		{
			DialNumber("8");
		}

		public void Dial9()
		{
			DialNumber("9");
		}

		public void DialAsterisk()
		{
			DialNumber("*");
		}

		public void DialPound()
		{
			DialNumber("#");
		}

		private void DialNumber(string digit)
		{
			PlayDialTone(digit);
			_curCombination += digit;
			nDigitContainer.AddChild(MakeDigitTexture(digit));
			if (CorrectCombinations.ContainsKey(_curCombination))
			{
				EndMinigame(CorrectCombinations[_curCombination]);
			}
			else if (_curCombination.Length >= 8)
			{
				EndMinigame("event_dial_wrong_number");
			}
		}

		private async void EndMinigame(string @event)
		{
			Game.InputProcessor = Inputs.Processor.None;
			nCursor.Visible = false;
			await DrkieUtil.DelaySeconds(1.0);
			Game.Minigames.End(@event);
		}

		private TextureRect MakeDigitTexture(string digit)
		{
			SlicedTextureRect tex = new SlicedTextureRect();
			tex.Texture = TexDigits;
			tex.Hframes = 3;
			tex.Vframes = 4;
			SlicedTextureRect slicedTextureRect = tex;
			int frame;
			if (digit != null)
			{
				int length = digit.Length;
				if (length == 1)
				{
					switch (digit[0])
					{
					case '1':
						break;
					case '2':
						goto IL_00ab;
					case '3':
						goto IL_00af;
					case '4':
						goto IL_00b3;
					case '5':
						goto IL_00b7;
					case '6':
						goto IL_00bb;
					case '7':
						goto IL_00bf;
					case '8':
						goto IL_00c3;
					case '9':
						goto IL_00c7;
					case '*':
						goto IL_00cb;
					case '0':
						goto IL_00d0;
					case '#':
						goto IL_00d5;
					default:
						goto IL_00da;
					}
					frame = 0;
					goto IL_00dc;
				}
			}
			goto IL_00da;
			IL_00d5:
			frame = 11;
			goto IL_00dc;
			IL_00da:
			frame = 0;
			goto IL_00dc;
			IL_00c7:
			frame = 8;
			goto IL_00dc;
			IL_00c3:
			frame = 7;
			goto IL_00dc;
			IL_00bf:
			frame = 6;
			goto IL_00dc;
			IL_00bb:
			frame = 5;
			goto IL_00dc;
			IL_00b7:
			frame = 4;
			goto IL_00dc;
			IL_00b3:
			frame = 3;
			goto IL_00dc;
			IL_00af:
			frame = 2;
			goto IL_00dc;
			IL_00ab:
			frame = 1;
			goto IL_00dc;
			IL_00dc:
			slicedTextureRect.Frame = frame;
			return tex;
			IL_00d0:
			frame = 10;
			goto IL_00dc;
			IL_00cb:
			frame = 9;
			goto IL_00dc;
		}

		private void PlayDialTone(string digit)
		{
			AudioStream audioStream;
			if (digit != null)
			{
				int length = digit.Length;
				if (length == 1)
				{
					switch (digit[0])
					{
					case '1':
						break;
					case '2':
						goto IL_0094;
					case '3':
						goto IL_009f;
					case '4':
						goto IL_00aa;
					case '5':
						goto IL_00b5;
					case '6':
						goto IL_00c0;
					case '7':
						goto IL_00cb;
					case '8':
						goto IL_00d6;
					case '9':
						goto IL_00e1;
					case '*':
						goto IL_00ed;
					case '0':
						goto IL_00f9;
					case '#':
						goto IL_0104;
					default:
						goto IL_0110;
					}
					audioStream = SfxDialTones[1];
					goto IL_0119;
				}
			}
			goto IL_0110;
			IL_0104:
			audioStream = SfxDialTones[11];
			goto IL_0119;
			IL_0110:
			audioStream = SfxDialTones[0];
			goto IL_0119;
			IL_00e1:
			audioStream = SfxDialTones[9];
			goto IL_0119;
			IL_00d6:
			audioStream = SfxDialTones[8];
			goto IL_0119;
			IL_00cb:
			audioStream = SfxDialTones[7];
			goto IL_0119;
			IL_00c0:
			audioStream = SfxDialTones[6];
			goto IL_0119;
			IL_00b5:
			audioStream = SfxDialTones[5];
			goto IL_0119;
			IL_00aa:
			audioStream = SfxDialTones[4];
			goto IL_0119;
			IL_009f:
			audioStream = SfxDialTones[3];
			goto IL_0119;
			IL_0094:
			audioStream = SfxDialTones[2];
			goto IL_0119;
			IL_0119:
			AudioStream sfx = audioStream;
			Game.Audio.StopSfx();
			Game.Audio.PlaySfx(sfx);
			return;
			IL_00f9:
			audioStream = SfxDialTones[0];
			goto IL_0119;
			IL_00ed:
			audioStream = SfxDialTones[10];
			goto IL_0119;
		}
	}
}
