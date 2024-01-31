// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1BlackPassHub
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;
using LacieEngine.Subgame.PaperLily;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1BlackPassHub : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private Texture texLacieDark;
    [LacieEngine.API.GetNode("Other/Stairs_Bottom1")]
    private Node nStairsBottom1Trig;
    [LacieEngine.API.GetNode("Other/Stairs_Bottom2")]
    private Node nStairsBottom2Trig;
    [LacieEngine.API.GetNode("Background/FloorStairsBottom1")]
    private Node2D nStairsBottom1;
    [LacieEngine.API.GetNode("Background/FloorStairsBottom2")]
    private Node2D nStairsBottom2;
    [LacieEngine.API.GetNode("Main/Door1")]
    private StaticBody2D nDoor1;
    [LacieEngine.API.GetNode("Main/Door2")]
    private StaticBody2D nDoor2;
    [LacieEngine.API.GetNode("Main/Door3")]
    private StaticBody2D nDoor3;
    [LacieEngine.API.GetNode("Main/Door4")]
    private StaticBody2D nDoor4;
    [LacieEngine.API.GetNode("Main/Door5")]
    private StaticBody2D nDoor5;
    [LacieEngine.API.GetNode("Main/Door6")]
    private StaticBody2D nDoor6;
    [LacieEngine.API.GetNode("Other/Events/misc_door_1")]
    private EventTrigger nDoor1Event;
    [LacieEngine.API.GetNode("Other/Events/misc_door_2")]
    private EventTrigger nDoor2Event;
    [LacieEngine.API.GetNode("Other/Events/misc_door_3")]
    private EventTrigger nDoor3Event;
    [LacieEngine.API.GetNode("Other/Events/misc_door_4")]
    private EventTrigger nDoor4Event;
    [LacieEngine.API.GetNode("Other/Events/misc_door_5")]
    private EventTrigger nDoor5Event;
    [LacieEngine.API.GetNode("Other/Events/misc_door_6")]
    private EventTrigger nDoor6Event;
    [LacieEngine.API.GetNode("Other/Stairs_Red")]
    private Stairs nRedStairs;
    [LacieEngine.API.GetNode("Other/Stairs_Red_Boundary")]
    private CollisionObject2D nRedStairsBoundary;
    [LacieEngine.API.GetNode("Background/FloorStairsRed")]
    private Node2D nStairsRedSpr;
    [LacieEngine.API.GetNode("Main/LargeDoor")]
    private Sprite nLargeDoor;
    [LacieEngine.API.GetNode("Main/FurnitureBed")]
    private Node2D nBed;
    [LacieEngine.API.GetNode("Other/Events/misc_bed")]
    private EventTrigger nBedEvent;
    private PVar varVisits = (PVar) "general.blackpass_visits";
    private PVar varRedItems = (PVar) "general.blackpass_red_items";
    private PVar varRedItem1 = (PVar) "general.blackpass_red_item_1_took";
    private PVar varRedItem2 = (PVar) "general.blackpass_red_item_2_took";
    private PVar varRedItem3 = (PVar) "general.blackpass_red_item_3_took";
    private PVar varItemStorage = (PVar) "ch1.temp_blackpass_item_storage";
    private PVar varEnding1_3 = (PVar) "ch1.temp_ending_1_3";
    private PEvent evtLost = (PEvent) "ch1_ending_1_3";

    public override void _BeforeFadeIn()
    {
      this.nDoor1.Visible = false;
      this.nDoor2.Visible = false;
      this.nDoor3.Visible = false;
      this.nDoor4.Visible = false;
      this.nDoor5.Visible = false;
      this.nDoor6.Visible = false;
      this.nDoor1.CollisionLayer = 0U;
      this.nDoor2.CollisionLayer = 0U;
      this.nDoor3.CollisionLayer = 0U;
      this.nDoor4.CollisionLayer = 0U;
      this.nDoor5.CollisionLayer = 0U;
      this.nDoor6.CollisionLayer = 0U;
      this.nDoor1Event.Enabled = false;
      this.nDoor2Event.Enabled = false;
      this.nDoor3Event.Enabled = false;
      this.nDoor4Event.Enabled = false;
      this.nDoor5Event.Enabled = false;
      this.nDoor6Event.Enabled = false;
      this.nStairsRedSpr.Visible = false;
      this.nRedStairs.Enabled = false;
      this.nRedStairsBoundary.CollisionLayer = 0U;
      if ((bool) this.varEnding1_3.Value)
      {
        this.nBed.Visible = false;
        this.nBedEvent.Enabled = false;
        this.nStairsBottom1Trig.Delete();
        this.nStairsBottom2Trig.Delete();
        this.nStairsBottom1.Visible = false;
        this.nStairsBottom2.Visible = false;
        this.nLargeDoor.Frame = 1;
        this.DisableRunning = true;
      }
      else
      {
        if (this.varVisits.Value == 1)
        {
          this.nStairsBottom1Trig.Delete();
          this.nStairsBottom2Trig.Delete();
          this.nStairsBottom1.Visible = false;
          this.nStairsBottom2.Visible = false;
          this.EnableDoor1();
        }
        else if (this.varVisits.Value == 2)
          this.EnableDoor1();
        else if (this.varVisits.Value == 3)
        {
          this.EnableDoor1();
          this.EnableDoor2();
        }
        else if (this.varVisits.Value == 4)
        {
          this.EnableDoor1();
          this.EnableDoor2();
          this.EnableDoor3();
        }
        if ((int) this.varRedItems.Value <= 0)
          return;
        this.nStairsRedSpr.Visible = true;
        this.nRedStairs.Enabled = true;
        this.nRedStairsBoundary.CollisionLayer = 2U;
      }
    }

    public async void EventLost()
    {
      Ch1BlackPassHub baseNode = this;
      await baseNode.DelaySeconds(20.0);
      baseNode.evtLost.Play();
    }

    public void SetLacieShadow()
    {
      WalkingCharacter node = Game.Player.Node as WalkingCharacter;
      node.OverrideTextureForState("stand", this.texLacieDark);
      node.OverrideTextureForState("walk", this.texLacieDark);
    }

    public void RemoveInventory() => PaperLilyFunctions.RemoveInventory(this.varItemStorage);

    public void RestoreInventory() => PaperLilyFunctions.RestoreInventory(this.varItemStorage);

    public void GetRedItems()
    {
      if ((bool) this.varRedItem1.Value)
        Game.Items.AddItem("global.reditem_1");
      if ((bool) this.varRedItem2.Value)
        Game.Items.AddItem("global.reditem_2");
      if (!(bool) this.varRedItem3.Value)
        return;
      Game.Items.AddItem("global.reditem_3");
    }

    private void EnableDoor1()
    {
      this.nDoor1.Visible = true;
      this.nDoor1.CollisionLayer = 2U;
      this.nDoor1Event.Enabled = true;
    }

    private void EnableDoor2()
    {
      this.nDoor2.Visible = true;
      this.nDoor2.CollisionLayer = 2U;
      this.nDoor2Event.Enabled = true;
    }

    private void EnableDoor3()
    {
      this.nDoor3.Visible = true;
      this.nDoor3.CollisionLayer = 2U;
      this.nDoor3Event.Enabled = true;
    }
  }
}
