// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1RedB1f
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using System;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1RedB1f : GameRoom
  {
    [Inject]
    private IExtraFunctions Functions;
    [LacieEngine.API.GetNode("Background/MiscJar")]
    private Node2D nJar;
    [LacieEngine.API.GetNode("Background/WallMirror")]
    private Node2D nMirror;
    [LacieEngine.API.GetNode("Background/MiscMirrorBroken")]
    private Node2D nMirrorBroken;
    [LacieEngine.API.GetNode("Main/FurnitureCage/LacieDeathPrisoner")]
    private SimpleAnimatedSprite nLacieStrangle;
    [LacieEngine.API.GetNode("Main/FurnitureCage/PrisonerLight")]
    private SimpleAnimatedSprite nPrisonerLight;
    private PVar varCode = (PVar) "persistent.ch1_red_passage_code";
    private PVar varTookJar = (PVar) "ch1.forest_red_took_jar";
    private PVar varBrokeMirror = (PVar) "ch1.forest_red_cracked_mirror";
    private PVar varPrisonerQuestDone = (PVar) "ch1.forest_red_prisoner_stage";
    private PVar varTookNotes = (PVar) "ch1.forest_red_took_2nd_notes";

    public override void _BeforeFadeIn()
    {
      if ((bool) this.varCode.Value)
        return;
      this.varCode.NewValue = (PVar.PVarSetProxy) new Random().Next(1000, 10000);
      PersistentState.Save();
    }

    public override void _UpdateRoom()
    {
      this.nJar.Visible = !(bool) this.varTookJar.Value;
      this.nMirror.Visible = !(bool) this.varBrokeMirror.Value;
      this.nMirrorBroken.Visible = (bool) this.varBrokeMirror.Value;
      this.nPrisonerLight.Visible = this.varPrisonerQuestDone.Value == 6;
      this.nPrisonerLight.Playing = this.varPrisonerQuestDone.Value == 6;
    }

    public void LacieStrangle()
    {
      Game.Player.Node.Visible = false;
      this.nLacieStrangle.Visible = true;
      this.nLacieStrangle.Playing = true;
    }

    public void LacieEscape()
    {
      this.nLacieStrangle.Visible = false;
      this.nLacieStrangle.Playing = false;
      Game.Player.SetPlayerState(CharacterState.InObject);
      Game.Player.SpriteState = "fall";
      Game.Player.SpriteDirection = Direction.Up;
      Game.Player.Node.Visible = true;
    }

    public void StopTimer() => this.Functions.StopTimer();
  }
}
