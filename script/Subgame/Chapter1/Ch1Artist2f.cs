// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1Artist2f
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;
using System;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1Artist2f : GameRoom
  {
	[Export(PropertyHint.None, "")]
	private Texture texKettMad;
	[LacieEngine.API.GetNode("Other/Navigation")]
	private Navigation2D nNavigation;
	private PVar varKettChase = (PVar) "ch1.forest_kett_chase";
	private NPCChaser nKettChasing;

	public override void _BeforeFadeIn()
	{
	  if (!(bool) this.varKettChase.Value)
		return;
	  this.nKettChasing = new NPCChaser("kett");
	  this.nKettChasing.MoveSpeedMultiplier = 1.3f;
	  this.GetMainLayer().AddChild((Node) this.nKettChasing);
	  this.nKettChasing.OverrideTextureForState("stand", this.texKettMad);
	  this.nKettChasing.OverrideTextureForState("walk", this.texKettMad);
	  this.nKettChasing.SetNavigation(this.nNavigation);
	  this.nKettChasing.Visible = false;
	  this.nKettChasing.Position = this.GetPoint("from_downstairs");
	}

	public override void _AfterFadeIn()
	{
	  if (!this.nKettChasing.IsValid())
		return;
	  this.SpawnKett();
	}

	public async void SpawnKett()
	{
	  Ch1Artist2f baseNode = this;
	  await baseNode.DelaySeconds(0.5);
	  baseNode.nKettChasing.Caught += new Action(baseNode.PlayerDeath);
	  baseNode.nKettChasing.Visible = true;
	  baseNode.nKettChasing.Chasing = true;
	}

	public void PlayerDeath() => Game.Events.PlayEvent("ch1_death_impact");

	private void PauseChaser()
	{
	  if (!this.nKettChasing.IsValid())
		return;
	  this.nKettChasing.Chasing = false;
	}
  }
}
