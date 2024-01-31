// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1BlackPassClassroom
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1BlackPassClassroom : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private Texture texLacieDark;
    [LacieEngine.API.GetNode("Main/Bouncer")]
    private Node2D nBouncer;
    [LacieEngine.API.GetNode("Main/People_1")]
    private Node2D nPeople1;
    [LacieEngine.API.GetNode("Main/People_2")]
    private Node2D nPeople2;
    [LacieEngine.API.GetNode("Main/People_3")]
    private Node2D nPeople3;
    [LacieEngine.API.GetNode("Main/Desk/FurnitureDesk4/MiscKnife")]
    private Node2D nKnife;
    [LacieEngine.API.GetNode("Main/Wardrobe")]
    private Sprite nWardrobe;
    [LacieEngine.API.GetNode("Main/Desk/FurnitureDesk4/Crow")]
    private Sprite nCrow;
    [LacieEngine.API.GetNode("Other/Events/misc_people_1")]
    private EventTrigger nWall1Event;
    [LacieEngine.API.GetNode("Other/Events/misc_people_2")]
    private EventTrigger nWall2Event;
    [LacieEngine.API.GetNode("Other/Events/misc_person_left")]
    private EventTrigger nWall3Event;
    [LacieEngine.API.GetNode("Other/Events/misc_person_right")]
    private EventTrigger nWall4Event;
    [LacieEngine.API.GetNode("Other/Events/misc_wardrobe")]
    private EventTrigger nWardrobeEvent;
    [LacieEngine.API.GetNode("Main/Desk/FurnitureDesk4/Crow/RedItem")]
    private Node2D nRedItem;
    private PVar varTookKnife = (PVar) "ch1.blackpass_classroom_took_knife";
    private PVar varKilledBird = (PVar) "ch1.blackpass_classroom_killed_bird";
    private PVar varCutsceneStage = (PVar) "ch1.blackpass_classroom_cutscene_stage";
    private PVar varTookRedItem = (PVar) "general.blackpass_red_item_3_took";
    private PVar varSpawnedRedItem = (PVar) "general.blackpass_red_item_3_spawned";

    public override void _UpdateRoom()
    {
      this.nKnife.Visible = !(bool) this.varTookKnife.Value;
      this.nBouncer.Visible = this.varCutsceneStage.Value == 2;
      this.nPeople1.Visible = this.varCutsceneStage.Value == 1;
      this.nPeople2.Visible = this.varCutsceneStage.Value == 2;
      this.nPeople3.Visible = this.varCutsceneStage.Value == 3;
      this.nWall1Event.Enabled = (int) this.varCutsceneStage.Value < 4;
      this.nWall2Event.Enabled = (int) this.varCutsceneStage.Value < 4;
      this.nWall3Event.Enabled = (int) this.varCutsceneStage.Value < 4;
      this.nWall4Event.Enabled = (int) this.varCutsceneStage.Value < 4;
      this.nWardrobe.Visible = (int) this.varCutsceneStage.Value >= 4;
      this.nWardrobeEvent.Enabled = (int) this.varCutsceneStage.Value >= 4;
      this.nCrow.Frame = (bool) this.varKilledBird.Value ? 1 : 0;
      this.nRedItem.Visible = (bool) this.varSpawnedRedItem.Value && !(bool) this.varTookRedItem.Value;
    }

    public async void OpenWardrobe()
    {
      Ch1BlackPassClassroom baseNode = this;
      baseNode.nWardrobe.Frame = 1;
      await baseNode.DelaySeconds(0.5);
      baseNode.nWardrobe.Frame = 2;
    }

    public void SetLacieShadow()
    {
      WalkingCharacter node = Game.Player.Node as WalkingCharacter;
      node.OverrideTextureForState("stand", this.texLacieDark);
      node.OverrideTextureForState("walk", this.texLacieDark);
    }
  }
}
