using Godot;
using LacieEngine.Core;

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
		private Color FrameBgModulate { get; set; } = Colors.White;


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
			SpecialNinePatch frame = GDUtil.MakeNode<SpecialNinePatch>("FrameTexture");
			frame.Texture = TexFrameBorder;
			frame.BgTexture = TexFrameBg;
			frame.BgModulate = FrameBgModulate;
			frame.BgMaskTexture = TexFrameBgMask;
			frame.PatchMarginLeft = PatchMarginLeft;
			frame.PatchMarginTop = PatchMarginTop;
			frame.PatchMarginRight = PatchMarginRight;
			frame.PatchMarginBottom = PatchMarginBottom;
			frame.ScaleFactor = PatchScaleFactor;
			frame.RectMinSize = base.MinimumSize;
			AddChild(frame);
			base.Container.SetContainerMarginLeft(base.ContentMarginLeft);
			base.Container.SetContainerMarginTop(base.ContentMarginTop);
			base.Container.SetContainerMarginRight(base.ContentMarginRight);
			base.Container.SetContainerMarginBottom(base.ContentMarginBottom);
			AddChild(base.Container);
		}
	}
}
