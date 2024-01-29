using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class DialogueFrameNode : Frame
	{
		private static readonly Vector2 ContinueIndicatorSize = new Vector2(30f, 30f);

		[Export(PropertyHint.None, "")]
		public Texture TexFrame { get; set; }

		[Export(PropertyHint.None, "")]
		public Texture TexNameSeparator { get; set; }

		[Export(PropertyHint.None, "")]
		public Color FrameModulate { get; set; } = Colors.White;


		[Export(PropertyHint.None, "")]
		public float TextureScaleFactor { get; set; } = 1f;


		[Export(PropertyHint.None, "")]
		public Vector2 NamePlateSize { get; set; }

		[Export(PropertyHint.None, "")]
		public Vector2 NamePlateTextPosition { get; set; }

		[Export(PropertyHint.None, "")]
		public Vector2 NameSeparatorPosition { get; set; }

		public TextureRect Background { get; private set; }

		public Control NamePlate { get; private set; }

		public Label SpeakerName { get; private set; }

		public TextureRect NameSeparator { get; private set; }

		public RichTextLabel DialogueText { get; private set; }

		public TextureRect ContinueIndicator { get; private set; }

		public override void _EnterTree()
		{
			Texture texture = TexFrame;
			Background = GDUtil.MakeNode<TextureRect>("FrameTexture");
			Background.Expand = true;
			Background.Texture = texture;
			Background.Modulate = FrameModulate;
			Background.RectMinSize = texture.GetSize() / TextureScaleFactor;
			AddChild(Background);
			base.Container.SetContainerMarginLeft(base.ContentMarginLeft);
			base.Container.SetContainerMarginTop(base.ContentMarginTop);
			base.Container.SetContainerMarginRight(base.ContentMarginRight);
			base.Container.SetContainerMarginBottom(base.ContentMarginBottom);
			AddChild(base.Container);
			NamePlate = GDUtil.MakeNode<Control>("NamePlateContainer");
			NamePlate.RectMinSize = NamePlateSize;
			SpeakerName = GDUtil.MakeNode<Label>("NameLabel");
			SpeakerName.SetDefaultFontAndColor();
			SpeakerName.SetFont("res://resources/font/default_title.tres");
			SpeakerName.RectPosition = NamePlateTextPosition;
			NameSeparator = GDUtil.MakeNode<TextureRect>("NameSeparator");
			Texture nameSeparatorTexture = TexNameSeparator;
			NameSeparator.Texture = nameSeparatorTexture;
			NameSeparator.Expand = true;
			NameSeparator.RectMinSize = nameSeparatorTexture.GetSize() / TextureScaleFactor;
			NameSeparator.RectPosition = NameSeparatorPosition;
			NamePlate.AddChild(SpeakerName);
			NamePlate.AddChild(NameSeparator);
			DialogueText = GDUtil.MakeNode<RichTextLabel>("DialogueText");
			DialogueText.SizeFlagsVertical = 3;
			DialogueText.ScrollActive = false;
			DialogueText.BbcodeEnabled = true;
			DialogueText.SetDefaultFontAndColor();
			Control textBoxContainer = GDUtil.MakeNode<VBoxContainer>("TextBoxContainer");
			textBoxContainer.AddChild(NamePlate);
			textBoxContainer.AddChild(DialogueText);
			AddToFrame(textBoxContainer);
			Control continueIndicatorContainer = GDUtil.MakeNode<Control>("ContinueIndicatorContainer");
			continueIndicatorContainer.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			ContinueIndicator = MakeContinueIndicator();
			ContinueIndicator.GrowHorizontal = GrowDirection.Both;
			continueIndicatorContainer.AddChild(ContinueIndicator);
			AddToFrame(continueIndicatorContainer);
		}

		private AnimatedTextureRect MakeContinueIndicator()
		{
			AnimatedTextureRect continueIndicator = GDUtil.MakeNode<AnimatedTextureRect>("ContinueIndicator");
			continueIndicator.Texture = GD.Load<Texture>("res://assets/img/ui/continue_indicator.png");
			continueIndicator.Hframes = 3;
			continueIndicator.FPS = 3f;
			continueIndicator.AnimationFrames = new int[4] { 0, 1, 2, 1 };
			continueIndicator.Expand = true;
			continueIndicator.RectMinSize = ContinueIndicatorSize;
			continueIndicator.SetAnchorsGrowFrom(1f, 1f, Direction.UpLeft);
			return continueIndicator;
		}
	}
}
