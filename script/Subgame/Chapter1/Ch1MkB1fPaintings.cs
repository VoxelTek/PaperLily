using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1MkB1fPaintings : GameRoom
	{
		[GetNode("Background/Painting1")]
		private Sprite nPainting1;

		[GetNode("Background/Painting2")]
		private Sprite nPainting2;

		[GetNode("Background/Painting3")]
		private Sprite nPainting3;

		[GetNode("Background/Painting4")]
		private Sprite nPainting4;

		[GetNode("Background/Painting5")]
		private Sprite nPainting5;

		[GetNode("Background/Painting6")]
		private Sprite nPainting6;

		private PVar varPainting1 = "ch1.mk_painting_1";

		private PVar varPainting2 = "ch1.mk_painting_2";

		private PVar varPainting3 = "ch1.mk_painting_3";

		private PVar varPainting4 = "ch1.mk_painting_4";

		private PVar varPainting5 = "ch1.mk_painting_5";

		private PVar varPainting6 = "ch1.mk_painting_6";

		private PEvent evtSuccess = "event_clear";

		private const int PAINTINGS = 6;

		private const int BLESSINGS = 5;

		private PVar[] _vars;

		private int[,] _values = new int[6, 5]
		{
			{ 1, 3, 5, 4, 2 },
			{ 4, 2, 3, 1, 5 },
			{ 3, 2, 1, 4, 5 },
			{ 5, 2, 4, 1, 3 },
			{ 3, 1, 5, 4, 2 },
			{ 1, 2, 5, 4, 3 }
		};

		public override void _BeforeFadeIn()
		{
			_vars = new PVar[6] { varPainting1, varPainting2, varPainting3, varPainting4, varPainting5, varPainting6 };
		}

		public override void _UpdateRoom()
		{
			if ((bool)varPainting1.Value)
			{
				nPainting1.Frame = _values[0, (int)varPainting1.Value];
			}
			if ((bool)varPainting2.Value)
			{
				nPainting2.Frame = _values[1, (int)varPainting2.Value];
			}
			if ((bool)varPainting3.Value)
			{
				nPainting3.Frame = _values[2, (int)varPainting3.Value];
			}
			if ((bool)varPainting4.Value)
			{
				nPainting4.Frame = _values[3, (int)varPainting4.Value];
			}
			if ((bool)varPainting5.Value)
			{
				nPainting5.Frame = _values[4, (int)varPainting5.Value];
			}
			if ((bool)varPainting6.Value)
			{
				nPainting6.Frame = _values[5, (int)varPainting6.Value];
			}
		}

		public void RotatePainting1()
		{
			RotatePainting(0);
		}

		public void RotatePainting2()
		{
			RotatePainting(1);
		}

		public void RotatePainting3()
		{
			RotatePainting(2);
		}

		public void RotatePainting4()
		{
			RotatePainting(3);
		}

		public void RotatePainting5()
		{
			RotatePainting(4);
		}

		public void RotatePainting6()
		{
			RotateFinalPainting();
		}

		private void RotatePainting(int idx)
		{
			int cursor = (int)_vars[idx].Value + 1;
			if (cursor >= 5)
			{
				cursor = 0;
			}
			_vars[idx].NewValue = cursor;
		}

		private void RotateFinalPainting()
		{
			if (_values[0, (int)varPainting1.Value] == 1 && _values[1, (int)varPainting2.Value] == 2 && _values[2, (int)varPainting3.Value] == 3 && _values[3, (int)varPainting4.Value] == 4 && _values[4, (int)varPainting5.Value] == 5)
			{
				evtSuccess.Play();
			}
			else
			{
				RotatePainting(5);
			}
		}
	}
}
