using Godot;
using LacieEngine.Core;

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
			get
			{
				return _titleText;
			}
			set
			{
				_titleText = value;
				if (nTitleLabel != null)
				{
					nTitleLabel.Text = value;
				}
			}
		}

		public bool TitleVisible
		{
			get
			{
				return _titleVisible;
			}
			set
			{
				_titleVisible = value;
				if (nTitle != null)
				{
					nTitle.Visible = value;
				}
				UpdateMargins();
			}
		}

		public string DividerTexture { get; set; } = "res://assets/img/ui/divider_sm.png";


		public TitledMenuFrame()
		{
			base.Name = "TitledMenuFrame";
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
			AddChild(CreateTitle(_titleText, DividerTexture));
			UpdateMargins();
			AddChild(base.Container);
		}

		private Control CreateTitle(string titleText, string dividerTexture)
		{
			MarginContainer titleContainer = GDUtil.MakeNode<MarginContainer>("TitleContainer");
			titleContainer.SetContainerMarginTop(10);
			titleContainer.SizeFlagsVertical = 0;
			titleContainer.Visible = _titleVisible;
			CenterContainer titleCenterer = GDUtil.MakeNode<CenterContainer>("TitleCenterer");
			VBoxContainer titleVBox = GDUtil.MakeNode<VBoxContainer>("TitleVBox");
			titleVBox.SetSeparation(-5);
			titleVBox.Alignment = BoxContainer.AlignMode.Center;
			Label title = GDUtil.MakeNode<Label>("Title");
			title.SetDefaultFontAndColor();
			title.SetFont("res://resources/font/default_title.tres");
			title.Text = titleText;
			title.Align = Label.AlignEnum.Center;
			nTitleLabel = title;
			TextureRect divider = GDUtil.MakeNode<TextureRect>("Divider");
			divider.Texture = GD.Load<Texture>(dividerTexture);
			divider.RectMinSize = divider.Texture.GetSize() / 3f;
			divider.Expand = true;
			CenterContainer dividerContainer = GDUtil.MakeNode<CenterContainer>("DividerContainer");
			dividerContainer.AddChild(divider);
			titleVBox.AddChild(title);
			titleVBox.AddChild(dividerContainer);
			titleCenterer.AddChild(titleVBox);
			titleContainer.AddChild(titleCenterer);
			nTitle = titleContainer;
			return titleContainer;
		}

		private void UpdateMargins()
		{
			base.Container.SetContainerMarginLeft(base.ContentMarginLeft);
			base.Container.SetContainerMarginTop(base.ContentMarginTop + (_titleVisible ? 25 : 0));
			base.Container.SetContainerMarginRight(base.ContentMarginRight);
			base.Container.SetContainerMarginBottom(base.ContentMarginBottom);
		}
	}
}
