using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class TitleSettingsMenuContainer : CenterContainer, IMenuEntryContainer
	{
		private static readonly Vector2 FrameSize = new Vector2(300f, 0f);

		public Frame Control { get; private set; }

		public IMenuEntryList Menu { get; set; }

		public Action OnClose { get; set; }

		public override void _EnterTree()
		{
			SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			Control = Game.Settings.UiProvider.MakeDefaultFrame();
			Control.MinimumSize = FrameSize;
			Menu = new TitleSettingsMenu(null, this);
			Menu.OnBack = delegate
			{
				OnClose();
			};
			Menu.Root();
			AddChild(UIUtil.CreateDarkenerOverlay());
			AddChild(Control);
		}

		public override void _Input(InputEvent @event)
		{
			Menu.HandleInput(@event);
		}
	}
}
