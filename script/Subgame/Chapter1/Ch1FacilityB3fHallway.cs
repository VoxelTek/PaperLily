using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityB3fHallway : Ch1FacilityRoom
	{
		[GetNode("Main/DoorAirtight")]
		private Node2D nAirtight;

		[GetNode("TilesWater")]
		private Node2D nWater;

		[GetNode("Other/Events/misc_airtight_door")]
		private EventTrigger nAirtightEvt;

		[GetNode("Background/Elevator")]
		private SimpleAnimatedSprite nElevator;

		[GetNode("Other/Events/misc_elevator_door")]
		private EventTrigger nElevatorClosed;

		[GetNode("Other/Events/move_elevator")]
		private EventTrigger nElevatorOpen;

		private PVar varFloodClear = "ch1.facility_flood_engaged";

		private PVar varElevatorLocation = "ch1.facility_elevator_location";

		public override void _UpdateRoom()
		{
			base._UpdateRoom();
			nAirtight.Visible = !varFloodClear.Value;
			nAirtightEvt.Enabled = !varFloodClear.Value;
			nWater.Visible = !varFloodClear.Value;
			bool elevatorOpen = varElevatorLocation.Value == 3;
			nElevator.Frame = (elevatorOpen ? 5 : 0);
			nElevatorClosed.Enabled = !elevatorOpen;
			nElevatorOpen.Enabled = elevatorOpen;
		}

		public void OpenElevatorDoors()
		{
			nElevator.Playing = true;
		}
	}
}
