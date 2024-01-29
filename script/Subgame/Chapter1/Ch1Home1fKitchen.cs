using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1Home1fKitchen : GameRoom
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

		[Inject]
		private IExtraFunctions Functions;

		[GetNode("Background/wall_window_day")]
		private Node2D nWallWindowDay;

		[GetNode("Background/wall_window_evening")]
		private Node2D nWallWindowEvening;

		[GetNode("Background/wall_window_night")]
		private Node2D nWallWindowNight;

		[GetNode("Foreground/window_light")]
		private Node2D nWindowLight;

		[GetNode("Foreground/window_light_projection")]
		private Node2D nWindowLightProjection;

		[GetNode("Background/misc_sink/misc_sink_water")]
		private Node2D nMiscSinkWater;

		[GetNode("Background/misc_sink/misc_sink_water/Animation")]
		private AnimationPlayer nMiscSinkWaterAnimation;

		[GetNode("Main/furniture_table/misc_note")]
		private Node2D nMiscNote;

		[GetNode("Background/misc_telephone")]
		private Node2D nMiscTelephone;

		[GetNode("Background/misc_telephone_ribbon")]
		private Node2D nMiscTelephoneRibbon;

		[GetNode("Background/misc_fridge")]
		private Sprite nMiscFridge;

		[GetNode("Background/furniture_counter3/item_birdfood")]
		private Sprite nItemBirdfood;

		private PVar varPhoneCallRinging = "ch1.home_phone_call_ringing";

		private PVar varTookBirdFood = "ch1.took_birdfood";

		private PVar varReadCursedLetter = "ch1.home_read_cursed_letter";

		private PVar varInbetween = "general.inbetween";

		private const float PhoneVolume = 1f;

		public override void _AfterFadeIn()
		{
			if ((bool)varPhoneCallRinging.Value)
			{
				AddChild(new HomePhoneRinger(sfxPhoneRing, 1f));
			}
		}

		public override void _UpdateRoom()
		{
			Game.Room.ResetLighting();
			nWallWindowDay.Visible = true;
			nWallWindowEvening.Visible = false;
			nWallWindowNight.Visible = false;
			nWindowLight.Visible = false;
			nWindowLightProjection.Visible = false;
			if (Game.Variables.GetVariable("general.part_of_day") == "day")
			{
				if (!Game.Variables.GetFlag("general.lights_home_1f_kitchen"))
				{
					lightDayLightsOff.Apply();
					nWindowLightProjection.Visible = true;
				}
				else
				{
					lightDayLightsOn.Apply();
				}
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
			{
				nWallWindowEvening.Visible = true;
				if (!Game.Variables.GetFlag("general.lights_home_1f_kitchen"))
				{
					lightEveningLightsOff.Apply();
					nWindowLightProjection.Visible = true;
				}
				else
				{
					lightEveningLightsOn.Apply();
				}
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "night")
			{
				nWallWindowNight.Visible = true;
				if (!Game.Variables.GetFlag("general.lights_home_1f_kitchen"))
				{
					lightNightLightsOff.Apply();
					nWindowLightProjection.Visible = true;
					nWindowLight.Visible = true;
				}
				else
				{
					lightNightLightsOn.Apply();
				}
			}
			if (Game.Variables.GetFlag("general.home1f_kitchen_tap"))
			{
				nMiscSinkWater.Visible = true;
				nMiscSinkWaterAnimation.PlayFirstAnimation();
			}
			else
			{
				nMiscSinkWaterAnimation.Stop();
				nMiscSinkWater.Visible = false;
			}
			if (Game.Variables.GetVariable("general.chapter") == "1")
			{
				nMiscNote.Visible = true;
				nItemBirdfood.Visible = !varTookBirdFood.Value;
				if (Game.Variables.GetFlag("ch1.set_phoneribbon"))
				{
					nMiscTelephoneRibbon.Visible = true;
					nMiscTelephone.Visible = false;
				}
				else
				{
					nMiscTelephoneRibbon.Visible = false;
					nMiscTelephone.Visible = true;
				}
				if ((bool)varInbetween.Value && (bool)varReadCursedLetter.Value)
				{
					nMiscNote.Visible = false;
				}
			}
		}

		public void OpenFridge()
		{
			nMiscFridge.Frame = 1;
		}

		public void CloseFridge()
		{
			nMiscFridge.Frame = 0;
		}

		public void Ch1StartTimer()
		{
			Functions.StartTimer(30f, delegate
			{
				Game.Events.PlayEvent("ch1_event_busleft");
			}, destroyTimerOnTimeout: true);
		}
	}
}
