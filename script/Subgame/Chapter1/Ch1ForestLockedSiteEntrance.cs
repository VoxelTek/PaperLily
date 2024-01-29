using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1ForestLockedSiteEntrance : GameRoom
	{
		[Inject]
		private IExtraFunctions Functions;

		[GetNode("Main/StructFenceGate")]
		private Sprite nGate;

		[GetNode("Main/StructFenceGate/MiscPoster1")]
		private Node2D nPoster;

		[GetNode("Main/MiscLock")]
		private Sprite nLocks;

		[GetNode("Background/Note")]
		private Node2D nNote;

		[GetNode("Other/Events/misc_door")]
		private EventTrigger nGateEvent;

		[GetNode("Other/Events/misc_door_open")]
		private EventTrigger nGateOpenEvent;

		private PVar varUnlocked = "ch1.forest_lockedsite_unlocked";

		private PVar varOpen = "ch1.forest_lockedsite_open";

		private PVar varTookNote = "ch1.forest_lockedsite_took_letter";

		public override void _UpdateRoom()
		{
			nNote.Visible = !varTookNote.Value;
			if ((bool)varUnlocked.Value)
			{
				nLocks.Frame = 1;
			}
			if ((bool)varOpen.Value)
			{
				nGate.Frame = 1;
				nPoster.Visible = false;
				nGateEvent.Enabled = false;
				nGateOpenEvent.Enabled = true;
			}
			else
			{
				nGate.Frame = 0;
				nPoster.Visible = true;
				nGateEvent.Enabled = true;
				nGateOpenEvent.Enabled = false;
			}
		}

		public void AdjustPlayerPosForGate()
		{
			if (Game.Player.GetDirection() == Direction.Down)
			{
				Game.Player.Node.Position = GetPoint("gate_inside");
				Game.Player.Teleport();
			}
		}

		public void StopTimer()
		{
			Functions.StopTimer();
		}
	}
}
