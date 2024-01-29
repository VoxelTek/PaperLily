using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1Home2fBedroomC : GameRoom
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

		[GetNode("Background/wall_window_day")]
		private Node2D nWallWindowDay;

		[GetNode("Background/wall_window_evening")]
		private Node2D nWallWindowEvening;

		[GetNode("Background/wall_window_night")]
		private Node2D nWallWindowNight;

		[GetNode("Foreground/lamp_light")]
		private Node2D nLampLight;

		[GetNode("Foreground/lamp_light_top")]
		private Node2D nLampLightTop;

		[GetNode("Background/furniture_wardrobe")]
		private Sprite nWardrobe;

		[GetNode("Foreground/WindowLights")]
		private Node2D nWindowLights;

		private PVar varLampOn = "general.lights_home_2f_bedroom_c_lamp";

		public override void _UpdateRoom()
		{
			Game.Room.ResetLighting();
			nWallWindowDay.Visible = true;
			nWallWindowEvening.Visible = false;
			nWallWindowNight.Visible = false;
			nLampLight.Visible = false;
			nLampLightTop.Visible = false;
			nWindowLights.Visible = false;
			if (Game.Variables.GetVariable("general.part_of_day") == "day")
			{
				if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_c"))
				{
					lightDayLightsOff.Apply();
					nWindowLights.Visible = true;
				}
				else
				{
					lightDayLightsOn.Apply();
				}
				if ((bool)varLampOn.Value)
				{
					nLampLightTop.Visible = true;
				}
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
			{
				nWallWindowDay.Visible = false;
				nWallWindowEvening.Visible = true;
				if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_c"))
				{
					lightEveningLightsOff.Apply();
					nWindowLights.Visible = true;
					if ((bool)varLampOn.Value)
					{
						nLampLight.Visible = true;
					}
				}
				else
				{
					lightEveningLightsOn.Apply();
					if ((bool)varLampOn.Value)
					{
						nLampLightTop.Visible = true;
					}
				}
			}
			else
			{
				if (!(Game.Variables.GetVariable("general.part_of_day") == "night"))
				{
					return;
				}
				nWallWindowDay.Visible = false;
				nWallWindowNight.Visible = true;
				if (!Game.Variables.GetFlag("general.lights_home_2f_bedroom_c"))
				{
					lightNightLightsOff.Apply();
					nWindowLights.Visible = true;
					if ((bool)varLampOn.Value)
					{
						nLampLight.Visible = true;
					}
				}
				else
				{
					lightNightLightsOn.Apply();
					if ((bool)varLampOn.Value)
					{
						nLampLightTop.Visible = true;
					}
				}
			}
		}

		public void OpenWardrobe()
		{
			nWardrobe.Frame = 2;
		}

		public async void CloseWardrobe()
		{
			nWardrobe.Frame = 1;
			await this.DelaySeconds(0.4);
			nWardrobe.Frame = 0;
			await this.DelaySeconds(0.4);
			nWardrobe.Frame = 1;
		}
	}
}
