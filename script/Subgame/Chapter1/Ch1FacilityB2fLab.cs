// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1FacilityB2fLab
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1FacilityB2fLab : Ch1FacilityRoom
  {
    [LacieEngine.API.GetNode("Main/SaiLean")]
    private Node2D nSai;
    [LacieEngine.API.GetNode("Foreground/LightFadeout")]
    private Node2D nDoorFadeout;
    [LacieEngine.API.GetNode("Other/Events/chr_sai")]
    private EventTrigger nSaiEvent;
    [LacieEngine.API.GetNode("Main/MiscGate")]
    private Sprite nGate;
    [LacieEngine.API.GetNode("Other/Events/misc_gate")]
    private EventTrigger nGateEvent;
    [LacieEngine.API.GetNode("Ground/ShinyFuse")]
    private Node2D nFuseShiny;
    private PVar varLabLocked = (PVar) "ch1.facility_lab_locked";
    private PVar varGateUnlocked = (PVar) "ch1.facility_lab_gate_unlocked";
    private PVar varSaiHide = (PVar) "ch1.facility_sai_hidden";
    private PVar varTookFuse = (PVar) "ch1.facility_lab_took_fuse_floor";
    private PVar varLightsOut = (PVar) "ch1.facility_lab_lights_out";

    public override void _BeforeFadeIn()
    {
      if (!(bool) this.varSaiHide.Value || !Game.Room.RegisteredNPCs.ContainsKey("sai") || !(Game.Room.RegisteredNPCs["sai"] is NPCFollower registeredNpC))
        return;
      registeredNpC.DeleteIfValid();
    }

    public override void _UpdateRoom()
    {
      base._UpdateRoom();
      this.nSai.Visible = (bool) this.varLabLocked.Value;
      this.nSaiEvent.Enabled = (bool) this.varLabLocked.Value;
      this.nDoorFadeout.Visible = !(bool) this.varLabLocked.Value;
      this.nGate.Frame = (bool) this.varGateUnlocked.Value ? 1 : 0;
      this.nGateEvent.Enabled = !(bool) this.varGateUnlocked.Value;
      this.nFuseShiny.Visible = !(bool) this.varTookFuse.Value;
      if (!(bool) this.varLightsOut.Value)
        return;
      this.nLights.Visible = false;
      this.lightOff.Apply();
      Game.Player.GetLight().Visible = false;
    }
  }
}
