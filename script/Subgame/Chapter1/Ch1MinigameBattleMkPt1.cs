// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.Ch1MinigameBattleMkPt1
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Audio;
using LacieEngine.Core;
using LacieEngine.UI;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.Minigames
{
    public class Ch1MinigameBattleMkPt1 : Ch1MinigameBattleMk
    {
        [Export(PropertyHint.None, "")]
        private AudioStream bgmPart0;
        [Export(PropertyHint.None, "")]
        private AudioStream bgmPartA;
        [Export(PropertyHint.None, "")]
        private AudioStream bgmPartB;
        [Export(PropertyHint.None, "")]
        private AudioStream bgmPartC;
        [Export(PropertyHint.None, "")]
        private bool QuickStart;
        [LacieEngine.API.GetNode("Bg")]
        private CanvasItem nBg;
        [LacieEngine.API.GetNode("Fg")]
        private CanvasItem nFg;
        private AdvancedMusicPlayer nMusicPlayer;
        private AdvancedMusicPlayer.AudioInformation introTrack;
        private AdvancedMusicPlayer.AudioInformation partATrack;
        private AdvancedMusicPlayer.AudioInformation partBTrack;
        private AdvancedMusicPlayer.AudioInformation partCTrack;
        private static readonly Color BG_HIDDEN_MODULATE = new Color("303030");

        public override void Init()
        {
            base.Init();
            this.introTrack = new AdvancedMusicPlayer.AudioInformation() {
                Stream = this.bgmPart0,
                LeftAttachPoint = 0.0f,
                RightAttchPoint = 30.429f
            };
            this.partATrack = new AdvancedMusicPlayer.AudioInformation() {
                Stream = this.bgmPartA,
                LeftAttachPoint = 1.785f,
                RightAttchPoint = 59.096f
            };
            this.partBTrack = new AdvancedMusicPlayer.AudioInformation() {
                Stream = this.bgmPartB,
                LeftAttachPoint = 1.82f,
                RightAttchPoint = 30.446f
            };
            this.partCTrack = new AdvancedMusicPlayer.AudioInformation() {
                Stream = this.bgmPartC,
                LeftAttachPoint = 1.788f,
                RightAttchPoint = 30.44f
            };
            this.nMusicPlayer = Game.Screen.GetFromLayer(IScreenManager.Layer.Pixel, "AdvancedMusicPlayer") as AdvancedMusicPlayer;
            if (this.nMusicPlayer.IsValid())
                return;
            this.nMusicPlayer = new AdvancedMusicPlayer();
            Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, (Node)this.nMusicPlayer);
        }

        public override void _Ready()
        {
            if (this.nMusicPlayer.Playing)
            {
                this.nMusicPlayer.ClearQueue();
                this.nMusicPlayer.Queue(this.partBTrack);
            }
            else
            {
                this.nMusicPlayer.Queue(this.partATrack);
                this.nMusicPlayer.Queue(this.partBTrack);
            }
            this.nMusicPlayer.Loop = true;
            if (!this.QuickStart)
            {
                this.SetProcess(false);
                this.SetProcessInput(false);
                this.FightStartSlow();
            }
            else
                this.FightStartQuick();
            double time1 = 2.8;
            this.SlashPlayerPanel(time1);
            double time2 = time1 + 1.0;
            this.ThrowCol(0, time2 + 0.1);
            this.ThrowCol(1, time2);
            this.ThrowCol(2, time2 + 0.1);
            double time3 = time2 + 1.0;
            this.ThrowCol(4, time3 + 0.1);
            this.ThrowCol(5, time3);
            this.ThrowCol(6, time3 + 0.1);
            double time4 = time3 + 1.0;
            this.SlashPlayerPanel(time4);
            double time5 = time4 + 1.0;
            this.ThrowRow(3, time5);
            this.ThrowRow(2, time5 + 0.1);
            double time6 = time5 + 1.0;
            this.ThrowRow(0, time6, true);
            this.ThrowRow(1, time6 + 0.1, true);
            double time7 = time6 + 1.0;
            this.ThrowCol(0, time7);
            this.ThrowCol(1, time7 + 0.1);
            this.ThrowCol(2, time7 + 0.1 + 0.1);
            this.ThrowCol(3, time7 + 0.1 + 0.1 + 0.1);
            this.ThrowCol(4, time7 + 0.1 + 0.1 + 0.1 + 0.1);
            double time8 = time7 + 1.5;
            this.ThrowCol(6, time8);
            this.ThrowCol(5, time8 + 0.1);
            this.ThrowCol(4, time8 + 0.1 + 0.1);
            this.ThrowCol(3, time8 + 0.1 + 0.1 + 0.1);
            this.ThrowCol(2, time8 + 0.1 + 0.1 + 0.1 + 0.1);
            double finalT1 = time8 + 2.0;
            this.SlashMPattern(finalT1, out finalT1);
            this.ThrowRow(0, finalT1);
            this.ThrowRow(1, finalT1 + 0.1);
            double time9 = finalT1 + 1.0;
            this.ThrowRow(2, time9 + 0.1, true);
            this.ThrowRow(3, time9, true);
            double time10 = 16.0;
            this.SlashPlayerPanel(time10);
            double time11 = time10 + 1.0;
            this.ThrowCol(2, time11 + 0.1);
            this.ThrowCol(3, time11);
            this.ThrowCol(4, time11 + 0.1);
            double time12 = time11 + 1.5;
            this.ThrowCol(0, time12 + 0.1);
            this.ThrowCol(1, time12);
            this.ThrowCol(2, time12 + 0.1);
            this.ThrowCol(4, time12 + 0.1);
            this.ThrowCol(5, time12);
            this.ThrowCol(6, time12 + 0.1);
            double time13 = time12 + 1.0;
            this.AttackPlayerPanel(time13);
            this.AttackPlayerPanel(time13 + 0.5);
            this.AttackPlayerPanel(time13 + 0.5 + 0.5);
            this.AttackPlayerPanel(time13 + 0.5 + 0.5 + 0.5);
            double time14 = time13 + 1.0;
            this.ThrowRow(3, time14);
            this.ThrowRow(2, time14 + 0.1);
            double time15 = time14 + 1.0;
            this.ThrowRow(0, time15, true);
            this.ThrowRow(1, time15 + 0.1, true);
            double time16 = time15 + 1.0;
            this.ThrowCol(6, time16);
            this.ThrowCol(5, time16 + 0.1);
            this.ThrowCol(4, time16 + 0.1 + 0.1);
            this.ThrowCol(3, time16 + 0.1 + 0.1 + 0.1);
            this.ThrowCol(2, time16 + 0.1 + 0.1 + 0.1 + 0.1);
            double time17 = time16 + 1.5;
            this.ThrowCol(0, time17);
            this.ThrowCol(1, time17 + 0.1);
            this.ThrowCol(2, time17 + 0.1 + 0.1);
            this.ThrowCol(3, time17 + 0.1 + 0.1 + 0.1);
            this.ThrowCol(4, time17 + 0.1 + 0.1 + 0.1 + 0.1);
            double finalT2 = time17 + 2.0;
            this.SlashMPattern(finalT2, out finalT2);
            this.SlashPanel(5, 0, finalT2);
            this.SlashPanel(3, 0, finalT2);
            this.SlashPanel(5, 2, finalT2);
            this.SlashPanel(3, 2, finalT2);
            this.SlashPanel(1, 0, finalT2 + 1.0);
            this.SlashPanel(3, 0, finalT2 + 1.0);
            this.SlashPanel(1, 2, finalT2 + 1.0);
            this.SlashPanel(3, 2, finalT2 + 1.0);
            double finalT3 = 32.0;
            this.DoubleSlowPattern(finalT3, 5, out finalT3);
            this.SlashPlayerPanel(finalT3 - 0.5);
            this.SlashPlayerPanel(finalT3 + 0.5);
            this.SlashPlayerPanel(finalT3 + 0.5 + 0.5);
            this.SlashPlayerPanel(finalT3 + 0.5 + 0.5 + 0.5);
            this.ThrowRow(3, finalT3);
            this.ThrowRow(2, finalT3 + 0.1);
            double time18 = finalT3 + 1.0;
            this.ThrowRow(0, time18, true);
            this.ThrowRow(1, time18 + 0.1, true);
            double t = time18 + 2.0;
            double finalT4;
            this.HorizontalSlowPattern(t, 8, out finalT4);
            double time19 = t + 1.0;
            this.AttackPlayerPanel(time19);
            double time20 = time19 + 2.0;
            this.AttackPlayerPanel(time20);
            double time21 = time20 + 1.8;
            this.AttackPlayerPanel(time21);
            double time22 = time21 + 1.5;
            this.AttackPlayerPanel(time22);
            double time23 = time22 + 1.0;
            this.AttackPlayerPanel(time23);
            double time24 = time23 + 0.8;
            this.AttackPlayerPanel(time24);
            double time25 = time24 + 0.5;
            this.AttackPlayerPanel(time25);
            double time26 = time25 + 0.5;
            this.AttackPlayerPanel(time26);
            double time27 = time26 + 0.5;
            this.AttackPlayerPanel(time27);
            double num = time27 + 0.5;
            this.AttackPlayerPanel(num);
            this.RandomFallPattern(num, out finalT4);
            double time28 = num + 0.5;
            this.AttackPlayerPanel(time28);
            double time29 = time28 + 0.5;
            this.AttackPlayerPanel(time29);
            double time30 = time29 + 0.5;
            this.AttackPlayerPanel(time30);
            double time31 = time30 + 0.5;
            this.AttackPlayerPanel(time31);
            this.SlashPlayerPanel(time31);
            this.TelegraphAll(time31 + 1.0);
        }

        public override async Task TransitionOut()
        {
            Ch1MinigameBattleMkPt1 baseNode = this;
            if (baseNode._hp > 0)
            {
                Control overlay = (Control)UIUtil.CreateOverlay(Godot.Colors.White);
                baseNode.GetParent().AddChild((Node)overlay);
                FadeInAnimation animation = new FadeInAnimation((CanvasItem)overlay, 1f);
                Game.Animations.Play((LacieAnimation)animation);
                await animation.WaitUntilFinished();
                baseNode.Visible = false;
                Game.Room.Visible = true;
                Game.Variables.SetFlag("ch1.mk_clear_noskip_pt1", true);
                await baseNode.DelaySeconds(1.0);
            }
            else
            {
                baseNode.nMusicPlayer.Stop(0.0f);
                Control overlay = (Control)UIUtil.CreateOverlay(Godot.Colors.Black);
                baseNode.GetParent().AddChild((Node)overlay);
                baseNode.Visible = false;
                Game.Room.Visible = true;
                await baseNode.DelaySeconds(0.1);
            }
        }

        public async void FightStartQuick()
        {
            Ch1MinigameBattleMkPt1 baseNode = this;
            await baseNode.DelaySeconds(1.0);
            baseNode.LowerBgOpacity();
            baseNode.WaitUntilEnd();
        }

        public async void FightStartSlow()
        {
            Ch1MinigameBattleMkPt1 baseNode = this;
            await baseNode.DelaySeconds(1.0);
            baseNode.LowerBgOpacity();
            baseNode.WaitUntilEnd();
        }

        public async void WaitUntilEnd()
        {
            Ch1MinigameBattleMkPt1 minigameBattleMkPt1 = this;
            await minigameBattleMkPt1.DelaySeconds(57.5);
            if (!minigameBattleMkPt1.IsValid())
                return;
            this.Invincible = true;
            minigameBattleMkPt1.RestoreBgOpacity();
            await minigameBattleMkPt1.DelaySeconds(0.5);
            minigameBattleMkPt1.TimeoutTime = 0.0f;
        }

        public void LowerBgOpacity()
        {
            Game.Animations.Play((LacieAnimation)new ModulateAnimation(this.nBg, 0.5f, Godot.Colors.White, Ch1MinigameBattleMkPt1.BG_HIDDEN_MODULATE));
            Game.Animations.Play((LacieAnimation)new ModulateAnimation(this.nFg, 0.5f, Godot.Colors.White, Ch1MinigameBattleMkPt1.BG_HIDDEN_MODULATE));
        }

        public void RestoreBgOpacity()
        {
            Game.Animations.Play((LacieAnimation)new ModulateAnimation(this.nBg, 0.5f, Ch1MinigameBattleMkPt1.BG_HIDDEN_MODULATE, Godot.Colors.White));
            Game.Animations.Play((LacieAnimation)new ModulateAnimation(this.nFg, 0.5f, Ch1MinigameBattleMkPt1.BG_HIDDEN_MODULATE, Godot.Colors.White));
        }
    }
}
