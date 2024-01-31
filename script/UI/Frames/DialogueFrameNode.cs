// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.DialogueFrameNode
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
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
    public Color FrameModulate { get; set; } = Godot.Colors.White;

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
      Texture texFrame = this.TexFrame;
      this.Background = GDUtil.MakeNode<TextureRect>("FrameTexture");
      this.Background.Expand = true;
      this.Background.Texture = texFrame;
      this.Background.Modulate = this.FrameModulate;
      this.Background.RectMinSize = texFrame.GetSize() / this.TextureScaleFactor;
      this.AddChild((Node) this.Background);
      this.Container.SetContainerMarginLeft(this.ContentMarginLeft);
      this.Container.SetContainerMarginTop(this.ContentMarginTop);
      this.Container.SetContainerMarginRight(this.ContentMarginRight);
      this.Container.SetContainerMarginBottom(this.ContentMarginBottom);
      this.AddChild((Node) this.Container);
      this.NamePlate = GDUtil.MakeNode<Control>("NamePlateContainer");
      this.NamePlate.RectMinSize = this.NamePlateSize;
      this.SpeakerName = GDUtil.MakeNode<Label>("NameLabel");
      this.SpeakerName.SetDefaultFontAndColor();
      this.SpeakerName.SetFont("res://resources/font/default_title.tres");
      this.SpeakerName.RectPosition = this.NamePlateTextPosition;
      this.NameSeparator = GDUtil.MakeNode<TextureRect>("NameSeparator");
      Texture texNameSeparator = this.TexNameSeparator;
      this.NameSeparator.Texture = texNameSeparator;
      this.NameSeparator.Expand = true;
      this.NameSeparator.RectMinSize = texNameSeparator.GetSize() / this.TextureScaleFactor;
      this.NameSeparator.RectPosition = this.NameSeparatorPosition;
      this.NamePlate.AddChild((Node) this.SpeakerName);
      this.NamePlate.AddChild((Node) this.NameSeparator);
      this.DialogueText = GDUtil.MakeNode<RichTextLabel>("DialogueText");
      this.DialogueText.SizeFlagsVertical = 3;
      this.DialogueText.ScrollActive = false;
      this.DialogueText.BbcodeEnabled = true;
      this.DialogueText.SetDefaultFontAndColor();
      Control child1 = (Control) GDUtil.MakeNode<VBoxContainer>("TextBoxContainer");
      child1.AddChild((Node) this.NamePlate);
      child1.AddChild((Node) this.DialogueText);
      this.AddToFrame((Node) child1);
      Control child2 = GDUtil.MakeNode<Control>("ContinueIndicatorContainer");
      child2.SetAnchorsAndMarginsPreset(Control.LayoutPreset.Wide);
      this.ContinueIndicator = (TextureRect) this.MakeContinueIndicator();
      this.ContinueIndicator.GrowHorizontal = Control.GrowDirection.Both;
      child2.AddChild((Node) this.ContinueIndicator);
      this.AddToFrame((Node) child2);
    }

    private AnimatedTextureRect MakeContinueIndicator()
    {
      AnimatedTextureRect control = GDUtil.MakeNode<AnimatedTextureRect>("ContinueIndicator");
      control.Texture = GD.Load<Texture>("res://assets/img/ui/continue_indicator.png");
      control.Hframes = 3;
      control.FPS = 3f;
      control.AnimationFrames = new int[4]{ 0, 1, 2, 1 };
      control.Expand = true;
      control.RectMinSize = DialogueFrameNode.ContinueIndicatorSize;
      control.SetAnchorsGrowFrom(1f, 1f, Direction.UpLeft);
      return control;
    }
  }
}
