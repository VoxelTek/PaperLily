using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1Red1f : GameRoom
	{
		[GetNode("Background/FloorHatch")]
		private Node2D nHatch;

		[GetNode("Background/FloorHatch/GrateClosed")]
		private Node2D nGrateClosed;

		[GetNode("Background/FloorHatch/GrateOpen")]
		private Node2D nGrateOpen;

		[GetNode("Background/FurnitureNotapizzaoven/Thing")]
		private Node2D nThing;

		[GetNode("Background/MiscNotes")]
		private Node2D nNotes;

		[GetNode("Background/CarpetMoved")]
		private Node2D nCarpetMoved;

		[GetNode("Other/Events/move_downstairs")]
		private EventTrigger nLadderEvt;

		private PVar varTookThing = "ch1.forest_red_took_pot";

		private PVar varHatchExposed = "ch1.forest_red_hatch_exposed";

		private PVar varHatchOpen = "ch1.forest_red_hatch_open";

		private PVar varTookNotes = "ch1.forest_red_took_notes";

		public override void _UpdateRoom()
		{
			nHatch.Visible = varHatchExposed.Value;
			nCarpetMoved.Visible = varHatchExposed.Value;
			nThing.Visible = !varTookThing.Value;
			nNotes.Visible = !varTookNotes.Value;
			nGrateOpen.Visible = varHatchOpen.Value;
			nGrateClosed.Visible = !nGrateOpen.Visible;
			nLadderEvt.Enabled = varHatchOpen.Value;
		}
	}
}
