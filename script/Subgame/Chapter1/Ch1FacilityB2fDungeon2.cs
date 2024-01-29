using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityB2fDungeon2 : Ch1FacilityRoom
	{
		[GetNode("Background/Door1")]
		private Sprite nDoor1;

		[GetNode("Background/Door2")]
		private Sprite nDoor2;

		[GetNode("Background/Door3")]
		private Sprite nDoor3;

		[GetNode("Background/Door4")]
		private Sprite nDoor4;

		[GetNode("Background/Door5")]
		private Sprite nDoor5;

		[GetNode("Background/Door6")]
		private Sprite nDoor6;

		[GetNode("Background/Door3/Eyes")]
		private Node2D nEyes1;

		[GetNode("Background/Door6/Eyes")]
		private Node2D nEyes2;

		[GetNode("Other/Events/misc_cell_2")]
		private EventTrigger nDoor2ClosedEvt;

		[GetNode("Other/Events/misc_cell_3")]
		private EventTrigger nDoor3ClosedEvt;

		[GetNode("Other/Events/misc_cell_4")]
		private EventTrigger nDoor4ClosedEvt;

		[GetNode("Other/Events/misc_cell_5")]
		private EventTrigger nDoor5ClosedEvt;

		[GetNode("Other/Events/misc_cell_6")]
		private EventTrigger nDoor6ClosedEvt;

		[GetNode("Other/Events/move_cell_2")]
		private EventTrigger nDoor2OpenEvt;

		[GetNode("Other/Events/move_cell_3")]
		private EventTrigger nDoor3OpenEvt;

		[GetNode("Other/Events/move_cell_4")]
		private EventTrigger nDoor4OpenEvt;

		[GetNode("Other/Events/move_cell_5")]
		private EventTrigger nDoor5OpenEvt;

		[GetNode("Other/Events/move_cell_6")]
		private EventTrigger nDoor6OpenEvt;

		private PVar varDoorsOpen = "ch1.facility_dungeon_2_doors_open";

		private PVar varPlacedPlank = "ch1.facility_dungeon_2_placed_plank";

		private PVar varReleasedPrimals = "ch1.facility_dungeon_2_released";

		public override void _UpdateRoom()
		{
			base._UpdateRoom();
			bool doorsOpen = varDoorsOpen.Value;
			nDoor1.Frame = (doorsOpen ? 1 : 0);
			if ((bool)varPlacedPlank.Value)
			{
				nDoor1.Frame = 1;
			}
			nDoor2.Frame = (doorsOpen ? 1 : 0);
			nDoor3.Frame = (doorsOpen ? 1 : 0);
			nDoor4.Frame = (doorsOpen ? 1 : 0);
			nDoor5.Frame = (doorsOpen ? 1 : 0);
			nDoor6.Frame = (doorsOpen ? 1 : 0);
			nDoor2OpenEvt.Enabled = doorsOpen;
			nDoor3OpenEvt.Enabled = doorsOpen;
			nDoor4OpenEvt.Enabled = doorsOpen;
			nDoor5OpenEvt.Enabled = doorsOpen;
			nDoor6OpenEvt.Enabled = doorsOpen;
			nDoor2ClosedEvt.Enabled = !doorsOpen;
			nDoor3ClosedEvt.Enabled = !doorsOpen;
			nDoor4ClosedEvt.Enabled = !doorsOpen;
			nDoor5ClosedEvt.Enabled = !doorsOpen;
			nDoor6ClosedEvt.Enabled = !doorsOpen;
			nEyes1.Visible = !doorsOpen && !varReleasedPrimals.Value;
			nEyes2.Visible = !doorsOpen && !varReleasedPrimals.Value;
		}
	}
}
