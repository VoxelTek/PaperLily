using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1Home2fBathroom : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Lighting lightDay;

		[Export(PropertyHint.None, "")]
		private Lighting lightEvening;

		[Export(PropertyHint.None, "")]
		private Lighting lightNight;

		[GetNode("Background/furniture_toilet")]
		private Sprite nToilet;

		public override void _UpdateRoom()
		{
			Game.Room.ResetLighting();
			if (Game.Variables.GetVariable("general.part_of_day") == "day")
			{
				if (!Game.Variables.GetFlag("general.lights_home_2f_bathroom"))
				{
					lightDay.Apply();
				}
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "evening")
			{
				if (!Game.Variables.GetFlag("general.lights_home_2f_bathroom"))
				{
					lightEvening.Apply();
				}
			}
			else if (Game.Variables.GetVariable("general.part_of_day") == "night" && !Game.Variables.GetFlag("general.lights_home_2f_bathroom"))
			{
				lightNight.Apply();
			}
			if (!Game.Variables.GetFlag("general.home_2f_bathroom_toilet_down"))
			{
				nToilet.Frame = 1;
			}
			else
			{
				nToilet.Frame = 0;
			}
		}
	}
}
