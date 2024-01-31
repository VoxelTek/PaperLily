// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1ForestLakeside
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Subgame.Chapter1;
using System;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1ForestLakeside : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private AudioStream sfxBunnyMelt;
    [LacieEngine.API.GetNode("Main/Bucket")]
    private Node2D nBucket;
    [LacieEngine.API.GetNode("Other/Events/item_bucket_get")]
    private EventTrigger nBucketEvt;
    [LacieEngine.API.GetNode("Foreground/MiscFishingrod")]
    private Node2D nFishingRod;
    [LacieEngine.API.GetNode("Other/Events/misc_fishingrod")]
    private EventTrigger nFishingRodEvt;
    [LacieEngine.API.GetNode("Ground/Rope1")]
    private Node2D nRope1;
    [LacieEngine.API.GetNode("Ground/Rope2")]
    private Node2D nRope2;
    [LacieEngine.API.GetNode("Main/foliage_tree/MiscRopeTree")]
    private Node2D nRope3;
    [LacieEngine.API.GetNode("Main/Cage")]
    private Sprite nCage;
    [LacieEngine.API.GetNode("Water/Underwater/Cage")]
    private Node2D nCageUnderwater;
    [LacieEngine.API.GetNode("Water/Underwater/AttackingCage")]
    private Node2D nCageAttacking;
    [LacieEngine.API.GetNode("Other/Navigation")]
    private Navigation2D nNavigation;
    [LacieEngine.API.GetNode("Other/Events/misc_cage")]
    private EventTrigger nCageEvt;
    [LacieEngine.API.GetNode("Other/Events/misc_cage_cord")]
    private EventTrigger nCageCordEvt;
    private Ch1LakesideBunny nSpecialBunny;
    private PVar varPickedUpBunny = (PVar) "ch1.forest_lakeside_took_bunny";
    private PVar varCageContent = (PVar) "ch1.forest_lakeside_cage_content";
    private PVar varCageInWater = (PVar) "ch1.forest_lakeside_cage_in_water";
    private PVar varFeedingMermaid = (PVar) "ch1.forest_lakeside_feeding_mermaid";
    private PVar varTookBucket = (PVar) "ch1.forest_lakeside_took_bucket";
    private PVar varTookCoin = (PVar) "ch1.forest_lakeside_took_fishing_coin";
    private PVar varFishingResult = (PVar) "ch1.temp_lakeside_fishing_result";
    private PVar varEnding8 = (PVar) "ch1.temp_ending_1_8";
    private PEvent evtBunnyMelted = (PEvent) "misc_bunny_blood";
    private PEvent evtBunnySpecial = (PEvent) "misc_bunny_special";

    public override void _BeforeFadeIn()
    {
      if ((bool) this.varEnding8.Value)
        return;
      this.MakeBunny(1);
      this.MakeBunny(2);
      this.MakeBunny(3);
      this.MakeBunny(4);
      this.MakeSpecialBunny();
    }

    public override void _UpdateRoom()
    {
      if ((bool) this.varPickedUpBunny.Value)
        this.nSpecialBunny.DeleteIfValid();
      this.nBucket.Visible = this.nBucketEvt.Enabled = !(bool) this.varTookBucket.Value;
      this.nRope1.Visible = !(bool) this.varCageInWater.Value;
      this.nRope2.Visible = !this.nRope1.Visible;
      this.nCage.Visible = !(bool) this.varCageInWater.Value;
      this.nCageUnderwater.Visible = !this.nCage.Visible;
      if ((bool) this.varFeedingMermaid.Value)
      {
        this.nCageUnderwater.Visible = false;
        this.nCageAttacking.Visible = true;
        ((IAnimation2D) this.nRope2).Play();
      }
      Sprite nCage = this.nCage;
      int num;
      switch ((string) this.varCageContent.Value)
      {
        case "bunny":
          num = 1;
          break;
        case "fish":
          num = 2;
          break;
        case "snack1":
          num = 3;
          break;
        case "snack2":
          num = 4;
          break;
        default:
          num = 0;
          break;
      }
      nCage.Frame = num;
      this.nCageEvt.Enabled = this.nCage.Visible;
      this.nCageCordEvt.Enabled = (bool) this.varCageInWater.Value;
      if (!(bool) this.varEnding8.Value)
        return;
      this.nFishingRod.Visible = this.nFishingRodEvt.Enabled = false;
      this.nBucket.Visible = this.nBucketEvt.Enabled = false;
      this.nRope1.Visible = false;
      this.nRope2.Visible = false;
      this.nRope3.Visible = false;
      this.nCage.Visible = false;
      this.nCageUnderwater.Visible = false;
      this.nCageEvt.Enabled = false;
      this.nCageCordEvt.Enabled = false;
    }

    public void FishingReward()
    {
      int num = new Random().Next((bool) this.varTookCoin.Value ? 3 : 4);
      PVar varFishingResult = this.varFishingResult;
      string str;
      switch (num)
      {
        case 0:
          str = "green";
          break;
        case 1:
          str = "blue";
          break;
        case 2:
          str = "black";
          break;
        case 3:
          str = "coin";
          break;
        default:
          str = "green";
          break;
      }
      varFishingResult.NewValue = (PVar.PVarSetProxy) str;
    }

    private void MakeBunny(int num)
    {
      Ch1LakesideBunny ch1LakesideBunny = new Ch1LakesideBunny();
      ch1LakesideBunny.CanMelt = true;
      ch1LakesideBunny.EventAfterMelt = this.evtBunnyMelted.Id;
      ch1LakesideBunny.MoveSpeedMultiplier = 1.4f;
      ch1LakesideBunny.Position = this.GetPoint("bunny_" + num.ToString());
      ch1LakesideBunny.SfxMelt = this.sfxBunnyMelt;
      this.GetMainLayer().AddChild((Node) ch1LakesideBunny);
      ch1LakesideBunny.SetNavigation(this.nNavigation);
    }

    private void MakeSpecialBunny()
    {
      this.nSpecialBunny = new Ch1LakesideBunny();
      this.nSpecialBunny.CanInteract = true;
      this.nSpecialBunny.EventWhileRunning = this.evtBunnySpecial.Id;
      this.nSpecialBunny.MoveSpeedMultiplier = 0.9f;
      this.nSpecialBunny.Position = this.GetPoint("bunny_special");
      this.GetMainLayer().AddChild((Node) this.nSpecialBunny);
      this.nSpecialBunny.SetNavigation(this.nNavigation);
    }
  }
}
