using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class SpecialNinePatch : Control
	{
		public Texture Texture;

		public Texture BgTexture;

		public Texture BgMaskTexture;

		public Texture DecorTexture;

		public Texture DecorBgTexture;

		public LayoutPreset DecorBgAlignment;

		public int PatchMarginLeft;

		public int PatchMarginTop;

		public int PatchMarginRight;

		public int PatchMarginBottom;

		public int DecorMarginTop;

		public int DecorMarginBottom;

		public float ScaleFactor = 1f;

		public Color BgModulate = Colors.White;

		public bool DrawCenter;

		private Viewport nViewport;

		private Viewport nMaskViewport;

		private Light2D nBgMask;

		public override void _EnterTree()
		{
			Render();
		}

		public override void _Ready()
		{
			GetParent<Control>().Connect("resized", this, "UpdateSize");
			UpdateSize();
		}

		public void UpdateSize()
		{
			nViewport.Size = GetParent<Control>().RectSize * UIUtil.GetScaleFactor();
			nViewport.SetSizeOverride(enable: true, GetParent<Control>().RectSize);
			if (nMaskViewport != null)
			{
				nMaskViewport.Size = GetParent<Control>().RectSize;
				nBgMask.Position = nMaskViewport.GetTexture().GetSize() / 2f;
			}
		}

		private void Render()
		{
			Control frame = MakeNinePatch(Texture, BgTexture, DrawCenter);
			if (DecorTexture != null)
			{
				TextureRect decorTop = GDUtil.MakeNode<TextureRect>("DecorTop");
				decorTop.Expand = true;
				decorTop.Texture = DecorTexture;
				decorTop.RectMinSize = DecorTexture.GetSize() * ScaleFactor;
				decorTop.SetAnchorsAndMarginsPreset(LayoutPreset.CenterTop, DecorTexture.GetSize() * ScaleFactor);
				decorTop.RectPosition += new Vector2(0f, DecorMarginTop);
				TextureRect decorBottom = GDUtil.MakeNode<TextureRect>("DecorBottom");
				decorBottom.Expand = true;
				decorBottom.Texture = DecorTexture;
				decorBottom.RectMinSize = DecorTexture.GetSize() * ScaleFactor;
				decorBottom.SetAnchorsAndMarginsPreset(LayoutPreset.CenterBottom, DecorTexture.GetSize() * ScaleFactor);
				decorBottom.RectPosition -= new Vector2(0f, DecorMarginBottom);
				decorBottom.FlipV = true;
				frame.AddChild(decorTop);
				frame.AddChild(decorBottom);
			}
			if (BgMaskTexture != null)
			{
				Control maskFrame = MakeNinePatch(BgMaskTexture, null, drawCenter: true);
				nMaskViewport = GDUtil.MakeNode<Viewport>("MaskViewport");
				nMaskViewport.TransparentBg = true;
				nMaskViewport.RenderTargetVFlip = true;
				nMaskViewport.SizeOverrideStretch = true;
				nMaskViewport.RenderTargetUpdateMode = Viewport.UpdateMode.Always;
				nMaskViewport.AddChild(maskFrame);
				nBgMask = GDUtil.MakeNode<Light2D>("Mask");
				nBgMask.Mode = Light2D.ModeEnum.Mix;
				nBgMask.RangeItemCullMask = 524288;
				nBgMask.Texture = nMaskViewport.GetTexture();
				nBgMask.Texture.Flags = 4u;
				nBgMask.Position = nMaskViewport.GetTexture().GetSize() / 2f;
				nBgMask.AddChild(nMaskViewport);
				frame.AddChild(nBgMask);
			}
			nViewport = GDUtil.MakeNode<Viewport>("NinePatchViewport");
			nViewport.TransparentBg = true;
			nViewport.RenderTargetVFlip = true;
			nViewport.SizeOverrideStretch = true;
			nViewport.AddChild(frame);
			TextureRect frameTexture = GDUtil.MakeNode<TextureRect>("NinePatchTexture");
			frameTexture.Expand = true;
			frameTexture.SetAnchorsPreset(LayoutPreset.Wide);
			frameTexture.Texture = nViewport.GetTexture();
			frameTexture.AddChild(nViewport);
			AddChild(frameTexture);
		}

		private Control MakeNinePatch(Texture texture, Texture bgTexture, bool drawCenter)
		{
			Control nFrame = GDUtil.MakeNode<Control>("Frame");
			nFrame.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			if (bgTexture != null)
			{
				TextureRect bg = GDUtil.MakeNode<TextureRect>("BG");
				bg.Expand = true;
				bg.Texture = bgTexture;
				bg.Modulate = BgModulate;
				if (BgMaskTexture != null)
				{
					bg.LightMask = 524288;
					bg.Material = GD.Load<Material>("res://resources/material/light_only.tres");
				}
				bg.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
				nFrame.AddChild(bg);
			}
			if (DecorBgTexture != null)
			{
				TextureRect decorBg = GDUtil.MakeNode<TextureRect>("DecorBg");
				decorBg.Expand = true;
				decorBg.Texture = DecorBgTexture;
				decorBg.RectMinSize = DecorBgTexture.GetSize() * ScaleFactor;
				decorBg.SetAnchorsAndMarginsPreset(DecorBgAlignment, DecorBgTexture.GetSize() * ScaleFactor);
				nFrame.AddChild(decorBg);
			}
			GridContainer grid = GDUtil.MakeNode<GridContainer>("Grid");
			grid.Columns = 3;
			grid.SetSeparation(0, 0);
			grid.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			Control[] nGridCells = new Control[9];
			for (int i = 0; i < nGridCells.Length; i++)
			{
				nGridCells[i] = GDUtil.MakeNode<Control>("Cell" + i);
				grid.AddChild(nGridCells[i]);
			}
			float patchRightSideStart = texture.GetSize().x - (float)PatchMarginRight;
			float patchBottomSideStart = texture.GetSize().y - (float)PatchMarginBottom;
			float patchMiddleSizeX = texture.GetSize().x - (float)PatchMarginLeft - (float)PatchMarginRight;
			float patchMiddleSizeY = texture.GetSize().y - (float)PatchMarginTop - (float)PatchMarginBottom;
			nGridCells[0].RectMinSize = new Vector2(PatchMarginLeft, PatchMarginTop) * ScaleFactor;
			nGridCells[1].RectMinSize = new Vector2(0f, PatchMarginTop) * ScaleFactor;
			nGridCells[2].RectMinSize = new Vector2(PatchMarginRight, PatchMarginTop) * ScaleFactor;
			nGridCells[3].RectMinSize = new Vector2(PatchMarginLeft, 0f) * ScaleFactor;
			nGridCells[4].SizeFlagsHorizontal = 3;
			nGridCells[4].SizeFlagsVertical = 3;
			nGridCells[5].RectMinSize = new Vector2(PatchMarginRight, 0f) * ScaleFactor;
			nGridCells[6].RectMinSize = new Vector2(PatchMarginLeft, PatchMarginBottom) * ScaleFactor;
			nGridCells[7].RectMinSize = new Vector2(0f, PatchMarginBottom) * ScaleFactor;
			nGridCells[8].RectMinSize = new Vector2(PatchMarginRight, PatchMarginBottom) * ScaleFactor;
			nFrame.AddChild(grid);
			nGridCells[0].AddChild(MakeTextureFragment("CornerTL", texture, new Rect2(0f, 0f, PatchMarginLeft, PatchMarginTop)));
			nGridCells[1].AddChild(MakeTextureFragment("EdgeT", texture, new Rect2(PatchMarginLeft, 0f, patchMiddleSizeX, PatchMarginTop)));
			nGridCells[2].AddChild(MakeTextureFragment("CornerTR", texture, new Rect2(patchRightSideStart, 0f, PatchMarginRight, PatchMarginTop)));
			nGridCells[3].AddChild(MakeTextureFragment("EdgeL", texture, new Rect2(0f, PatchMarginTop, PatchMarginLeft, patchMiddleSizeY)));
			if (drawCenter)
			{
				nGridCells[4].AddChild(MakeTextureFragment("Center", texture, new Rect2(PatchMarginLeft, PatchMarginTop, patchMiddleSizeX, patchMiddleSizeY)));
			}
			nGridCells[5].AddChild(MakeTextureFragment("EdgeR", texture, new Rect2(patchRightSideStart, PatchMarginTop, PatchMarginRight, patchMiddleSizeY)));
			nGridCells[6].AddChild(MakeTextureFragment("CornerBL", texture, new Rect2(0f, patchBottomSideStart, PatchMarginLeft, PatchMarginBottom)));
			nGridCells[7].AddChild(MakeTextureFragment("EdgeB", texture, new Rect2(PatchMarginLeft, patchBottomSideStart, patchMiddleSizeX, PatchMarginBottom)));
			nGridCells[8].AddChild(MakeTextureFragment("CornerBR", texture, new Rect2(patchRightSideStart, patchBottomSideStart, PatchMarginRight, PatchMarginBottom)));
			return nFrame;
		}

		private TextureRect MakeTextureFragment(string name, Texture texture, Rect2 region)
		{
			TextureRect textureRect = GDUtil.MakeNode<TextureRect>(name);
			AtlasTexture atlasTex = new AtlasTexture
			{
				Atlas = texture,
				Region = region
			};
			textureRect.Expand = true;
			textureRect.Texture = atlasTex;
			textureRect.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			textureRect.LightMask = 0;
			return textureRect;
		}
	}
}
