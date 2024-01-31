// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.Ch1ForestLockedSite
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;
using System;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  [Tool]
  public class Ch1ForestLockedSite : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private AudioStream sfxBush;
    [Export(PropertyHint.None, "")]
    private AudioStream sfxHeartbeatLow;
    [Export(PropertyHint.None, "")]
    private AudioStream sfxHeartbeatHigh;
    [Export(PropertyHint.None, "")]
    private PackedScene scnLightCandleBlue;
    [Export(PropertyHint.None, "")]
    private PackedScene scnLightCandleRed;
    [Export(PropertyHint.None, "")]
    private PackedScene scnLightFireflies;
    [Export(PropertyHint.None, "")]
    private PackedScene scnLightFlashlight;
    [LacieEngine.API.GetNode("Main/Bush1")]
    private HidingBush nBush1;
    [LacieEngine.API.GetNode("Main/Bush2")]
    private HidingBush nBush2;
    [LacieEngine.API.GetNode("Main/Bush3")]
    private HidingBush nBush3;
    [LacieEngine.API.GetNode("Main/Bush4")]
    private HidingBush nBush4;
    [LacieEngine.API.GetNode("Main/Bush5")]
    private HidingBush nBush5;
    [LacieEngine.API.GetNode("Main/Bush6")]
    private HidingBush nBush6;
    [LacieEngine.API.GetNode("Main/Bush7")]
    private HidingBush nBush7;
    [LacieEngine.API.GetNode("Main/Bush8")]
    private HidingBush nBush8;
    [LacieEngine.API.GetNode("Main/Bush9")]
    private HidingBush nBush9;
    [LacieEngine.API.GetNode("Main/Bush10")]
    private HidingBush nBush10;
    [LacieEngine.API.GetNode("Main/Bush11")]
    private HidingBush nBush11;
    [LacieEngine.API.GetNode("Main/Bush12")]
    private HidingBush nBush12;
    [LacieEngine.API.GetNode("Main/Bush13")]
    private HidingBush nBush13;
    [LacieEngine.API.GetNode("Main/Bush14")]
    private HidingBush nBush14;
    [LacieEngine.API.GetNode("Main/Bush15")]
    private HidingBush nBush15;
    [LacieEngine.API.GetNode("Main/Fence1Uncut")]
    private Node2D nFence1Uncut;
    [LacieEngine.API.GetNode("Main/Fence1Cut")]
    private Node2D nFence1Cut;
    [LacieEngine.API.GetNode("Main/Fence2Uncut")]
    private Node2D nFence2Uncut;
    [LacieEngine.API.GetNode("Main/Fence2Cut")]
    private Node2D nFence2Cut;
    [LacieEngine.API.GetNode("Ground/FloorPaper")]
    private Node2D nNote;
    [LacieEngine.API.GetNode("Other/Navigation")]
    private Navigation2D nNavigation;
    private PVar varChaseClear = (PVar) "ch1.forest_lockedsite_clear";
    private PVar varFence1Cut = (PVar) "ch1.forest_lockedsite_cut_fence_1";
    private PVar varFence2Cut = (PVar) "ch1.forest_lockedsite_cut_fence_2";
    private PEvent evtDeath = (PEvent) "ch1_death_impact";
    private const float CHASER_SPEED = 1.65f;
    private const float MIN_SPAWN_DISTANCE_SQ = 250000f;
    private const float CLOSE_ENCOUNTER_DISTANCE_SQ = 40000f;
    private const float TENSION_LV_MIN = 0.25f;
    private const float LIGHT_BLUECANDLE_VAL = 0.015f;
    private const float LIGHT_FIREFLIES_VAL = 0.025f;
    private const float LIGHT_CANDLE_VAL = 0.035f;
    private const float LIGHT_FRASHLIGHT_VAL = 0.06f;
    private NPCChaser nChaser;
    private float _tensionLevel;
    private int _closeCalls;
    private AudioStream _currentHeartbeat;
    private bool _chasePaused;
    private float _lightChosenVal;

    public override void _BeforeFadeIn()
    {
      int num1 = (int) Game.StoryPlayer.Connect("DialogueStarted", (Godot.Object) this, "PauseChaser");
      int num2 = (int) Game.StoryPlayer.Connect("DialogueEnded", (Godot.Object) this, "ResumeChaser");
      if (Game.Items.GetOwnedAmount("ch1.candleman_candle_blue") > 0)
      {
        this._lightChosenVal = 0.015f;
        Game.Player.SetLight(this.scnLightCandleBlue.Instance<Light2D>());
        Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
      }
      else if (Game.Items.GetOwnedAmount("ch1.forest_lamp_full") > 0 || Game.Items.GetOwnedAmount("ch1.forest_jar_full") > 0)
      {
        this._lightChosenVal = 0.025f;
        Game.Player.SetLight(this.scnLightFireflies.Instance<Light2D>());
        Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
      }
      else if (Game.Items.GetOwnedAmount("ch1.candleman_candle") > 0)
      {
        this._lightChosenVal = 0.035f;
        Game.Player.SetLight(this.scnLightCandleRed.Instance<Light2D>());
        Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
      }
      else
      {
        this._lightChosenVal = 0.06f;
        Game.Player.SetLight(this.scnLightFlashlight.Instance<Light2D>());
        Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
      }
    }

    public override void _AfterFadeIn()
    {
      this._tensionLevel = 0.25f;
      if (OS.IsDebugBuild())
      {
        Timer timer = GDUtil.MakeNode<Timer>("DebugPrintTensionTimer");
        timer.Autostart = true;
        timer.WaitTime = 1f;
        int num = (int) timer.Connect("timeout", (Godot.Object) this, "DebugPrintTensionLevel");
        this.AddChild((Node) timer);
      }
      if (!this.nChaser.IsValid())
        return;
      this.SpawnEnemy();
    }

    public override void _UpdateRoom()
    {
      this.nFence1Cut.Visible = (bool) this.varFence1Cut.Value;
      this.nFence1Uncut.Visible = !this.nFence1Cut.Visible;
      this.nFence2Cut.Visible = (bool) this.varFence2Cut.Value;
      this.nFence2Uncut.Visible = !this.nFence2Cut.Visible;
    }

    public override void _RoomProcess(float delta)
    {
      if ((bool) this.varChaseClear.Value || this._chasePaused)
        return;
      CharacterState playerState = Game.Player.GetPlayerState();
      float num1;
      switch (playerState)
      {
        case CharacterState.Standing:
          num1 = -0.005f;
          break;
        case CharacterState.Walking:
          num1 = 0.01f;
          break;
        case CharacterState.Running:
          num1 = 0.081f;
          break;
        case CharacterState.Idle:
          num1 = 0.005f;
          break;
        case CharacterState.InObject:
          num1 = -0.07f;
          break;
        default:
          throw new NotImplementedException();
      }
      float num2 = num1;
      if (playerState != CharacterState.InObject && Game.Player.GetLight() != null)
        num2 += Game.Player.GetLight().Visible ? this._lightChosenVal : 0.0f;
      this._tensionLevel += num2 * delta;
      this._tensionLevel = Math.Clamp(this._tensionLevel, 0.25f, 1f);
      if ((double) this._tensionLevel >= 1.0 && !this.nChaser.IsValid())
      {
        Log.Trace((object) "Spawning enemy!");
        this.SpawnEnemy();
      }
      AudioStream track = this.PickHeartbeat(this._tensionLevel);
      if (track == this._currentHeartbeat)
        return;
      this._currentHeartbeat = track;
      if (track != null)
      {
        Game.Audio.PlayAmbiance(track);
        Game.Audio.ChangeBgmVolume(0.65f);
      }
      else
      {
        Game.Audio.StopAmbiance();
        Game.Audio.ChangeBgmVolume(1f);
      }
    }

    public void ActivateBush1() => this.ActivateBush(this.nBush1);

    public void ActivateBush2() => this.ActivateBush(this.nBush2);

    public void ActivateBush3() => this.ActivateBush(this.nBush3);

    public void ActivateBush4() => this.ActivateBush(this.nBush4);

    public void ActivateBush5() => this.ActivateBush(this.nBush5);

    public void ActivateBush6() => this.ActivateBush(this.nBush6);

    public void ActivateBush7() => this.ActivateBush(this.nBush7);

    public void ActivateBush8() => this.ActivateBush(this.nBush8);

    public void ActivateBush9() => this.ActivateBush(this.nBush9);

    public void ActivateBush10() => this.ActivateBush(this.nBush10);

    public void ActivateBush11() => this.ActivateBush(this.nBush11);

    public void ActivateBush12() => this.ActivateBush(this.nBush12);

    public void ActivateBush13() => this.ActivateBush(this.nBush13);

    public void ActivateBush14() => this.ActivateBush(this.nBush14);

    public void ActivateBush15() => this.ActivateBush(this.nBush15);

    public void DebugPrintTensionLevel()
    {
      Log.Trace((object) "Tension Level: ", (object) Math.Round((double) this._tensionLevel * 100.0, 2), (object) "%");
    }

    private void ActivateBush(HidingBush bush)
    {
      if ((bool) this.varChaseClear.Value)
        return;
      bush.SfxBush = this.sfxBush;
      if (this.nChaser.IsValid())
      {
        if ((double) this.nChaser.Position.DistanceSquaredTo(Game.Player.Node.Position) <= 40000.0)
        {
          Log.Trace((object) "Close call!");
          ++this._closeCalls;
          bush.CloseCall = true;
          bush.CloseCallFailureEvent = this.evtDeath.Id;
          if (this._closeCalls > 1 && DrkieUtil.TossCoin())
            bush.CloseCallSuccessEvent = this.evtDeath.Id;
        }
        if ((double) this._tensionLevel >= 1.0 && !bush.CloseCall)
          this._tensionLevel -= 0.05f;
        this.nChaser.Delete();
      }
      bush.Activate();
    }

    private void SpawnEnemy()
    {
      this.nChaser = new NPCChaser("ch1_lockedsite_primal");
      this.nChaser.Position = this.FindSpawnPoint().Position;
      this.nChaser.MoveSpeedMultiplier = 1.65f;
      this.GetMainLayer().AddChild((Node) this.nChaser);
      this.nChaser.SetNavigation(this.nNavigation);
      this.nChaser.Caught += new Action(this.PlayerDeath);
      this.nChaser.Chasing = true;
    }

    private SpawnPoint FindSpawnPoint()
    {
      float num1 = float.MaxValue;
      SpawnPoint spawnPoint1 = (SpawnPoint) null;
      foreach (SpawnPoint spawnPoint2 in Game.Room.RegisteredPoints.Values)
      {
        if (spawnPoint2.Name.StartsWith("enemy_spawn"))
        {
          float num2 = spawnPoint2.Position.DistanceSquaredTo(Game.Player.Node.Position);
          if ((double) num2 < (double) num1 && (double) num2 > 250000.0)
          {
            num1 = num2;
            spawnPoint1 = spawnPoint2;
          }
        }
      }
      return spawnPoint1;
    }

    private AudioStream PickHeartbeat(float tensionLevel)
    {
      if ((double) tensionLevel < 0.60000002384185791)
        return (AudioStream) null;
      return (double) tensionLevel < 0.85000002384185791 ? this.sfxHeartbeatLow : this.sfxHeartbeatHigh;
    }

    public void ChaseClear()
    {
      this.nChaser.DeleteIfValid();
      this._tensionLevel = 0.0f;
      this.varChaseClear.NewValue = (PVar.PVarSetProxy) true;
      Game.Audio.StopAmbiance();
      Game.Objectives.ClearObjectives();
      Game.Objectives.Add("ch1.forest_find_lighthouse");
      Game.Objectives.SilenceNotifications();
    }

    private void PauseChaser()
    {
      this._chasePaused = true;
      if (!this.nChaser.IsValid())
        return;
      this.nChaser.Chasing = false;
    }

    private void ResumeChaser()
    {
      this._chasePaused = false;
      if ((bool) this.varChaseClear.Value || !this.nChaser.IsValid())
        return;
      this.nChaser.Chasing = true;
    }

    public void PlayerDeath()
    {
      this.evtDeath.Play();
      this.SetProcess(false);
    }
  }
}
