using System;
using Godot;

namespace LacieEngine.UI
{
	public class QuitMenuContainer : CenterContainer, IMenuEntryContainer
	{
		private static readonly Vector2 MainPanelSize = new Vector2(300f, 0f);

		public Frame Control { get; private set; }

		public IMenuEntryList Menu { get; set; }

		public Action OnClose { get; set; }

		public override void _EnterTree()
		{
			ColorRect darkener = UIUtil.CreateDarkenerOverlay();
			SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			Control = new MenuFrame();
			Control.MinimumSize = MainPanelSize;
			Menu = new QuitMenu(this);
			Menu.OnBack = delegate
			{
				OnClose();
			};
			Menu.Root();
			AddChild(darkener);
			AddChild(Control);
		}

		public override void _Input(InputEvent @event)
		{
			Menu.HandleInput(@event);
		}
	}
}
