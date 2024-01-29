using Godot;
using LacieEngine.Core;

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
			base.ContentMarginLeft = 10;
			base.ContentMarginTop = 10;
			base.ContentMarginRight = 10;
			base.ContentMarginBottom = 10;
		}

		public override void _EnterTree()
		{
			SpecialNinePatch frame = GDUtil.MakeNode<SpecialNinePatch>("FrameTexture");
			frame.Texture = GD.Load<Texture>("res://assets/img/ui/frame_infobox.png");
			frame.BgTexture = GD.Load<Texture>("res://assets/sprite/common/white.png");
			frame.BgMaskTexture = GD.Load<Texture>("res://assets/img/ui/frame_menu_2_bg_mask.png");
			frame.PatchMarginLeft = 40;
			frame.PatchMarginTop = 40;
			frame.PatchMarginRight = 40;
			frame.PatchMarginBottom = 40;
			frame.ScaleFactor = 0.33f;
			frame.RectMinSize = base.MinimumSize;
			frame.BgModulate = BgModulate;
			AddChild(frame);
			base.Container.SetContainerMarginLeft(base.ContentMarginLeft);
			base.Container.SetContainerMarginTop(base.ContentMarginTop);
			base.Container.SetContainerMarginRight(base.ContentMarginRight);
			base.Container.SetContainerMarginBottom(base.ContentMarginBottom);
			AddChild(base.Container);
		}
	}
}
