using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Minigames;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityB1fControlRoom : Ch1FacilityRoom
	{
		[Inject]
		private IItemManager Items;

		[Export(PropertyHint.None, "")]
		private Texture texValveSquare;

		[Export(PropertyHint.None, "")]
		private Texture texValvePentagon;

		[Export(PropertyHint.None, "")]
		private Texture texValveStar;

		[Export(PropertyHint.None, "")]
		private Texture texValveDiamond;

		[GetNode("Background/StructMachinery/MiscValve1")]
		private Sprite nValve1;

		[GetNode("Background/StructMachinery/MiscValve2")]
		private Sprite nValve2;

		[GetNode("Background/StructMachinery/MiscValve3")]
		private Sprite nValve3;

		[GetNode("Background/StructMachinery/MiscValve4")]
		private Sprite nValve4;

		[GetNode("Background/Barrel/Valve")]
		private Sprite nValveOnBarrel;

		[GetNode("Background/StructMachinery/IndicatorRed1")]
		private Node2D nIndicatorRed1;

		[GetNode("Background/StructMachinery/IndicatorRed2")]
		private Node2D nIndicatorRed2;

		[GetNode("Background/StructMachinery/IndicatorRed3")]
		private Node2D nIndicatorRed3;

		[GetNode("Background/StructMachinery/IndicatorRed4")]
		private Node2D nIndicatorRed4;

		[GetNode("Background/StructMachinery/IndicatorGreen1")]
		private Node2D nIndicatorGreen1;

		[GetNode("Background/StructMachinery/IndicatorGreen2")]
		private Node2D nIndicatorGreen2;

		[GetNode("Background/StructMachinery/IndicatorGreen3")]
		private Node2D nIndicatorGreen3;

		[GetNode("Background/StructMachinery/IndicatorGreen4")]
		private Node2D nIndicatorGreen4;

		[GetNode("Background/StructMachinery/Animation")]
		private AnimationPlayer nPowerOffAnimation;

		private PVar varValve1Set = "ch1.facility_valve_1_set";

		private PVar varValve2Set = "ch1.facility_valve_2_set";

		private PVar varValve3Set = "ch1.facility_valve_3_set";

		private PVar varValve4Set = "ch1.facility_valve_4_set";

		private PVar varValve1Turned = "ch1.facility_valve_1_turned";

		private PVar varValve2Turned = "ch1.facility_valve_2_turned";

		private PVar varValve3Turned = "ch1.facility_valve_3_turned";

		private PVar varValve4Turned = "ch1.facility_valve_4_turned";

		private PVar varFloodPower = "ch1.facility_power_flood";

		private PVar varFloodEngaged = "ch1.facility_flood_engaged";

		private PVar varTookValve = "ch1.facility_took_valve_square";

		private PVar varHasFuses = "ch1.temp_player_has_fuses";

		private PVar varRuneGone = "ch1.facility_rune_gone";

		private PEvent evtFloodReact = "event_flood_react";

		public override void _UpdateRoom()
		{
			base._UpdateRoom();
			UpdateValveHandle(nValve1, varValve1Set.Value);
			UpdateValveHandle(nValve2, varValve2Set.Value);
			UpdateValveHandle(nValve3, varValve3Set.Value);
			UpdateValveHandle(nValve4, varValve4Set.Value);
			nValveOnBarrel.Visible = !varTookValve.Value;
			if ((bool)varFloodPower.Value)
			{
				nPowerOffAnimation.Stop();
				nIndicatorGreen1.Visible = varValve1Turned.Value;
				nIndicatorGreen2.Visible = varValve2Turned.Value;
				nIndicatorGreen3.Visible = varValve3Turned.Value;
				nIndicatorGreen4.Visible = varValve4Turned.Value;
				nIndicatorRed1.Visible = !nIndicatorGreen1.Visible;
				nIndicatorRed2.Visible = !nIndicatorGreen2.Visible;
				nIndicatorRed3.Visible = !nIndicatorGreen3.Visible;
				nIndicatorRed4.Visible = !nIndicatorGreen4.Visible;
			}
			else
			{
				nIndicatorGreen1.Visible = false;
				nIndicatorGreen2.Visible = false;
				nIndicatorGreen3.Visible = false;
				nIndicatorGreen4.Visible = false;
				nPowerOffAnimation.PlayFirstAnimation();
			}
		}

		public void CheckFloodEngaged()
		{
			if ((bool)varFloodPower.Value && (bool)varValve1Turned.Value && (bool)varValve2Turned.Value && (bool)varValve3Turned.Value && (bool)varValve4Turned.Value)
			{
				varFloodEngaged.NewValue = true;
			}
			if ((bool)varFloodEngaged.Value)
			{
				Game.Objectives.Complete("ch1.facility_remove_flood");
				Game.Objectives.Fail("ch1.facility_fix_elevator");
				varRuneGone.NewValue = true;
				evtFloodReact.Play();
			}
		}

		public void CheckHasFuses()
		{
			varHasFuses.NewValue = false;
			foreach (IItem ownedItem in Items.GetOwnedItems())
			{
				if (ownedItem.Id.Contains(".facility_fuse_"))
				{
					varHasFuses.NewValue = true;
				}
			}
		}

		public void PlaceFuse()
		{
			if (Game.Minigames.Running)
			{
				((Ch1PncFusebox)Game.Minigames.Node).PlaceFuse();
			}
		}

		public void TakeFuse()
		{
			if (Game.Minigames.Running)
			{
				((Ch1PncFusebox)Game.Minigames.Node).TakeFuse();
			}
		}

		private void UpdateValveHandle(Sprite sprite, string value)
		{
			sprite.Visible = true;
			if (value == null)
			{
				sprite.Visible = false;
				return;
			}
			switch (value)
			{
			case "square":
				sprite.Texture = texValveSquare;
				break;
			case "pentagon":
				sprite.Texture = texValvePentagon;
				break;
			case "star":
				sprite.Texture = texValveStar;
				break;
			case "diamond":
				sprite.Texture = texValveDiamond;
				break;
			}
		}
	}
}
