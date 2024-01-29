using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1MkWisteria : GameRoom
	{
		[GetNode("Background/Canvas")]
		private Sprite nCanvas;

		[GetNode("Background/Canvas/MkDarkness")]
		private Node2D nDarkness;

		[GetNode("Background/Canvas/Painting")]
		private Sprite nPainting;

		[GetNode("Background/WallPainting")]
		private Sprite nItemHolderPainting;

		private PVar varRevealedCanvas = "ch1.mk_revealed_canvas";

		private PVar varTookCanvas = "ch1.mk_took_canvas";

		private PVar varPainting = "ch1.mk_canvas_color";

		private PVar varItemHolderItem = "ch1.mk_wisteria_item";

		public override void _UpdateRoom()
		{
			nCanvas.Frame = (varTookCanvas.Value ? 2 : (((bool)varRevealedCanvas.Value) ? 1 : 0));
			nDarkness.Visible = nCanvas.Frame == 1;
			nPainting.Visible = !varTookCanvas.Value && (bool)varPainting.Value;
			Sprite sprite;
			string text;
			int frame;
			char c;
			if (nPainting.Visible)
			{
				sprite = nPainting;
				text = varPainting.Value;
				if (text == null)
				{
					goto IL_01bd;
				}
				switch (text.Length)
				{
				case 3:
					break;
				case 6:
					goto IL_00e8;
				case 5:
					goto IL_0108;
				case 4:
					goto IL_0170;
				default:
					goto IL_01bd;
				}
				c = text[0];
				if (c != 'a')
				{
					if (c != 'r' || !(text == "red"))
					{
						goto IL_01bd;
					}
					frame = 0;
				}
				else
				{
					if (!(text == "all"))
					{
						goto IL_01bd;
					}
					frame = 6;
				}
				goto IL_01bf;
			}
			goto IL_01c6;
			IL_01c6:
			nItemHolderPainting.Frame = varItemHolderItem.Value;
			return;
			IL_00e8:
			c = text[0];
			if (c != 'o')
			{
				if (c != 'p')
				{
					if (c != 'y' || !(text == "yellow"))
					{
						goto IL_01bd;
					}
					frame = 2;
				}
				else
				{
					if (!(text == "purple"))
					{
						goto IL_01bd;
					}
					frame = 3;
				}
			}
			else
			{
				if (!(text == "orange"))
				{
					goto IL_01bd;
				}
				frame = 4;
			}
			goto IL_01bf;
			IL_01bd:
			frame = 0;
			goto IL_01bf;
			IL_01bf:
			sprite.Frame = frame;
			goto IL_01c6;
			IL_0170:
			if (!(text == "blue"))
			{
				goto IL_01bd;
			}
			frame = 1;
			goto IL_01bf;
			IL_0108:
			c = text[0];
			if (c != 'b')
			{
				if (c != 'g' || !(text == "green"))
				{
					goto IL_01bd;
				}
				frame = 5;
			}
			else
			{
				if (!(text == "brown"))
				{
					goto IL_01bd;
				}
				frame = 7;
			}
			goto IL_01bf;
		}
	}
}
