using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1ForestStationTracks : GameRoom
	{
		private PVar varCurStage = "ch1.forest_station_tracks_stage";

		private PVar varCorrectPath = "ch1.forest_station_correct_path";

		public override void _BeforeFadeIn()
		{
			if (varCurStage.Value != 3)
			{
				int r = new Random().Next(3);
				PVar pVar = varCorrectPath;
				pVar.NewValue = r switch
				{
					0 => "1", 
					1 => "2", 
					2 => "3", 
					_ => "1", 
				};
			}
			else
			{
				varCorrectPath.NewValue = "0";
			}
		}
	}
}
