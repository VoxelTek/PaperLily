using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1Home2fBedroomA : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Lighting lightDayLightsOff;

		[Export(PropertyHint.None, "")]
		private Lighting lightDayLightsOn;

		[Export(PropertyHint.None, "")]
		private Lighting lightEveningLightsOff;

		[Export(PropertyHint.None, "")]
		private Lighting lightEveningLightsOn;

		[Export(PropertyHint.None, "")]
		private Lighting lightNightLightsOff;

		[Export(PropertyHint.None, "")]
		private Lighting lightNightLightsOn;

		[GetNode("Main/FurnitureChair/lacie")]
		private Node2D nLacieAtComputer;

		[GetNode("Foreground/WindowLights")]
		private Node2D nWindowLights;

		[GetNode("Foreground/wall_shelf_2/shiny")]
		private Node2D nCh1ItemsShiny;

		private PVar varTookCh1Items = "ch1.home_took_wardrobe_items";

		public override void _UpdateRoom()
		{
			Game.Room.ResetLighting();
			nWindowLights.Visible = false;
			if (Game.Variables.GetVariable("general.part_of_day") == "day")
			{
				if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_a"))
				{
					lightDayLightsOff.Apply();
					nWindowLights.Visible = true;
				}
				else
				{
					lightDayLightsOn.Apply();
				}
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
			{
				if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_a"))
				{
					lightEveningLightsOff.Apply();
					nWindowLights.Visible = true;
				}
				else
				{
					lightEveningLightsOn.Apply();
				}
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "night")
			{
				if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_a"))
				{
					lightNightLightsOff.Apply();
					nWindowLights.Visible = true;
					nWindowLights.Visible = true;
				}
				else
				{
					lightNightLightsOn.Apply();
				}
			}
			if (Game.Variables.GetVariable("general.chapter") == "1")
			{
				nCh1ItemsShiny.Visible = !varTookCh1Items.Value;
			}
		}

		public void ComputerSit()
		{
			Game.Player.Node.Visible = false;
			nLacieAtComputer.Visible = true;
		}

		public void ComputerGetUp()
		{
			Game.Player.Node.Visible = true;
			nLacieAtComputer.Visible = false;
		}

		public void HidePlayer()
		{
			Game.Player.Node.Visible = false;
		}
	}
}
