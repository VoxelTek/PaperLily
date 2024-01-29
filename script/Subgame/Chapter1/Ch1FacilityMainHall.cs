using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1FacilityMainHall : Ch1FacilityRoom
	{
		private int layer = 1;

		[GetNode("Other/BoundaryF1")]
		private StaticBody2D nBoundaryF1;

		[GetNode("Other/BoundaryF2")]
		private StaticBody2D nBoundaryF2;

		[GetNode("MainF1")]
		private Node2D nMainF1;

		[GetNode("MainF2")]
		private Node2D nMainF2;

		[GetNode("TilesWater")]
		private Node2D nTilesWater;

		[GetNode("Background/WaterRipples")]
		private Node2D nWaterRipples1;

		[GetNode("Background/WaterRipplesPillar")]
		private Node2D nWaterRipples2;

		[GetNode("Other/trigger_f1_left")]
		private Area2D nTriggerF1a;

		[GetNode("Other/trigger_f1_right")]
		private Area2D nTriggerF1b;

		[GetNode("Other/trigger_f2_left")]
		private Area2D nTriggerF2a;

		[GetNode("Other/trigger_f2_right")]
		private Area2D nTriggerF2b;

		[GetNode("Other/EventsF1/move_hallway_tl")]
		private EventTrigger nMoveHallwayTl;

		[GetNode("Other/EventsF1/move_hallway_tr")]
		private EventTrigger nMoveHallwayTr;

		[GetNode("Other/EventsF1/move_hallway_bl")]
		private EventTrigger nMoveHallwayBl;

		[GetNode("Other/EventsF1/move_hallway_br")]
		private EventTrigger nMoveHallwayBr;

		[GetNode("Other/EventsF1/move_downstairs_left")]
		private EventTrigger nMoveDownstairsLeft;

		[GetNode("Other/EventsF1/move_downstairs_right")]
		private EventTrigger nMoveDownstairsRight;

		[GetNode("Other/EventsF1/misc_water")]
		private EventTrigger nWaterBlock;

		[GetNode("Other/EventsF1/misc_gate")]
		private EventTrigger nGateEvt;

		[GetNode("MainF1/chr_rune")]
		private Node2D nRuneSpr;

		[GetNode("Other/EventsF1/chr_rune")]
		private EventTrigger nRuneEvt;

		[GetNode("MainF1/FoliageTreeStump/Crow")]
		private Node2D nCrowSpr;

		[GetNode("Background/Gate")]
		private Node2D nGate;

		private PVar varFloodEngaged = "ch1.facility_flood_engaged";

		private PVar varGateOpen = "ch1.facility_council_door_open";

		private PVar varRuneGone = "ch1.facility_rune_gone";

		private PVar varSaveGone = "ch1.facility_sai_attempting_leave";

		public override void _BeforeFadeIn()
		{
			nTriggerF1a.Connect("body_entered", this, "TriggerLayer1");
			nTriggerF1b.Connect("body_entered", this, "TriggerLayer1");
			nTriggerF2a.Connect("body_entered", this, "TriggerLayer2");
			nTriggerF2b.Connect("body_entered", this, "TriggerLayer2");
		}

		public override void _UpdateRoom()
		{
			base._UpdateRoom();
			bool floodEngaged = varFloodEngaged.Value;
			nWaterBlock.Enabled = !floodEngaged;
			nTilesWater.Visible = !floodEngaged;
			nWaterRipples1.Visible = !floodEngaged;
			nWaterRipples2.Visible = !floodEngaged;
			nGate.Visible = !varGateOpen.Value;
			nGateEvt.Enabled = nGate.Visible;
			nRuneSpr.Visible = !varRuneGone.Value;
			nRuneEvt.Enabled = !varRuneGone.Value;
			nCrowSpr.Visible = !varSaveGone.Value;
		}

		public override Node2D GetMainLayer()
		{
			if (layer != 1)
			{
				return nMainF2;
			}
			return nMainF1;
		}

		public void TriggerLayer1(Node body)
		{
			if (body != null && body.GetParent() == nMainF2)
			{
				CallDeferred("ReparentCharacter", body, nMainF1);
			}
			if (layer != 1)
			{
				nBoundaryF1.CollisionLayer = 2u;
				nBoundaryF2.CollisionLayer = 0u;
				nMoveHallwayTl.Enabled = true;
				nMoveHallwayTr.Enabled = true;
				nMoveHallwayBl.Enabled = true;
				nMoveHallwayBr.Enabled = true;
				nMoveDownstairsLeft.Enabled = true;
				nMoveDownstairsRight.Enabled = true;
				layer = 1;
			}
		}

		public void TriggerLayer2(Node body)
		{
			if (body != null && body.GetParent() == nMainF1)
			{
				CallDeferred("ReparentCharacter", body, nMainF2);
			}
			if (layer != 2)
			{
				nBoundaryF1.CollisionLayer = 0u;
				nBoundaryF2.CollisionLayer = 2u;
				nMoveHallwayTl.Enabled = false;
				nMoveHallwayTr.Enabled = false;
				nMoveHallwayBl.Enabled = false;
				nMoveHallwayBr.Enabled = false;
				nMoveDownstairsLeft.Enabled = false;
				nMoveDownstairsRight.Enabled = false;
				layer = 2;
			}
		}

		public override void ChangeLayer(int newLayer)
		{
			layer = 0;
			if (newLayer == 1)
			{
				TriggerLayer1(null);
			}
			else
			{
				TriggerLayer2(null);
			}
		}

		public void ReparentCharacter(Node node, Node to)
		{
			if (node is IPhysicsInterpolated body)
			{
				body.Reparent(to);
			}
			Game.Camera.TrackPlayer();
		}
	}
}
