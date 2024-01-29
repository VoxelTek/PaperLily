using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Minigames
{
	public class Ch1PncFusebox : PointAndClick
	{
		[Export(PropertyHint.None, "")]
		public Texture texFuse5;

		[Export(PropertyHint.None, "")]
		public Texture texFuse10;

		[Export(PropertyHint.None, "")]
		public Texture texFuse15;

		[Export(PropertyHint.None, "")]
		public Texture texFuse20;

		[Export(PropertyHint.None, "")]
		public Texture texFuse25;

		[Export(PropertyHint.None, "")]
		public Texture texFuse40;

		[Export(PropertyHint.None, "")]
		public Texture texFuseEmpty;

		[Export(PropertyHint.None, "")]
		public Texture texFuse15Dead;

		[Export(PropertyHint.None, "")]
		public Texture texFuse25Dead;

		[GetNode("Targets/Slot1")]
		private TextureRect nSlot1;

		[GetNode("Targets/Slot2")]
		private TextureRect nSlot2;

		[GetNode("Targets/Slot3")]
		private TextureRect nSlot3;

		[GetNode("Targets/Slot4")]
		private TextureRect nSlot4;

		[GetNode("Targets/Slot5")]
		private TextureRect nSlot5;

		[GetNode("Targets/Slot6")]
		private TextureRect nSlot6;

		[GetNode("Targets/Slot7")]
		private TextureRect nSlot7;

		[GetNode("Targets/Slot8")]
		private TextureRect nSlot8;

		[GetNode("Bg/DisplayElevator")]
		private TextureRect nDisplayElevator;

		[GetNode("Bg/DisplayCouncil")]
		private TextureRect nDisplayCouncil;

		[GetNode("Bg/DisplayLights")]
		private TextureRect nDisplayLights;

		[GetNode("Bg/DisplayWater")]
		private TextureRect nDisplayWater;

		[GetNode("Bg/DisplayDungeon")]
		private TextureRect nDisplayDungeon;

		[GetNode("Targets/Spider1")]
		private TextureRect nSpider1;

		[GetNode("Targets/Spider2")]
		private TextureRect nSpider2;

		private PVar varSlot1 = "ch1.facility_fusebox_slot_1";

		private PVar varSlot2 = "ch1.facility_fusebox_slot_2";

		private PVar varSlot3 = "ch1.facility_fusebox_slot_3";

		private PVar varSlot4 = "ch1.facility_fusebox_slot_4";

		private PVar varSlot5 = "ch1.facility_fusebox_slot_5";

		private PVar varSlot6 = "ch1.facility_fusebox_slot_6";

		private PVar varSlot7 = "ch1.facility_fusebox_slot_7";

		private PVar varSlot8 = "ch1.facility_fusebox_slot_8";

		private PVar varFlood = "ch1.facility_power_flood";

		private PVar varLightsOff = "ch1.facility_lights_off";

		private PVar varElevator = "ch1.facility_elevator_on";

		private PVar varCouncil = "ch1.facility_council_button_on";

		private PVar varDungeon = "ch1.facility_dungeon_2_doors_open";

		private PVar varFuseToPlace = "ch1.temp_fuse_to_place";

		private PVar varRuneGone = "ch1.facility_rune_gone";

		private PVar varTriedRemoveDungeonFuse = "ch1.facility_fusebox_tried_slot_8";

		private PEvent evtTakeFuse = "pnc_fusebox_takefuse";

		private PEvent evtNoFuse = "pnc_fusebox_nofuse";

		private PEvent evtBlownReact = "event_blownfuse_react";

		private PEvent evtBlownTakeReact = "event_blownfuse_take_react";

		private PEvent evtNotAllFusesReact = "event_not_all_fuses_react";

		private PEvent evtLightsReact = "event_lights_react";

		private PEvent evtDungeonWarn = "event_dungeon_warn";

		private PEvent evtDungeonReact = "event_dungeon_react";

		private PEvent evtCouncilReact = "event_council_react";

		private PEvent evtElevatorReact = "event_elevator_react";

		private PEvent evtFixAllReact = "event_fixall_react";

		private PAchievement achElectrician = "CH1_ELECTRICIAN";

		private static readonly Color COLOR_CORRECT = new Color("#50ff80");

		private static readonly Color COLOR_INCORRECT = new Color("#ff7878");

		private Dictionary<int, Texture> _fuseTextures;

		private PVar _lastSlotInteracted;

		public override void Init()
		{
			base.Init();
			_fuseTextures = new Dictionary<int, Texture>();
			_fuseTextures[5] = texFuse5;
			_fuseTextures[10] = texFuse10;
			_fuseTextures[15] = texFuse15;
			_fuseTextures[20] = texFuse20;
			_fuseTextures[25] = texFuse25;
			_fuseTextures[40] = texFuse40;
			_fuseTextures[-1] = texFuseEmpty;
			_fuseTextures[-100] = texFuse15Dead;
			_fuseTextures[-101] = texFuse25Dead;
			if (DrkieUtil.RollPercent(5.0))
			{
				nSpider1.Visible = true;
			}
			else if (DrkieUtil.RollPercent(3.0))
			{
				nSpider2.Visible = true;
			}
			RefreshPanel();
		}

		public void RefreshPanel()
		{
			int slot1 = varSlot1.Value;
			int slot2 = varSlot2.Value;
			int slot3 = varSlot3.Value;
			int slot4 = varSlot4.Value;
			int slot5 = varSlot5.Value;
			int slot6 = varSlot6.Value;
			int slot7 = varSlot7.Value;
			int slot8 = varSlot8.Value;
			nSlot1.Texture = _fuseTextures[slot1];
			nSlot2.Texture = _fuseTextures[slot2];
			nSlot3.Texture = _fuseTextures[slot3];
			nSlot4.Texture = _fuseTextures[slot4];
			nSlot5.Texture = _fuseTextures[slot5];
			nSlot6.Texture = _fuseTextures[slot6];
			nSlot7.Texture = _fuseTextures[slot7];
			nSlot8.Texture = _fuseTextures[slot8];
			int num;
			int num2;
			if (slot1 != -1 && slot2 != -1 && slot3 != -1)
			{
				num = ((slot4 == -1) ? 1 : 0);
				if (num == 0)
				{
					num2 = ((slot1 + slot2 + (slot3 - slot4) == 55) ? 1 : 0);
					goto IL_016c;
				}
			}
			else
			{
				num = 1;
			}
			num2 = 0;
			goto IL_016c;
			IL_016c:
			bool elevator = (byte)num2 != 0;
			bool council = slot5 == 5;
			bool lights = slot6 == 15;
			bool flood = slot7 == 40;
			bool dungeon = slot8 == 25;
			if (num != 0 && slot1 + slot2 + (slot3 - slot4) == 55)
			{
				evtNotAllFusesReact.Play();
			}
			if (slot1 + slot2 + (slot3 - slot4) == 44)
			{
				evtBlownReact.Play();
			}
			if (elevator && council && lights && flood && dungeon)
			{
				evtFixAllReact.Play();
			}
			nDisplayElevator.Modulate = (elevator ? COLOR_CORRECT : COLOR_INCORRECT);
			nDisplayCouncil.Modulate = (council ? COLOR_CORRECT : COLOR_INCORRECT);
			nDisplayLights.Modulate = (lights ? COLOR_CORRECT : COLOR_INCORRECT);
			nDisplayWater.Modulate = (flood ? COLOR_CORRECT : COLOR_INCORRECT);
			nDisplayDungeon.Modulate = (dungeon ? COLOR_CORRECT : COLOR_INCORRECT);
			varElevator.NewValue = elevator;
			varCouncil.NewValue = council;
			varLightsOff.NewValue = !lights;
			varFlood.NewValue = flood;
			varDungeon.NewValue = !dungeon;
			if (council)
			{
				evtCouncilReact.Play();
			}
			if (elevator)
			{
				evtElevatorReact.Play();
			}
			if (!dungeon)
			{
				evtDungeonReact.Play();
			}
			if (!lights)
			{
				evtLightsReact.Play();
			}
			if (elevator && council && lights && flood && dungeon)
			{
				achElectrician.Unlock();
			}
			if (elevator || !dungeon || !lights)
			{
				varRuneGone.NewValue = true;
			}
		}

		public void Slot1()
		{
			ExecuteSlot(varSlot1);
		}

		public void Slot2()
		{
			ExecuteSlot(varSlot2);
		}

		public void Slot3()
		{
			ExecuteSlot(varSlot3);
		}

		public void Slot4()
		{
			ExecuteSlot(varSlot4);
		}

		public void Slot5()
		{
			ExecuteSlot(varSlot5);
		}

		public void Slot6()
		{
			ExecuteSlot(varSlot6);
		}

		public void Slot7()
		{
			ExecuteSlot(varSlot7);
		}

		public void Slot8()
		{
			if ((bool)varTriedRemoveDungeonFuse.Value)
			{
				ExecuteSlot(varSlot8);
			}
			else
			{
				evtDungeonWarn.Play();
			}
		}

		public void PlaceFuse()
		{
			_lastSlotInteracted.NewValue = varFuseToPlace.Value;
			RefreshPanel();
		}

		public void TakeFuse()
		{
			if ((int)_lastSlotInteracted.Value > 0)
			{
				Game.Items.AddItem("ch1.facility_fuse_" + _lastSlotInteracted.Value);
			}
			if ((int)_lastSlotInteracted.Value <= -100)
			{
				evtBlownTakeReact.Play();
			}
			_lastSlotInteracted.NewValue = -1;
			RefreshPanel();
		}

		private void ExecuteSlot(PVar var)
		{
			_lastSlotInteracted = var;
			PEvent pEvent = (((int)var.Value != -1) ? evtTakeFuse : evtNoFuse);
			pEvent.Play();
		}
	}
}
