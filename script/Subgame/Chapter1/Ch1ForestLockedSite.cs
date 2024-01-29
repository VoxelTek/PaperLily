using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

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

		[GetNode("Main/Bush1")]
		private HidingBush nBush1;

		[GetNode("Main/Bush2")]
		private HidingBush nBush2;

		[GetNode("Main/Bush3")]
		private HidingBush nBush3;

		[GetNode("Main/Bush4")]
		private HidingBush nBush4;

		[GetNode("Main/Bush5")]
		private HidingBush nBush5;

		[GetNode("Main/Bush6")]
		private HidingBush nBush6;

		[GetNode("Main/Bush7")]
		private HidingBush nBush7;

		[GetNode("Main/Bush8")]
		private HidingBush nBush8;

		[GetNode("Main/Bush9")]
		private HidingBush nBush9;

		[GetNode("Main/Bush10")]
		private HidingBush nBush10;

		[GetNode("Main/Bush11")]
		private HidingBush nBush11;

		[GetNode("Main/Bush12")]
		private HidingBush nBush12;

		[GetNode("Main/Bush13")]
		private HidingBush nBush13;

		[GetNode("Main/Bush14")]
		private HidingBush nBush14;

		[GetNode("Main/Bush15")]
		private HidingBush nBush15;

		[GetNode("Main/Fence1Uncut")]
		private Node2D nFence1Uncut;

		[GetNode("Main/Fence1Cut")]
		private Node2D nFence1Cut;

		[GetNode("Main/Fence2Uncut")]
		private Node2D nFence2Uncut;

		[GetNode("Main/Fence2Cut")]
		private Node2D nFence2Cut;

		[GetNode("Ground/FloorPaper")]
		private Node2D nNote;

		[GetNode("Other/Navigation")]
		private Navigation2D nNavigation;

		private PVar varChaseClear = "ch1.forest_lockedsite_clear";

		private PVar varFence1Cut = "ch1.forest_lockedsite_cut_fence_1";

		private PVar varFence2Cut = "ch1.forest_lockedsite_cut_fence_2";

		private PEvent evtDeath = "ch1_death_impact";

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
			Game.StoryPlayer.Connect("DialogueStarted", this, "PauseChaser");
			Game.StoryPlayer.Connect("DialogueEnded", this, "ResumeChaser");
			if (Game.Items.GetOwnedAmount("ch1.candleman_candle_blue") > 0)
			{
				_lightChosenVal = 0.015f;
				Game.Player.SetLight(scnLightCandleBlue.Instance<Light2D>());
				Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
			}
			else if (Game.Items.GetOwnedAmount("ch1.forest_lamp_full") > 0 || Game.Items.GetOwnedAmount("ch1.forest_jar_full") > 0)
			{
				_lightChosenVal = 0.025f;
				Game.Player.SetLight(scnLightFireflies.Instance<Light2D>());
				Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
			}
			else if (Game.Items.GetOwnedAmount("ch1.candleman_candle") > 0)
			{
				_lightChosenVal = 0.035f;
				Game.Player.SetLight(scnLightCandleRed.Instance<Light2D>());
				Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
			}
			else
			{
				_lightChosenVal = 0.06f;
				Game.Player.SetLight(scnLightFlashlight.Instance<Light2D>());
				Game.Player.GetLight().Mode = Light2D.ModeEnum.Mix;
			}
		}

		public override void _AfterFadeIn()
		{
			_tensionLevel = 0.25f;
			if (OS.IsDebugBuild())
			{
				Timer debugPrintTimer = GDUtil.MakeNode<Timer>("DebugPrintTensionTimer");
				debugPrintTimer.Autostart = true;
				debugPrintTimer.WaitTime = 1f;
				debugPrintTimer.Connect("timeout", this, "DebugPrintTensionLevel");
				AddChild(debugPrintTimer);
			}
			if (nChaser.IsValid())
			{
				SpawnEnemy();
			}
		}

		public override void _UpdateRoom()
		{
			nFence1Cut.Visible = varFence1Cut.Value;
			nFence1Uncut.Visible = !nFence1Cut.Visible;
			nFence2Cut.Visible = varFence2Cut.Value;
			nFence2Uncut.Visible = !nFence2Cut.Visible;
		}

		public override void _RoomProcess(float delta)
		{
			if ((bool)varChaseClear.Value || _chasePaused)
			{
				return;
			}
			CharacterState state = Game.Player.GetPlayerState();
			float val = state switch
			{
				CharacterState.Standing => -0.005f, 
				CharacterState.Walking => 0.01f, 
				CharacterState.Running => 0.081f, 
				CharacterState.Idle => 0.005f, 
				CharacterState.InObject => -0.07f, 
				_ => throw new NotImplementedException(), 
			};
			if (state != CharacterState.InObject && Game.Player.GetLight() != null)
			{
				val += (Game.Player.GetLight().Visible ? _lightChosenVal : 0f);
			}
			_tensionLevel += val * delta;
			_tensionLevel = Math.Clamp(_tensionLevel, 0.25f, 1f);
			if (_tensionLevel >= 1f && !nChaser.IsValid())
			{
				Log.Trace("Spawning enemy!");
				SpawnEnemy();
			}
			AudioStream newHeartbeat = PickHeartbeat(_tensionLevel);
			if (newHeartbeat != _currentHeartbeat)
			{
				_currentHeartbeat = newHeartbeat;
				if (newHeartbeat != null)
				{
					Game.Audio.PlayAmbiance(newHeartbeat);
					Game.Audio.ChangeBgmVolume(0.65f);
				}
				else
				{
					Game.Audio.StopAmbiance();
					Game.Audio.ChangeBgmVolume(1f);
				}
			}
		}

		public void ActivateBush1()
		{
			ActivateBush(nBush1);
		}

		public void ActivateBush2()
		{
			ActivateBush(nBush2);
		}

		public void ActivateBush3()
		{
			ActivateBush(nBush3);
		}

		public void ActivateBush4()
		{
			ActivateBush(nBush4);
		}

		public void ActivateBush5()
		{
			ActivateBush(nBush5);
		}

		public void ActivateBush6()
		{
			ActivateBush(nBush6);
		}

		public void ActivateBush7()
		{
			ActivateBush(nBush7);
		}

		public void ActivateBush8()
		{
			ActivateBush(nBush8);
		}

		public void ActivateBush9()
		{
			ActivateBush(nBush9);
		}

		public void ActivateBush10()
		{
			ActivateBush(nBush10);
		}

		public void ActivateBush11()
		{
			ActivateBush(nBush11);
		}

		public void ActivateBush12()
		{
			ActivateBush(nBush12);
		}

		public void ActivateBush13()
		{
			ActivateBush(nBush13);
		}

		public void ActivateBush14()
		{
			ActivateBush(nBush14);
		}

		public void ActivateBush15()
		{
			ActivateBush(nBush15);
		}

		public void DebugPrintTensionLevel()
		{
			Log.Trace("Tension Level: ", Math.Round(_tensionLevel * 100f, 2), "%");
		}

		private void ActivateBush(HidingBush bush)
		{
			if ((bool)varChaseClear.Value)
			{
				return;
			}
			bush.SfxBush = sfxBush;
			if (nChaser.IsValid())
			{
				if (nChaser.Position.DistanceSquaredTo(Game.Player.Node.Position) <= 40000f)
				{
					Log.Trace("Close call!");
					_closeCalls++;
					bush.CloseCall = true;
					bush.CloseCallFailureEvent = evtDeath.Id;
					if (_closeCalls > 1 && DrkieUtil.TossCoin())
					{
						bush.CloseCallSuccessEvent = evtDeath.Id;
					}
				}
				if (_tensionLevel >= 1f && !bush.CloseCall)
				{
					_tensionLevel -= 0.05f;
				}
				nChaser.Delete();
			}
			bush.Activate();
		}

		private void SpawnEnemy()
		{
			nChaser = new NPCChaser("ch1_lockedsite_primal");
			SpawnPoint spawn = FindSpawnPoint();
			nChaser.Position = spawn.Position;
			nChaser.MoveSpeedMultiplier = 1.65f;
			GetMainLayer().AddChild(nChaser);
			nChaser.SetNavigation(nNavigation);
			nChaser.Caught += PlayerDeath;
			nChaser.Chasing = true;
		}

		private SpawnPoint FindSpawnPoint()
		{
			float selectedDistanceSq = float.MaxValue;
			SpawnPoint selectedPoint = null;
			foreach (SpawnPoint point in Game.Room.RegisteredPoints.Values)
			{
				if (point.Name.StartsWith("enemy_spawn"))
				{
					float distanceSq = point.Position.DistanceSquaredTo(Game.Player.Node.Position);
					if (distanceSq < selectedDistanceSq && distanceSq > 250000f)
					{
						selectedDistanceSq = distanceSq;
						selectedPoint = point;
					}
				}
			}
			return selectedPoint;
		}

		private AudioStream PickHeartbeat(float tensionLevel)
		{
			if (tensionLevel < 0.6f)
			{
				return null;
			}
			if (tensionLevel < 0.85f)
			{
				return sfxHeartbeatLow;
			}
			return sfxHeartbeatHigh;
		}

		public void ChaseClear()
		{
			nChaser.DeleteIfValid();
			_tensionLevel = 0f;
			varChaseClear.NewValue = true;
			Game.Audio.StopAmbiance();
			Game.Objectives.ClearObjectives();
			Game.Objectives.Add("ch1.forest_find_lighthouse");
			Game.Objectives.SilenceNotifications();
		}

		private void PauseChaser()
		{
			_chasePaused = true;
			if (nChaser.IsValid())
			{
				nChaser.Chasing = false;
			}
		}

		private void ResumeChaser()
		{
			_chasePaused = false;
			if (!varChaseClear.Value && nChaser.IsValid())
			{
				nChaser.Chasing = true;
			}
		}

		public void PlayerDeath()
		{
			evtDeath.Play();
			SetProcess(enable: false);
		}
	}
}
