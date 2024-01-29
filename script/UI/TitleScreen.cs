using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public class TitleScreen : Control, IMenuEntryContainer
	{
		[Export(PropertyHint.None, "")]
		public AudioStream Bgm;

		[GetNode("MenuContainer")]
		private Control nMenuContainer;

		[GetNode("InfoMargin/InfoVBox/VersionInfo")]
		private Label nVersionInfo;

		[GetNode("InfoMargin/InfoVBox/ExtraInfo")]
		private Label nExtraInfo;

		private static readonly Vector2 FrameSize = new Vector2(120f, 0f);

		public Frame Control { get; private set; }

		public IMenuEntryList Menu { get; set; }

		public Action OnClose { get; set; }

		public override void _EnterTree()
		{
			Menu = new TitleMenu(this);
		}

		public override void _Ready()
		{
			nVersionInfo.Text = "v" + Game.Settings.ProductVersion + (OS.IsDebugBuild() ? " Debug" : "") + " © " + Game.Settings.ProductCopyright;
			nExtraInfo.Text = Game.Language.GetTranslatorCredit();
			Control = Game.Settings.UiProvider.MakeDefaultFrame();
			Control.MinimumSize = FrameSize;
			Control.AddToFrame(Menu.DrawContent());
			nMenuContainer.AddChild(Control);
			Game.Audio.PlayBgm(Bgm);
			Game.Audio.StopAmbiance();
			Menu.ResetSelection();
		}

		public override void _Input(InputEvent @event)
		{
			Menu.HandleInput(@event);
		}

		public void UpdateExtraInfo()
		{
			nExtraInfo.SetDefaultFontAndColor();
			nExtraInfo.Text = Game.Language.GetTranslatorCredit();
		}
	}
}
