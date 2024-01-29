using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1Home1fLivingroom : GameRoom
	{
		[Inject]
		private IExtraFunctions ExtraFunctions;

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

		[GetNode("Background/wall_window_day")]
		private Node2D nWallWindowDay;

		[GetNode("Background/wall_window_2_day")]
		private Node2D nWallWindow2Day;

		[GetNode("Background/wall_window_evening")]
		private Node2D nWallWindowEvening;

		[GetNode("Background/wall_window_2_evening")]
		private Node2D nWallWindow2Evening;

		[GetNode("Background/wall_window_night")]
		private Node2D nWallWindowNight;

		[GetNode("Background/wall_window_2_night")]
		private Node2D nWallWindow2Night;

		[GetNode("Foreground/window_light_projection")]
		private Node2D nWindowLightProjection;

		[GetNode("Foreground/window_light_projection_2")]
		private Node2D nWindowLightProjection2;

		[GetNode("Foreground/wall_window_light")]
		private Node2D nWallWindowLight;

		[GetNode("Foreground/wall_window_light_2")]
		private Node2D nWallWindowLight2;

		[GetNode("Foreground/lamp_light")]
		private Node2D nLampLight;

		[GetNode("Foreground/lamp_light_top")]
		private Node2D nLampLightTop;

		[GetNode("Background/misc_tv/Animation")]
		private AnimationPlayer nTvAnimation;

		[GetNode("Background/wall_painting3")]
		private Sprite nCatPainting;

		private PVar varInbetween = "general.inbetween";

		private PVar varPhoneCallRinging = "ch1.home_phone_call_ringing";

		private PVar varPhoneCallDone = "ch1.home_phone_call_done";

		private PVar varLampOn = "general.lights_home1f_lamp";

		private const float PhoneVolume = 0.6f;

		public override void _AfterFadeIn()
		{
			if ((bool)varPhoneCallRinging.Value)
			{
				AddChild(new HomePhoneRinger(sfxPhoneRing, 0.6f));
			}
		}

		public override void _UpdateRoom()
		{
			Game.Room.ResetLighting();
			nWallWindowDay.Visible = true;
			nWallWindow2Day.Visible = true;
			nWallWindowEvening.Visible = false;
			nWallWindow2Evening.Visible = false;
			nWallWindowNight.Visible = false;
			nWallWindow2Night.Visible = false;
			nWindowLightProjection.Visible = false;
			nWindowLightProjection2.Visible = false;
			nWallWindowLight.Visible = false;
			nWallWindowLight2.Visible = false;
			nLampLight.Visible = false;
			nLampLightTop.Visible = false;
			if (Game.Variables.GetVariable("general.part_of_day") == "day")
			{
				if (!Game.Variables.GetFlag("general.lights_home1f_livingroom"))
				{
					lightDayLightsOff.Apply();
					nWindowLightProjection.Visible = true;
					nWindowLightProjection2.Visible = true;
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
				nWallWindowEvening.Visible = true;
				nWallWindow2Evening.Visible = true;
				if (!Game.Variables.GetFlag("general.lights_home1f_livingroom"))
				{
					lightEveningLightsOff.Apply();
					nWindowLightProjection.Visible = true;
					nWindowLightProjection2.Visible = true;
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
				nWallWindowNight.Visible = true;
				nWallWindow2Night.Visible = true;
				if (!Game.Variables.GetFlag("general.lights_home1f_livingroom"))
				{
					lightNightLightsOff.Apply();
					nWindowLightProjection.Visible = true;
					nWindowLightProjection2.Visible = true;
					nWallWindowLight.Visible = true;
					nWallWindowLight2.Visible = true;
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
			if (Game.Variables.GetFlag("general.tv_home1f"))
			{
				if (Game.Variables.GetFlag("general.inbetween"))
				{
					nTvAnimation.Play("static");
				}
				else if (Game.Variables.GetVariable("general.part_of_day") == "day")
				{
					nTvAnimation.Play("program_1");
				}
				else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
				{
					nTvAnimation.Play("program_2");
				}
				else if (Game.Variables.GetVariable("general.part_of_day") == "night")
				{
					nTvAnimation.Play("program_3");
				}
			}
			else
			{
				nTvAnimation.Play("off");
			}
			if ((bool)varInbetween.Value)
			{
				nCatPainting.Frame = 1;
			}
		}

		public void AttemptTriggerPhoneCall()
		{
			if (DrkieUtil.RollPercent(4.0))
			{
				ExtraFunctions.StartTimer(30f, IgnorePhoneCall, destroyTimerOnTimeout: true);
				ExtraFunctions.GetTimer().Visible = false;
				AddChild(new HomePhoneRinger(sfxPhoneRing, 0.6f));
				varPhoneCallRinging.NewValue = true;
			}
		}

		public void IgnorePhoneCall()
		{
			varPhoneCallRinging.NewValue = false;
			varPhoneCallDone.NewValue = true;
		}
	}
}
