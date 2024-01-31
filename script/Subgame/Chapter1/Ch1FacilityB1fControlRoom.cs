// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityB1fControlRoom
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Minigames;

#nullable disable
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
    [LacieEngine.API.GetNode("Background/StructMachinery/MiscValve1")]
    private Sprite nValve1;
    [LacieEngine.API.GetNode("Background/StructMachinery/MiscValve2")]
    private Sprite nValve2;
    [LacieEngine.API.GetNode("Background/StructMachinery/MiscValve3")]
    private Sprite nValve3;
    [LacieEngine.API.GetNode("Background/StructMachinery/MiscValve4")]
    private Sprite nValve4;
    [LacieEngine.API.GetNode("Background/Barrel/Valve")]
    private Sprite nValveOnBarrel;
    [LacieEngine.API.GetNode("Background/StructMachinery/IndicatorRed1")]
    private Node2D nIndicatorRed1;
    [LacieEngine.API.GetNode("Background/StructMachinery/IndicatorRed2")]
    private Node2D nIndicatorRed2;
    [LacieEngine.API.GetNode("Background/StructMachinery/IndicatorRed3")]
    private Node2D nIndicatorRed3;
    [LacieEngine.API.GetNode("Background/StructMachinery/IndicatorRed4")]
    private Node2D nIndicatorRed4;
    [LacieEngine.API.GetNode("Background/StructMachinery/IndicatorGreen1")]
    private Node2D nIndicatorGreen1;
    [LacieEngine.API.GetNode("Background/StructMachinery/IndicatorGreen2")]
    private Node2D nIndicatorGreen2;
    [LacieEngine.API.GetNode("Background/StructMachinery/IndicatorGreen3")]
    private Node2D nIndicatorGreen3;
    [LacieEngine.API.GetNode("Background/StructMachinery/IndicatorGreen4")]
    private Node2D nIndicatorGreen4;
    [LacieEngine.API.GetNode("Background/StructMachinery/Animation")]
    private AnimationPlayer nPowerOffAnimation;
    private PVar varValve1Set = (PVar) "ch1.facility_valve_1_set";
    private PVar varValve2Set = (PVar) "ch1.facility_valve_2_set";
    private PVar varValve3Set = (PVar) "ch1.facility_valve_3_set";
    private PVar varValve4Set = (PVar) "ch1.facility_valve_4_set";
    private PVar varValve1Turned = (PVar) "ch1.facility_valve_1_turned";
    private PVar varValve2Turned = (PVar) "ch1.facility_valve_2_turned";
    private PVar varValve3Turned = (PVar) "ch1.facility_valve_3_turned";
    private PVar varValve4Turned = (PVar) "ch1.facility_valve_4_turned";
    private PVar varFloodPower = (PVar) "ch1.facility_power_flood";
    private PVar varFloodEngaged = (PVar) "ch1.facility_flood_engaged";
    private PVar varTookValve = (PVar) "ch1.facility_took_valve_square";
    private PVar varHasFuses = (PVar) "ch1.temp_player_has_fuses";
    private PVar varRuneGone = (PVar) "ch1.facility_rune_gone";
    private PEvent evtFloodReact = (PEvent) "event_flood_react";

    public override void _UpdateRoom()
    {
      base._UpdateRoom();
      this.UpdateValveHandle(this.nValve1, (string) this.varValve1Set.Value);
      this.UpdateValveHandle(this.nValve2, (string) this.varValve2Set.Value);
      this.UpdateValveHandle(this.nValve3, (string) this.varValve3Set.Value);
      this.UpdateValveHandle(this.nValve4, (string) this.varValve4Set.Value);
      this.nValveOnBarrel.Visible = !(bool) this.varTookValve.Value;
      if ((bool) this.varFloodPower.Value)
      {
        this.nPowerOffAnimation.Stop();
        this.nIndicatorGreen1.Visible = (bool) this.varValve1Turned.Value;
        this.nIndicatorGreen2.Visible = (bool) this.varValve2Turned.Value;
        this.nIndicatorGreen3.Visible = (bool) this.varValve3Turned.Value;
        this.nIndicatorGreen4.Visible = (bool) this.varValve4Turned.Value;
        this.nIndicatorRed1.Visible = !this.nIndicatorGreen1.Visible;
        this.nIndicatorRed2.Visible = !this.nIndicatorGreen2.Visible;
        this.nIndicatorRed3.Visible = !this.nIndicatorGreen3.Visible;
        this.nIndicatorRed4.Visible = !this.nIndicatorGreen4.Visible;
      }
      else
      {
        this.nIndicatorGreen1.Visible = false;
        this.nIndicatorGreen2.Visible = false;
        this.nIndicatorGreen3.Visible = false;
        this.nIndicatorGreen4.Visible = false;
        this.nPowerOffAnimation.PlayFirstAnimation();
      }
    }

    public void CheckFloodEngaged()
    {
      if ((bool) this.varFloodPower.Value && (bool) this.varValve1Turned.Value && (bool) this.varValve2Turned.Value && (bool) this.varValve3Turned.Value && (bool) this.varValve4Turned.Value)
        this.varFloodEngaged.NewValue = (PVar.PVarSetProxy) true;
      if (!(bool) this.varFloodEngaged.Value)
        return;
      Game.Objectives.Complete("ch1.facility_remove_flood");
      Game.Objectives.Fail("ch1.facility_fix_elevator");
      this.varRuneGone.NewValue = (PVar.PVarSetProxy) true;
      this.evtFloodReact.Play();
    }

    public void CheckHasFuses()
    {
      this.varHasFuses.NewValue = (PVar.PVarSetProxy) false;
      foreach (IItem ownedItem in this.Items.GetOwnedItems())
      {
        if (ownedItem.Id.Contains(".facility_fuse_"))
          this.varHasFuses.NewValue = (PVar.PVarSetProxy) true;
      }
    }

    public void PlaceFuse()
    {
      if (!Game.Minigames.Running)
        return;
      ((Ch1PncFusebox) Game.Minigames.Node).PlaceFuse();
    }

    public void TakeFuse()
    {
      if (!Game.Minigames.Running)
        return;
      ((Ch1PncFusebox) Game.Minigames.Node).TakeFuse();
    }

    private void UpdateValveHandle(Sprite sprite, string value)
    {
      sprite.Visible = true;
      switch (value)
      {
        case null:
          sprite.Visible = false;
          break;
        case "square":
          sprite.Texture = this.texValveSquare;
          break;
        case "pentagon":
          sprite.Texture = this.texValvePentagon;
          break;
        case "star":
          sprite.Texture = this.texValveStar;
          break;
        case "diamond":
          sprite.Texture = this.texValveDiamond;
          break;
      }
    }
  }
}
