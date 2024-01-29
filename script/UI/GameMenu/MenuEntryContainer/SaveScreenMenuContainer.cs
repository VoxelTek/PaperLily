using System;
using Godot;

namespace LacieEngine.UI
{
	public class SaveScreenMenuContainer : CenterContainer, IMenuEntryContainer
	{
		private static readonly Vector2 MainPanelSize = new Vector2(484f, 200f);

		public Frame Control { get; private set; }

		public IMenuEntryList Menu { get; set; }

		public Action OnClose { get; set; }

		public override void _EnterTree()
		{
			SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			TitledMenuFrame frame = new TitledMenuFrame();
			frame.MinimumSize = MainPanelSize;
			frame.TitleText = "system.menu.save.game";
			frame.DividerTexture = "res://assets/img/ui/divider_md.png";
			Control = frame;
			Menu = new SaveLoadMenu(SaveLoadMenu.Mode.Save, null, this);
			Menu.OnBack = delegate
			{
				OnClose();
			};
			Menu.Root();
			AddChild(Control);
		}

		public override void _Input(InputEvent @event)
		{
			Menu.HandleInput(@event);
		}
	}
}
