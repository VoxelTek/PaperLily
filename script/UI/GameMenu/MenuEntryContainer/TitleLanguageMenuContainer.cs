using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class TitleLanguageMenuContainer : CenterContainer, IMenuEntryContainer
	{
		private static readonly Vector2 FrameSize = new Vector2(300f, 0f);

		private Control nInfoBox;

		public Frame Control { get; private set; }

		public IMenuEntryList Menu { get; set; }

		public Action OnClose { get; set; }

		public override void _EnterTree()
		{
			SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			TitledMenuFrame inventoryFrame = new TitledMenuFrame();
			inventoryFrame.MinimumSize = FrameSize;
			inventoryFrame.TitleText = "system.settings.game.language.select";
			inventoryFrame.DividerTexture = "res://assets/img/ui/divider_md.png";
			Control = inventoryFrame;
			Menu = new StartupLanguageMenu(extraLanguages: false, null, this);
			Menu.OnBack = delegate
			{
				Close();
			};
			Menu.Root();
			AddChild(UIUtil.CreateDarkenerOverlay());
			AddChild(Control);
			UpdateInfoBox();
		}

		public override void _Input(InputEvent @event)
		{
			Menu.HandleInput(@event);
		}

		private void Close()
		{
			nInfoBox.Delete();
			nInfoBox = null;
			OnClose();
		}

		private void UpdateInfoBox()
		{
			if (nInfoBox.IsValid())
			{
				nInfoBox.Delete();
			}
			nInfoBox = UIUtil.CreateInfoBoxWithInputIcons("system.menu.startup.settings.info", "input_up", "input_down", "input_action");
			nInfoBox.SetAnchorsPreset(LayoutPreset.BottomRight);
			nInfoBox.GrowHorizontal = GrowDirection.Begin;
			nInfoBox.GrowVertical = GrowDirection.Begin;
			Game.Screen.AddToLayer(IScreenManager.Layer.Screen, nInfoBox);
		}
	}
}
