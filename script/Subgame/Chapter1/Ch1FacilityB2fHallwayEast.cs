using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityB2fHallwayEast : Ch1FacilityRoom
	{
		[GetNode("Background/WallWindchimes/MiscVentman")]
		private Node2D nVentMan;

		[GetNode("Other/Navigation")]
		private Navigation2D nNavigation;

		private PVar varSaiHide = "ch1.facility_sai_hidden";

		private PVar varSeenRune = "ch1.facility_seen_event_rune";

		private PVar varSeenVentman = "ch1.facility_seen_ventman";

		private const int PRIMAL_SPAWNS = 5;

		public override void _BeforeFadeIn()
		{
			if ((bool)varSaiHide.Value && Game.Room.RegisteredNPCs.ContainsKey("sai") && Game.Room.RegisteredNPCs["sai"] is NPCFollower sai)
			{
				sai.DeleteIfValid();
			}
			if ((bool)varSeenRune.Value && !varSeenVentman.Value)
			{
				nVentMan.Visible = true;
			}
			TrySpawnPrimal(nNavigation, 5);
		}
	}
}
