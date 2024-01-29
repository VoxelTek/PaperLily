using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1MkChasePainting : GameRoom
	{
		[Export(PropertyHint.None, "")]
		private Texture texPaintNormal;

		[Export(PropertyHint.None, "")]
		private Texture texPaintHurt;

		[Export(PropertyHint.None, "")]
		private Texture texPaintNoEye;

		[GetNode("Other/Navigation")]
		private Navigation2D nNavigation;

		private PVar varTookEyeLady = "ch1.mk_took_eye_lady";

		private PVar varHurtLady = "ch1.mk_hurt_lady";

		private PEvent evtDeath = "ch1_death_impact";

		private NPCChaser nChaser;

		private const float CHASER_SPEED = 1.72f;

		private const float CHASER_SPEED_HURT = 1.35f;

		private const int CHASER_HITBOX_RADIUS = 16;

		public override void _BeforeFadeIn()
		{
			SpawnEnemy();
		}

		public override void _AfterFadeIn()
		{
			nChaser.Chasing = true;
		}

		private void SpawnEnemy()
		{
			nChaser = new NPCChaser("ch1_mk_painting");
			SpawnPoint spawn = GetSpawnPoint("enemy_spawn");
			nChaser.Position = spawn.Position;
			nChaser.MoveSpeedMultiplier = (varHurtLady.Value ? 1.35f : 1.72f);
			nChaser.TouchHitboxRadius = 16;
			Texture tex = texPaintNormal;
			if ((bool)varTookEyeLady.Value)
			{
				tex = texPaintNoEye;
			}
			else if ((bool)varHurtLady.Value)
			{
				tex = texPaintHurt;
			}
			GetMainLayer().AddChild(nChaser);
			nChaser.OverrideTextureForState("walk", tex);
			nChaser.SetNavigation(nNavigation);
			nChaser.Caught += PlayerDeath;
			nChaser.Turn(spawn.Direction);
		}

		public void PlayerDeath()
		{
			evtDeath.Play();
			SetProcess(enable: false);
		}

		private void PauseChaser()
		{
			if (nChaser.IsValid())
			{
				nChaser.Chasing = false;
			}
		}
	}
}
