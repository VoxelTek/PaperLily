// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.TitleScreen
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.UI
{
  public class TitleScreen : Godot.Control, IMenuEntryContainer
  {
	[Export(PropertyHint.None, "")]
	public AudioStream Bgm;
	[LacieEngine.API.GetNode("MenuContainer")]
	private Godot.Control nMenuContainer;
	[LacieEngine.API.GetNode("InfoMargin/InfoVBox/VersionInfo")]
	private Label nVersionInfo;
	[LacieEngine.API.GetNode("InfoMargin/InfoVBox/ExtraInfo")]
	private Label nExtraInfo;
	private static readonly Vector2 FrameSize = new Vector2(120f, 0.0f);

	public Frame Control { get; private set; }

	public IMenuEntryList Menu { get; set; }

	public Action OnClose { get; set; }

	public override void _EnterTree()
	{
	  this.Menu = (IMenuEntryList) new TitleMenu((IMenuEntryContainer) this);
	}

	public override void _Ready()
	{
	  this.nVersionInfo.Text = "v" + Game.Settings.ProductVersion + (OS.IsDebugBuild() ? " Debug" : "") + " © " + Game.Settings.ProductCopyright;
	  this.nExtraInfo.Text = Game.Language.GetTranslatorCredit();
	  this.Control = Game.Settings.UiProvider.MakeDefaultFrame();
	  this.Control.MinimumSize = TitleScreen.FrameSize;
	  this.Control.AddToFrame((Node) this.Menu.DrawContent());
	  this.nMenuContainer.AddChild((Node) this.Control);
	  Game.Audio.PlayBgm(this.Bgm);
	  Game.Audio.StopAmbiance();
	  this.Menu.ResetSelection();
	}

	public override void _Input(InputEvent @event) => this.Menu.HandleInput(@event);

	public void UpdateExtraInfo()
	{
	  this.nExtraInfo.SetDefaultFontAndColor();
	  this.nExtraInfo.Text = Game.Language.GetTranslatorCredit();
	}
  }
}
