using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1HomeExteriorBack : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Lighting lightDay;

		[Export(PropertyHint.None, "")]
		private Lighting lightEvening;

		[Export(PropertyHint.None, "")]
		private Lighting lightNight;

		[GetNode("Foreground/livingroom_door")]
		private Node2D nLivingroomDoor;

		[GetNode("Foreground/hallway_window")]
		private Node2D nHallwayWindow;

		[GetNode("Foreground/hallway_window_2")]
		private Node2D nHallwayWindow2;

		[GetNode("Foreground/lamp_1")]
		private Node2D nLamp1;

		[GetNode("Foreground/lamp_2")]
		private Node2D nLamp2;

		[GetNode("Foreground/lamp_3")]
		private Node2D lamp_3;

		[GetNode("Foreground/lamp_4")]
		private Node2D nLamp4;

		[GetNode("Background/furniture_bench/chr_hiro")]
		private NpcStaticTurnableVer2 nHiroBench;

		[GetNode("Other/Events/chr_hiro")]
		private EventTrigger nHiroEvent;

		private PVar varHiroAllTalks = "ch1.home_hiro_alltalks";

		public override void _UpdateRoom()
		{
			Game.Room.ResetLighting();
			nLivingroomDoor.Visible = false;
			nHallwayWindow.Visible = false;
			nHallwayWindow2.Visible = false;
			nLamp1.Visible = false;
			nLamp2.Visible = false;
			lamp_3.Visible = false;
			nLamp4.Visible = false;
			if (Game.Variables.GetVariable("general.part_of_day") == "day")
			{
				lightDay.Apply();
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
			{
				lightEvening.Apply();
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "night")
			{
				lightNight.Apply();
				nLamp1.Visible = true;
				nLamp2.Visible = true;
				lamp_3.Visible = true;
				nLamp4.Visible = true;
				if (Game.Variables.GetFlag("general.lights_home1f_livingroom"))
				{
					nLivingroomDoor.Visible = true;
				}
				if (Game.Variables.GetFlag("general.lights_home_2f_hallway"))
				{
					nHallwayWindow.Visible = true;
					nHallwayWindow2.Visible = true;
				}
			}
			if (Game.Variables.GetVariable("general.chapter") == "1")
			{
				if (Game.Variables.GetVariable("general.part_of_day") == "evening")
				{
					nHiroBench.Visible = true;
					nHiroEvent.Enabled = true;
				}
				else
				{
					nHiroBench.Visible = false;
					nHiroEvent.Enabled = false;
				}
				if ((bool)varHiroAllTalks.Value)
				{
					nHiroBench.TurningEnabled = true;
					nHiroBench.TurnToDefault();
					nHiroBench.TurningEnabled = false;
				}
			}
		}
	}
}
