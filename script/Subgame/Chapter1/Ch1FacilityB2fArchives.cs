using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityB2fArchives : Ch1FacilityRoom
	{
		[GetNode("Background/PreArrange")]
		private Node2D nPreArrangeLayout;

		[GetNode("Background/PostArrange")]
		private Node2D nPostArrangeLayout;

		[GetNode("Background/Vent")]
		private Sprite nVent;

		[GetNode("Background/Cover")]
		private Node2D nFloorVentCover;

		[GetNode("Background/shiny")]
		private Node2D nShiny;

		private PVar varSolved = "ch1.facility_boxpuzzle_solved";

		private PVar varOpenedVent = "ch1.facility_archive_vent_open";

		private PVar varTookValve = "ch1.facility_took_valve_star";

		public override void _BeforeFadeIn()
		{
			nPreArrangeLayout.Visible = true;
			nPostArrangeLayout.Visible = false;
			if (!varSolved.Value)
			{
				return;
			}
			nPreArrangeLayout.Visible = false;
			nPostArrangeLayout.Visible = true;
			foreach (Node child in nPostArrangeLayout.GetChildren())
			{
				child.Reparent(GetMainLayer());
			}
		}

		public override void _UpdateRoom()
		{
			base._UpdateRoom();
			nVent.Frame = (((bool)varOpenedVent.Value) ? 1 : 0);
			nFloorVentCover.Visible = varOpenedVent.Value;
			nShiny.Visible = !varTookValve.Value;
		}
	}
}
