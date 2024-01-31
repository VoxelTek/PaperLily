using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Minigames
{
    // Token: 0x02000120 RID: 288
    public class Ch1PncFusebox : PointAndClick
    {
        // Token: 0x060009F7 RID: 2551 RVA: 0x00027FC0 File Offset: 0x000261C0
        public override void Init()
        {
            base.Init();
            this._fuseTextures = new Dictionary<int, Texture>();
            this._fuseTextures[5] = this.texFuse5;
            this._fuseTextures[10] = this.texFuse10;
            this._fuseTextures[15] = this.texFuse15;
            this._fuseTextures[20] = this.texFuse20;
            this._fuseTextures[25] = this.texFuse25;
            this._fuseTextures[40] = this.texFuse40;
            this._fuseTextures[-1] = this.texFuseEmpty;
            this._fuseTextures[-100] = this.texFuse15Dead;
            this._fuseTextures[-101] = this.texFuse25Dead;
            if (DrkieUtil.RollPercent(5.0))
            {
                this.nSpider1.Visible = true;
            }
            else if (DrkieUtil.RollPercent(3.0))
            {
                this.nSpider2.Visible = true;
            }
            this.RefreshPanel();
        }

        // Token: 0x060009F8 RID: 2552 RVA: 0x000280C8 File Offset: 0x000262C8
        public void RefreshPanel()
        {
            int slot = this.varSlot1.Value;
            int slot2 = this.varSlot2.Value;
            int slot3 = this.varSlot3.Value;
            int slot4 = this.varSlot4.Value;
            int slot5 = this.varSlot5.Value;
            int slot6 = this.varSlot6.Value;
            int slot7 = this.varSlot7.Value;
            int slot8 = this.varSlot8.Value;
            this.nSlot1.Texture = this._fuseTextures[slot];
            this.nSlot2.Texture = this._fuseTextures[slot2];
            this.nSlot3.Texture = this._fuseTextures[slot3];
            this.nSlot4.Texture = this._fuseTextures[slot4];
            this.nSlot5.Texture = this._fuseTextures[slot5];
            this.nSlot6.Texture = this._fuseTextures[slot6];
            this.nSlot7.Texture = this._fuseTextures[slot7];
            this.nSlot8.Texture = this._fuseTextures[slot8];
            bool flag = slot == -1 || slot2 == -1 || slot3 == -1 || slot4 == -1;
            bool elevator = !flag && slot + slot2 + (slot3 - slot4) == 55;
            bool council = slot5 == 5;
            bool lights = slot6 == 15;
            bool flood = slot7 == 40;
            bool dungeon = slot8 == 25;
            if (flag && slot + slot2 + (slot3 - slot4) == 55)
            {
                this.evtNotAllFusesReact.Play();
            }
            if (slot + slot2 + (slot3 - slot4) == 44)
            {
                this.evtBlownReact.Play();
            }
            if (elevator && council && lights && flood && dungeon)
            {
                this.evtFixAllReact.Play();
            }
            this.nDisplayElevator.Modulate = (elevator ? Ch1PncFusebox.COLOR_CORRECT : Ch1PncFusebox.COLOR_INCORRECT);
            this.nDisplayCouncil.Modulate = (council ? Ch1PncFusebox.COLOR_CORRECT : Ch1PncFusebox.COLOR_INCORRECT);
            this.nDisplayLights.Modulate = (lights ? Ch1PncFusebox.COLOR_CORRECT : Ch1PncFusebox.COLOR_INCORRECT);
            this.nDisplayWater.Modulate = (flood ? Ch1PncFusebox.COLOR_CORRECT : Ch1PncFusebox.COLOR_INCORRECT);
            this.nDisplayDungeon.Modulate = (dungeon ? Ch1PncFusebox.COLOR_CORRECT : Ch1PncFusebox.COLOR_INCORRECT);
            this.varElevator.NewValue = elevator;
            this.varCouncil.NewValue = council;
            this.varLightsOff.NewValue = !lights;
            this.varFlood.NewValue = flood;
            this.varDungeon.NewValue = !dungeon;
            if (council)
            {
                this.evtCouncilReact.Play();
            }
            if (elevator)
            {
                this.evtElevatorReact.Play();
            }
            if (!dungeon)
            {
                this.evtDungeonReact.Play();
            }
            if (!lights)
            {
                this.evtLightsReact.Play();
            }
            if (elevator && council && lights && flood && dungeon)
            {
                this.achElectrician.Unlock();
            }
            if (elevator || !dungeon || !lights)
            {
                this.varRuneGone.NewValue = true;
            }
        }

        // Token: 0x060009F9 RID: 2553 RVA: 0x00028406 File Offset: 0x00026606
        public void Slot1()
        {
            this.ExecuteSlot(this.varSlot1);
        }

        // Token: 0x060009FA RID: 2554 RVA: 0x00028414 File Offset: 0x00026614
        public void Slot2()
        {
            this.ExecuteSlot(this.varSlot2);
        }

        // Token: 0x060009FB RID: 2555 RVA: 0x00028422 File Offset: 0x00026622
        public void Slot3()
        {
            this.ExecuteSlot(this.varSlot3);
        }

        // Token: 0x060009FC RID: 2556 RVA: 0x00028430 File Offset: 0x00026630
        public void Slot4()
        {
            this.ExecuteSlot(this.varSlot4);
        }

        // Token: 0x060009FD RID: 2557 RVA: 0x0002843E File Offset: 0x0002663E
        public void Slot5()
        {
            this.ExecuteSlot(this.varSlot5);
        }

        // Token: 0x060009FE RID: 2558 RVA: 0x0002844C File Offset: 0x0002664C
        public void Slot6()
        {
            this.ExecuteSlot(this.varSlot6);
        }

        // Token: 0x060009FF RID: 2559 RVA: 0x0002845A File Offset: 0x0002665A
        public void Slot7()
        {
            this.ExecuteSlot(this.varSlot7);
        }

        // Token: 0x06000A00 RID: 2560 RVA: 0x00028468 File Offset: 0x00026668
        public void Slot8()
        {
            if (this.varTriedRemoveDungeonFuse.Value)
            {
                this.ExecuteSlot(this.varSlot8);
                return;
            }
            this.evtDungeonWarn.Play();
        }

        // Token: 0x06000A01 RID: 2561 RVA: 0x00028494 File Offset: 0x00026694
        public void PlaceFuse()
        {
            this._lastSlotInteracted.NewValue = this.varFuseToPlace.Value;
            this.RefreshPanel();
        }

        // Token: 0x06000A02 RID: 2562 RVA: 0x000284B8 File Offset: 0x000266B8
        public void TakeFuse()
        {
            if (this._lastSlotInteracted.Value > 0)
            {
                Game.Items.AddItem("ch1.facility_fuse_" + this._lastSlotInteracted.Value);
            }
            if (this._lastSlotInteracted.Value <= -100)
            {
                this.evtBlownTakeReact.Play();
            }
            this._lastSlotInteracted.NewValue = -1;
            this.RefreshPanel();
        }

        // Token: 0x06000A03 RID: 2563 RVA: 0x00028534 File Offset: 0x00026734
        private void ExecuteSlot(PVar var)
        {
            this._lastSlotInteracted = var;
            PEvent pevent;
            if (var.Value == -1)
            {
                pevent = this.evtNoFuse;
            }
            else
            {
                pevent = this.evtTakeFuse;
            }
            pevent.Play();
        }

        // Token: 0x04000858 RID: 2136
        [Export(PropertyHint.None, "")]
        public Texture texFuse5;

        // Token: 0x04000859 RID: 2137
        [Export(PropertyHint.None, "")]
        public Texture texFuse10;

        // Token: 0x0400085A RID: 2138
        [Export(PropertyHint.None, "")]
        public Texture texFuse15;

        // Token: 0x0400085B RID: 2139
        [Export(PropertyHint.None, "")]
        public Texture texFuse20;

        // Token: 0x0400085C RID: 2140
        [Export(PropertyHint.None, "")]
        public Texture texFuse25;

        // Token: 0x0400085D RID: 2141
        [Export(PropertyHint.None, "")]
        public Texture texFuse40;

        // Token: 0x0400085E RID: 2142
        [Export(PropertyHint.None, "")]
        public Texture texFuseEmpty;

        // Token: 0x0400085F RID: 2143
        [Export(PropertyHint.None, "")]
        public Texture texFuse15Dead;

        // Token: 0x04000860 RID: 2144
        [Export(PropertyHint.None, "")]
        public Texture texFuse25Dead;

        // Token: 0x04000861 RID: 2145
        [GetNode("Targets/Slot1")]
        private TextureRect nSlot1;

        // Token: 0x04000862 RID: 2146
        [GetNode("Targets/Slot2")]
        private TextureRect nSlot2;

        // Token: 0x04000863 RID: 2147
        [GetNode("Targets/Slot3")]
        private TextureRect nSlot3;

        // Token: 0x04000864 RID: 2148
        [GetNode("Targets/Slot4")]
        private TextureRect nSlot4;

        // Token: 0x04000865 RID: 2149
        [GetNode("Targets/Slot5")]
        private TextureRect nSlot5;

        // Token: 0x04000866 RID: 2150
        [GetNode("Targets/Slot6")]
        private TextureRect nSlot6;

        // Token: 0x04000867 RID: 2151
        [GetNode("Targets/Slot7")]
        private TextureRect nSlot7;

        // Token: 0x04000868 RID: 2152
        [GetNode("Targets/Slot8")]
        private TextureRect nSlot8;

        // Token: 0x04000869 RID: 2153
        [GetNode("Bg/DisplayElevator")]
        private TextureRect nDisplayElevator;

        // Token: 0x0400086A RID: 2154
        [GetNode("Bg/DisplayCouncil")]
        private TextureRect nDisplayCouncil;

        // Token: 0x0400086B RID: 2155
        [GetNode("Bg/DisplayLights")]
        private TextureRect nDisplayLights;

        // Token: 0x0400086C RID: 2156
        [GetNode("Bg/DisplayWater")]
        private TextureRect nDisplayWater;

        // Token: 0x0400086D RID: 2157
        [GetNode("Bg/DisplayDungeon")]
        private TextureRect nDisplayDungeon;

        // Token: 0x0400086E RID: 2158
        [GetNode("Targets/Spider1")]
        private TextureRect nSpider1;

        // Token: 0x0400086F RID: 2159
        [GetNode("Targets/Spider2")]
        private TextureRect nSpider2;

        // Token: 0x04000870 RID: 2160
        private PVar varSlot1 = "ch1.facility_fusebox_slot_1";

        // Token: 0x04000871 RID: 2161
        private PVar varSlot2 = "ch1.facility_fusebox_slot_2";

        // Token: 0x04000872 RID: 2162
        private PVar varSlot3 = "ch1.facility_fusebox_slot_3";

        // Token: 0x04000873 RID: 2163
        private PVar varSlot4 = "ch1.facility_fusebox_slot_4";

        // Token: 0x04000874 RID: 2164
        private PVar varSlot5 = "ch1.facility_fusebox_slot_5";

        // Token: 0x04000875 RID: 2165
        private PVar varSlot6 = "ch1.facility_fusebox_slot_6";

        // Token: 0x04000876 RID: 2166
        private PVar varSlot7 = "ch1.facility_fusebox_slot_7";

        // Token: 0x04000877 RID: 2167
        private PVar varSlot8 = "ch1.facility_fusebox_slot_8";

        // Token: 0x04000878 RID: 2168
        private PVar varFlood = "ch1.facility_power_flood";

        // Token: 0x04000879 RID: 2169
        private PVar varLightsOff = "ch1.facility_lights_off";

        // Token: 0x0400087A RID: 2170
        private PVar varElevator = "ch1.facility_elevator_on";

        // Token: 0x0400087B RID: 2171
        private PVar varCouncil = "ch1.facility_council_button_on";

        // Token: 0x0400087C RID: 2172
        private PVar varDungeon = "ch1.facility_dungeon_2_doors_open";

        // Token: 0x0400087D RID: 2173
        private PVar varFuseToPlace = "ch1.temp_fuse_to_place";

        // Token: 0x0400087E RID: 2174
        private PVar varRuneGone = "ch1.facility_rune_gone";

        // Token: 0x0400087F RID: 2175
        private PVar varTriedRemoveDungeonFuse = "ch1.facility_fusebox_tried_slot_8";

        // Token: 0x04000880 RID: 2176
        private PEvent evtTakeFuse = "pnc_fusebox_takefuse";

        // Token: 0x04000881 RID: 2177
        private PEvent evtNoFuse = "pnc_fusebox_nofuse";

        // Token: 0x04000882 RID: 2178
        private PEvent evtBlownReact = "event_blownfuse_react";

        // Token: 0x04000883 RID: 2179
        private PEvent evtBlownTakeReact = "event_blownfuse_take_react";

        // Token: 0x04000884 RID: 2180
        private PEvent evtNotAllFusesReact = "event_not_all_fuses_react";

        // Token: 0x04000885 RID: 2181
        private PEvent evtLightsReact = "event_lights_react";

        // Token: 0x04000886 RID: 2182
        private PEvent evtDungeonWarn = "event_dungeon_warn";

        // Token: 0x04000887 RID: 2183
        private PEvent evtDungeonReact = "event_dungeon_react";

        // Token: 0x04000888 RID: 2184
        private PEvent evtCouncilReact = "event_council_react";

        // Token: 0x04000889 RID: 2185
        private PEvent evtElevatorReact = "event_elevator_react";

        // Token: 0x0400088A RID: 2186
        private PEvent evtFixAllReact = "event_fixall_react";

        // Token: 0x0400088B RID: 2187
        private PAchievement achElectrician = "CH1_ELECTRICIAN";

        // Token: 0x0400088C RID: 2188
        private static readonly Color COLOR_CORRECT = new Color("#50ff80");

        // Token: 0x0400088D RID: 2189
        private static readonly Color COLOR_INCORRECT = new Color("#ff7878");

        // Token: 0x0400088E RID: 2190
        private Dictionary<int, Texture> _fuseTextures;

        // Token: 0x0400088F RID: 2191
        private PVar _lastSlotInteracted;
    }
}
