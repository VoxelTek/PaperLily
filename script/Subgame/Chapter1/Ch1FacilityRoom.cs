// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1FacilityRoom
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Subgame.Chapter1;
using System;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1FacilityRoom : GameRoom
  {
    [Export(PropertyHint.None, "")]
    protected Lighting lightOn;
    [Export(PropertyHint.None, "")]
    protected Lighting lightOff;
    [Export(PropertyHint.None, "")]
    protected float primalChanceEz;
    [Export(PropertyHint.None, "")]
    protected float primalChanceHd;
    [Export(PropertyHint.None, "")]
    protected string primalSpawnVar;
    [Export(PropertyHint.None, "")]
    protected string primalTypeVar;
    [LacieEngine.API.GetNode("Lighting")]
    protected Node2D nLights;
    private PVar varLightsOff = (PVar) "ch1.facility_lights_off";
    private PVar varReleased = (PVar) "ch1.facility_dungeon_2_released";
    private PVar varPrimalsCanSpawn = (PVar) "ch1.facility_primals_can_spawn";
    private PEvent evtDeath = (PEvent) "ch1_death_impact";
    protected PVar varPrimalSpawn;
    protected PVar varPrimalType;
    protected NPCChaser nChaser;

    public override void _UpdateRoom()
    {
      bool flag = !(bool) this.varLightsOff.Value;
      this.nLights.Visible = flag;
      if (flag)
        this.lightOn.Apply();
      else
        this.lightOff.Apply();
    }

    public override void _AfterFadeIn()
    {
      if (!(this.nChaser is Ch1FacilityPrimal2))
        return;
      this.nChaser.Chasing = true;
    }

    public void TrySpawnPrimal(Navigation2D nav, int spawnPoints)
    {
      if (!(bool) this.varPrimalsCanSpawn.Value || this.primalSpawnVar.IsNullOrEmpty())
        return;
      this.varPrimalSpawn = (PVar) this.primalSpawnVar;
      this.varPrimalType = (PVar) this.primalTypeVar;
      if (!DrkieUtil.RollPercent(!(bool) this.varPrimalSpawn.Value ? ((bool) this.varReleased.Value ? (double) this.primalChanceHd : (double) this.primalChanceEz) : 100.0))
        return;
      this.SpawnEnemy(nav, spawnPoints);
    }

    private void SpawnEnemy(Navigation2D nav, int spawnPoints)
    {
      if (!(bool) this.varPrimalSpawn.Value)
      {
        this.varPrimalSpawn.NewValue = (PVar.PVarSetProxy) this.DecideSpawn(spawnPoints);
        this.varPrimalType.NewValue = (PVar.PVarSetProxy) this.DecideEnemy();
      }
      this.nChaser = this.MakeEnemy((int) this.varPrimalType.Value);
      SpawnPoint spawnPoint = this.GetSpawnPoint((string) this.varPrimalSpawn.Value);
      this.nChaser.Position = spawnPoint.Position;
      this.nChaser.DefaultDirection = (Direction) spawnPoint.Direction;
      this.GetMainLayer().AddChild((Node) this.nChaser);
      this.nChaser.SetNavigation(nav);
      this.nChaser.Caught += new Action(this.PlayerDeath);
    }

    public void PlayerDeath() => this.evtDeath.Play();

    private string DecideSpawn(int max)
    {
      return "primal_spawn_" + new Random().Next(1, max + 1).ToString();
    }

    private int DecideEnemy() => DrkieUtil.RollPercent(75.0) ? 1 : 2;

    private NPCChaser MakeEnemy(int number)
    {
      return number == 1 ? (NPCChaser) new Ch1FacilityPrimal() : (NPCChaser) new Ch1FacilityPrimal2();
    }
  }
}
