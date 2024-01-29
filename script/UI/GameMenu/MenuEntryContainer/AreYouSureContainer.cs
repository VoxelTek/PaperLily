using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class AreYouSureContainer : CenterContainer, IMenuEntryContainer
	{
		private static readonly Vector2 MainPanelSize = new Vector2(150f, 0f);

		public string Text { get; set; }

		public Action OnYes { get; set; }

		public Frame Control { get; private set; }

		public IMenuEntryList Menu { get; set; }

		public Action OnClose { get; set; }

		public override void _EnterTree()
		{
			ColorRect darkener = UIUtil.CreateDarkenerOverlay();
			SetAnchorsAndMarginsPreset(LayoutPreset.Wide);
			Control = new MenuFrame();
			Control.MinimumSize = MainPanelSize;
			VBoxContainer vBox = GDUtil.MakeNode<VBoxContainer>("AreYouSureContainer");
			vBox.SetSeparation(10);
			Label areYouSureText = GDUtil.MakeNode<Label>("AreYouSureText");
			areYouSureText.Text = Text;
			areYouSureText.SetDefaultFontAndColor();
			areYouSureText.Align = Label.AlignEnum.Center;
			vBox.AddChild(areYouSureText);
			Menu = new AreYouSureMenu(OnYes, this);
			Menu.OnBack = delegate
			{
				OnClose();
			};
			vBox.AddChild(Menu.DrawContent());
			Menu.Selection = 0;
			Menu.HighlightSelection();
			Control.AddToFrame(vBox);
			AddChild(darkener);
			AddChild(Control);
		}

		public override void _Input(InputEvent @event)
		{
			Menu.HandleInput(@event);
		}
	}
}
