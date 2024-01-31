// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.LegacyFrame
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class LegacyFrame : Frame
  {
    private const string Texture = "res://assets/img/ui/frame_legacy.png";
    private const int PatchMargin = 20;
    private const int DefaultContentMargin = 8;
    private const float ScaleFactor = 0.5f;
    public static readonly Color BgModulate = new Color(0.07f, 0.0f, 0.04f, 0.9f);

    public LegacyFrame()
    {
      this.ContentMarginLeft = 8;
      this.ContentMarginTop = 8;
      this.ContentMarginRight = 8;
      this.ContentMarginBottom = 8;
    }

    public override void _EnterTree()
    {
      SpecialNinePatch specialNinePatch = GDUtil.MakeNode<SpecialNinePatch>("FrameTexture");
      specialNinePatch.Texture = GD.Load<Godot.Texture>("res://assets/img/ui/frame_legacy.png");
      specialNinePatch.BgTexture = GD.Load<Godot.Texture>("res://assets/sprite/common/white.png");
      specialNinePatch.PatchMarginLeft = 20;
      specialNinePatch.PatchMarginTop = 20;
      specialNinePatch.PatchMarginRight = 20;
      specialNinePatch.PatchMarginBottom = 20;
      specialNinePatch.ScaleFactor = 0.5f;
      specialNinePatch.RectMinSize = this.MinimumSize;
      specialNinePatch.BgModulate = LegacyFrame.BgModulate;
      this.AddChild((Node) specialNinePatch);
      this.Container.SetContainerMarginLeft(this.ContentMarginLeft);
      this.Container.SetContainerMarginTop(this.ContentMarginTop);
      this.Container.SetContainerMarginRight(this.ContentMarginRight);
      this.Container.SetContainerMarginBottom(this.ContentMarginBottom);
      this.AddChild((Node) this.Container);
    }
  }
}
