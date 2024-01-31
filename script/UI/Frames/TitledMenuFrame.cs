// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.TitledMenuFrame
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class TitledMenuFrame : Frame
  {
    private const string PatchTexture = "res://assets/img/ui/frame_menu_2.png";
    private const string BgMaskTexture = "res://assets/img/ui/frame_menu_2_mask.png";
    private const int PatchMargin = 50;
    private const int DefaultContentMarginX = 25;
    private const int DefaultContentMarginY = 15;
    private const int TitleSpace = 25;
    private const float ScaleFactor = 0.25f;
    private static readonly Color BgModulate = UIUtil.MenuBgColor;
    private string _titleText = "Title";
    private bool _titleVisible = true;
    private Control nTitle;
    private Label nTitleLabel;

    public string TitleText
    {
      get => this._titleText;
      set
      {
        this._titleText = value;
        if (this.nTitleLabel == null)
          return;
        this.nTitleLabel.Text = value;
      }
    }

    public bool TitleVisible
    {
      get => this._titleVisible;
      set
      {
        this._titleVisible = value;
        if (this.nTitle != null)
          this.nTitle.Visible = value;
        this.UpdateMargins();
      }
    }

    public string DividerTexture { get; set; } = "res://assets/img/ui/divider_sm.png";

    public TitledMenuFrame()
    {
      this.Name = nameof (TitledMenuFrame);
      this.ContentMarginLeft = 25;
      this.ContentMarginTop = 15;
      this.ContentMarginRight = 25;
      this.ContentMarginBottom = 15;
    }

    public override void _EnterTree()
    {
      SpecialNinePatch specialNinePatch = GDUtil.MakeNode<SpecialNinePatch>("FrameTexture");
      specialNinePatch.Texture = GD.Load<Texture>("res://assets/img/ui/frame_menu_2.png");
      specialNinePatch.BgTexture = GD.Load<Texture>("res://assets/sprite/common/white.png");
      specialNinePatch.BgMaskTexture = GD.Load<Texture>("res://assets/img/ui/frame_menu_2_mask.png");
      specialNinePatch.PatchMarginLeft = 50;
      specialNinePatch.PatchMarginTop = 50;
      specialNinePatch.PatchMarginRight = 50;
      specialNinePatch.PatchMarginBottom = 50;
      specialNinePatch.ScaleFactor = 0.25f;
      specialNinePatch.RectMinSize = this.MinimumSize;
      specialNinePatch.BgModulate = TitledMenuFrame.BgModulate;
      if (this.DecorBgTexture != null)
        specialNinePatch.DecorBgTexture = GD.Load<Texture>(this.DecorBgTexture);
      specialNinePatch.DecorBgAlignment = this.DecorBgAlignment;
      this.AddChild((Node) specialNinePatch);
      this.AddChild((Node) this.CreateTitle(this._titleText, this.DividerTexture));
      this.UpdateMargins();
      this.AddChild((Node) this.Container);
    }

    private Control CreateTitle(string titleText, string dividerTexture)
    {
      MarginContainer container = GDUtil.MakeNode<MarginContainer>("TitleContainer");
      container.SetContainerMarginTop(10);
      container.SizeFlagsVertical = 0;
      container.Visible = this._titleVisible;
      CenterContainer centerContainer1 = GDUtil.MakeNode<CenterContainer>("TitleCenterer");
      VBoxContainer box = GDUtil.MakeNode<VBoxContainer>("TitleVBox");
      box.SetSeparation(-5);
      box.Alignment = BoxContainer.AlignMode.Center;
      Label label = GDUtil.MakeNode<Label>("Title");
      label.SetDefaultFontAndColor();
      label.SetFont("res://resources/font/default_title.tres");
      label.Text = titleText;
      label.Align = Label.AlignEnum.Center;
      this.nTitleLabel = label;
      TextureRect textureRect = GDUtil.MakeNode<TextureRect>("Divider");
      textureRect.Texture = GD.Load<Texture>(dividerTexture);
      textureRect.RectMinSize = textureRect.Texture.GetSize() / 3f;
      textureRect.Expand = true;
      CenterContainer centerContainer2 = GDUtil.MakeNode<CenterContainer>("DividerContainer");
      centerContainer2.AddChild((Node) textureRect);
      box.AddChild((Node) label);
      box.AddChild((Node) centerContainer2);
      centerContainer1.AddChild((Node) box);
      container.AddChild((Node) centerContainer1);
      this.nTitle = (Control) container;
      return (Control) container;
    }

    private void UpdateMargins()
    {
      this.Container.SetContainerMarginLeft(this.ContentMarginLeft);
      this.Container.SetContainerMarginTop(this.ContentMarginTop + (this._titleVisible ? 25 : 0));
      this.Container.SetContainerMarginRight(this.ContentMarginRight);
      this.Container.SetContainerMarginBottom(this.ContentMarginBottom);
    }
  }
}
