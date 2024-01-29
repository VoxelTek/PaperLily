using System.Linq;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1ForestEntrance : GameRoom
	{
		[GetNode("Foreground/lamp_light_1")]
		private Node2D nLight1;

		[GetNode("Foreground/lamp_light_2")]
		private Node2D nLight2;

		[GetNode("Foreground/lamp_light_3")]
		private Node2D nLight3;

		[GetNode("Foreground/lamp_light_4")]
		private Node2D nLight4;

		[GetNode("Foreground/lamp_light_5")]
		private Node2D nLight5;

		[GetNode("Foreground/lamp_light_6")]
		private Node2D nLight6;

		[GetNode("Ground/Cables/Cable1")]
		private Node2D nCable1;

		[GetNode("Ground/Cables/Cable2")]
		private Node2D nCable2;

		[GetNode("Ground/Cables/Cable3")]
		private Node2D nCable3;

		[GetNode("Ground/Cables/Cable4")]
		private Node2D nCable4;

		[GetNode("Ground/Cables/Cable5")]
		private Node2D nCable5;

		[GetNode("Ground/Cables/Cable6")]
		private Node2D nCable6;

		[GetNode("Ground/Cables/Cable1Cut")]
		private Node2D nCable1Cut;

		[GetNode("Ground/Cables/Cable2Cut")]
		private Node2D nCable2Cut;

		[GetNode("Ground/Cables/Cable3Cut")]
		private Node2D nCable3Cut;

		[GetNode("Ground/Cables/Cable4Cut")]
		private Node2D nCable4Cut;

		[GetNode("Ground/Cables/Cable5Cut")]
		private Node2D nCable5Cut;

		[GetNode("Ground/Cables/Cable6Cut")]
		private Node2D nCable6Cut;

		[GetNode("Ground/StructWall/Gate")]
		private Node2D nGate;

		[GetNode("Other/Events/misc_door")]
		private EventTrigger nGateCollision;

		private PVar varGateOpen = "ch1.forest_entrance_gate_open";

		private PVar varLight1SwitchOn = "ch1.forest_entrance_light_1_switch_on";

		private PVar varLight2SwitchOn = "ch1.forest_entrance_light_2_switch_on";

		private PVar varLight3SwitchOn = "ch1.forest_entrance_light_3_switch_on";

		private PVar varLight4SwitchOn = "ch1.forest_entrance_light_4_switch_on";

		private PVar varLight5SwitchOn = "ch1.forest_entrance_light_5_switch_on";

		private PVar varLight6SwitchOn = "ch1.forest_entrance_light_6_switch_on";

		private PVar varCable1Cut = "ch1.forest_entrance_light_1_cable_cut";

		private PVar varCable2Cut = "ch1.forest_entrance_light_2_cable_cut";

		private PVar varCable3Cut = "ch1.forest_entrance_light_3_cable_cut";

		private PVar varCable4Cut = "ch1.forest_entrance_light_4_cable_cut";

		private PVar varCable5Cut = "ch1.forest_entrance_light_5_cable_cut";

		private PVar varCable6Cut = "ch1.forest_entrance_light_6_cable_cut";

		private PVar varLight1Broken = "ch1.forest_entrance_light_1_broken";

		private PVar varLight2Broken = "ch1.forest_entrance_light_2_broken";

		private PVar varLight3Broken = "ch1.forest_entrance_light_3_broken";

		private PVar varLight4Broken = "ch1.forest_entrance_light_4_broken";

		private PVar varLight5Broken = "ch1.forest_entrance_light_5_broken";

		private PVar varLight6Broken = "ch1.forest_entrance_light_6_broken";

		private bool[] lightSwitchesTouched = new bool[6];

		public override void _UpdateRoom()
		{
			nGate.Visible = !varGateOpen.Value;
			nGateCollision.Enabled = !varGateOpen.Value;
			nCable1.Visible = !varCable1Cut.Value;
			nCable2.Visible = !varCable2Cut.Value;
			nCable3.Visible = !varCable3Cut.Value;
			nCable4.Visible = !varCable4Cut.Value;
			nCable5.Visible = !varCable5Cut.Value;
			nCable6.Visible = !varCable6Cut.Value;
			nCable1Cut.Visible = !nCable1.Visible;
			nCable2Cut.Visible = !nCable2.Visible;
			nCable3Cut.Visible = !nCable3.Visible;
			nCable4Cut.Visible = !nCable4.Visible;
			nCable5Cut.Visible = !nCable5.Visible;
			nCable6Cut.Visible = !nCable6.Visible;
			UpdateLights();
			UpdateLightSolutions();
		}

		public void LightSwitch1()
		{
			SwitchLights(0, 1, 2, 3);
		}

		public void LightSwitch2()
		{
			SwitchLights(1, 2, 3, 4, 6);
		}

		public void LightSwitch3()
		{
			SwitchLights(2, 3, 5, 6);
		}

		public void LightSwitch4()
		{
			SwitchLights(3, 1, 3, 4, 6);
		}

		public void LightSwitch5()
		{
			SwitchLights(4, 2, 4, 5, 6);
		}

		public void LightSwitch6()
		{
			SwitchLights(5, 1, 2, 3, 4);
		}

		public void CheckEvents()
		{
			if (!Game.Events.SeenEvent(base.Name + ".event_figure_out"))
			{
				if (CountInteractedLightSwitches() >= 3)
				{
					Game.Events.PlayEvent("event_figure_out");
				}
				else if (CountInteractedLightSwitches() == 0 && !Game.Events.SeenEvent(base.Name + ".event_noticed_switch") && Game.Events.GetEventInteractionCount(base.Name + ".misc_light") >= 3)
				{
					Game.Events.PlayEvent("event_noticed_switch");
				}
			}
		}

		private void SwitchLights(int lightIdx, params int[] lights)
		{
			lightSwitchesTouched[lightIdx] = true;
			foreach (int i in lights)
			{
				Game.Variables.ToggleFlag("ch1.forest_entrance_light_" + i + "_switch_on");
			}
			UpdateLights();
			UpdateLightSolutions();
		}

		private void UpdateLights()
		{
			nLight1.Visible = (bool)varLight1SwitchOn.Value && !varCable1Cut.Value && !varLight1Broken.Value;
			nLight2.Visible = (bool)varLight2SwitchOn.Value && !varCable2Cut.Value && !varLight2Broken.Value;
			nLight3.Visible = (bool)varLight3SwitchOn.Value && !varCable3Cut.Value && !varLight3Broken.Value;
			nLight4.Visible = (bool)varLight4SwitchOn.Value && !varCable4Cut.Value && !varLight4Broken.Value;
			nLight5.Visible = (bool)varLight5SwitchOn.Value && !varCable5Cut.Value && !varLight5Broken.Value;
			nLight6.Visible = (bool)varLight6SwitchOn.Value && !varCable6Cut.Value && !varLight6Broken.Value;
		}

		private void UpdateLightSolutions()
		{
			if (!Game.Events.SeenEvent(base.Name + ".event_just_opened") && !nLight1.Visible && !nLight2.Visible && !nLight3.Visible && !nLight4.Visible && !nLight5.Visible && !nLight6.Visible)
			{
				Game.Events.PlayEvent("event_just_opened");
			}
		}

		private int CountInteractedLightSwitches()
		{
			return lightSwitchesTouched.Count((bool x) => x);
		}
	}
}
