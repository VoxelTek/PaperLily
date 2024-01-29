using System.Threading.Tasks;
using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Audio;
using LacieEngine.Core;
using LacieEngine.UI;

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

		[GetNode("Bg")]
		private CanvasItem nBg;

		[GetNode("Fg")]
		private CanvasItem nFg;

		private AdvancedMusicPlayer nMusicPlayer;

		private AdvancedMusicPlayer.AudioInformation introTrack;

		private AdvancedMusicPlayer.AudioInformation partATrack;

		private AdvancedMusicPlayer.AudioInformation partBTrack;

		private AdvancedMusicPlayer.AudioInformation partCTrack;

		private AdvancedMusicPlayer.AudioInformation partDTrack;

		private PEvent evtOutro = "ch1_event_mk_battle_outro";

		private static readonly Color BG_HIDDEN_MODULATE = new Color("303030");

		public override void Init()
		{
			base.Init();
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
			partDTrack = new AdvancedMusicPlayer.AudioInformation
			{
				Stream = bgmPartD,
				LeftAttachPoint = 1.785f,
				RightAttchPoint = 59.096f
			};
			nMusicPlayer = Game.Screen.GetFromLayer(IScreenManager.Layer.Pixel, "AdvancedMusicPlayer") as AdvancedMusicPlayer;
			if (!nMusicPlayer.IsValid())
			{
				nMusicPlayer = new AdvancedMusicPlayer();
				Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, nMusicPlayer);
			}
		}

		public override void _Ready()
		{
			nMusicPlayer.ClearQueue();
			nMusicPlayer.Queue(partDTrack);
			nMusicPlayer.Loop = true;
			FightStartQuick();
			double t = 2.8;
			AttackPlayerPanel(t);
			AttackPlayerPanel(t + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5 + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5 + 0.5 + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5 + 0.5 + 0.5 + 0.5);
			RandomFallPattern(t, out var finalT);
			t += 3.0;
			AttackPlayerPanel(t);
			AttackPlayerPanel(t + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5 + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5 + 0.5 + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5 + 0.5 + 0.5 + 0.5);
			RandomFallPattern(t, out finalT);
			t += 3.0;
			AttackPlayerPanel(t);
			AttackPlayerPanel(t + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5 + 0.5);
			RandomFallPattern(t, out finalT);
			t += 4.0;
			BombPanel(3, 1, t);
			t += 3.0;
			BombPanel(2, 0, t);
			BombPanel(4, 0, t);
			t += 2.5;
			BombPanel(1, 1, t);
			BombPanel(5, 2, t);
			t += 2.5;
			BombPanel(0, 0, t);
			BombPanel(6, 0, t);
			BombPanel(0, 3, t);
			BombPanel(6, 3, t);
			BurnCol(0, t + 0.1);
			BurnCol(6, t + 0.1);
			t += 3.0;
			HorizontalSlowPattern(t, 4, out finalT, invert: true);
			ThrowCol(5, t);
			ThrowCol(4, t + 0.1);
			ThrowCol(3, t + 0.1 + 0.1);
			ThrowCol(2, t + 0.1 + 0.1 + 0.1);
			t += 1.5;
			ThrowCol(5, t);
			ThrowCol(4, t + 0.1);
			ThrowCol(3, t + 0.1 + 0.1);
			ThrowCol(2, t + 0.1 + 0.1 + 0.1);
			t += 1.5;
			ThrowCol(5, t);
			ThrowCol(4, t + 0.1);
			ThrowCol(3, t + 0.1 + 0.1);
			ThrowCol(2, t + 0.1 + 0.1 + 0.1);
			t += 1.5;
			ThrowCol(5, t);
			ThrowCol(4, t + 0.1);
			ThrowCol(3, t + 0.1 + 0.1);
			ThrowCol(2, t + 0.1 + 0.1 + 0.1);
			t += 2.5;
			HorizontalSlowPattern(t, 4, out finalT);
			ThrowCol(1, t);
			ThrowCol(2, t + 0.1);
			ThrowCol(3, t + 0.1 + 0.1);
			ThrowCol(4, t + 0.1 + 0.1 + 0.1);
			t += 1.5;
			ThrowCol(1, t);
			ThrowCol(2, t + 0.1);
			ThrowCol(3, t + 0.1 + 0.1);
			ThrowCol(4, t + 0.1 + 0.1 + 0.1);
			t += 1.5;
			ThrowCol(1, t);
			ThrowCol(2, t + 0.1);
			ThrowCol(3, t + 0.1 + 0.1);
			ThrowCol(4, t + 0.1 + 0.1 + 0.1);
			t += 1.5;
			ThrowCol(1, t);
			ThrowCol(2, t + 0.1);
			ThrowCol(3, t + 0.1 + 0.1);
			ThrowCol(4, t + 0.1 + 0.1 + 0.1);
			t += 2.0;
			BombPanel(5, 0, t);
			BombPanel(1, 0, t);
			t += 2.0;
			BombPanel(3, 0, t);
			BombPanel(2, 3, t);
			t += 2.0;
			BombPanel(3, 3, t);
			BombPanel(4, 0, t);
			t += 2.0;
			BombPanel(4, 1, t);
			BombPanel(2, 2, t);
			t += 1.0;
			BombPanel(2, 1, t);
			BombPanel(4, 2, t);
			t += 2.5;
			BombPanel(1, 0, t);
			BombPanel(1, 3, t);
			BombPanel(5, 0, t);
			BombPanel(5, 3, t);
			BurnCol(1, t + 0.1);
			BurnCol(5, t + 0.1);
			t += 3.0;
			HorizontalSlowPatternDouble(t, 6, out t);
			t += 2.0;
			BombPanel(3, 0, t);
			BombPanel(3, 3, t);
			t += 2.0;
			BombPanel(2, 1, t);
			BombPanel(4, 2, t);
			t += 2.0;
			BombPanel(2, 0, t);
			BombPanel(2, 3, t);
			BombPanel(4, 0, t);
			BombPanel(4, 3, t);
			BurnRow(0, t + 0.1);
			BurnRow(3, t + 0.1);
			t += 3.0;
			SlashPanel(2, 1, t);
			SlashPanel(3, 2, t + 1.0);
			SlashPanel(4, 1, t + 1.0 + 1.0);
			SlashPanel(3, 0, t + 1.0 + 1.0 + 1.0);
			t += 4.5;
			BombPanel(2, 1, t);
			BombPanel(4, 1, t);
			MakeTelegraph(2, 2, t);
			MakeTelegraph(3, 1, t);
			MakeTelegraph(4, 2, t);
			BurnCol(2, t + 0.1);
			BurnCol(4, t + 0.1);
			t += 2.0;
			AttackPlayerPanel(t);
			AttackPlayerPanel(t + 1.0);
			AttackPlayerPanel(t + 1.0 + 1.0);
			AttackPlayerPanel(t + 1.0 + 1.0 + 1.0);
			AttackPlayerPanel(t + 1.0 + 1.0 + 1.0 + 0.9);
			AttackPlayerPanel(t + 1.0 + 1.0 + 1.0 + 0.9 + 0.9);
			AttackPlayerPanel(t + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8);
			AttackPlayerPanel(t + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8);
			AttackPlayerPanel(t + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7);
			AttackPlayerPanel(t + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7 + 0.7);
			AttackPlayerPanel(t + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7 + 0.7 + 0.6);
			AttackPlayerPanel(t + 1.0 + 1.0 + 1.0 + 0.9 + 0.9 + 0.8 + 0.8 + 0.7 + 0.7 + 0.6 + 0.6);
		}

		public override async Task TransitionOut()
		{
			if (base._hp > 0)
			{
				Control overlay2 = UIUtil.CreateOverlay(Colors.White);
				GetParent().AddChild(overlay2);
				FadeInAnimation transition = new FadeInAnimation(overlay2, 4f);
				Game.Animations.Play(transition);
				await transition.WaitUntilFinished();
				base.Visible = false;
				Game.Room.Visible = true;
				Game.Variables.SetFlag("ch1.mk_clear_noskip_pt3", value: true);
				await this.DelaySeconds(1.0);
				nMusicPlayer.Delete();
			}
			else
			{
				nMusicPlayer.Stop(0f);
				Control overlay = UIUtil.CreateOverlay(Colors.Black);
				GetParent().AddChild(overlay);
				base.Visible = false;
				Game.Room.Visible = true;
				await this.DelaySeconds(0.1);
			}
		}

		public async void FightStartQuick()
		{
			await this.DelaySeconds(1.0);
			LowerBgOpacity();
			WaitUntilEnd();
		}

		public async void WaitUntilEnd()
		{
			await this.DelaySeconds(80.0);
			if (this.IsValid())
			{
				RestoreBgOpacity();
				await this.DelaySeconds(0.5);
				evtOutro.Play();
				await this.DelaySeconds(2.0);
				nMusicPlayer.Stop(4f);
			}
		}

		public void LowerBgOpacity()
		{
			Game.Animations.Play(new ModulateAnimation(nBg, 0.5f, Colors.White, BG_HIDDEN_MODULATE));
			Game.Animations.Play(new ModulateAnimation(nFg, 0.5f, Colors.White, BG_HIDDEN_MODULATE));
		}

		public void RestoreBgOpacity()
		{
			Game.Animations.Play(new ModulateAnimation(nBg, 0.5f, BG_HIDDEN_MODULATE, Colors.White));
			Game.Animations.Play(new ModulateAnimation(nFg, 0.5f, BG_HIDDEN_MODULATE, Colors.White));
		}
	}
}
