using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class ObjectivesMenuContainer : Control, IMenuEntryContainer
	{
		private static readonly Vector2 MainPanelSize = new Vector2(300f, 200f);

		private static readonly Vector2 DescriptionPanelSize = new Vector2(182f, 200f);

		public Frame Control { get; private set; }

		public IMenuEntryList Menu { get; set; }

		public Action OnClose { get; set; }

		public override void _EnterTree()
		{
			MenuFrame descriptionFrame = new MenuFrame();
			descriptionFrame.MinimumSize = DescriptionPanelSize;
			descriptionFrame.DecorBgTexture = "res://assets/img/ui/menu_bg_decor.png";
			descriptionFrame.DecorBgAlignment = LayoutPreset.BottomRight;
			RichTextLabel descriptionLabel = GDUtil.MakeNode<RichTextLabel>("ObjectiveDescription");
			descriptionLabel.ScrollActive = false;
			descriptionLabel.BbcodeEnabled = true;
			descriptionLabel.SetDefaultFontAndColor();
			descriptionFrame.AddToFrame(descriptionLabel);
			TitledMenuFrame frame = new TitledMenuFrame();
			frame.TitleText = "system.menu.objectives";
			frame.DividerTexture = "res://assets/img/ui/divider_md.png";
			frame.MinimumSize = MainPanelSize;
			Control = frame;
			Menu = new ObjectivesMenu(this, descriptionLabel);
			Menu.OnBack = delegate
			{
				OnClose();
			};
			Menu.Root();
			HBoxContainer hBox = GDUtil.MakeNode<HBoxContainer>("ObjectivesContainer");
			hBox.SetSeparation(2);
			hBox.AddChild(Control);
			hBox.AddChild(descriptionFrame);
			AddChild(hBox);
		}

		public override void _Input(InputEvent @event)
		{
			Menu.HandleInput(@event);
		}
	}
}
