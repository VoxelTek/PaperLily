// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1MkChasePainting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using System;

#nullable disable
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
        [LacieEngine.API.GetNode("Other/Navigation")]
        private Navigation2D nNavigation;
        private PVar varTookEyeLady = (PVar)"ch1.mk_took_eye_lady";
        private PVar varHurtLady = (PVar)"ch1.mk_hurt_lady";
        private PEvent evtDeath = (PEvent)"event_death";
        private NPCChaser nChaser;
        private const float CHASER_SPEED = 1.72f;
        private const float CHASER_SPEED_HURT = 1.35f;
        private const int CHASER_HITBOX_RADIUS = 16;

        public override void _BeforeFadeIn() => this.SpawnEnemy();

        public override void _AfterFadeIn() => this.nChaser.Chasing = true;

        private void SpawnEnemy()
        {
            this.nChaser = new NPCChaser("ch1_mk_painting");
            SpawnPoint spawnPoint = this.GetSpawnPoint("enemy_spawn");
            this.nChaser.Position = spawnPoint.Position;
            this.nChaser.MoveSpeedMultiplier = (bool)this.varHurtLady.Value ? 1.35f : 1.72f;
            this.nChaser.TouchHitboxRadius = 16;
            Texture texture = this.texPaintNormal;
            if ((bool)this.varTookEyeLady.Value)
                texture = this.texPaintNoEye;
            else if ((bool)this.varHurtLady.Value)
                texture = this.texPaintHurt;
            this.GetMainLayer().AddChild((Node)this.nChaser);
            this.nChaser.OverrideTextureForState("walk", texture);
            this.nChaser.SetNavigation(this.nNavigation);
            this.nChaser.Caught += new Action(this.PlayerDeath);
            this.nChaser.Turn((Direction)spawnPoint.Direction);
        }

        public void PlayerDeath()
        {
            this.evtDeath.Play();
            this.SetProcess(false);
        }

        private void PauseChaser()
        {
            if (!this.nChaser.IsValid())
                return;
            this.nChaser.Chasing = false;
        }
    }
}
