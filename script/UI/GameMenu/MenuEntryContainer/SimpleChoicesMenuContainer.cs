using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class SimpleChoicesMenuContainer : CenterContainer, IMenuEntryContainer
	{
		private class SimpleChoicesMenu : SimpleVerticalMenu
		{
			public SimpleChoicesMenu(IMenuEntryContainer container, List<IMenuEntry> entries)
			{
				base.Container = container;
				base.Entries = entries;
			}
		}

		private List<IMenuEntry> _entries;

		public Frame Control { get; private set; }

		public IMenuEntryList Menu { get; set; }

		public Action OnClose { get; set; }

		public bool DarkenBackground { get; set; }

		public SimpleChoicesMenuContainer()
		{
		}

		public SimpleChoicesMenuContainer(List<IMenuEntry> entries)
		{
			_entries = entries;
		}

		public override void _EnterTree()
		{
			SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			Control = Game.Settings.UiProvider.MakeChoicesFrame();
			Menu = new SimpleChoicesMenu(this, _entries);
			Menu.OnBack = delegate
			{
				OnClose();
			};
			Menu.Root();
			if (DarkenBackground)
			{
				AddChild(UIUtil.CreateDarkenerOverlay());
			}
			AddChild(Control);
		}

		public override void _Input(InputEvent @event)
		{
			Menu.HandleInput(@event);
		}
	}
}
