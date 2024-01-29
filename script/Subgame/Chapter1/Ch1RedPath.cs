using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1RedPath : GameRoom
	{
		[GetNode("Main/Gate1")]
		private Node2D nGate1;

		[GetNode("Main/Gate2")]
		private Node2D nGate2;

		[GetNode("Main/Gate3")]
		private Node2D nGate3;

		[GetNode("Other/Events/misc_gate_1")]
		private EventTrigger nGate1Evt;

		[GetNode("Other/Events/misc_gate_2")]
		private EventTrigger nGate2Evt;

		[GetNode("Other/Events/misc_gate_3")]
		private EventTrigger nGate3Evt;

		private PVar varGate1Open = "ch1.forest_red_passage_gate_1_open";

		private PVar varGate2Open = "ch1.forest_red_passage_gate_2_open";

		private PVar varGate3Open = "ch1.forest_red_passage_gate_3_open";

		public override void _UpdateRoom()
		{
			Node2D node2D = nGate1;
			bool visible = (nGate1Evt.Enabled = !varGate1Open.Value);
			node2D.Visible = visible;
			Node2D node2D2 = nGate2;
			visible = (nGate2Evt.Enabled = !varGate2Open.Value);
			node2D2.Visible = visible;
			Node2D node2D3 = nGate3;
			visible = (nGate3Evt.Enabled = !varGate3Open.Value);
			node2D3.Visible = visible;
		}
	}
}
