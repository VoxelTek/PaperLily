// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.MenuFrame3
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class MenuFrame3 : Frame
  {
    private const string PatchTexture = "res://assets/img/ui/frame_menu_3.png";
    private const string BgMaskTexture = "res://assets/img/ui/frame_menu_2_mask.png";
    private const int PatchMargin = 50;
    private const int DefaultContentMargin = 10;
    private const float ScaleFactor = 0.25f;
    private static readonly Color BgModulate = UIUtil.MenuBgColor;

    public MenuFrame3()
    {
      this.ContentMarginLeft = 10;
      this.ContentMarginTop = 10;
      this.ContentMarginRight = 10;
      this.ContentMarginBottom = 10;
    }

    public override void _EnterTree()
    {
      SpecialNinePatch specialNinePatch = GDUtil.MakeNode<SpecialNinePatch>("FrameTexture");
      specialNinePatch.Texture = GD.Load<Texture>("res://assets/img/ui/frame_menu_3.png");
      specialNinePatch.BgTexture = GD.Load<Texture>("res://assets/sprite/common/white.png");
      specialNinePatch.BgMaskTexture = GD.Load<Texture>("res://assets/img/ui/frame_menu_2_mask.png");
      specialNinePatch.PatchMarginLeft = 50;
      specialNinePatch.PatchMarginTop = 50;
      specialNinePatch.PatchMarginRight = 50;
      specialNinePatch.PatchMarginBottom = 50;
      specialNinePatch.ScaleFactor = 0.25f;
      specialNinePatch.RectMinSize = this.MinimumSize;
      specialNinePatch.BgModulate = MenuFrame3.BgModulate;
      this.AddChild((Node) specialNinePatch);
      this.Container.SetContainerMarginLeft(this.ContentMarginLeft);
      this.Container.SetContainerMarginTop(this.ContentMarginTop);
      this.Container.SetContainerMarginRight(this.ContentMarginRight);
      this.Container.SetContainerMarginBottom(this.ContentMarginBottom);
      this.AddChild((Node) this.Container);
    }
  }
}
