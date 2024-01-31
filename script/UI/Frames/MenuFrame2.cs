// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.MenuFrame2
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class MenuFrame2 : Frame
  {
    private const string PatchTexture = "res://assets/img/ui/frame_menu_2b.png";
    private const string BgMaskTexture = "res://assets/img/ui/frame_menu_2bc_mask.png";
    private const int PatchMargin = 50;
    private const int DefaultContentMarginX = 25;
    private const int DefaultContentMarginY = 15;
    private const float ScaleFactor = 0.25f;
    private static readonly Color BgModulate = UIUtil.MenuBgColor;

    public MenuFrame2()
    {
      this.ContentMarginLeft = 25;
      this.ContentMarginTop = 15;
      this.ContentMarginRight = 25;
      this.ContentMarginBottom = 15;
    }

    public override void _EnterTree()
    {
      SpecialNinePatch specialNinePatch = GDUtil.MakeNode<SpecialNinePatch>("FrameTexture");
      specialNinePatch.Texture = GD.Load<Texture>("res://assets/img/ui/frame_menu_2b.png");
      specialNinePatch.BgTexture = GD.Load<Texture>("res://assets/sprite/common/white.png");
      specialNinePatch.BgMaskTexture = GD.Load<Texture>("res://assets/img/ui/frame_menu_2bc_mask.png");
      specialNinePatch.PatchMarginLeft = 50;
      specialNinePatch.PatchMarginTop = 50;
      specialNinePatch.PatchMarginRight = 50;
      specialNinePatch.PatchMarginBottom = 50;
      specialNinePatch.ScaleFactor = 0.25f;
      specialNinePatch.RectMinSize = this.MinimumSize;
      specialNinePatch.BgModulate = MenuFrame2.BgModulate;
      this.AddChild((Node) specialNinePatch);
      this.Container.SetContainerMarginLeft(this.ContentMarginLeft);
      this.Container.SetContainerMarginTop(this.ContentMarginTop);
      this.Container.SetContainerMarginRight(this.ContentMarginRight);
      this.Container.SetContainerMarginBottom(this.ContentMarginBottom);
      this.AddChild((Node) this.Container);
    }
  }
}
