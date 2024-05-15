// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.Ch1MinigameBattleMkPt2
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
    public class Ch1MinigameBattleMkPt2 : Ch1MinigameBattleMk
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
        private AudioStream bgmPartD;
        [LacieEngine.API.GetNode("Bg")]
        private CanvasItem nBg;
        [LacieEngine.API.GetNode("Fg")]
        private CanvasItem nFg;
        private AdvancedMusicPlayer nMusicPlayer;
        private AdvancedMusicPlayer.AudioInformation introTrack;
        private AdvancedMusicPlayer.AudioInformation partATrack;
        private AdvancedMusicPlayer.AudioInformation partBTrack;
        private AdvancedMusicPlayer.AudioInformation partCTrack;
        private AdvancedMusicPlayer.AudioInformation partDTrack;
        private PEvent evtOutro = (PEvent)"ch1_event_mk_battle_outro";
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
            this.partDTrack = new AdvancedMusicPlayer.AudioInformation() {
                Stream = this.bgmPartD,
                LeftAttachPoint = 1.785f,
                RightAttchPoint = 59.096f
            };
            this.nMusicPlayer = Game.Screen.GetFromLayer(IScreenManager.Layer.Pixel, "AdvancedMusicPlayer") as AdvancedMusicPlayer;
            if (this.nMusicPlayer.IsValid())
                return;
            this.nMusicPlayer = new AdvancedMusicPlayer();
            Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, (Node)this.nMusicPlayer);
        }

        public override void _Ready()
        {
            this.nMusicPlayer.ClearQueue();
            this.nMusicPlayer.Queue(this.partDTrack);
            this.nMusicPlayer.Loop = true;
            this.FightStartQuick();
            double num = 2.8;
            base.AttackPlayerPanel(num);
            base.AttackPlayerPanel(num + 0.5);
            base.AttackPlayerPanel(num + 0.5 + 0.5);
            base.AttackPlayerPanel(num + 0.5 + 0.5 + 0.5);
            base.AttackPlayerPanel(num + 0.5 + 0.5 + 0.5 + 0.5);
            base.AttackPlayerPanel(num + 0.5 + 0.5 + 0.5 + 0.5 + 0.5);
            double num2;
            base.RandomFallPattern(num, out num2);
            num += 3.0;
            base.AttackPlayerPanel(num);
            base.AttackPlayerPanel(num + 0.5);
            base.AttackPlayerPanel(num + 0.5 + 0.5);
            base.AttackPlayerPanel(num + 0.5 + 0.5 + 0.5);
            base.AttackPlayerPanel(num + 0.5 + 0.5 + 0.5 + 0.5);
            base.AttackPlayerPanel(num + 0.5 + 0.5 + 0.5 + 0.5 + 0.5);
            base.RandomFallPattern(num, out num2);
            num += 3.0;
            base.AttackPlayerPanel(num);
            base.AttackPlayerPanel(num + 0.5);
            base.AttackPlayerPanel(num + 0.5 + 0.5);
            base.AttackPlayerPanel(num + 0.5 + 0.5 + 0.5);
            base.RandomFallPattern(num, out num2);
            num += 4.0;
            base.BombPanel(3, 1, num);
            num += 3.0;
            base.BombPanel(2, 0, num);
            base.BombPanel(4, 0, num);
            num += 2.5;
            base.BombPanel(1, 1, num);
            base.BombPanel(5, 2, num);
            num += 2.5;
            base.BombPanel(0, 0, num);
            base.BombPanel(6, 0, num);
            base.BombPanel(0, 3, num);
            base.BombPanel(6, 3, num);
            base.BurnCol(0, num + 0.1);
            base.BurnCol(6, num + 0.1);
            num += 3.0;
            base.HorizontalSlowPattern(num, 4, out num2, true);
            base.ThrowCol(5, num, false);
            base.ThrowCol(4, num + 0.1, false);
            base.ThrowCol(3, num + 0.1 + 0.1, false);
            base.ThrowCol(2, num + 0.1 + 0.1 + 0.1, false);
            num += 1.5;
            base.ThrowCol(5, num, false);
            base.ThrowCol(4, num + 0.1, false);
            base.ThrowCol(3, num + 0.1 + 0.1, false);
            base.ThrowCol(2, num + 0.1 + 0.1 + 0.1, false);
            num += 1.5;
            base.ThrowCol(5, num, false);
            base.ThrowCol(4, num + 0.1, false);
            base.ThrowCol(3, num + 0.1 + 0.1, false);
            base.ThrowCol(2, num + 0.1 + 0.1 + 0.1, false);
            num += 1.5;
            base.ThrowCol(5, num, false);
            base.ThrowCol(4, num + 0.1, false);
            base.ThrowCol(3, num + 0.1 + 0.1, false);
            base.ThrowCol(2, num + 0.1 + 0.1 + 0.1, false);
            num += 2.5;
            base.HorizontalSlowPattern(num, 4, out num2, false);
            base.ThrowCol(1, num, false);
            base.ThrowCol(2, num + 0.1, false);
            base.ThrowCol(3, num + 0.1 + 0.1, false);
            base.ThrowCol(4, num + 0.1 + 0.1 + 0.1, false);
            num += 1.5;
            base.ThrowCol(1, num, false);
            base.ThrowCol(2, num + 0.1, false);
            base.ThrowCol(3, num + 0.1 + 0.1, false);
            base.ThrowCol(4, num + 0.1 + 0.1 + 0.1, false);
            num += 1.5;
            base.ThrowCol(1, num, false);
            base.ThrowCol(2, num + 0.1, false);
            base.ThrowCol(3, num + 0.1 + 0.1, false);
            base.ThrowCol(4, num + 0.1 + 0.1 + 0.1, false);
            num += 1.5;
            base.ThrowCol(1, num, false);
            base.ThrowCol(2, num + 0.1, false);
            base.ThrowCol(3, num + 0.1 + 0.1, false);
            base.ThrowCol(4, num + 0.1 + 0.1 + 0.1, false);
            num += 2.0;
            base.BombPanel(5, 0, num);
            base.BombPanel(1, 0, num);
            num += 2.0;
            base.BombPanel(3, 0, num);
            base.BombPanel(2, 3, num);
            num += 2.0;
            base.BombPanel(3, 3, num);
            base.BombPanel(4, 0, num);
            num += 2.0;
            base.BombPanel(4, 1, num);
            base.BombPanel(2, 2, num);
            num += 1.0;
            base.BombPanel(2, 1, num);
            base.BombPanel(4, 2, num);
            num += 2.5;
            base.BombPanel(1, 0, num);
            base.BombPanel(1, 3, num);
            base.BombPanel(5, 0, num);
            base.BombPanel(5, 3, num);
            base.BurnCol(1, num + 0.1);
            base.BurnCol(5, num + 0.1);
            num += 3.0;
            base.HorizontalSlowPatternDouble(num, 6, out num);
            num += 2.0;
            base.BombPanel(3, 0, num);
            base.BombPanel(3, 3, num);
            num += 2.0;
            base.BombPanel(2, 1, num);
            base.BombPanel(4, 2, num);
            num += 2.0;
            base.BombPanel(2, 0, num);
            base.BombPanel(2, 3, num);
            base.BombPanel(4, 0, num);
            base.BombPanel(4, 3, num);
            base.BurnPanel(2, 0, num + 0.1);
            base.BurnPanel(3, 0, num + 0.1);
            base.BurnPanel(4, 0, num + 0.1);
            base.BurnPanel(2, 3, num + 0.1);
            base.BurnPanel(3, 3, num + 0.1);
            base.BurnPanel(4, 3, num + 0.1);
            num += 3.0;
            base.SlashPanel(2, 1, num);
            base.SlashPanel(3, 2, num + 1.0);
            base.SlashPanel(4, 1, num + 1.0 + 1.0);
            base.SlashPanel(3, 0, num + 1.0 + 1.0 + 1.0);
            num += 4.5;
            base.BombPanel(2, 1, num);
            base.BombPanel(4, 1, num);
            base.MakeTelegraph(2, 2, num);
            base.MakeTelegraph(3, 1, num);
            base.MakeTelegraph(4, 2, num);
            base.BurnPanel(2, 1, num + 0.1);
            base.BurnPanel(2, 2, num + 0.1);
            base.BurnPanel(4, 1, num + 0.1);
            base.BurnPanel(4, 2, num + 0.1);
            num += 2.0;
            base.AttackPlayerPanel(num);
            base.AttackPlayerPanel(num + 1.0);
            base.AttackPlayerPanel(num + 1.0 + 1.0);
            base.AttackPlayerPanel(num + 1.0 + 1.0 + 1.0);
            base.AttackPlayerPanel(num + 1.0 + 1.0 + 1.0 + 0.9);
            base.AttackPlayerPanel(num + 1.0 + 1.0 + 1.0 + 0.9 + 0.9);
            base.AttackPlayerPanel(num + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8);
            base.AttackPlayerPanel(num + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8);
            base.AttackPlayerPanel(num + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7);
            base.AttackPlayerPanel(num + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7 + 0.7);
            base.AttackPlayerPanel(num + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7 + 0.7 + 0.6);
            base.AttackPlayerPanel(num + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7 + 0.7 + 0.6 + 0.6);
        }

        public override async Task TransitionOut()
        {
            Ch1MinigameBattleMkPt2 baseNode = this;
            if (baseNode._hp > 0)
            {
                Control overlay = (Control)UIUtil.CreateOverlay(Godot.Colors.White);
                baseNode.GetParent().AddChild((Node)overlay);
                FadeInAnimation animation = new FadeInAnimation((CanvasItem)overlay, 4f);
                Game.Animations.Play((LacieAnimation)animation);
                await animation.WaitUntilFinished();
                baseNode.Visible = false;
                Game.Room.Visible = true;
                Game.Variables.SetFlag("ch1.mk_clear_noskip_pt3", true);
                await baseNode.DelaySeconds(1.0);
                baseNode.nMusicPlayer.Delete();
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
            Ch1MinigameBattleMkPt2 baseNode = this;
            await baseNode.DelaySeconds(1.0);
            baseNode.LowerBgOpacity();
            baseNode.WaitUntilEnd();
        }

        public async void WaitUntilEnd()
        {
            Ch1MinigameBattleMkPt2 minigameBattleMkPt2 = this;
            await minigameBattleMkPt2.DelaySeconds(80.0);
            if (!minigameBattleMkPt2.IsValid())
                return;
            this.Invincible = true;
            minigameBattleMkPt2.RestoreBgOpacity();
            await minigameBattleMkPt2.DelaySeconds(0.5);
            minigameBattleMkPt2.evtOutro.Play();
            await minigameBattleMkPt2.DelaySeconds(2.0);
            minigameBattleMkPt2.nMusicPlayer.Stop(4f);
        }

        public void LowerBgOpacity()
        {
            Game.Animations.Play((LacieAnimation)new ModulateAnimation(this.nBg, 0.5f, Godot.Colors.White, Ch1MinigameBattleMkPt2.BG_HIDDEN_MODULATE));
            Game.Animations.Play((LacieAnimation)new ModulateAnimation(this.nFg, 0.5f, Godot.Colors.White, Ch1MinigameBattleMkPt2.BG_HIDDEN_MODULATE));
        }

        public void RestoreBgOpacity()
        {
            Game.Animations.Play((LacieAnimation)new ModulateAnimation(this.nBg, 0.5f, Ch1MinigameBattleMkPt2.BG_HIDDEN_MODULATE, Godot.Colors.White));
            Game.Animations.Play((LacieAnimation)new ModulateAnimation(this.nFg, 0.5f, Ch1MinigameBattleMkPt2.BG_HIDDEN_MODULATE, Godot.Colors.White));
        }
    }
}
