// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.GameMenuContainer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class GameMenuContainer : Godot.Control, IMenuEntryContainer
  {
    private static readonly Vector2 MainPanelSize = new Vector2(150f, 200f);
    private static readonly Vector2 TextPanelSize = new Vector2(150f, 45f);
    private static readonly Vector2 PartyPanelSize = new Vector2(150f, 80f);
    private PVar varCurrency = (PVar) "general.currency_demon";

    public Frame Control { get; private set; }

    public IMenuEntryList Menu { get; set; }

    public Action OnClose { get; set; }

    public override void _EnterTree()
    {
      this.Name = "GameMenu";
      this.SetAnchorsAndMarginsPreset(Godot.Control.LayoutPreset.Wide);
      ColorRect darkenerOverlay = UIUtil.CreateDarkenerOverlay();
      MarginContainer container = GDUtil.MakeNode<MarginContainer>(nameof (GameMenuContainer));
      container.SetContainerMargins(2);
      HBoxContainer hboxContainer1 = GDUtil.MakeNode<HBoxContainer>("MenuColumnsContainer");
      hboxContainer1.SetSeparation(2);
      VBoxContainer box = GDUtil.MakeNode<VBoxContainer>("LeftColumnContainer");
      box.SetSeparation(2);
      this.Control = (Frame) new MenuFrame();
      this.Control.MinimumSize = GameMenuContainer.MainPanelSize;
      this.Menu = (IMenuEntryList) new GameMenu((Godot.Control) hboxContainer1, (IMenuEntryContainer) this);
      this.Menu.OnBack = (Action) (() => this.OnClose());
      this.Menu.Root();
      container.AddChild((Node) hboxContainer1);
      hboxContainer1.AddChild((Node) box);
      box.AddChild((Node) this.Control);
      Frame node1 = (Frame) new MenuFrame2();
      node1.MinimumSize = GameMenuContainer.TextPanelSize;
      Label label = GDUtil.MakeNode<Label>("MoneyLabel");
      label.SetDefaultFontAndColor();
      if ((int) this.varCurrency.Value > 0)
        label.Text = Game.Language.GetFormattedCaption("system.menu.currency", (string) this.varCurrency.Value);
      else
        label.Text = Game.Settings.ProductName;
      label.Align = Label.AlignEnum.Center;
      node1.AddToFrame((Node) label);
      node1.Modulate = UIUtil.Transparent;
      box.AddChild((Node) node1);
      Frame node2 = (Frame) new MenuFrame2();
      node2.MinimumSize = GameMenuContainer.PartyPanelSize;
      node2.ContentMarginLeft = 0;
      node2.ContentMarginTop = 15;
      node2.ContentMarginRight = 0;
      node2.ContentMarginBottom = 15;
      node2.Modulate = UIUtil.Transparent;
      HBoxContainer hboxContainer2 = GDUtil.MakeNode<HBoxContainer>("CharacterHBox");
      hboxContainer2.SetSeparation(0);
      hboxContainer2.Alignment = BoxContainer.AlignMode.Center;
      foreach (string characterId in Game.State.Party)
        hboxContainer2.AddFirst((Node) this.CreateWalkingCharacterTexture(characterId));
      node2.AddToFrame((Node) hboxContainer2);
      box.AddChild((Node) node2);
      Frame boxWithInputIcons = UIUtil.CreateInfoBoxWithInputIcons("system.menu.info", "input_action", "input_cancel");
      boxWithInputIcons.Modulate = UIUtil.Transparent;
      boxWithInputIcons.SetAnchorsPreset(Godot.Control.LayoutPreset.BottomRight);
      boxWithInputIcons.GrowHorizontal = Godot.Control.GrowDirection.Begin;
      boxWithInputIcons.GrowVertical = Godot.Control.GrowDirection.Begin;
      this.AddChild((Node) darkenerOverlay);
      this.AddChild((Node) container);
      this.AddChild((Node) boxWithInputIcons);
      Game.Animations.Play((LacieAnimation) new SlideInLeftAnimation((Godot.Control) this.Control, new float?(150f)));
      Game.Animations.PlayDelayed((LacieAnimation) new SlideInLeftAnimation((Godot.Control) node1, new float?(150f)), 0.08f);
      Game.Animations.PlayDelayed((LacieAnimation) new SlideInLeftAnimation((Godot.Control) node2, new float?(150f)), 0.16f);
      Game.Animations.PlayDelayed((LacieAnimation) new SlideInBottomAnimation((Godot.Control) boxWithInputIcons, new float?(150f)), 0.01f);
    }

    public override void _Input(InputEvent @event) => this.Menu.HandleInput(@event);

    private AnimatedTextureRect CreateWalkingCharacterTexture(string characterId)
    {
      AnimatedTextureRect characterTexture = new AnimatedTextureRect();
      Texture texture = GD.Load<Texture>("res://assets/sprite/common/character/" + characterId + "/" + characterId + ".png");
      characterTexture.Texture = texture;
      characterTexture.Hframes = 9;
      characterTexture.Vframes = 4;
      characterTexture.FPS = 6f;
      characterTexture.AnimationFrames = new int[4]
      {
        18,
        19,
        18,
        20
      };
      characterTexture.SizeFlagsVertical = 8;
      return characterTexture;
    }
  }
}
