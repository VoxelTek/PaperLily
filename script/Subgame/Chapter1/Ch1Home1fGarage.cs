using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1Home1fGarage : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Lighting lightDay;

		[Export(PropertyHint.None, "")]
		private Lighting lightEvening;

		[Export(PropertyHint.None, "")]
		private Lighting lightNight;

		[Export(PropertyHint.None, "")]
		private AudioStream sfxPhoneRing;

		[GetNode("Background/item_redpaint")]
		private Node2D nRedPaint;

		private PVar varPhoneCallRinging = "ch1.home_phone_call_ringing";

		private const float PhoneVolume = 0.4f;

		public override void _AfterFadeIn()
		{
			if ((bool)varPhoneCallRinging.Value)
			{
				AddChild(new HomePhoneRinger(sfxPhoneRing, 0.4f));
			}
		}

		public override void _UpdateRoom()
		{
			Game.Room.ResetLighting();
			if (Game.Variables.GetVariable("general.part_of_day") == "day")
			{
				if (!Game.Variables.GetFlag("general.lights_home_1f_garage"))
				{
					lightDay.Apply();
				}
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
			{
				if (!Game.Variables.GetFlag("general.lights_home_1f_garage"))
				{
					lightEvening.Apply();
				}
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "night" && !Game.Variables.GetFlag("general.lights_home_1f_garage"))
			{
				lightNight.Apply();
			}
			if (Game.Variables.GetVariable("general.chapter") == "1")
			{
				if (Game.Variables.GetFlag("ch1.took_redpaint"))
				{
					nRedPaint.Visible = false;
				}
				else
				{
					nRedPaint.Visible = true;
				}
			}
		}
	}
}
