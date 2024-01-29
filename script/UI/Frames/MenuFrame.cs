using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class MenuFrame : Frame
	{
		private const string PatchTexture = "res://assets/img/ui/frame_menu_2.png";

		private const string BgMaskTexture = "res://assets/img/ui/frame_menu_2_mask.png";

		private const int PatchMargin = 50;

		private const int DefaultContentMarginX = 25;

		private const int DefaultContentMarginY = 15;

		private const float ScaleFactor = 0.25f;

		private static readonly Color BgModulate = UIUtil.MenuBgColor;

		public MenuFrame()
		{
			base.Name = "MenuFrame";
			base.ContentMarginLeft = 25;
			base.ContentMarginTop = 15;
			base.ContentMarginRight = 25;
			base.ContentMarginBottom = 15;
		}

		public override void _EnterTree()
		{
			SpecialNinePatch frame = GDUtil.MakeNode<SpecialNinePatch>("FrameTexture");
			frame.Texture = GD.Load<Texture>("res://assets/img/ui/frame_menu_2.png");
			frame.BgTexture = GD.Load<Texture>("res://assets/sprite/common/white.png");
			frame.BgMaskTexture = GD.Load<Texture>("res://assets/img/ui/frame_menu_2_mask.png");
			frame.PatchMarginLeft = 50;
			frame.PatchMarginTop = 50;
			frame.PatchMarginRight = 50;
			frame.PatchMarginBottom = 50;
			frame.ScaleFactor = 0.25f;
			frame.RectMinSize = base.MinimumSize;
			frame.BgModulate = BgModulate;
			if (base.DecorBgTexture != null)
			{
				frame.DecorBgTexture = GD.Load<Texture>(base.DecorBgTexture);
			}
			frame.DecorBgAlignment = base.DecorBgAlignment;
			AddChild(frame);
			base.Container.SetContainerMarginLeft(base.ContentMarginLeft);
			base.Container.SetContainerMarginTop(base.ContentMarginTop);
			base.Container.SetContainerMarginRight(base.ContentMarginRight);
			base.Container.SetContainerMarginBottom(base.ContentMarginBottom);
			AddChild(base.Container);
		}
	}
}
