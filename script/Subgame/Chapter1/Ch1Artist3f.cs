// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1Artist3f
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
  public class Ch1Artist3f : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private Texture texKettMad;
    [LacieEngine.API.GetNode("Background/Candleman")]
    private Node2D nCandleman;
    [LacieEngine.API.GetNode("Background/CandlemanBroken")]
    private Node2D nCandlemanBroken;
    [LacieEngine.API.GetNode("Background/CandlemanBase")]
    private Node2D nCandlemanGone;
    [LacieEngine.API.GetNode("Other/Navigation")]
    private Navigation2D nNavigation;
    [LacieEngine.API.GetNode("Background/CandlemanBroken/Shiny")]
    private Node2D nUnlitCandle;
    [LacieEngine.API.GetNode("Events/item_unlitcandle_get")]
    private EventTrigger nUnlitCandleEvt;
    private PVar varCandlemanDead = (PVar) "ch1.forest_candleman_dead";
    private PVar varKettChase = (PVar) "ch1.forest_kett_chase";
    private PVar varCandlemanBlew = (PVar) "ch1.forest_candleman_blew_candle";
    private PVar varTookUnlitCandle = (PVar) "ch1.forest_candleman_took_unlit_candle";
    private PVar varCandlemanSaved = (PVar) "ch1.forest_candleman_saved";
    private NPCChaser nKettChasing;

    public override void _UpdateRoom()
    {
      this.nCandleman.Visible = !(bool) this.varCandlemanDead.Value && !(bool) this.varCandlemanSaved.Value;
      this.nCandlemanBroken.Visible = (bool) this.varCandlemanDead.Value;
      this.nCandlemanGone.Visible = (bool) this.varCandlemanSaved.Value;
      if ((bool) this.varCandlemanBlew.Value && !(bool) this.varTookUnlitCandle.Value)
      {
        this.nUnlitCandle.Visible = true;
        this.nUnlitCandleEvt.Enabled = true;
      }
      else
      {
        this.nUnlitCandle.Visible = false;
        this.nUnlitCandleEvt.Enabled = false;
      }
    }

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
      Ch1Artist3f baseNode = this;
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
