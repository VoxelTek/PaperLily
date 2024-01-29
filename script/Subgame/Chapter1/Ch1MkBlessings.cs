using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Audio;
using LacieEngine.Core;
using LacieEngine.Minigames;
using LacieEngine.Nodes;
using LacieEngine.Subgame.Chapter1;
using LacieEngine.UI;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1MkBlessings : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Material matGhostShake;

		[Export(PropertyHint.None, "")]
		private Material matInvincibility;

		[Export(PropertyHint.None, "")]
		private AudioStream bgmPart0;

		[Export(PropertyHint.None, "")]
		private AudioStream bgmPartA;

		[Export(PropertyHint.None, "")]
		private AudioStream bgmPartB;

		[Export(PropertyHint.None, "")]
		private AudioStream bgmPartC;

		[Export(PropertyHint.None, "")]
		private AudioStream sfxDamage;

		[Export(PropertyHint.None, "")]
		private AudioStream sfxDeath;

		[Export(PropertyHint.None, "")]
		private PackedScene scnHpMeter;

		[GetNode("Other/Navigation")]
		private Navigation2D nNavigation;

		private PEvent evtDeath = "ch1_death_mk_pt2";

		public const float SPAWNTIME_INITIAL = 1f;

		public const float SPAWNTIME_DECREASE_ONDMG = 2f;

		public const float SPAWNTIME_MAX = 1f;

		public const float SPAWNTIME_INCREASE_INITIAL = 5f;

		public const float SPAWNTIME_INCREASE_ACCEL = 0.3f;

		public const float MOVESPEED_INITIAL = 0.65f;

		public const float MOVESPEED_MAX = 1.2f;

		public const float MOVESPEED_INCREASE_INITIAL = 0.02f;

		public const float MOVESPEED_INCREASE_ACCEL = 0.001f;

		public const float MOVESPEED_DECREASE_ONDMG = 0.35f;

		public const float PLAYER_INVULNERABILITY_TIMER = 2f;

		private AdvancedMusicPlayer nMusicPlayer;

		private AdvancedMusicPlayer.AudioInformation introTrack;

		private AdvancedMusicPlayer.AudioInformation partATrack;

		private AdvancedMusicPlayer.AudioInformation partBTrack;

		private AdvancedMusicPlayer.AudioInformation partCTrack;

		private float _elapsed;

		private float _nextSpawnFactor = 5f;

		private float _nextSpawnTime = 1f;

		private float _nextMoveSpeed = 0.65f;

		private float _nextSpeedFactor = 0.02f;

		private string _characterName;

		private PVar varHp;

		private PVar varMaxHp;

		private int _hp;

		private float _invulnerabilityTimer;

		private List<NPCChaser> _chasers = new List<NPCChaser>();

		private Control _flashOverlay;

		private HpMeter _hpMeter;

		public override void _BeforeFadeIn()
		{
			introTrack = new AdvancedMusicPlayer.AudioInformation
			{
				Stream = bgmPart0,
				LeftAttachPoint = 0f,
				RightAttchPoint = 30.429f
			};
			partATrack = new AdvancedMusicPlayer.AudioInformation
			{
				Stream = bgmPartA,
				LeftAttachPoint = 1.785f,
				RightAttchPoint = 59.096f
			};
			partBTrack = new AdvancedMusicPlayer.AudioInformation
			{
				Stream = bgmPartB,
				LeftAttachPoint = 1.82f,
				RightAttchPoint = 30.446f
			};
			partCTrack = new AdvancedMusicPlayer.AudioInformation
			{
				Stream = bgmPartC,
				LeftAttachPoint = 1.788f,
				RightAttchPoint = 30.44f
			};
			nMusicPlayer = Game.Screen.GetFromLayer(IScreenManager.Layer.Pixel, "AdvancedMusicPlayer") as AdvancedMusicPlayer;
			if (!nMusicPlayer.IsValid())
			{
				nMusicPlayer = new AdvancedMusicPlayer();
				Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, nMusicPlayer);
			}
			if (nMusicPlayer.Playing)
			{
				nMusicPlayer.ClearQueue();
				nMusicPlayer.Queue(partCTrack);
			}
			else
			{
				nMusicPlayer.Queue(partBTrack);
				nMusicPlayer.Queue(partCTrack);
			}
			nMusicPlayer.Loop = true;
			Game.State.MenuDisabled = true;
			_characterName = Game.State.Party[0];
			varHp = "general.hp." + _characterName;
			varMaxHp = "general.maxhp." + _characterName;
			_hp = Math.Max(varHp.Value, 1);
			_hpMeter = scnHpMeter.Instance<HpMeter>();
			_flashOverlay = UIUtil.CreateOverlay(Colors.White);
			_flashOverlay.Modulate = Colors.Transparent;
			Game.Screen.AddToLayer(IScreenManager.Layer.UI, _flashOverlay);
			Game.Screen.AddToLayer(IScreenManager.Layer.UI, _hpMeter);
		}

		public override void _BeforeFadeOut()
		{
			if (_hpMeter.IsValid())
			{
				_hpMeter.Delete();
			}
		}

		public override void _RoomProcess(float delta)
		{
			_elapsed += delta;
			if (_elapsed >= _nextSpawnTime)
			{
				SpawnEnemy();
				_nextSpawnTime += _nextSpawnFactor;
				_nextMoveSpeed += _nextSpeedFactor;
				if (_nextSpawnFactor > 1f)
				{
					_nextSpawnFactor -= 0.3f;
				}
				if (_nextSpeedFactor < 1.2f)
				{
					_nextSpeedFactor += 0.001f;
				}
			}
			if (_invulnerabilityTimer > 0f)
			{
				_invulnerabilityTimer -= delta;
				if (_invulnerabilityTimer <= 0f)
				{
					Game.Player.VisualNode.Material = null;
				}
			}
		}

		public void SpawnEnemy()
		{
			NPCChaser nChaser = new Ch1MkBlessingChaser();
			nChaser.Position = Game.Player.Node.Position + new Vector2(350 * (DrkieUtil.TossCoin() ? 1 : (-1)), 250 * (DrkieUtil.TossCoin() ? 1 : (-1)));
			nChaser.DefaultDirection = Direction.Down;
			nChaser.MoveSpeedMultiplier = _nextMoveSpeed;
			GetMainLayer().AddChild(nChaser);
			nChaser.VisualNode.Material = matGhostShake;
			nChaser.SetNavigation(nNavigation);
			nChaser.Caught += PlayerDamage;
			nChaser.Chasing = true;
			nChaser.Teleport();
			_chasers.Add(nChaser);
		}

		public void PlayerDamage()
		{
			if (!(_invulnerabilityTimer > 0f))
			{
				_invulnerabilityTimer = 2f;
				_nextMoveSpeed = Math.Max(_nextMoveSpeed - 0.35f, 0.65f);
				_nextSpawnTime = Math.Max(_nextSpawnTime, _elapsed + 2f);
				ClearChasers();
				varHp.NewValue = (_hp -= 1);
				if (_hp <= 0)
				{
					Game.Audio.PlaySfx(sfxDeath);
					nMusicPlayer.Stop(0f);
					_hpMeter.Visible = false;
					evtDeath.Play();
				}
				else
				{
					Game.Audio.PlaySfx(sfxDamage);
					Game.Player.VisualNode.Material = matInvincibility;
					_hpMeter.UpdateHp();
					_flashOverlay.Modulate = Colors.White;
					Game.Animations.Play(new FadeOutAnimation(_flashOverlay, 0.25f));
				}
			}
		}

		public void ClearChasers()
		{
			List<NPCChaser> chasers = _chasers;
			_chasers = new List<NPCChaser>();
			foreach (NPCChaser item in chasers)
			{
				item.Chasing = false;
				item.QueueFree();
			}
		}

		public void StopChasers()
		{
			_nextSpawnTime = float.MaxValue;
			foreach (NPCChaser chaser in _chasers)
			{
				chaser.Chasing = false;
			}
		}

		public void EndMinigame()
		{
			if (_hpMeter.IsValid())
			{
				_hpMeter.Delete();
			}
		}

		public async void CenterCameraOnMk()
		{
			Game.Camera.Unlock();
			await GDUtil.DelayOneFrame();
			Game.Camera.Node.SmoothingEnabled = true;
			Game.Camera.Node.Position = GetPoint("camera_mk");
		}
	}
}
