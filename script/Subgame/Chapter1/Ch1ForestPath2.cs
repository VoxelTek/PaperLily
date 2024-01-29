using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1ForestPath2 : GameRoom
	{
		[Inject]
		private IVariableManager Vars;

		[Inject]
		private IEventManager Events;

		[GetNode("Main/signpost_blue_1")]
		private Sprite nBlueSign1;

		[GetNode("Main/signpost_blue_2")]
		private Sprite nBlueSign2;

		[GetNode("Main/signpost_blue_3")]
		private Sprite nBlueSign3;

		[GetNode("Main/signpost_yellow_1")]
		private Sprite nYellowSign1;

		[GetNode("Main/signpost_yellow_2")]
		private Sprite nYellowSign2;

		[GetNode("Main/signpost_yellow_3")]
		private Sprite nYellowSign3;

		[GetNode("Main/signpost_red_1")]
		private Sprite nRedSign1;

		[GetNode("Main/signpost_red_2")]
		private Sprite nRedSign2;

		[GetNode("TilesNorthBlock1")]
		private TileMap nTilesNorthBlock1;

		[GetNode("TilesNorthBlock2")]
		private TileMap nTilesNorthBlock2;

		[GetNode("Background/EntranceL")]
		private Node2D nEntranceL;

		[GetNode("Background/EntranceR")]
		private Node2D nEntranceR;

		[GetNode("Other/Boundary")]
		private StaticBody2D nPassageWalls;

		[GetNode("Other/BoundaryInStairs")]
		private StaticBody2D nTunnelWalls;

		[GetNode("Other/Events/move_station")]
		private EventTrigger nPassageEvent;

		[GetNode("Main/StructStairs")]
		private Node2D nPassageStairs;

		[GetNode("Main/StructStairsFence")]
		private Node2D nPassageFence;

		[GetNode("Foreground/MiscStairsMask")]
		private Node2D nHidePlayerMask;

		[GetNode("Other/Stairs")]
		private Stairs nStairs;

		[GetNode("Other/ActivateStairsTrigger")]
		private Area2D nStairsTrigger;

		[GetNode("Lighting/LightStationAccess")]
		private Node2D nLightStationAccess;

		private PVar varTryResetPuzzle = "ch1.temp_forest_trigger_event_try_reset";

		private string _signToRotate;

		public override void _BeforeFadeIn()
		{
			Vars.SetInitialValue("ch1.forest_entrance_sign_blue_1", "left");
			Vars.SetInitialValue("ch1.forest_entrance_sign_blue_2", "left");
			Vars.SetInitialValue("ch1.forest_entrance_sign_blue_3", "left");
			Vars.SetInitialValue("ch1.forest_entrance_sign_yellow_1", "left");
			Vars.SetInitialValue("ch1.forest_entrance_sign_yellow_2", "left");
			Vars.SetInitialValue("ch1.forest_entrance_sign_yellow_3", "left");
			Vars.SetInitialValue("ch1.forest_entrance_sign_red_1", "left");
			Vars.SetInitialValue("ch1.forest_entrance_sign_red_2", "left");
			if (!Vars.GetFlag("ch1.forest_entrance_gate_open") && !Events.SeenEvent("Ch1_Forest_Entrance.event_try_reset") && 0 + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_1"), 1) + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_2"), 1) + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_3"), 1) + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_4"), 1) + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_5"), 1) + Math.Min(Game.Events.GetEventInteractionCount("Ch1_Forest_Entrance.misc_light_switch_6"), 1) >= 4)
			{
				TriggerEventTryReset();
			}
		}

		public override void _UpdateRoom()
		{
			UpdateSignDirection(nBlueSign1, "ch1.forest_entrance_sign_blue_1");
			UpdateSignDirection(nBlueSign2, "ch1.forest_entrance_sign_blue_2");
			UpdateSignDirection(nBlueSign3, "ch1.forest_entrance_sign_blue_3");
			UpdateSignDirection(nYellowSign1, "ch1.forest_entrance_sign_yellow_1");
			UpdateSignDirection(nYellowSign2, "ch1.forest_entrance_sign_yellow_2");
			UpdateSignDirection(nYellowSign3, "ch1.forest_entrance_sign_yellow_3");
			UpdateSignDirection(nRedSign1, "ch1.forest_entrance_sign_red_1");
			UpdateSignDirection(nRedSign2, "ch1.forest_entrance_sign_red_2");
			if (Vars.GetFlag("ch1.forest_entrance_south_opened_exit"))
			{
				nTilesNorthBlock1.Visible = false;
				nTilesNorthBlock1.CollisionLayer = 0u;
				nTilesNorthBlock2.Visible = false;
				nTilesNorthBlock2.CollisionLayer = 0u;
			}
			else
			{
				nTilesNorthBlock1.Visible = true;
				nTilesNorthBlock1.CollisionLayer = 2u;
				nTilesNorthBlock2.Visible = true;
				nTilesNorthBlock2.CollisionLayer = 2u;
			}
			DeactivateStairs(null);
			if (Vars.GetFlag("ch1.forest_entrance_south_opened_station"))
			{
				nPassageWalls.CollisionLayer = 2u;
				nStairsTrigger.CollisionLayer = 1u;
				nPassageStairs.Visible = true;
				nPassageFence.Visible = true;
				nLightStationAccess.Visible = true;
			}
			else
			{
				nPassageWalls.CollisionLayer = 0u;
				nStairsTrigger.CollisionLayer = 0u;
				nPassageStairs.Visible = false;
				nPassageFence.Visible = false;
				nLightStationAccess.Visible = false;
			}
			nEntranceL.Visible = Vars.GetFlag("ch1.forest_entrance_south_opened_secret_1");
			nEntranceR.Visible = Vars.GetFlag("ch1.forest_entrance_south_opened_secret_2");
			if (nStairsTrigger.OverlapsBody(Game.Player.Node))
			{
				ActivateStairs(Game.Player.Node);
			}
		}

		public void ActivateStairs(Node _)
		{
			nTunnelWalls.CollisionLayer = 10u;
			nPassageEvent.Enabled = true;
			nHidePlayerMask.Visible = true;
			nStairs.Enabled = true;
		}

		public void DeactivateStairs(Node _)
		{
			if (!nStairs.OverlapsBody(Game.Player.Node))
			{
				nTunnelWalls.CollisionLayer = 0u;
				nPassageEvent.Enabled = false;
				nHidePlayerMask.Visible = false;
				nStairs.Enabled = false;
			}
		}

		public void RotateBlue1()
		{
			_signToRotate = "ch1.forest_entrance_sign_blue_1";
		}

		public void RotateBlue2()
		{
			_signToRotate = "ch1.forest_entrance_sign_blue_2";
		}

		public void RotateBlue3()
		{
			_signToRotate = "ch1.forest_entrance_sign_blue_3";
		}

		public void RotateYellow1()
		{
			_signToRotate = "ch1.forest_entrance_sign_yellow_1";
		}

		public void RotateYellow2()
		{
			_signToRotate = "ch1.forest_entrance_sign_yellow_2";
		}

		public void RotateYellow3()
		{
			_signToRotate = "ch1.forest_entrance_sign_yellow_3";
		}

		public void RotateRed1()
		{
			_signToRotate = "ch1.forest_entrance_sign_red_1";
		}

		public void RotateRed2()
		{
			_signToRotate = "ch1.forest_entrance_sign_red_2";
		}

		public void RotateLeft()
		{
			RotateSignVariable(_signToRotate, Direction.Left);
		}

		public void RotateUp()
		{
			RotateSignVariable(_signToRotate, Direction.Up);
		}

		public void RotateRight()
		{
			RotateSignVariable(_signToRotate, Direction.Right);
		}

		public void RotateDown()
		{
			RotateSignVariable(_signToRotate, Direction.Down);
		}

		private async void TriggerEventTryReset()
		{
			varTryResetPuzzle.NewValue = true;
			await this.DelaySeconds(10.0);
			varTryResetPuzzle.NewValue = false;
		}

		private void RotateSignVariable(string variable, Direction direction)
		{
			Vars.SetVariable(variable, direction.ToString());
			UpdateSignSolutions();
		}

		private void UpdateSignDirection(Sprite sign, string variable)
		{
			sign.Frame = Vars.GetVariable(variable) switch
			{
				"left" => 0, 
				"up" => 1, 
				"right" => 2, 
				"down" => 3, 
				_ => throw new InvalidOperationException(), 
			};
		}

		private void UpdateSignSolutions()
		{
			Vars.SetFlag("ch1.forest_entrance_south_opened_exit", Vars.GetVariable("ch1.forest_entrance_sign_blue_1") == "up" && Vars.GetVariable("ch1.forest_entrance_sign_blue_2") == "down" && Vars.GetVariable("ch1.forest_entrance_sign_blue_3") == "up");
			Vars.SetFlag("ch1.forest_entrance_south_opened_station", Vars.GetVariable("ch1.forest_entrance_sign_yellow_1") == "up" && Vars.GetVariable("ch1.forest_entrance_sign_yellow_2") == "right" && Vars.GetVariable("ch1.forest_entrance_sign_yellow_3") == "right");
			Vars.SetFlag("ch1.forest_entrance_south_opened_secret_1", Vars.GetVariable("ch1.forest_entrance_sign_red_1") == "left" && Vars.GetVariable("ch1.forest_entrance_sign_red_2") == "down");
			Vars.SetFlag("ch1.forest_entrance_south_opened_secret_2", Vars.GetVariable("ch1.forest_entrance_sign_red_1") == "up" && Vars.GetVariable("ch1.forest_entrance_sign_red_2") == "right");
		}
	}
}
