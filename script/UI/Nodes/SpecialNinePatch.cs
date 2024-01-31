// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SpecialNinePatch
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class SpecialNinePatch : Control
  {
    public Texture Texture;
    public Texture BgTexture;
    public Texture BgMaskTexture;
    public Texture DecorTexture;
    public Texture DecorBgTexture;
    public Control.LayoutPreset DecorBgAlignment;
    public int PatchMarginLeft;
    public int PatchMarginTop;
    public int PatchMarginRight;
    public int PatchMarginBottom;
    public int DecorMarginTop;
    public int DecorMarginBottom;
    public float ScaleFactor = 1f;
    public Color BgModulate = Godot.Colors.White;
    public bool DrawCenter;
    private Viewport nViewport;
    private Viewport nMaskViewport;
    private Light2D nBgMask;

    public override void _EnterTree() => this.Render();

    public override void _Ready()
    {
      int num = (int) this.GetParent<Control>().Connect("resized", (Object) this, "UpdateSize");
      this.UpdateSize();
    }

    public void UpdateSize()
    {
      this.nViewport.Size = this.GetParent<Control>().RectSize * UIUtil.GetScaleFactor();
      this.nViewport.SetSizeOverride(true, new Vector2?(this.GetParent<Control>().RectSize));
      if (this.nMaskViewport == null)
        return;
      this.nMaskViewport.Size = this.GetParent<Control>().RectSize;
      this.nBgMask.Position = this.nMaskViewport.GetTexture().GetSize() / 2f;
    }

    private void Render()
    {
      Control control1 = this.MakeNinePatch(this.Texture, this.BgTexture, this.DrawCenter);
      if (this.DecorTexture != null)
      {
        TextureRect control2 = GDUtil.MakeNode<TextureRect>("DecorTop");
        control2.Expand = true;
        control2.Texture = this.DecorTexture;
        control2.RectMinSize = this.DecorTexture.GetSize() * this.ScaleFactor;
        control2.SetAnchorsAndMarginsPreset(Control.LayoutPreset.CenterTop, this.DecorTexture.GetSize() * this.ScaleFactor);
        TextureRect textureRect1 = control2;
        textureRect1.RectPosition = textureRect1.RectPosition + new Vector2(0.0f, (float) this.DecorMarginTop);
        TextureRect control3 = GDUtil.MakeNode<TextureRect>("DecorBottom");
        control3.Expand = true;
        control3.Texture = this.DecorTexture;
        control3.RectMinSize = this.DecorTexture.GetSize() * this.ScaleFactor;
        control3.SetAnchorsAndMarginsPreset(Control.LayoutPreset.CenterBottom, this.DecorTexture.GetSize() * this.ScaleFactor);
        TextureRect textureRect2 = control3;
        textureRect2.RectPosition = textureRect2.RectPosition - new Vector2(0.0f, (float) this.DecorMarginBottom);
        control3.FlipV = true;
        control1.AddChild((Node) control2);
        control1.AddChild((Node) control3);
      }
      if (this.BgMaskTexture != null)
      {
        Control control4 = this.MakeNinePatch(this.BgMaskTexture, (Texture) null, true);
        this.nMaskViewport = GDUtil.MakeNode<Viewport>("MaskViewport");
        this.nMaskViewport.TransparentBg = true;
        this.nMaskViewport.RenderTargetVFlip = true;
        this.nMaskViewport.SizeOverrideStretch = true;
        this.nMaskViewport.RenderTargetUpdateMode = Viewport.UpdateMode.Always;
        this.nMaskViewport.AddChild((Node) control4);
        this.nBgMask = GDUtil.MakeNode<Light2D>("Mask");
        this.nBgMask.Mode = Light2D.ModeEnum.Mix;
        this.nBgMask.RangeItemCullMask = 524288;
        this.nBgMask.Texture = (Texture) this.nMaskViewport.GetTexture();
        this.nBgMask.Texture.Flags = 4U;
        this.nBgMask.Position = this.nMaskViewport.GetTexture().GetSize() / 2f;
        this.nBgMask.AddChild((Node) this.nMaskViewport);
        control1.AddChild((Node) this.nBgMask);
      }
      this.nViewport = GDUtil.MakeNode<Viewport>("NinePatchViewport");
      this.nViewport.TransparentBg = true;
      this.nViewport.RenderTargetVFlip = true;
      this.nViewport.SizeOverrideStretch = true;
      this.nViewport.AddChild((Node) control1);
      TextureRect textureRect = GDUtil.MakeNode<TextureRect>("NinePatchTexture");
      textureRect.Expand = true;
      textureRect.SetAnchorsPreset(Control.LayoutPreset.Wide);
      textureRect.Texture = (Texture) this.nViewport.GetTexture();
      textureRect.AddChild((Node) this.nViewport);
      this.AddChild((Node) textureRect);
    }

    private Control MakeNinePatch(Texture texture, Texture bgTexture, bool drawCenter)
    {
      Control control1 = GDUtil.MakeNode<Control>("Frame");
      control1.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      if (bgTexture != null)
      {
        TextureRect textureRect = GDUtil.MakeNode<TextureRect>("BG");
        textureRect.Expand = true;
        textureRect.Texture = bgTexture;
        textureRect.Modulate = this.BgModulate;
        if (this.BgMaskTexture != null)
        {
          textureRect.LightMask = 524288;
          textureRect.Material = GD.Load<Material>("res://resources/material/light_only.tres");
        }
        textureRect.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
        control1.AddChild((Node) textureRect);
      }
      if (this.DecorBgTexture != null)
      {
        TextureRect control2 = GDUtil.MakeNode<TextureRect>("DecorBg");
        control2.Expand = true;
        control2.Texture = this.DecorBgTexture;
        control2.RectMinSize = this.DecorBgTexture.GetSize() * this.ScaleFactor;
        control2.SetAnchorsAndMarginsPreset(this.DecorBgAlignment, this.DecorBgTexture.GetSize() * this.ScaleFactor);
        control1.AddChild((Node) control2);
      }
      GridContainer box = GDUtil.MakeNode<GridContainer>("Grid");
      box.Columns = 3;
      box.SetSeparation(0, 0);
      box.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      Control[] controlArray = new Control[9];
      for (int index = 0; index < controlArray.Length; ++index)
      {
        controlArray[index] = GDUtil.MakeNode<Control>("Cell" + index.ToString());
        box.AddChild((Node) controlArray[index]);
      }
      float x = texture.GetSize().x - (float) this.PatchMarginRight;
      float y = texture.GetSize().y - (float) this.PatchMarginBottom;
      float width = texture.GetSize().x - (float) this.PatchMarginLeft - (float) this.PatchMarginRight;
      float height = texture.GetSize().y - (float) this.PatchMarginTop - (float) this.PatchMarginBottom;
      controlArray[0].RectMinSize = new Vector2((float) this.PatchMarginLeft, (float) this.PatchMarginTop) * this.ScaleFactor;
      controlArray[1].RectMinSize = new Vector2(0.0f, (float) this.PatchMarginTop) * this.ScaleFactor;
      controlArray[2].RectMinSize = new Vector2((float) this.PatchMarginRight, (float) this.PatchMarginTop) * this.ScaleFactor;
      controlArray[3].RectMinSize = new Vector2((float) this.PatchMarginLeft, 0.0f) * this.ScaleFactor;
      controlArray[4].SizeFlagsHorizontal = 3;
      controlArray[4].SizeFlagsVertical = 3;
      controlArray[5].RectMinSize = new Vector2((float) this.PatchMarginRight, 0.0f) * this.ScaleFactor;
      controlArray[6].RectMinSize = new Vector2((float) this.PatchMarginLeft, (float) this.PatchMarginBottom) * this.ScaleFactor;
      controlArray[7].RectMinSize = new Vector2(0.0f, (float) this.PatchMarginBottom) * this.ScaleFactor;
      controlArray[8].RectMinSize = new Vector2((float) this.PatchMarginRight, (float) this.PatchMarginBottom) * this.ScaleFactor;
      control1.AddChild((Node) box);
      controlArray[0].AddChild((Node) this.MakeTextureFragment("CornerTL", texture, new Rect2(0.0f, 0.0f, (float) this.PatchMarginLeft, (float) this.PatchMarginTop)));
      controlArray[1].AddChild((Node) this.MakeTextureFragment("EdgeT", texture, new Rect2((float) this.PatchMarginLeft, 0.0f, width, (float) this.PatchMarginTop)));
      controlArray[2].AddChild((Node) this.MakeTextureFragment("CornerTR", texture, new Rect2(x, 0.0f, (float) this.PatchMarginRight, (float) this.PatchMarginTop)));
      controlArray[3].AddChild((Node) this.MakeTextureFragment("EdgeL", texture, new Rect2(0.0f, (float) this.PatchMarginTop, (float) this.PatchMarginLeft, height)));
      if (drawCenter)
        controlArray[4].AddChild((Node) this.MakeTextureFragment("Center", texture, new Rect2((float) this.PatchMarginLeft, (float) this.PatchMarginTop, width, height)));
      controlArray[5].AddChild((Node) this.MakeTextureFragment("EdgeR", texture, new Rect2(x, (float) this.PatchMarginTop, (float) this.PatchMarginRight, height)));
      controlArray[6].AddChild((Node) this.MakeTextureFragment("CornerBL", texture, new Rect2(0.0f, y, (float) this.PatchMarginLeft, (float) this.PatchMarginBottom)));
      controlArray[7].AddChild((Node) this.MakeTextureFragment("EdgeB", texture, new Rect2((float) this.PatchMarginLeft, y, width, (float) this.PatchMarginBottom)));
      controlArray[8].AddChild((Node) this.MakeTextureFragment("CornerBR", texture, new Rect2(x, y, (float) this.PatchMarginRight, (float) this.PatchMarginBottom)));
      return control1;
    }

    private TextureRect MakeTextureFragment(string name, Texture texture, Rect2 region)
    {
      TextureRect textureRect = GDUtil.MakeNode<TextureRect>(name);
      AtlasTexture atlasTexture = new AtlasTexture();
      atlasTexture.Atlas = texture;
      atlasTexture.Region = region;
      textureRect.Expand = true;
      textureRect.Texture = (Texture) atlasTexture;
      textureRect.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      textureRect.LightMask = 0;
      return textureRect;
    }
  }
}
