using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1FacilityB2fLab : Ch1FacilityRoom
	{
		[GetNode("Main/SaiLean")]
		private Node2D nSai;

		[GetNode("Foreground/LightFadeout")]
		private Node2D nDoorFadeout;

		[GetNode("Other/Events/chr_sai")]
		private EventTrigger nSaiEvent;

		[GetNode("Main/MiscGate")]
		private Sprite nGate;

		[GetNode("Other/Events/misc_gate")]
		private EventTrigger nGateEvent;

		[GetNode("Ground/ShinyFuse")]
		private Node2D nFuseShiny;

		private PVar varLabLocked = "ch1.facility_lab_locked";

		private PVar varGateUnlocked = "ch1.facility_lab_gate_unlocked";

		private PVar varSaiHide = "ch1.facility_sai_hidden";

		private PVar varTookFuse = "ch1.facility_lab_took_fuse_floor";

		private PVar varLightsOut = "ch1.facility_lab_lights_out";

		public override void _BeforeFadeIn()
		{
			if ((bool)varSaiHide.Value && Game.Room.RegisteredNPCs.ContainsKey("sai") && Game.Room.RegisteredNPCs["sai"] is NPCFollower sai)
			{
				sai.DeleteIfValid();
			}
		}

		public override void _UpdateRoom()
		{
			base._UpdateRoom();
			nSai.Visible = varLabLocked.Value;
			nSaiEvent.Enabled = varLabLocked.Value;
			nDoorFadeout.Visible = !varLabLocked.Value;
			nGate.Frame = (((bool)varGateUnlocked.Value) ? 1 : 0);
			nGateEvent.Enabled = !varGateUnlocked.Value;
			nFuseShiny.Visible = !varTookFuse.Value;
			if ((bool)varLightsOut.Value)
			{
				nLights.Visible = false;
				lightOff.Apply();
				Game.Player.GetLight().Visible = false;
			}
		}
	}
}
