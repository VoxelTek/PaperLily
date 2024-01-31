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
    private string[] HandledInputs = new string[1]
    {
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
      this.Name = "Tutorial";
      this.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      CenterContainer centerContainer = GDUtil.MakeNode<CenterContainer>("BgCenterer");
      centerContainer.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      this.nBackground = GDUtil.MakeNode<ColorRect>("Bg");
      this.nBackground.Color = Godot.Colors.Black;
      centerContainer.AddChild((Node) this.nBackground);
      this.nImage = GDUtil.MakeNode<AnimatedTextureRect>("Image");
      this.nImage.Expand = true;
      this.nImage.FPS = 2f;
      this.nImage.Hframes = (int) TutorialScreen.TextureSlices.x;
      this.nImage.Vframes = (int) TutorialScreen.TextureSlices.y;
      this.nImage.Playing = false;
      this.nImage.SetAnchorsGrowFrom(0.5f, 0.5f);
      CenterContainer control1 = GDUtil.MakeNode<CenterContainer>("TextCenterer");
      control1.SetAnchorsGrowFrom(0.5f, 0.8f);
      this.nText = GDUtil.MakeNode<LabelWithInputIcons>("TutorialText");
      this.nText.LookupCaption = false;
      control1.AddChild((Node) this.nText);
      HBoxContainer control2 = GDUtil.MakeNode<HBoxContainer>("OkayTextContainer");
      control2.SetAnchorsGrowFrom(0.99f, 0.99f, Direction.UpLeft);
      this.nOkayText = GDUtil.MakeNode<LabelWithInputIcons>("OkayText");
      control2.AddChild((Node) this.nOkayText);
      this.UpdateContent();
      this.nImage.Visible = false;
      this.nText.Visible = false;
      this.nOkayText.Visible = false;
      this.AddChild((Node) centerContainer);
      this.AddChild((Node) this.nImage);
      this.AddChild((Node) control1);
      this.AddChild((Node) control2);
    }

    public override void _Input(InputEvent @event)
    {
      if (!(Inputs.Handle(@event, Inputs.Processor.Tutorial, this.HandledInputs) == "input_action"))
        return;
      if (this.Tutorial.NextPage != null)
        this.ShowNextPage();
      else
        Injector.Get<ITutorialManager>().HideTutorial();
    }

    public async void Start(bool animate)
    {
      if (animate)
      {
        LacieAnimation animation = (LacieAnimation) new ResizeAnimationControl((Control) this.nBackground, Vector2.Zero, Game.Settings.BaseResolution, 0.25f);
        Game.Animations.Play(animation);
        await animation.WaitUntilFinished();
      }
      else
        this.nBackground.RectMinSize = Game.Settings.BaseResolution;
      this.nImage.Visible = true;
      this.nText.Visible = true;
      this.nOkayText.Visible = true;
      this.nImage.Playing = true;
      Game.InputProcessor = Inputs.Processor.Tutorial;
    }

    private void UpdateContent()
    {
      this.nImage.Texture = this.Tutorial.Image;
      this.nImage.RectMinSize = this.nImage.Texture.GetSize() / TutorialScreen.TextureSlices / 4f;
      this.nImage.Playing = false;
      this.nText.SetContent(this.Tutorial.Text, this.Tutorial.Inputs);
      if (this.Tutorial.NextPage != null)
        this.nOkayText.SetContent("system.tutorials.next", "input_action");
      else
        this.nOkayText.SetContent("system.tutorials.okay", "input_action");
      this.nImage.Playing = true;
    }

    private void ShowNextPage()
    {
      this.Tutorial = this.Tutorial.NextPage;
      this.UpdateContent();
    }
  }
}
