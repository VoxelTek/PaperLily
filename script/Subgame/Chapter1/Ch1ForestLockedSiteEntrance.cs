// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1ForestLockedSiteEntrance
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1ForestLockedSiteEntrance : GameRoom
  {
    [Inject]
    private IExtraFunctions Functions;
    [LacieEngine.API.GetNode("Main/StructFenceGate")]
    private Sprite nGate;
    [LacieEngine.API.GetNode("Main/StructFenceGate/MiscPoster1")]
    private Node2D nPoster;
    [LacieEngine.API.GetNode("Main/MiscLock")]
    private Sprite nLocks;
    [LacieEngine.API.GetNode("Background/Note")]
    private Node2D nNote;
    [LacieEngine.API.GetNode("Other/Events/misc_door")]
    private EventTrigger nGateEvent;
    [LacieEngine.API.GetNode("Other/Events/misc_door_open")]
    private EventTrigger nGateOpenEvent;
    private PVar varUnlocked = (PVar) "ch1.forest_lockedsite_unlocked";
    private PVar varOpen = (PVar) "ch1.forest_lockedsite_open";
    private PVar varTookNote = (PVar) "ch1.forest_lockedsite_took_letter";

    public override void _UpdateRoom()
    {
      this.nNote.Visible = !(bool) this.varTookNote.Value;
      if ((bool) this.varUnlocked.Value)
        this.nLocks.Frame = 1;
      if ((bool) this.varOpen.Value)
      {
        this.nGate.Frame = 1;
        this.nPoster.Visible = false;
        this.nGateEvent.Enabled = false;
        this.nGateOpenEvent.Enabled = true;
      }
      else
      {
        this.nGate.Frame = 0;
        this.nPoster.Visible = true;
        this.nGateEvent.Enabled = true;
        this.nGateOpenEvent.Enabled = false;
      }
    }

    public void AdjustPlayerPosForGate()
    {
      if (!(Game.Player.GetDirection() == Direction.Down))
        return;
      Game.Player.Node.Position = this.GetPoint("gate_inside");
      Game.Player.Teleport();
    }

    public void StopTimer() => this.Functions.StopTimer();
  }
}
