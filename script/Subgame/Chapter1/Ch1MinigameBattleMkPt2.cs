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
    private PEvent evtOutro = (PEvent) "ch1_event_mk_battle_outro";
    private static readonly Color BG_HIDDEN_MODULATE = new Color("303030");

    public override void Init()
    {
      base.Init();
      this.introTrack = new AdvancedMusicPlayer.AudioInformation()
      {
        Stream = this.bgmPart0,
        LeftAttachPoint = 0.0f,
        RightAttchPoint = 30.429f
      };
      this.partATrack = new AdvancedMusicPlayer.AudioInformation()
      {
        Stream = this.bgmPartA,
        LeftAttachPoint = 1.785f,
        RightAttchPoint = 59.096f
      };
      this.partBTrack = new AdvancedMusicPlayer.AudioInformation()
      {
        Stream = this.bgmPartB,
        LeftAttachPoint = 1.82f,
        RightAttchPoint = 30.446f
      };
      this.partCTrack = new AdvancedMusicPlayer.AudioInformation()
      {
        Stream = this.bgmPartC,
        LeftAttachPoint = 1.788f,
        RightAttchPoint = 30.44f
      };
      this.partDTrack = new AdvancedMusicPlayer.AudioInformation()
      {
        Stream = this.bgmPartD,
        LeftAttachPoint = 1.785f,
        RightAttchPoint = 59.096f
      };
      this.nMusicPlayer = Game.Screen.GetFromLayer(IScreenManager.Layer.Pixel, "AdvancedMusicPlayer") as AdvancedMusicPlayer;
      if (this.nMusicPlayer.IsValid())
        return;
      this.nMusicPlayer = new AdvancedMusicPlayer();
      Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, (Node) this.nMusicPlayer);
    }

    public override void _Ready()
    {
      this.nMusicPlayer.ClearQueue();
      this.nMusicPlayer.Queue(this.partDTrack);
      this.nMusicPlayer.Loop = true;
      this.FightStartQuick();
      double num1 = 2.8;
      this.AttackPlayerPanel(num1);
      this.AttackPlayerPanel(num1 + 0.5);
      this.AttackPlayerPanel(num1 + 0.5 + 0.5);
      this.AttackPlayerPanel(num1 + 0.5 + 0.5 + 0.5);
      this.AttackPlayerPanel(num1 + 0.5 + 0.5 + 0.5 + 0.5);
      this.AttackPlayerPanel(num1 + 0.5 + 0.5 + 0.5 + 0.5 + 0.5);
      double finalT1;
      this.RandomFallPattern(num1, out finalT1);
      double num2 = num1 + 3.0;
      this.AttackPlayerPanel(num2);
      this.AttackPlayerPanel(num2 + 0.5);
      this.AttackPlayerPanel(num2 + 0.5 + 0.5);
      this.AttackPlayerPanel(num2 + 0.5 + 0.5 + 0.5);
      this.AttackPlayerPanel(num2 + 0.5 + 0.5 + 0.5 + 0.5);
      this.AttackPlayerPanel(num2 + 0.5 + 0.5 + 0.5 + 0.5 + 0.5);
      this.RandomFallPattern(num2, out finalT1);
      double num3 = num2 + 3.0;
      this.AttackPlayerPanel(num3);
      this.AttackPlayerPanel(num3 + 0.5);
      this.AttackPlayerPanel(num3 + 0.5 + 0.5);
      this.AttackPlayerPanel(num3 + 0.5 + 0.5 + 0.5);
      this.RandomFallPattern(num3, out finalT1);
      double time1 = num3 + 4.0;
      this.BombPanel(3, 1, time1);
      double time2 = time1 + 3.0;
      this.BombPanel(2, 0, time2);
      this.BombPanel(4, 0, time2);
      double time3 = time2 + 2.5;
      this.BombPanel(1, 1, time3);
      this.BombPanel(5, 2, time3);
      double time4 = time3 + 2.5;
      this.BombPanel(0, 0, time4);
      this.BombPanel(6, 0, time4);
      this.BombPanel(0, 3, time4);
      this.BombPanel(6, 3, time4);
      this.BurnCol(0, time4 + 0.1);
      this.BurnCol(6, time4 + 0.1);
      double num4 = time4 + 3.0;
      this.HorizontalSlowPattern(num4, 4, out finalT1, true);
      this.ThrowCol(5, num4);
      this.ThrowCol(4, num4 + 0.1);
      this.ThrowCol(3, num4 + 0.1 + 0.1);
      this.ThrowCol(2, num4 + 0.1 + 0.1 + 0.1);
      double time5 = num4 + 1.5;
      this.ThrowCol(5, time5);
      this.ThrowCol(4, time5 + 0.1);
      this.ThrowCol(3, time5 + 0.1 + 0.1);
      this.ThrowCol(2, time5 + 0.1 + 0.1 + 0.1);
      double time6 = time5 + 1.5;
      this.ThrowCol(5, time6);
      this.ThrowCol(4, time6 + 0.1);
      this.ThrowCol(3, time6 + 0.1 + 0.1);
      this.ThrowCol(2, time6 + 0.1 + 0.1 + 0.1);
      double time7 = time6 + 1.5;
      this.ThrowCol(5, time7);
      this.ThrowCol(4, time7 + 0.1);
      this.ThrowCol(3, time7 + 0.1 + 0.1);
      this.ThrowCol(2, time7 + 0.1 + 0.1 + 0.1);
      double num5 = time7 + 2.5;
      this.HorizontalSlowPattern(num5, 4, out finalT1);
      this.ThrowCol(1, num5);
      this.ThrowCol(2, num5 + 0.1);
      this.ThrowCol(3, num5 + 0.1 + 0.1);
      this.ThrowCol(4, num5 + 0.1 + 0.1 + 0.1);
      double time8 = num5 + 1.5;
      this.ThrowCol(1, time8);
      this.ThrowCol(2, time8 + 0.1);
      this.ThrowCol(3, time8 + 0.1 + 0.1);
      this.ThrowCol(4, time8 + 0.1 + 0.1 + 0.1);
      double time9 = time8 + 1.5;
      this.ThrowCol(1, time9);
      this.ThrowCol(2, time9 + 0.1);
      this.ThrowCol(3, time9 + 0.1 + 0.1);
      this.ThrowCol(4, time9 + 0.1 + 0.1 + 0.1);
      double time10 = time9 + 1.5;
      this.ThrowCol(1, time10);
      this.ThrowCol(2, time10 + 0.1);
      this.ThrowCol(3, time10 + 0.1 + 0.1);
      this.ThrowCol(4, time10 + 0.1 + 0.1 + 0.1);
      double time11 = time10 + 2.0;
      this.BombPanel(5, 0, time11);
      this.BombPanel(1, 0, time11);
      double time12 = time11 + 2.0;
      this.BombPanel(3, 0, time12);
      this.BombPanel(2, 3, time12);
      double time13 = time12 + 2.0;
      this.BombPanel(3, 3, time13);
      this.BombPanel(4, 0, time13);
      double time14 = time13 + 2.0;
      this.BombPanel(4, 1, time14);
      this.BombPanel(2, 2, time14);
      double time15 = time14 + 1.0;
      this.BombPanel(2, 1, time15);
      this.BombPanel(4, 2, time15);
      double time16 = time15 + 2.5;
      this.BombPanel(1, 0, time16);
      this.BombPanel(1, 3, time16);
      this.BombPanel(5, 0, time16);
      this.BombPanel(5, 3, time16);
      this.BurnCol(1, time16 + 0.1);
      this.BurnCol(5, time16 + 0.1);
      double finalT2 = time16 + 3.0;
      this.HorizontalSlowPatternDouble(finalT2, 6, out finalT2);
      double time17 = finalT2 + 2.0;
      this.BombPanel(3, 0, time17);
      this.BombPanel(3, 3, time17);
      double time18 = time17 + 2.0;
      this.BombPanel(2, 1, time18);
      this.BombPanel(4, 2, time18);
      double time19 = time18 + 2.0;
      this.BombPanel(2, 0, time19);
      this.BombPanel(2, 3, time19);
      this.BombPanel(4, 0, time19);
      this.BombPanel(4, 3, time19);
      this.BurnRow(0, time19 + 0.1);
      this.BurnRow(3, time19 + 0.1);
      double time20 = time19 + 3.0;
      this.SlashPanel(2, 1, time20);
      this.SlashPanel(3, 2, time20 + 1.0);
      this.SlashPanel(4, 1, time20 + 1.0 + 1.0);
      this.SlashPanel(3, 0, time20 + 1.0 + 1.0 + 1.0);
      double time21 = time20 + 4.5;
      this.BombPanel(2, 1, time21);
      this.BombPanel(4, 1, time21);
      this.MakeTelegraph(2, 2, time21);
      this.MakeTelegraph(3, 1, time21);
      this.MakeTelegraph(4, 2, time21);
      this.BurnCol(2, time21 + 0.1);
      this.BurnCol(4, time21 + 0.1);
      double time22 = time21 + 2.0;
      this.AttackPlayerPanel(time22);
      this.AttackPlayerPanel(time22 + 1.0);
      this.AttackPlayerPanel(time22 + 1.0 + 1.0);
      this.AttackPlayerPanel(time22 + 1.0 + 1.0 + 1.0);
      this.AttackPlayerPanel(time22 + 1.0 + 1.0 + 1.0 + 0.9);
      this.AttackPlayerPanel(time22 + 1.0 + 1.0 + 1.0 + 0.9 + 0.9);
      this.AttackPlayerPanel(time22 + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8);
      this.AttackPlayerPanel(time22 + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8);
      this.AttackPlayerPanel(time22 + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7);
      this.AttackPlayerPanel(time22 + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7 + 0.7);
      this.AttackPlayerPanel(time22 + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7 + 0.7 + 0.6);
      this.AttackPlayerPanel(time22 + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7 + 0.7 + 0.6 + 0.6);
    }

    public override async Task TransitionOut()
    {
      Ch1MinigameBattleMkPt2 baseNode = this;
      if (baseNode._hp > 0)
      {
        Control overlay = (Control) UIUtil.CreateOverlay(Godot.Colors.White);
        baseNode.GetParent().AddChild((Node) overlay);
        FadeInAnimation animation = new FadeInAnimation((CanvasItem) overlay, 4f);
        Game.Animations.Play((LacieAnimation) animation);
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
        Control overlay = (Control) UIUtil.CreateOverlay(Godot.Colors.Black);
        baseNode.GetParent().AddChild((Node) overlay);
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
      minigameBattleMkPt2.RestoreBgOpacity();
      await minigameBattleMkPt2.DelaySeconds(0.5);
      minigameBattleMkPt2.evtOutro.Play();
      await minigameBattleMkPt2.DelaySeconds(2.0);
      minigameBattleMkPt2.nMusicPlayer.Stop(4f);
    }

    public void LowerBgOpacity()
    {
      Game.Animations.Play((LacieAnimation) new ModulateAnimation(this.nBg, 0.5f, Godot.Colors.White, Ch1MinigameBattleMkPt2.BG_HIDDEN_MODULATE));
      Game.Animations.Play((LacieAnimation) new ModulateAnimation(this.nFg, 0.5f, Godot.Colors.White, Ch1MinigameBattleMkPt2.BG_HIDDEN_MODULATE));
    }

    public void RestoreBgOpacity()
    {
      Game.Animations.Play((LacieAnimation) new ModulateAnimation(this.nBg, 0.5f, Ch1MinigameBattleMkPt2.BG_HIDDEN_MODULATE, Godot.Colors.White));
      Game.Animations.Play((LacieAnimation) new ModulateAnimation(this.nFg, 0.5f, Ch1MinigameBattleMkPt2.BG_HIDDEN_MODULATE, Godot.Colors.White));
    }
  }
}
