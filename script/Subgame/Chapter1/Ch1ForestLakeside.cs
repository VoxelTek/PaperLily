using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Subgame.Chapter1;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1ForestLakeside : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private AudioStream sfxBunnyMelt;

		[GetNode("Main/Bucket")]
		private Node2D nBucket;

		[GetNode("Other/Events/item_bucket_get")]
		private EventTrigger nBucketEvt;

		[GetNode("Foreground/MiscFishingrod")]
		private Node2D nFishingRod;

		[GetNode("Other/Events/misc_fishingrod")]
		private EventTrigger nFishingRodEvt;

		[GetNode("Ground/Rope1")]
		private Node2D nRope1;

		[GetNode("Ground/Rope2")]
		private Node2D nRope2;

		[GetNode("Main/foliage_tree/MiscRopeTree")]
		private Node2D nRope3;

		[GetNode("Main/Cage")]
		private Sprite nCage;

		[GetNode("Water/Underwater/Cage")]
		private Node2D nCageUnderwater;

		[GetNode("Water/Underwater/AttackingCage")]
		private Node2D nCageAttacking;

		[GetNode("Other/Navigation")]
		private Navigation2D nNavigation;

		[GetNode("Other/Events/misc_cage")]
		private EventTrigger nCageEvt;

		[GetNode("Other/Events/misc_cage_cord")]
		private EventTrigger nCageCordEvt;

		private Ch1LakesideBunny nSpecialBunny;

		private PVar varPickedUpBunny = "ch1.forest_lakeside_took_bunny";

		private PVar varCageContent = "ch1.forest_lakeside_cage_content";

		private PVar varCageInWater = "ch1.forest_lakeside_cage_in_water";

		private PVar varFeedingMermaid = "ch1.forest_lakeside_feeding_mermaid";

		private PVar varTookBucket = "ch1.forest_lakeside_took_bucket";

		private PVar varTookCoin = "ch1.forest_lakeside_took_fishing_coin";

		private PVar varFishingResult = "ch1.temp_lakeside_fishing_result";

		private PVar varEnding8 = "ch1.temp_ending_1_8";

		private PEvent evtBunnyMelted = "misc_bunny_blood";

		private PEvent evtBunnySpecial = "misc_bunny_special";

		public override void _BeforeFadeIn()
		{
			if (!varEnding8.Value)
			{
				MakeBunny(1);
				MakeBunny(2);
				MakeBunny(3);
				MakeBunny(4);
				MakeSpecialBunny();
			}
		}

		public override void _UpdateRoom()
		{
			if ((bool)varPickedUpBunny.Value)
			{
				nSpecialBunny.DeleteIfValid();
			}
			Node2D node2D = nBucket;
			bool visible = (nBucketEvt.Enabled = !varTookBucket.Value);
			node2D.Visible = visible;
			nRope1.Visible = !varCageInWater.Value;
			nRope2.Visible = !nRope1.Visible;
			nCage.Visible = !varCageInWater.Value;
			nCageUnderwater.Visible = !nCage.Visible;
			if ((bool)varFeedingMermaid.Value)
			{
				nCageUnderwater.Visible = false;
				nCageAttacking.Visible = true;
				((IAnimation2D)nRope2).Play();
			}
			Sprite sprite = nCage;
			sprite.Frame = (string)varCageContent.Value switch
			{
				"bunny" => 1, 
				"fish" => 2, 
				"snack1" => 3, 
				"snack2" => 4, 
				_ => 0, 
			};
			nCageEvt.Enabled = nCage.Visible;
			nCageCordEvt.Enabled = varCageInWater.Value;
			if ((bool)varEnding8.Value)
			{
				Node2D node2D2 = nFishingRod;
				visible = (nFishingRodEvt.Enabled = false);
				node2D2.Visible = visible;
				Node2D node2D3 = nBucket;
				visible = (nBucketEvt.Enabled = false);
				node2D3.Visible = visible;
				nRope1.Visible = false;
				nRope2.Visible = false;
				nRope3.Visible = false;
				nCage.Visible = false;
				nCageUnderwater.Visible = false;
				nCageEvt.Enabled = false;
				nCageCordEvt.Enabled = false;
			}
		}

		public void FishingReward()
		{
			int r = new Random().Next(varTookCoin.Value ? 3 : 4);
			PVar pVar = varFishingResult;
			pVar.NewValue = r switch
			{
				0 => "green", 
				1 => "blue", 
				2 => "black", 
				3 => "coin", 
				_ => "green", 
			};
		}

		private void MakeBunny(int num)
		{
			Ch1LakesideBunny nBunny = new Ch1LakesideBunny();
			nBunny.CanMelt = true;
			nBunny.EventAfterMelt = evtBunnyMelted.Id;
			nBunny.MoveSpeedMultiplier = 1.4f;
			nBunny.Position = GetPoint("bunny_" + num);
			nBunny.SfxMelt = sfxBunnyMelt;
			GetMainLayer().AddChild(nBunny);
			nBunny.SetNavigation(nNavigation);
		}

		private void MakeSpecialBunny()
		{
			nSpecialBunny = new Ch1LakesideBunny();
			nSpecialBunny.CanInteract = true;
			nSpecialBunny.EventWhileRunning = evtBunnySpecial.Id;
			nSpecialBunny.MoveSpeedMultiplier = 0.9f;
			nSpecialBunny.Position = GetPoint("bunny_special");
			GetMainLayer().AddChild(nSpecialBunny);
			nSpecialBunny.SetNavigation(nNavigation);
		}
	}
}
