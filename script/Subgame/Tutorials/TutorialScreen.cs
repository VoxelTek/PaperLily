using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.Modules.Tutorials
{
	public class TutorialScreen : Control
	{
		private string[] HandledInputs = new string[1] { "input_action" };

		private AnimatedTextureRect nImage;

		private LabelWithInputIcons nText;

		private LabelWithInputIcons nOkayText;

		private ColorRect nBackground;

		private static readonly Vector2 TextureSlices = new Vector2(1f, 2f);

		private const int TextureDownscale = 4;

		public Tutorial Tutorial { get; set; }

		public override void _EnterTree()
		{
			base.Name = "Tutorial";
			SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			CenterContainer bgCenterer = GDUtil.MakeNode<CenterContainer>("BgCenterer");
			bgCenterer.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			nBackground = GDUtil.MakeNode<ColorRect>("Bg");
			nBackground.Color = Colors.Black;
			bgCenterer.AddChild(nBackground);
			nImage = GDUtil.MakeNode<AnimatedTextureRect>("Image");
			nImage.Expand = true;
			nImage.FPS = 2f;
			nImage.Hframes = (int)TextureSlices.x;
			nImage.Vframes = (int)TextureSlices.y;
			nImage.Playing = false;
			nImage.SetAnchorsGrowFrom(0.5f, 0.5f);
			CenterContainer textCenterer = GDUtil.MakeNode<CenterContainer>("TextCenterer");
			textCenterer.SetAnchorsGrowFrom(0.5f, 0.8f);
			nText = GDUtil.MakeNode<LabelWithInputIcons>("TutorialText");
			nText.LookupCaption = false;
			textCenterer.AddChild(nText);
			HBoxContainer okayTextContainer = GDUtil.MakeNode<HBoxContainer>("OkayTextContainer");
			okayTextContainer.SetAnchorsGrowFrom(0.99f, 0.99f, Direction.UpLeft);
			nOkayText = GDUtil.MakeNode<LabelWithInputIcons>("OkayText");
			okayTextContainer.AddChild(nOkayText);
			UpdateContent();
			nImage.Visible = false;
			nText.Visible = false;
			nOkayText.Visible = false;
			AddChild(bgCenterer);
			AddChild(nImage);
			AddChild(textCenterer);
			AddChild(okayTextContainer);
		}

		public override void _Input(InputEvent @event)
		{
			if (Inputs.Handle(@event, Inputs.Processor.Tutorial, HandledInputs) == "input_action")
			{
				if (Tutorial.NextPage != null)
				{
					ShowNextPage();
				}
				else
				{
					Injector.Get<ITutorialManager>().HideTutorial();
				}
			}
		}

		public async void Start(bool animate)
		{
			if (animate)
			{
				LacieAnimation animation = new ResizeAnimationControl(nBackground, Vector2.Zero, Game.Settings.BaseResolution, 0.25f);
				Game.Animations.Play(animation);
				await animation.WaitUntilFinished();
			}
			else
			{
				nBackground.RectMinSize = Game.Settings.BaseResolution;
			}
			nImage.Visible = true;
			nText.Visible = true;
			nOkayText.Visible = true;
			nImage.Playing = true;
			Game.InputProcessor = Inputs.Processor.Tutorial;
		}

		private void UpdateContent()
		{
			nImage.Texture = Tutorial.Image;
			nImage.RectMinSize = nImage.Texture.GetSize() / TextureSlices / 4f;
			nImage.Playing = false;
			nText.SetContent(Tutorial.Text, Tutorial.Inputs);
			if (Tutorial.NextPage != null)
			{
				nOkayText.SetContent("system.tutorials.next", "input_action");
			}
			else
			{
				nOkayText.SetContent("system.tutorials.okay", "input_action");
			}
			nImage.Playing = true;
		}

		private void ShowNextPage()
		{
			Tutorial = Tutorial.NextPage;
			UpdateContent();
		}
	}
}
