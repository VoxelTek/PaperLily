using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class LegacyFrame : Frame
	{
		private const string Texture = "res://assets/img/ui/frame_legacy.png";

		private const int PatchMargin = 20;

		private const int DefaultContentMargin = 8;

		private const float ScaleFactor = 0.5f;

		public static readonly Color BgModulate = new Color(0.07f, 0f, 0.04f, 0.9f);

		public LegacyFrame()
		{
			base.ContentMarginLeft = 8;
			base.ContentMarginTop = 8;
			base.ContentMarginRight = 8;
			base.ContentMarginBottom = 8;
		}

		public override void _EnterTree()
		{
			SpecialNinePatch frame = GDUtil.MakeNode<SpecialNinePatch>("FrameTexture");
			frame.Texture = GD.Load<Texture>("res://assets/img/ui/frame_legacy.png");
			frame.BgTexture = GD.Load<Texture>("res://assets/sprite/common/white.png");
			frame.PatchMarginLeft = 20;
			frame.PatchMarginTop = 20;
			frame.PatchMarginRight = 20;
			frame.PatchMarginBottom = 20;
			frame.ScaleFactor = 0.5f;
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
