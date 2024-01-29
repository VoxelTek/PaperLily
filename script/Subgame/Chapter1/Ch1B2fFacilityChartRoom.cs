using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1B2fFacilityChartRoom : Ch1FacilityRoom
	{
		[GetNode("Background/Plank")]
		private Node2D nPlank;

		[GetNode("Main/Table/ShinyFuse")]
		private Node2D nFuseShiny;

		[GetNode("Background/WallScreenOn")]
		private Node2D nScreen;

		private PVar varPlacedPlank = "ch1.facility_chartroom_placed_plank";

		private PVar varTookFuse = "ch1.facility_chart_took_fuse";

		private PVar varScreenOn = "ch1.facility_chart_screen_on";

		public override void _UpdateRoom()
		{
			base._UpdateRoom();
			nPlank.Visible = !varPlacedPlank.Value;
			nFuseShiny.Visible = !varTookFuse.Value;
			nScreen.Visible = varScreenOn.Value;
		}
	}
}
