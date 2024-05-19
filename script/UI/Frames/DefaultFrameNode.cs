// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.DefaultFrameNode
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class DefaultFrameNode : Frame
  {
	[Export(PropertyHint.None, "")]
	private Texture TexFrameBorder { get; set; }

	[Export(PropertyHint.None, "")]
	private Texture TexFrameBg { get; set; }

	[Export(PropertyHint.None, "")]
	private Texture TexFrameBgMask { get; set; }

	[Export(PropertyHint.None, "")]
	private Color FrameBgModulate { get; set; } = Godot.Colors.White;

	[Export(PropertyHint.None, "")]
	private int PatchMarginLeft { get; set; }

	[Export(PropertyHint.None, "")]
	private int PatchMarginTop { get; set; }

	[Export(PropertyHint.None, "")]
	private int PatchMarginRight { get; set; }

	[Export(PropertyHint.None, "")]
	private int PatchMarginBottom { get; set; }

	[Export(PropertyHint.None, "")]
	private float PatchScaleFactor { get; set; } = 1f;

	public override void _EnterTree()
	{
	  SpecialNinePatch specialNinePatch = GDUtil.MakeNode<SpecialNinePatch>("FrameTexture");
	  specialNinePatch.Texture = this.TexFrameBorder;
	  specialNinePatch.BgTexture = this.TexFrameBg;
	  specialNinePatch.BgModulate = this.FrameBgModulate;
	  specialNinePatch.BgMaskTexture = this.TexFrameBgMask;
	  specialNinePatch.PatchMarginLeft = this.PatchMarginLeft;
	  specialNinePatch.PatchMarginTop = this.PatchMarginTop;
	  specialNinePatch.PatchMarginRight = this.PatchMarginRight;
	  specialNinePatch.PatchMarginBottom = this.PatchMarginBottom;
	  specialNinePatch.ScaleFactor = this.PatchScaleFactor;
	  specialNinePatch.RectMinSize = this.MinimumSize;
	  this.AddChild((Node) specialNinePatch);
	  this.Container.SetContainerMarginLeft(this.ContentMarginLeft);
	  this.Container.SetContainerMarginTop(this.ContentMarginTop);
	  this.Container.SetContainerMarginRight(this.ContentMarginRight);
	  this.Container.SetContainerMarginBottom(this.ContentMarginBottom);
	  this.AddChild((Node) this.Container);
	}
  }
}
