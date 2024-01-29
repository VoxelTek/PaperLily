using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1HomeExteriorFront : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Lighting lightDay;

		[Export(PropertyHint.None, "")]
		private Lighting lightEvening;

		[Export(PropertyHint.None, "")]
		private Lighting lightNight;

		[Export(PropertyHint.None, "")]
		private AudioStream sfxBusIdle;

		[Inject]
		private IAudioManager Audio;

		[Inject]
		private IExtraFunctions Functions;

		[GetNode("Foreground/bus")]
		private Node2D nBus;

		[GetNode("Foreground/livingroom_door")]
		private Node2D nLightingLivingroomDoor;

		[GetNode("Foreground/hallway_window")]
		private Node2D nLightingHallwayWindow;

		[GetNode("Foreground/hallway_window_2")]
		private Node2D nLightingHallwayWindow2;

		[GetNode("Foreground/bedroom_a_window")]
		private Node2D nLightingBedroomAWindow;

		[GetNode("Foreground/night")]
		private Node2D nLightingNight;

		[GetNode("Background/misc_markings")]
		private Node2D nMiscMarkings;

		[GetNode("Foreground/Crow")]
		private Node2D nCrow;

		[GetNode("Other/Events/misc_save")]
		private IToggleable nCrowEvt;

		[GetNode("Foreground/Crow/indicator_arrow1")]
		private Node2D nSaveArrow;

		[GetNode("Foreground/Crow/indicator_arrow1/Animation")]
		private AnimationPlayer nSaveArrowAnimation;

		private PVar varPartOfDay = "general.part_of_day";

		private PVar varChapter = "general.chapter";

		private PVar varLightsHome1fLivingroom = "general.lights_home1f_livingroom";

		private PVar varLightsHome2fHallway = "general.lights_home_2f_hallway";

		private PVar varLightsHome2fBedroomA = "general.lights_home_2f_bedroom_a";

		private PVar varRitualBusWaiting = "ch1.home_ritualbuswaiting";

		private PVar varBusNotThere = "ch1.bus_not_there";

		private PVar varHomeDrewMarkings = "ch1.home_drew_markings";

		private PVar varCheckedSave = "ch1.checked_save";

		public override void _BeforeFadeIn()
		{
			if (varChapter.Value == 1)
			{
				if (varPartOfDay.Value == "evening")
				{
					SaveLocation = "system.locations.ch1.home";
					SaveImage = "ch1_home_afternoon";
				}
				else if (varPartOfDay.Value == "night")
				{
					SaveLocation = "system.locations.ch1.home_night";
					SaveImage = "ch1_home_night";
				}
			}
		}

		public override void _UpdateRoom()
		{
			Game.Room.ResetLighting();
			nLightingLivingroomDoor.Visible = false;
			nLightingHallwayWindow.Visible = false;
			nLightingHallwayWindow2.Visible = false;
			nLightingBedroomAWindow.Visible = false;
			nLightingNight.Visible = false;
			if (varPartOfDay.Value == "day")
			{
				lightDay.Apply();
			}
			else if (varPartOfDay.Value == "evening")
			{
				lightEvening.Apply();
			}
			else if (varPartOfDay.Value == "night")
			{
				lightNight.Apply();
				nLightingNight.Visible = true;
				if ((bool)varLightsHome1fLivingroom.Value)
				{
					nLightingLivingroomDoor.Visible = true;
				}
				if ((bool)varLightsHome2fHallway.Value)
				{
					nLightingHallwayWindow.Visible = true;
					nLightingHallwayWindow2.Visible = true;
				}
				if ((bool)varLightsHome2fBedroomA.Value)
				{
					nLightingBedroomAWindow.Visible = true;
				}
			}
			if (varChapter.Value == 1)
			{
				if ((bool)varRitualBusWaiting.Value && !varBusNotThere.Value)
				{
					Audio.ChangeAmbianceVolume(0.6f);
					Audio.PlayAmbiance(sfxBusIdle);
					nBus.Visible = true;
				}
				else
				{
					nBus.Visible = false;
				}
				if ((bool)varBusNotThere.Value)
				{
					nCrow.Visible = false;
					nCrowEvt.Enabled = false;
				}
				else
				{
					nCrow.Visible = true;
					nCrowEvt.Enabled = true;
				}
				if ((bool)varHomeDrewMarkings.Value)
				{
					nMiscMarkings.Visible = true;
				}
				if (!varCheckedSave.Value)
				{
					nSaveArrow.Visible = true;
					nSaveArrowAnimation.PlayFirstAnimation();
				}
				else
				{
					nSaveArrow.Visible = false;
					nSaveArrowAnimation.Stop();
				}
			}
		}

		public void Ch1StopTimer()
		{
			Functions.StopTimer();
		}
	}
}
