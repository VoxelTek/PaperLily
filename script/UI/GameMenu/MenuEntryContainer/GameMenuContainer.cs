using System;
using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class GameMenuContainer : Control, IMenuEntryContainer
	{
		private static readonly Vector2 MainPanelSize = new Vector2(150f, 200f);

		private static readonly Vector2 TextPanelSize = new Vector2(150f, 45f);

		private static readonly Vector2 PartyPanelSize = new Vector2(150f, 80f);

		private PVar varCurrency = "general.currency_demon";

		public Frame Control { get; private set; }

		public IMenuEntryList Menu { get; set; }

		public Action OnClose { get; set; }

		public override void _EnterTree()
		{
			base.Name = "GameMenu";
			SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			ColorRect darkener = UIUtil.CreateDarkenerOverlay();
			MarginContainer container = GDUtil.MakeNode<MarginContainer>("GameMenuContainer");
			container.SetContainerMargins(2);
			HBoxContainer hBox = GDUtil.MakeNode<HBoxContainer>("MenuColumnsContainer");
			hBox.SetSeparation(2);
			VBoxContainer vBox = GDUtil.MakeNode<VBoxContainer>("LeftColumnContainer");
			vBox.SetSeparation(2);
			Control = new MenuFrame();
			Control.MinimumSize = MainPanelSize;
			Menu = new GameMenu(hBox, this);
			Menu.OnBack = delegate
			{
				OnClose();
			};
			Menu.Root();
			container.AddChild(hBox);
			hBox.AddChild(vBox);
			vBox.AddChild(Control);
			Frame moneyFrame = new MenuFrame2();
			moneyFrame.MinimumSize = TextPanelSize;
			Label moneyLabel = GDUtil.MakeNode<Label>("MoneyLabel");
			moneyLabel.SetDefaultFontAndColor();
			if ((int)varCurrency.Value > 0)
			{
				moneyLabel.Text = Game.Language.GetFormattedCaption("system.menu.currency", varCurrency.Value);
			}
			else
			{
				moneyLabel.Text = Game.Settings.ProductName;
			}
			moneyLabel.Align = Label.AlignEnum.Center;
			moneyFrame.AddToFrame(moneyLabel);
			moneyFrame.Modulate = UIUtil.Transparent;
			vBox.AddChild(moneyFrame);
			Frame charaFrame = new MenuFrame2();
			charaFrame.MinimumSize = PartyPanelSize;
			charaFrame.ContentMarginLeft = 0;
			charaFrame.ContentMarginTop = 15;
			charaFrame.ContentMarginRight = 0;
			charaFrame.ContentMarginBottom = 15;
			charaFrame.Modulate = UIUtil.Transparent;
			HBoxContainer characterHBox = GDUtil.MakeNode<HBoxContainer>("CharacterHBox");
			characterHBox.SetSeparation(0);
			characterHBox.Alignment = BoxContainer.AlignMode.Center;
			foreach (string partyMember in Game.State.Party)
			{
				characterHBox.AddFirst(CreateWalkingCharacterTexture(partyMember));
			}
			charaFrame.AddToFrame(characterHBox);
			vBox.AddChild(charaFrame);
			Frame infoBox = UIUtil.CreateInfoBoxWithInputIcons("system.menu.info", "input_action", "input_cancel");
			infoBox.Modulate = UIUtil.Transparent;
			infoBox.SetAnchorsPreset(LayoutPreset.BottomRight);
			infoBox.GrowHorizontal = GrowDirection.Begin;
			infoBox.GrowVertical = GrowDirection.Begin;
			AddChild(darkener);
			AddChild(container);
			AddChild(infoBox);
			Game.Animations.Play(new SlideInLeftAnimation(Control, 150f));
			Game.Animations.PlayDelayed(new SlideInLeftAnimation(moneyFrame, 150f), 0.08f);
			Game.Animations.PlayDelayed(new SlideInLeftAnimation(charaFrame, 150f), 0.16f);
			Game.Animations.PlayDelayed(new SlideInBottomAnimation(infoBox, 150f), 0.01f);
		}

		public override void _Input(InputEvent @event)
		{
			Menu.HandleInput(@event);
		}

		private AnimatedTextureRect CreateWalkingCharacterTexture(string characterId)
		{
			AnimatedTextureRect textureRect = new AnimatedTextureRect();
			Texture texture = (textureRect.Texture = GD.Load<Texture>("res://assets/sprite/common/character/" + characterId + "/" + characterId + ".png"));
			textureRect.Hframes = 9;
			textureRect.Vframes = 4;
			textureRect.FPS = 6f;
			textureRect.AnimationFrames = new int[4] { 18, 19, 18, 20 };
			textureRect.SizeFlagsVertical = 8;
			return textureRect;
		}
	}
}
