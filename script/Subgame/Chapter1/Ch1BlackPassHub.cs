using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;
using LacieEngine.Subgame.PaperLily;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1BlackPassHub : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Texture texLacieDark;

		[GetNode("Other/Stairs_Bottom1")]
		private Node nStairsBottom1Trig;

		[GetNode("Other/Stairs_Bottom2")]
		private Node nStairsBottom2Trig;

		[GetNode("Background/FloorStairsBottom1")]
		private Node2D nStairsBottom1;

		[GetNode("Background/FloorStairsBottom2")]
		private Node2D nStairsBottom2;

		[GetNode("Main/Door1")]
		private StaticBody2D nDoor1;

		[GetNode("Main/Door2")]
		private StaticBody2D nDoor2;

		[GetNode("Main/Door3")]
		private StaticBody2D nDoor3;

		[GetNode("Main/Door4")]
		private StaticBody2D nDoor4;

		[GetNode("Main/Door5")]
		private StaticBody2D nDoor5;

		[GetNode("Main/Door6")]
		private StaticBody2D nDoor6;

		[GetNode("Other/Events/misc_door_1")]
		private EventTrigger nDoor1Event;

		[GetNode("Other/Events/misc_door_2")]
		private EventTrigger nDoor2Event;

		[GetNode("Other/Events/misc_door_3")]
		private EventTrigger nDoor3Event;

		[GetNode("Other/Events/misc_door_4")]
		private EventTrigger nDoor4Event;

		[GetNode("Other/Events/misc_door_5")]
		private EventTrigger nDoor5Event;

		[GetNode("Other/Events/misc_door_6")]
		private EventTrigger nDoor6Event;

		[GetNode("Other/Stairs_Red")]
		private Stairs nRedStairs;

		[GetNode("Other/Stairs_Red_Boundary")]
		private CollisionObject2D nRedStairsBoundary;

		[GetNode("Background/FloorStairsRed")]
		private Node2D nStairsRedSpr;

		[GetNode("Main/LargeDoor")]
		private Sprite nLargeDoor;

		[GetNode("Main/FurnitureBed")]
		private Node2D nBed;

		[GetNode("Other/Events/misc_bed")]
		private EventTrigger nBedEvent;

		private PVar varVisits = "general.blackpass_visits";

		private PVar varRedItems = "general.blackpass_red_items";

		private PVar varRedItem1 = "general.blackpass_red_item_1_took";

		private PVar varRedItem2 = "general.blackpass_red_item_2_took";

		private PVar varRedItem3 = "general.blackpass_red_item_3_took";

		private PVar varItemStorage = "ch1.temp_blackpass_item_storage";

		private PVar varEnding1_3 = "ch1.temp_ending_1_3";

		private PEvent evtLost = "ch1_ending_1_3";

		public override void _BeforeFadeIn()
		{
			nDoor1.Visible = false;
			nDoor2.Visible = false;
			nDoor3.Visible = false;
			nDoor4.Visible = false;
			nDoor5.Visible = false;
			nDoor6.Visible = false;
			nDoor1.CollisionLayer = 0u;
			nDoor2.CollisionLayer = 0u;
			nDoor3.CollisionLayer = 0u;
			nDoor4.CollisionLayer = 0u;
			nDoor5.CollisionLayer = 0u;
			nDoor6.CollisionLayer = 0u;
			nDoor1Event.Enabled = false;
			nDoor2Event.Enabled = false;
			nDoor3Event.Enabled = false;
			nDoor4Event.Enabled = false;
			nDoor5Event.Enabled = false;
			nDoor6Event.Enabled = false;
			nStairsRedSpr.Visible = false;
			nRedStairs.Enabled = false;
			nRedStairsBoundary.CollisionLayer = 0u;
			if ((bool)varEnding1_3.Value)
			{
				nBed.Visible = false;
				nBedEvent.Enabled = false;
				nStairsBottom1Trig.Delete();
				nStairsBottom2Trig.Delete();
				nStairsBottom1.Visible = false;
				nStairsBottom2.Visible = false;
				nLargeDoor.Frame = 1;
				DisableRunning = true;
				return;
			}
			if (varVisits.Value == 1)
			{
				nStairsBottom1Trig.Delete();
				nStairsBottom2Trig.Delete();
				nStairsBottom1.Visible = false;
				nStairsBottom2.Visible = false;
				EnableDoor1();
			}
			else if (varVisits.Value == 2)
			{
				EnableDoor1();
			}
			else if (varVisits.Value == 3)
			{
				EnableDoor1();
				EnableDoor2();
			}
			else if (varVisits.Value == 4)
			{
				EnableDoor1();
				EnableDoor2();
				EnableDoor3();
			}
			if ((int)varRedItems.Value > 0)
			{
				nStairsRedSpr.Visible = true;
				nRedStairs.Enabled = true;
				nRedStairsBoundary.CollisionLayer = 2u;
			}
		}

		public async void EventLost()
		{
			await this.DelaySeconds(20.0);
			evtLost.Play();
		}

		public void SetLacieShadow()
		{
			WalkingCharacter obj = Game.Player.Node as WalkingCharacter;
			obj.OverrideTextureForState("stand", texLacieDark);
			obj.OverrideTextureForState("walk", texLacieDark);
		}

		public void RemoveInventory()
		{
			PaperLilyFunctions.RemoveInventory(varItemStorage);
		}

		public void RestoreInventory()
		{
			PaperLilyFunctions.RestoreInventory(varItemStorage);
		}

		public void GetRedItems()
		{
			if ((bool)varRedItem1.Value)
			{
				Game.Items.AddItem("global.reditem_1");
			}
			if ((bool)varRedItem2.Value)
			{
				Game.Items.AddItem("global.reditem_2");
			}
			if ((bool)varRedItem3.Value)
			{
				Game.Items.AddItem("global.reditem_3");
			}
		}

		private void EnableDoor1()
		{
			nDoor1.Visible = true;
			nDoor1.CollisionLayer = 2u;
			nDoor1Event.Enabled = true;
		}

		private void EnableDoor2()
		{
			nDoor2.Visible = true;
			nDoor2.CollisionLayer = 2u;
			nDoor2Event.Enabled = true;
		}

		private void EnableDoor3()
		{
			nDoor3.Visible = true;
			nDoor3.CollisionLayer = 2u;
			nDoor3Event.Enabled = true;
		}
	}
}
