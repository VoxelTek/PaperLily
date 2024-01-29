using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1BlackPassStairs : GameRoom
	{
		[GetNode("Background/Item1")]
		private Node2D nItem1Bg;

		[GetNode("Background/Item2")]
		private Node2D nItem2Bg;

		[GetNode("Background/Item3")]
		private Node2D nItem3Bg;

		[GetNode("Other/Events/misc_item_1")]
		private EventTrigger nItem1Evt;

		[GetNode("Other/Events/misc_item_2")]
		private EventTrigger nItem2Evt;

		[GetNode("Other/Events/misc_item_3")]
		private EventTrigger nItem3Evt;

		private PVar varRedItem1 = "general.blackpass_red_item_1_took";

		private PVar varRedItem2 = "general.blackpass_red_item_2_took";

		private PVar varRedItem3 = "general.blackpass_red_item_3_took";

		public override void _UpdateRoom()
		{
			int ownedRedItems = GetOwnedRedItems();
			nItem1Bg.Visible = ownedRedItems >= 1;
			nItem2Bg.Visible = ownedRedItems >= 2;
			nItem3Bg.Visible = ownedRedItems >= 3;
			nItem1Evt.Enabled = ownedRedItems == 1;
			nItem2Evt.Enabled = ownedRedItems == 2;
			nItem3Evt.Enabled = ownedRedItems == 3;
		}

		private int GetOwnedRedItems()
		{
			int result = 0;
			if ((bool)varRedItem1.Value)
			{
				result++;
			}
			if ((bool)varRedItem2.Value)
			{
				result++;
			}
			if ((bool)varRedItem3.Value)
			{
				result++;
			}
			return result;
		}
	}
}
