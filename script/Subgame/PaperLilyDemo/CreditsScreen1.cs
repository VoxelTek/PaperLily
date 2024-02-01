// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.CreditsScreen1
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.Core;
using LacieEngine.UI;

#nullable disable
namespace LacieEngine.Rooms
{
  public class CreditsScreen1 : SimpleScreen
  {
	[LacieEngine.API.GetNode("EndingLabel")]
	private Control nEndingLabel;
	[LacieEngine.API.GetNode("Part1")]
	private Control nPart1;
	[LacieEngine.API.GetNode("Part2")]
	private Control nPart2;
	[LacieEngine.API.GetNode("Part3")]
	private Control nPart3;
	[LacieEngine.API.GetNode("Part4")]
	private Control nPart4;
	private PEvent evtPostCredits = (PEvent) "ch1_postcredits";

	public override void _Ready()
	{
	  base._Ready();
	  this.SetProcessInput(false);
	  this.Timers();
	}

	private async void Timers()
	{
	  CreditsScreen1 baseNode = this;
	  await baseNode.DelaySeconds(2.0);
	  baseNode.ShowEnding();
	  await baseNode.DelaySeconds(2.5);
	  baseNode.HideEnding();
	  baseNode.ShowCredits2();
	  await baseNode.DelaySeconds(5.0);
	  baseNode.ShowCredits3();
	  await baseNode.DelaySeconds(5.5);
	  baseNode.ShowCredits4();
	  await baseNode.DelaySeconds(6.0);
	  baseNode.PlayPostCredits();
	}

	private void ShowEnding()
	{
	  this.nEndingLabel.Visible = true;
	  Game.Animations.Play((LacieAnimation) new FadeInAnimation((CanvasItem) this.nEndingLabel, 1f));
	}

	private void HideEnding()
	{
	  Game.Animations.Play((LacieAnimation) new FadeOutAnimation((CanvasItem) this.nEndingLabel, 1f));
	}

	private async void ShowCredits2()
	{
	  CreditsScreen1 baseNode = this;
	  Game.Animations.Play((LacieAnimation) new FadeOutAnimation((CanvasItem) baseNode.nPart1, 1f));
	  await baseNode.DelaySeconds(1.0);
	  baseNode.nPart2.Visible = true;
	  Game.Animations.Play((LacieAnimation) new FadeInAnimation((CanvasItem) baseNode.nPart2, 1f));
	}

	private async void ShowCredits3()
	{
	  CreditsScreen1 baseNode = this;
	  Game.Animations.Play((LacieAnimation) new FadeOutAnimation((CanvasItem) baseNode.nPart2, 1f));
	  await baseNode.DelaySeconds(1.0);
	  baseNode.nPart3.Visible = true;
	  Game.Animations.Play((LacieAnimation) new FadeInAnimation((CanvasItem) baseNode.nPart3, 1f));
	}

	private async void ShowCredits4()
	{
	  CreditsScreen1 baseNode = this;
	  Game.Animations.Play((LacieAnimation) new FadeOutAnimation((CanvasItem) baseNode.nPart3, 1f));
	  await baseNode.DelaySeconds(1.0);
	  baseNode.nPart4.Visible = true;
	  Game.Animations.Play((LacieAnimation) new FadeInAnimation((CanvasItem) baseNode.nPart4, 1f));
	}

	private async void PlayPostCredits()
	{
	  CreditsScreen1 node = this;
	  await Game.Screen.FadeToBlack();
	  node.Delete();
	  node.evtPostCredits.Play();
	  Game.Screen.FadeFromBlack();
	}
  }
}
