using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1BlackPassHallway : GameRoom
	{
		[GetNode("Main/LockersRow3/FurnitureLockers6/RedItem")]
		private Node2D nRedItemSpr;

		[GetNode("Other/Events/misc_reditem")]
		private EventTrigger nRedItemEvent;

		[GetNode("Background/Windows")]
		private Node2D nWindow;

		[GetNode("Main/LockersRow4/FurnitureWardrobe")]
		private Sprite nWardrobe;

		[GetNode("Other/Events/misc_wardrobe")]
		private EventTrigger nWardrobeEvent;

		[GetNode("Main/LockersRow3/FurnitureLockers6/LacieLocker")]
		private Sprite nLacieLocker;

		[GetNode("Other/Events/misc_locker_mess")]
		private EventTrigger nLockerMess;

		private PVar varRedItemSpawned = "general.blackpass_red_item_1_spawned";

		private PVar varCurtainsClosed = "ch1.blackpass_hallway_curtains_closed";

		private PVar varLockerOpen = "ch1.blackpass_opened_locker";

		private PVar varVisits = "general.blackpass_visits";

		public override void _UpdateRoom()
		{
			nRedItemSpr.Visible = (bool)varLockerOpen.Value && (bool)varRedItemSpawned.Value;
			nRedItemEvent.Enabled = varRedItemSpawned.Value;
			nWindow.Visible = !varCurtainsClosed.Value;
			if ((int)varVisits.Value >= 3)
			{
				nWardrobe.Visible = false;
				nWardrobeEvent.Enabled = false;
			}
			nLacieLocker.Frame = (varLockerOpen.Value ? 2 : 0);
			nLockerMess.Enabled = varLockerOpen.Value;
		}

		public void OpenLocker()
		{
			nLacieLocker.Frame = 2;
		}

		public async void OpenWardrobe()
		{
			nWardrobe.Frame = 1;
			await this.DelaySeconds(0.5);
			nWardrobe.Frame = 2;
		}
	}
}
