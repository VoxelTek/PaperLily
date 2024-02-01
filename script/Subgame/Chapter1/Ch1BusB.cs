// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1BusB
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;
using System;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1BusB : GameRoom
  {
	[Inject]
	private IExtraFunctions Functions;
	[LacieEngine.API.GetNode("Background/chr_juniper")]
	private Sprite nJuniper;
	[LacieEngine.API.GetNode("Background/chr_sai")]
	private Sprite nSai;
	[LacieEngine.API.GetNode("Background/curtains_b")]
	private SimpleAnimatedSprite nCurtains;
	private const float TIME_TO_GET_OFF = 10f;
	private const float TIME_LOST_PER_INTERACTION = 1f;

	public void StartGetOffTimer()
	{
	  this.Functions.StartTimer(11f, (Action) (() => Game.Events.PlayEvent("ch1_event_bustimeout")), true);
	  int num1 = (int) Game.StoryPlayer.Connect("DialogueStarted", (Godot.Object) this, "PauseTimer");
	  int num2 = (int) Game.StoryPlayer.Connect("DialogueEnded", (Godot.Object) this, "ResumeAndDecreaseTimer");
	}

	public void PauseTimer() => this.Functions.GetTimer().PauseTimer();

	public void ResumeAndDecreaseTimer()
	{
	  this.Functions.GetTimer().ResumeTimer();
	  this.Functions.GetTimer().DecreaseTime(1f);
	}

	public void GetOffBus()
	{
	  Game.Player.Node.Visible = false;
	  this.Functions.StopTimer();
	}

	public void StopGetOffTimer() => this.Functions.StopTimer();

	public void SaiGlance() => this.nSai.Frame = 1;

	public void MoveJuniperHead(object body, int frame)
	{
	  if (body != Game.Player)
		return;
	  this.nJuniper.Frame = frame;
	}

	public void OpenCurtains() => this.nCurtains.Playing = true;
  }
}
