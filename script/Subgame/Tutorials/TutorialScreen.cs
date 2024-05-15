// Decompiled with JetBrains decompiler
// Type: LacieEngine.Modules.Tutorials.TutorialScreen
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;

#nullable disable
namespace LacieEngine.Modules.Tutorials
{
    public class TutorialScreen : Control
    {
        private readonly string[] HandledInputs = new string[] {
            "input_action"
        };
        private AnimatedTextureRect nImage;
        private LabelWithInputIcons nText;
        private LabelWithInputIcons nOkayText;
        private ColorRect nBackground;
        private static readonly Vector2 TextureSlices = new Vector2(1f, 2f);
        private const int TextureDownscale = 4;

        public Tutorial Tutorial { get; set; }

        public override void _EnterTree()
        {
            Name = "Tutorial";
            SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
            var centerContainer = GDUtil.MakeNode<CenterContainer>("BgCenterer");
            centerContainer.SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
            nBackground = GDUtil.MakeNode<ColorRect>("Bg");
            nBackground.Color = Colors.Black;
            centerContainer.AddChild(nBackground);
            nImage = GDUtil.MakeNode<AnimatedTextureRect>("Image");
            nImage.Expand = true;
            nImage.FPS = 2f;
            nImage.Hframes = (int)TextureSlices.x;
            nImage.Vframes = (int)TextureSlices.y;
            nImage.Playing = false;
            nImage.SetAnchorsGrowFrom(0.5f, 0.5f);
            var control1 = GDUtil.MakeNode<CenterContainer>("TextCenterer");
            control1.SetAnchorsGrowFrom(0.5f, 0.8f);
            nText = GDUtil.MakeNode<LabelWithInputIcons>("TutorialText");
            nText.LookupCaption = false;
            control1.AddChild(nText);
            var control2 = GDUtil.MakeNode<HBoxContainer>("OkayTextContainer");
            control2.SetAnchorsGrowFrom(0.99f, 0.99f, Direction.UpLeft);
            nOkayText = GDUtil.MakeNode<LabelWithInputIcons>("OkayText");
            control2.AddChild(nOkayText);
            UpdateContent();
            nImage.Visible = false;
            nText.Visible = false;
            nOkayText.Visible = false;
            AddChild(centerContainer);
            AddChild(nImage);
            AddChild(control1);
            AddChild(control2);
        }

        public override void _Input(InputEvent @event)
        {
            if (Inputs.Handle(@event, Inputs.Processor.Tutorial, HandledInputs) != "input_action")
                return;
            if (Tutorial.NextPage != null)
                ShowNextPage();
            else
                Injector.Get<ITutorialManager>().HideTutorial();
        }

        public async void Start(bool animate)
        {
            if (animate)
            {
                var animation = new ResizeAnimationControl(nBackground, Vector2.Zero, Game.Settings.BaseResolution, 0.25f);
                Game.Animations.Play(animation);
                await animation.WaitUntilFinished();
            }
            else
                nBackground.RectMinSize = Game.Settings.BaseResolution;
            nImage.Visible = true;
            nText.Visible = true;
            nOkayText.Visible = true;
            nImage.Playing = true;
            Game.InputProcessor = Inputs.Processor.Tutorial;
        }

        private void UpdateContent()
        {
            nImage.Texture = Tutorial.Image;
            nImage.RectMinSize = nImage.Texture.GetSize() / TextureSlices / TextureDownscale;
            nImage.Playing = false;
            nText.SetContent(Tutorial.Text, Tutorial.Inputs);
            if (Tutorial.NextPage != null)
                nOkayText.SetContent("system.tutorials.next", "input_action");
            else
                nOkayText.SetContent("system.tutorials.okay", "input_action");
            nImage.Playing = true;
        }

        private void ShowNextPage()
        {
            Tutorial = Tutorial.NextPage;
            Injector.Get<ITutorialManager>().ApplyCaptions(Tutorial);
            UpdateContent();
        }
    }
}
