using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Subgame.Chapter1;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityB1fElevator : Ch1FacilityRoom
	{
		[GetNode("Background/Elevator")]
		private SimpleAnimatedSprite nElevator;

		[GetNode("Other/Events/misc_elevator_door")]
		private EventTrigger nElevatorClosed;

		[GetNode("Other/Events/move_elevator")]
		private EventTrigger nElevatorOpen;

		[GetNode("Other/Navigation")]
		private Navigation2D nNavigation;

		private PVar varElevatorLocation = "ch1.facility_elevator_location";

		private const int PRIMAL_SPAWNS = 4;

		public override void _BeforeFadeIn()
		{
			varPrimalSpawn = primalSpawnVar;
			varPrimalType = primalTypeVar;
			if (Game.Player.Node.Position != GetPoint("from_elevator"))
			{
				TrySpawnPrimal(nNavigation, 4);
			}
		}

		public override void _UpdateRoom()
		{
			base._UpdateRoom();
			bool elevatorOpen = varElevatorLocation.Value == 1;
			nElevator.Frame = (elevatorOpen ? 5 : 0);
			nElevatorClosed.Enabled = !elevatorOpen;
			nElevatorOpen.Enabled = elevatorOpen;
		}

		public void SpawnTutorialEnemy()
		{
			Ch1FacilityPrimal nChaser = new Ch1FacilityPrimal();
			varPrimalSpawn.NewValue = "tutorial_primal_spawn";
			varPrimalType.NewValue = 1;
			SpawnPoint spawn = GetSpawnPoint(varPrimalSpawn.Value);
			nChaser.Position = spawn.Position;
			nChaser.DefaultDirection = spawn.Direction;
			GetMainLayer().AddChild(nChaser);
			nChaser.SetNavigation(nNavigation);
			nChaser.Caught += base.PlayerDeath;
		}

		public void OpenElevatorDoors()
		{
			nElevator.Playing = true;
		}
	}
}
