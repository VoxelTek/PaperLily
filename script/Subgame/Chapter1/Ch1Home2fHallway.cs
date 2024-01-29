using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1Home2fHallway : GameRoom
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

		[Export(PropertyHint.None, "")]
		private AudioStream sfxPhoneRing;

		[GetNode("Background/wall_door2")]
		private Sprite nLastDoor;

		[GetNode("Lighting/WindowLights")]
		private Node2D nWindowLights;

		[GetNode("Lighting/lamp_light")]
		private Node2D nLampLight;

		[GetNode("Lighting/lamp_light_top")]
		private Node2D nLampLightTop;

		private PVar varChapter = "general.chapter";

		private PVar varInbeteween = "general.inbetween";

		private PVar varDoorSpook = "ch1.home_seen_spook_door";

		private PVar varPhoneCallRinging = "ch1.home_phone_call_ringing";

		private PVar varLampOn = "general.lights_home_2f_hallway_lamp";

		private PVar varMissedBus = "ch1.home_missed_bus";

		private const float PhoneVolume = 0.1f;

		public override void _AfterFadeIn()
		{
			if ((bool)varPhoneCallRinging.Value)
			{
				AddChild(new HomePhoneRinger(sfxPhoneRing, 0.1f));
			}
		}

		public override void _UpdateRoom()
		{
			Game.Room.ResetLighting();
			nWindowLights.Visible = false;
			nLampLight.Visible = false;
			nLampLightTop.Visible = false;
			if (Game.Variables.GetVariable("general.part_of_day") == "day")
			{
				if (!Game.Variables.GetFlag("general.lights_home_2f_hallway"))
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
				if (!Game.Variables.GetFlag("general.lights_home_2f_hallway"))
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
			else if (Game.Variables.GetVariable("general.part_of_day") == "night")
			{
				if (!Game.Variables.GetFlag("general.lights_home_2f_hallway"))
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
			if (varChapter.Value == 1 && (bool)varInbeteween.Value && !varMissedBus.Value)
			{
				nLastDoor.Frame = ((!varDoorSpook.Value) ? 1 : 0);
			}
		}
	}
}
