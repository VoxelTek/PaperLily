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
    // Token: 0x020000FC RID: 252
    [Tool]
    public class Ch1MkBlessings : GameRoom
    {
        // Token: 0x060008A0 RID: 2208 RVA: 0x0002070C File Offset: 0x0001E90C
        public override void _BeforeFadeIn()
        {
            this.introTrack = new AdvancedMusicPlayer.AudioInformation {
                Stream = this.bgmPart0,
                LeftAttachPoint = 0f,
                RightAttchPoint = 30.429f
            };
            this.partATrack = new AdvancedMusicPlayer.AudioInformation {
                Stream = this.bgmPartA,
                LeftAttachPoint = 1.785f,
                RightAttchPoint = 59.096f
            };
            this.partBTrack = new AdvancedMusicPlayer.AudioInformation {
                Stream = this.bgmPartB,
                LeftAttachPoint = 1.82f,
                RightAttchPoint = 30.446f
            };
            this.partCTrack = new AdvancedMusicPlayer.AudioInformation {
                Stream = this.bgmPartC,
                LeftAttachPoint = 1.788f,
                RightAttchPoint = 30.44f
            };
            this.nMusicPlayer = Game.Screen.GetFromLayer(IScreenManager.Layer.Pixel, "AdvancedMusicPlayer") as AdvancedMusicPlayer;
            if (!this.nMusicPlayer.IsValid())
            {
                this.nMusicPlayer = new AdvancedMusicPlayer();
                Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, this.nMusicPlayer);
            }
            if (this.nMusicPlayer.Playing)
            {
                this.nMusicPlayer.ClearQueue();
                this.nMusicPlayer.Queue(this.partCTrack);
            }
            else
            {
                this.nMusicPlayer.Queue(this.partBTrack);
                this.nMusicPlayer.Queue(this.partCTrack);
            }
            this.nMusicPlayer.Loop = true;
            Game.State.MenuDisabled = true;
            this._characterName = Game.State.Party[0];
            this.varHp = "general.hp." + this._characterName;
            this.varMaxHp = "general.maxhp." + this._characterName;
            this._hp = Math.Max(this.varHp.Value, 1);
            this._hpMeter = this.scnHpMeter.Instance<HpMeter>(PackedScene.GenEditState.Disabled);
            this._flashOverlay = UIUtil.CreateOverlay(Colors.White);
            this._flashOverlay.Modulate = Colors.Transparent;
            Game.Screen.AddToLayer(IScreenManager.Layer.UI, this._flashOverlay);
            Game.Screen.AddToLayer(IScreenManager.Layer.UI, this._hpMeter);
        }

        // Token: 0x060008A1 RID: 2209 RVA: 0x00020931 File Offset: 0x0001EB31
        public override void _BeforeFadeOut()
        {
            if (this._hpMeter.IsValid())
            {
                this._hpMeter.Delete();
            }
        }

        // Token: 0x060008A2 RID: 2210 RVA: 0x0002094C File Offset: 0x0001EB4C
        public override void _RoomProcess(float delta)
        {
            this._elapsed += delta;
            if (this._elapsed >= this._nextSpawnTime)
            {
                this.SpawnEnemy();
                this._nextSpawnTime += this._nextSpawnFactor;
                this._nextMoveSpeed += this._nextSpeedFactor;
                if (this._nextSpawnFactor > 1f)
                {
                    this._nextSpawnFactor -= 0.3f;
                }
                if (this._nextSpeedFactor < 1.2f)
                {
                    this._nextSpeedFactor += 0.001f;
                }
            }
            if (this._invulnerabilityTimer > 0f)
            {
                this._invulnerabilityTimer -= delta;
                if (this._invulnerabilityTimer <= 0f)
                {
                    Game.Player.VisualNode.Material = null;
                }
            }
        }

        // Token: 0x060008A3 RID: 2211 RVA: 0x00020A18 File Offset: 0x0001EC18
        public void SpawnEnemy()
        {
            NPCChaser nChaser = new Ch1MkBlessingChaser();
            nChaser.Position = Game.Player.Node.Position + new Vector2((float)(350 * (DrkieUtil.TossCoin() ? 1 : (-1))), (float)(250 * (DrkieUtil.TossCoin() ? 1 : (-1))));
            nChaser.DefaultDirection = Direction.Down;
            nChaser.MoveSpeedMultiplier = this._nextMoveSpeed;
            this.GetMainLayer().AddChild(nChaser, false);
            nChaser.VisualNode.Material = this.matGhostShake;
            nChaser.SetNavigation(this.nNavigation);
            nChaser.Caught += this.PlayerDamage;
            nChaser.Chasing = true;
            nChaser.Teleport();
            this._chasers.Add(nChaser);
        }

        // Token: 0x060008A4 RID: 2212 RVA: 0x00020ADC File Offset: 0x0001ECDC
        public void PlayerDamage()
        {
            if (this._invulnerabilityTimer > 0f)
            {
                return;
            }
            this._invulnerabilityTimer = 2f;
            this._nextMoveSpeed = Math.Max(this._nextMoveSpeed - 0.35f, 0.65f);
            this._nextSpawnTime = Math.Max(this._nextSpawnTime, this._elapsed + 2f);
            this.ClearChasers();
            PVar pvar = this.varHp;
            int num = this._hp - 1;
            this._hp = num;
            pvar.NewValue = num;
            if (this._hp <= 0)
            {
                Game.Audio.PlaySfx(this.sfxDeath);
                this.nMusicPlayer.Stop(0f);
                this._hpMeter.Visible = false;
                this.evtDeath.Play();
                return;
            }
            Game.Audio.PlaySfx(this.sfxDamage);
            Game.Player.VisualNode.Material = this.matInvincibility;
            this._hpMeter.UpdateHp();
            this._flashOverlay.Modulate = Colors.White;
            Game.Animations.Play(new FadeOutAnimation(this._flashOverlay, 0.25f));
        }

        // Token: 0x060008A5 RID: 2213 RVA: 0x00020C00 File Offset: 0x0001EE00
        public void ClearChasers()
        {
            List<NPCChaser> chasers = this._chasers;
            this._chasers = new List<NPCChaser>();
            foreach (NPCChaser npcchaser in chasers)
            {
                npcchaser.Chasing = false;
                npcchaser.QueueFree();
            }
        }

        // Token: 0x060008A6 RID: 2214 RVA: 0x00020C64 File Offset: 0x0001EE64
        public void StopChasers()
        {
            this._nextSpawnTime = float.MaxValue;
            foreach (NPCChaser npcchaser in this._chasers)
            {
                npcchaser.Chasing = false;
            }
        }

        // Token: 0x060008A7 RID: 2215 RVA: 0x00020CC0 File Offset: 0x0001EEC0
        public void EndMinigame()
        {
            if (this._hpMeter.IsValid())
            {
                this._hpMeter.Delete();
            }
        }

        // Token: 0x060008A8 RID: 2216 RVA: 0x00020CDC File Offset: 0x0001EEDC
        public async void CenterCameraOnMk()
        {
            Game.Camera.Unlock();
            await GDUtil.DelayOneFrame();
            Game.Camera.Node.SmoothingEnabled = true;
            Game.Camera.Node.Position = this.GetPoint("camera_mk");
        }

        // Token: 0x0400070B RID: 1803
        [Export(PropertyHint.None, "")]
        private Material matGhostShake;

        // Token: 0x0400070C RID: 1804
        [Export(PropertyHint.None, "")]
        private Material matInvincibility;

        // Token: 0x0400070D RID: 1805
        [Export(PropertyHint.None, "")]
        private AudioStream bgmPart0;

        // Token: 0x0400070E RID: 1806
        [Export(PropertyHint.None, "")]
        private AudioStream bgmPartA;

        // Token: 0x0400070F RID: 1807
        [Export(PropertyHint.None, "")]
        private AudioStream bgmPartB;

        // Token: 0x04000710 RID: 1808
        [Export(PropertyHint.None, "")]
        private AudioStream bgmPartC;

        // Token: 0x04000711 RID: 1809
        [Export(PropertyHint.None, "")]
        private AudioStream sfxDamage;

        // Token: 0x04000712 RID: 1810
        [Export(PropertyHint.None, "")]
        private AudioStream sfxDeath;

        // Token: 0x04000713 RID: 1811
        [Export(PropertyHint.None, "")]
        private PackedScene scnHpMeter;

        // Token: 0x04000714 RID: 1812
        [GetNode("Other/Navigation")]
        private Navigation2D nNavigation;

        // Token: 0x04000715 RID: 1813
        private PEvent evtDeath = "ch1_death_mk_pt2";

        // Token: 0x04000716 RID: 1814
        public const float SPAWNTIME_INITIAL = 1f;

        // Token: 0x04000717 RID: 1815
        public const float SPAWNTIME_DECREASE_ONDMG = 2f;

        // Token: 0x04000718 RID: 1816
        public const float SPAWNTIME_MAX = 1f;

        // Token: 0x04000719 RID: 1817
        public const float SPAWNTIME_INCREASE_INITIAL = 5f;

        // Token: 0x0400071A RID: 1818
        public const float SPAWNTIME_INCREASE_ACCEL = 0.3f;

        // Token: 0x0400071B RID: 1819
        public const float MOVESPEED_INITIAL = 0.65f;

        // Token: 0x0400071C RID: 1820
        public const float MOVESPEED_MAX = 1.2f;

        // Token: 0x0400071D RID: 1821
        public const float MOVESPEED_INCREASE_INITIAL = 0.02f;

        // Token: 0x0400071E RID: 1822
        public const float MOVESPEED_INCREASE_ACCEL = 0.001f;

        // Token: 0x0400071F RID: 1823
        public const float MOVESPEED_DECREASE_ONDMG = 0.35f;

        // Token: 0x04000720 RID: 1824
        public const float PLAYER_INVULNERABILITY_TIMER = 2f;

        // Token: 0x04000721 RID: 1825
        private AdvancedMusicPlayer nMusicPlayer;

        // Token: 0x04000722 RID: 1826
        private AdvancedMusicPlayer.AudioInformation introTrack;

        // Token: 0x04000723 RID: 1827
        private AdvancedMusicPlayer.AudioInformation partATrack;

        // Token: 0x04000724 RID: 1828
        private AdvancedMusicPlayer.AudioInformation partBTrack;

        // Token: 0x04000725 RID: 1829
        private AdvancedMusicPlayer.AudioInformation partCTrack;

        // Token: 0x04000726 RID: 1830
        private float _elapsed;

        // Token: 0x04000727 RID: 1831
        private float _nextSpawnFactor = 5f;

        // Token: 0x04000728 RID: 1832
        private float _nextSpawnTime = 1f;

        // Token: 0x04000729 RID: 1833
        private float _nextMoveSpeed = 0.65f;

        // Token: 0x0400072A RID: 1834
        private float _nextSpeedFactor = 0.02f;

        // Token: 0x0400072B RID: 1835
        private string _characterName;

        // Token: 0x0400072C RID: 1836
        private PVar varHp;

        // Token: 0x0400072D RID: 1837
        private PVar varMaxHp;

        // Token: 0x0400072E RID: 1838
        private int _hp;

        // Token: 0x0400072F RID: 1839
        private float _invulnerabilityTimer;

        // Token: 0x04000730 RID: 1840
        private List<NPCChaser> _chasers = new List<NPCChaser>();

        // Token: 0x04000731 RID: 1841
        private Control _flashOverlay;

        // Token: 0x04000732 RID: 1842
        private HpMeter _hpMeter;
    }
}
