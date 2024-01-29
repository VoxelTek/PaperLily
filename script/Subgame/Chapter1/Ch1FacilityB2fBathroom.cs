using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityB2fBathroom : Ch1FacilityRoom
	{
		[GetNode("Background/Stall1")]
		private Sprite nStall1;

		[GetNode("Background/Stall2")]
		private Sprite nStall2;

		[GetNode("Background/Stall3")]
		private Sprite nStall3;

		[GetNode("Background/Stall4")]
		private Sprite nStall4;

		[GetNode("Background/Stall5")]
		private Sprite nStall5;

		[GetNode("Background/Stall6")]
		private Sprite nStall6;

		[GetNode("Background/Stall7")]
		private Sprite nStall7;

		[GetNode("Background/Stall8")]
		private Sprite nStall8;

		[GetNode("Other/Events/misc_stall_1")]
		private EventTrigger nStall1Evt;

		[GetNode("Other/Events/misc_stall_2")]
		private EventTrigger nStall2Evt;

		[GetNode("Other/Events/misc_stall_3")]
		private EventTrigger nStall3Evt;

		[GetNode("Other/Events/misc_stall_4")]
		private EventTrigger nStall4Evt;

		[GetNode("Other/Events/misc_stall_5")]
		private EventTrigger nStall5Evt;

		[GetNode("Other/Events/misc_stall_6")]
		private EventTrigger nStall6Evt;

		[GetNode("Other/Events/misc_stall_7")]
		private EventTrigger nStall7Evt;

		[GetNode("Other/Events/misc_stall_8")]
		private EventTrigger nStall8Evt;

		private PVar varStall1Open = "ch1.facility_bathroom_stall_1_open";

		private PVar varStall2Open = "ch1.facility_bathroom_stall_2_open";

		private PVar varStall3Open = "ch1.facility_bathroom_stall_3_open";

		private PVar varStall4Open = "ch1.facility_bathroom_stall_4_open";

		private PVar varStall5Open = "ch1.facility_bathroom_stall_5_open";

		private PVar varStall6Open = "ch1.facility_bathroom_stall_6_open";

		private PVar varStall7Open = "ch1.facility_bathroom_stall_7_open";

		private PVar varStall8Open = "ch1.facility_bathroom_stall_8_open";

		public override void _UpdateRoom()
		{
			base._UpdateRoom();
			nStall1.Frame = (((bool)varStall1Open.Value) ? 1 : 0);
			nStall2.Frame = (((bool)varStall2Open.Value) ? 1 : 0);
			nStall3.Frame = (((bool)varStall3Open.Value) ? 1 : 0);
			nStall4.Frame = (((bool)varStall4Open.Value) ? 1 : 0);
			nStall5.Frame = (((bool)varStall5Open.Value) ? 1 : 0);
			nStall6.Frame = (((bool)varStall6Open.Value) ? 1 : 0);
			nStall7.Frame = (((bool)varStall7Open.Value) ? 1 : 0);
			nStall8.Frame = (((bool)varStall8Open.Value) ? 1 : 0);
			nStall1Evt.Enabled = !varStall1Open.Value;
			nStall2Evt.Enabled = !varStall2Open.Value;
			nStall3Evt.Enabled = !varStall3Open.Value;
			nStall4Evt.Enabled = !varStall4Open.Value;
			nStall5Evt.Enabled = !varStall5Open.Value;
			nStall6Evt.Enabled = !varStall6Open.Value;
			nStall7Evt.Enabled = !varStall7Open.Value;
			nStall8Evt.Enabled = !varStall8Open.Value;
		}
	}
}
