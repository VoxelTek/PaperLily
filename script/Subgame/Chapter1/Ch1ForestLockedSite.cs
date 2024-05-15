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
        [API.GetNode("Main/Bush1")]
        private HidingBush nBush1;
        [API.GetNode("Main/Bush2")]
        private HidingBush nBush2;
        [API.GetNode("Main/Bush3")]
        private HidingBush nBush3;
        [API.GetNode("Main/Bush4")]
        private HidingBush nBush4;
        [API.GetNode("Main/Bush5")]
        private HidingBush nBush5;
        [API.GetNode("Main/Bush6")]
        private HidingBush nBush6;
        [API.GetNode("Main/Bush7")]
        private HidingBush nBush7;
        [API.GetNode("Main/Bush8")]
        private HidingBush nBush8;
        [API.GetNode("Main/Bush9")]
        private HidingBush nBush9;
        [API.GetNode("Main/Bush10")]
        private HidingBush nBush10;
        [API.GetNode("Main/Bush11")]
        private HidingBush nBush11;
        [API.GetNode("Main/Bush12")]
        private HidingBush nBush12;
        [API.GetNode("Main/Bush13")]
        private HidingBush nBush13;
        [API.GetNode("Main/Bush14")]
        private HidingBush nBush14;
        [API.GetNode("Main/Bush15")]
        private HidingBush nBush15;
        [API.GetNode("Main/Fence1Uncut")]
        private Node2D nFence1Uncut;
        [API.GetNode("Main/Fence1Cut")]
        private Node2D nFence1Cut;
        [API.GetNode("Main/Fence2Uncut")]
        private Node2D nFence2Uncut;
        [API.GetNode("Main/Fence2Cut")]
        private Node2D nFence2Cut;
        [API.GetNode("Ground/FloorPaper")]
        private Node2D nNote;
        [API.GetNode("Other/Navigation")]
        private Navigation2D nNavigation;
        private PVar varChaseClear = (PVar)"ch1.forest_lockedsite_clear";
        private PVar varFence1Cut = (PVar)"ch1.forest_lockedsite_cut_fence_1";
        private PVar varFence2Cut = (PVar)"ch1.forest_lockedsite_cut_fence_2";
        private PEvent evtDeath = (PEvent)"ch1_death_impact";
        private const float CHASER_SPEED = 1.65f;
        private const int CHASER_HITBOX = 8;
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
            Game.StoryPlayer.Connect("DialogueStarted", this, "PauseChaser");
            Game.StoryPlayer.Connect("DialogueEnded", this, "ResumeChaser");
            if (Game.Items.GetOwnedAmount("ch1.candleman_candle_blue") > 0)
            {
                _lightChosenVal = LIGHT_BLUECANDLE_VAL;
                Game.Player.SetLight(scnLightCandleBlue.Instance<Light2D>());
                Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
            }
            else if (Game.Items.GetOwnedAmount("ch1.forest_lamp_full") > 0 || Game.Items.GetOwnedAmount("ch1.forest_jar_full") > 0)
            {
                _lightChosenVal = LIGHT_FIREFLIES_VAL;
                Game.Player.SetLight(scnLightFireflies.Instance<Light2D>());
                Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
            }
            else if (Game.Items.GetOwnedAmount("ch1.candleman_candle") > 0)
            {
                _lightChosenVal = LIGHT_CANDLE_VAL;
                Game.Player.SetLight(scnLightCandleRed.Instance<Light2D>());
                Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
            }
            else
            {
                _lightChosenVal = LIGHT_FRASHLIGHT_VAL;
                Game.Player.SetLight(scnLightFlashlight.Instance<Light2D>());
                Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
            }
        }

        public override void _AfterFadeIn()
        {
            _tensionLevel = TENSION_LV_MIN;
            if (OS.IsDebugBuild())
            {
                Timer timer = GDUtil.MakeNode<Timer>("DebugPrintTensionTimer");
                timer.Autostart = true;
                timer.WaitTime = 1f;
                timer.Connect("timeout", this, "DebugPrintTensionLevel");
                AddChild(timer);
            }
            if (!nChaser.IsValid())
                return;
            SpawnEnemy();
        }

        public override void _UpdateRoom()
        {
            nFence1Cut.Visible = (bool)varFence1Cut.Value;
            nFence1Uncut.Visible = !nFence1Cut.Visible;
            nFence2Cut.Visible = (bool)varFence2Cut.Value;
            nFence2Uncut.Visible = !nFence2Cut.Visible;
        }

        public override void _RoomProcess(float delta)
        {
            if ((bool)varChaseClear.Value || _chasePaused)
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
            var num2 = num1;
            if (playerState != CharacterState.InObject && Game.Player.GetLight() != null)
                num2 += Game.Player.GetLight().Visible ? _lightChosenVal : 0.0f;
            _tensionLevel += num2 * delta;
            _tensionLevel = Math.Clamp(_tensionLevel, TENSION_LV_MIN, 1f);
            if (_tensionLevel >= 1.0 && !nChaser.IsValid())
            {
                Log.Trace("Spawning enemy!");
                SpawnEnemy();
            }
            var track = PickHeartbeat(_tensionLevel);
            if (track == _currentHeartbeat)
                return;
            _currentHeartbeat = track;
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

        public void ActivateBush1() => ActivateBush(nBush1);

        public void ActivateBush2() => ActivateBush(nBush2);

        public void ActivateBush3() => ActivateBush(nBush3);

        public void ActivateBush4() => ActivateBush(nBush4);

        public void ActivateBush5() => ActivateBush(nBush5);

        public void ActivateBush6() => ActivateBush(nBush6);

        public void ActivateBush7() => ActivateBush(nBush7);

        public void ActivateBush8() => ActivateBush(nBush8);

        public void ActivateBush9() => ActivateBush(nBush9);

        public void ActivateBush10() => ActivateBush(nBush10);

        public void ActivateBush11() => ActivateBush(nBush11);

        public void ActivateBush12() => ActivateBush(nBush12);

        public void ActivateBush13() => ActivateBush(nBush13);

        public void ActivateBush14() => ActivateBush(nBush14);

        public void ActivateBush15() => ActivateBush(nBush15);

        public void DebugPrintTensionLevel()
        {
            Log.Trace("Tension Level: ", Math.Round(_tensionLevel * 100.0, 2), "%");
        }

        private void ActivateBush(HidingBush bush)
        {
            if ((bool)varChaseClear.Value)
                return;
            bush.SfxBush = sfxBush;
            if (nChaser.IsValid())
            {
                if ((double)nChaser.Position.DistanceSquaredTo(Game.Player.Node.Position) <= CLOSE_ENCOUNTER_DISTANCE_SQ)
                {
                    Log.Trace("Close call!");
                    ++_closeCalls;
                    bush.CloseCall = true;
                    bush.CloseCallFailureEvent = evtDeath.Id;
                    if (_closeCalls > 1 && DrkieUtil.TossCoin())
                        bush.CloseCallSuccessEvent = evtDeath.Id;
                }
                if (_tensionLevel >= 1.0 && !bush.CloseCall)
                    _tensionLevel -= 0.05f;
                nChaser.Delete();
            }
            bush.Activate();
        }

        private void SpawnEnemy()
        {
            nChaser = new NPCChaser("ch1_lockedsite_primal") {
                Position = FindSpawnPoint().Position,
                MoveSpeedMultiplier = CHASER_SPEED,
                TouchHitboxRadius = CHASER_HITBOX,
            };
            GetMainLayer().AddChild(nChaser);
            nChaser.SetNavigation(nNavigation);
            nChaser.Caught += new Action(PlayerDeath);
            nChaser.Chasing = true;
        }

        private SpawnPoint FindSpawnPoint()
        {
            var num1 = float.MaxValue;
            SpawnPoint spawnPoint1 = null;
            foreach (var spawnPoint2 in Game.Room.RegisteredPoints.Values)
            {
                if (spawnPoint2.Name.StartsWith("enemy_spawn"))
                {
                    var num2 = spawnPoint2.Position.DistanceSquaredTo(Game.Player.Node.Position);
                    if ((double)num2 < (double)num1 && (double)num2 > MIN_SPAWN_DISTANCE_SQ)
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
            if ((double)tensionLevel < 0.60000002384185791)
                return null;
            return (double)tensionLevel < 0.85000002384185791 ? sfxHeartbeatLow : sfxHeartbeatHigh;
        }

        public void ChaseClear()
        {
            nChaser.DeleteIfValid();
            _tensionLevel = 0.0f;
            varChaseClear.NewValue = (PVar.PVarSetProxy)true;
            Game.Audio.StopAmbiance();
            Game.Objectives.ClearObjectives();
            Game.Objectives.Add("ch1.forest_find_lighthouse");
            Game.Objectives.SilenceNotifications();
        }

        private void PauseChaser()
        {
            _chasePaused = true;
            if (!nChaser.IsValid())
                return;
            nChaser.Chasing = false;
        }

        private void ResumeChaser()
        {
            _chasePaused = false;
            if ((bool)varChaseClear.Value || !nChaser.IsValid())
                return;
            nChaser.Chasing = true;
        }

        public void PlayerDeath()
        {
            evtDeath.Play();
            SetProcess(false);
        }
    }
}
