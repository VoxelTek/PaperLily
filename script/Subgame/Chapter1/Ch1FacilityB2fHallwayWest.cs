using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityB2fHallwayWest : Ch1FacilityRoom
	{
		[GetNode("Other/Passthrough")]
		private Passthrough nChartPassthough;

		[GetNode("Background/DoorOpen")]
		private Node2D nChartDoor;

		[GetNode("Background/DoorOpen/Plank")]
		private Node2D nChartPlank;

		[GetNode("Background/Elevator")]
		private SimpleAnimatedSprite nElevator;

		[GetNode("Other/Events/misc_elevator_door")]
		private EventTrigger nElevatorClosed;

		[GetNode("Other/Events/move_elevator")]
		private EventTrigger nElevatorOpen;

		[GetNode("Other/Navigation")]
		private Navigation2D nNavigation;

		private PVar varPlacedPlank = "ch1.facility_chartroom_placed_plank";

		private PVar varChartRoomOpen = "ch1.facility_chartroom_seen_exit";

		private PVar varElevatorLocation = "ch1.facility_elevator_location";

		private const int PRIMAL_SPAWNS = 5;

		public override void _BeforeFadeIn()
		{
			TrySpawnPrimal(nNavigation, 5);
		}

		public override void _UpdateRoom()
		{
			base._UpdateRoom();
			nChartPassthough.Enabled = varPlacedPlank.Value;
			nChartDoor.Visible = varChartRoomOpen.Value;
			nChartPlank.Visible = varPlacedPlank.Value;
			bool elevatorOpen = varElevatorLocation.Value == 2;
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
