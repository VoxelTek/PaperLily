// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.InfoBoxFrame
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class InfoBoxFrame : Frame
  {
    private const string PatchTexture = "res://assets/img/ui/frame_infobox.png";
    private const string BgMaskTexture = "res://assets/img/ui/frame_menu_2_bg_mask.png";
    private const int PatchMargin = 40;
    private const int DefaultContentMargin = 10;
    private const float ScaleFactor = 0.33f;
    private static readonly Color BgModulate = UIUtil.MenuBgColor;

    public InfoBoxFrame()
    {
      this.ContentMarginLeft = 10;
      this.ContentMarginTop = 10;
      this.ContentMarginRight = 10;
      this.ContentMarginBottom = 10;
    }

    public override void _EnterTree()
    {
      SpecialNinePatch specialNinePatch = GDUtil.MakeNode<SpecialNinePatch>("FrameTexture");
      specialNinePatch.Texture = GD.Load<Texture>("res://assets/img/ui/frame_infobox.png");
      specialNinePatch.BgTexture = GD.Load<Texture>("res://assets/sprite/common/white.png");
      specialNinePatch.BgMaskTexture = GD.Load<Texture>("res://assets/img/ui/frame_menu_2_bg_mask.png");
      specialNinePatch.PatchMarginLeft = 40;
      specialNinePatch.PatchMarginTop = 40;
      specialNinePatch.PatchMarginRight = 40;
      specialNinePatch.PatchMarginBottom = 40;
      specialNinePatch.ScaleFactor = 0.33f;
      specialNinePatch.RectMinSize = this.MinimumSize;
      specialNinePatch.BgModulate = InfoBoxFrame.BgModulate;
      this.AddChild((Node) specialNinePatch);
      this.Container.SetContainerMarginLeft(this.ContentMarginLeft);
      this.Container.SetContainerMarginTop(this.ContentMarginTop);
      this.Container.SetContainerMarginRight(this.ContentMarginRight);
      this.Container.SetContainerMarginBottom(this.ContentMarginBottom);
      this.AddChild((Node) this.Container);
    }
  }
}
