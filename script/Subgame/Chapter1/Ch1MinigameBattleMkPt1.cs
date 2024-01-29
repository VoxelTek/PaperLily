using System.Threading.Tasks;
using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Audio;
using LacieEngine.Core;
using LacieEngine.UI;

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

		[GetNode("Bg")]
		private CanvasItem nBg;

		[GetNode("Fg")]
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
		}

		public override void _Ready()
		{
			if (nMusicPlayer.Playing)
			{
				nMusicPlayer.ClearQueue();
				nMusicPlayer.Queue(partBTrack);
			}
			else
			{
				nMusicPlayer.Queue(partATrack);
				nMusicPlayer.Queue(partBTrack);
			}
			nMusicPlayer.Loop = true;
			if (!QuickStart)
			{
				SetProcess(enable: false);
				SetProcessInput(enable: false);
				FightStartSlow();
			}
			else
			{
				FightStartQuick();
			}
			double t = 2.8;
			SlashPlayerPanel(t);
			t += 1.0;
			ThrowCol(0, t + 0.1);
			ThrowCol(1, t);
			ThrowCol(2, t + 0.1);
			t += 1.0;
			ThrowCol(4, t + 0.1);
			ThrowCol(5, t);
			ThrowCol(6, t + 0.1);
			t += 1.0;
			SlashPlayerPanel(t);
			t += 1.0;
			ThrowRow(3, t);
			ThrowRow(2, t + 0.1);
			t += 1.0;
			ThrowRow(0, t, invert: true);
			ThrowRow(1, t + 0.1, invert: true);
			t += 1.0;
			ThrowCol(0, t);
			ThrowCol(1, t + 0.1);
			ThrowCol(2, t + 0.1 + 0.1);
			ThrowCol(3, t + 0.1 + 0.1 + 0.1);
			ThrowCol(4, t + 0.1 + 0.1 + 0.1 + 0.1);
			t += 1.5;
			ThrowCol(6, t);
			ThrowCol(5, t + 0.1);
			ThrowCol(4, t + 0.1 + 0.1);
			ThrowCol(3, t + 0.1 + 0.1 + 0.1);
			ThrowCol(2, t + 0.1 + 0.1 + 0.1 + 0.1);
			t += 2.0;
			SlashMPattern(t, out t);
			ThrowRow(0, t);
			ThrowRow(1, t + 0.1);
			t += 1.0;
			ThrowRow(2, t + 0.1, invert: true);
			ThrowRow(3, t, invert: true);
			t = 16.0;
			SlashPlayerPanel(t);
			t += 1.0;
			ThrowCol(2, t + 0.1);
			ThrowCol(3, t);
			ThrowCol(4, t + 0.1);
			t += 1.5;
			ThrowCol(0, t + 0.1);
			ThrowCol(1, t);
			ThrowCol(2, t + 0.1);
			ThrowCol(4, t + 0.1);
			ThrowCol(5, t);
			ThrowCol(6, t + 0.1);
			t += 1.0;
			AttackPlayerPanel(t);
			AttackPlayerPanel(t + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5);
			AttackPlayerPanel(t + 0.5 + 0.5 + 0.5);
			t += 1.0;
			ThrowRow(3, t);
			ThrowRow(2, t + 0.1);
			t += 1.0;
			ThrowRow(0, t, invert: true);
			ThrowRow(1, t + 0.1, invert: true);
			t += 1.0;
			ThrowCol(6, t);
			ThrowCol(5, t + 0.1);
			ThrowCol(4, t + 0.1 + 0.1);
			ThrowCol(3, t + 0.1 + 0.1 + 0.1);
			ThrowCol(2, t + 0.1 + 0.1 + 0.1 + 0.1);
			t += 1.5;
			ThrowCol(0, t);
			ThrowCol(1, t + 0.1);
			ThrowCol(2, t + 0.1 + 0.1);
			ThrowCol(3, t + 0.1 + 0.1 + 0.1);
			ThrowCol(4, t + 0.1 + 0.1 + 0.1 + 0.1);
			t += 2.0;
			SlashMPattern(t, out t);
			SlashPanel(5, 0, t);
			SlashPanel(3, 0, t);
			SlashPanel(5, 2, t);
			SlashPanel(3, 2, t);
			SlashPanel(1, 0, t + 1.0);
			SlashPanel(3, 0, t + 1.0);
			SlashPanel(1, 2, t + 1.0);
			SlashPanel(3, 2, t + 1.0);
			t = 32.0;
			DoubleSlowPattern(t, 5, out t);
			SlashPlayerPanel(t - 0.5);
			SlashPlayerPanel(t + 0.5);
			SlashPlayerPanel(t + 0.5 + 0.5);
			SlashPlayerPanel(t + 0.5 + 0.5 + 0.5);
			ThrowRow(3, t);
			ThrowRow(2, t + 0.1);
			t += 1.0;
			ThrowRow(0, t, invert: true);
			ThrowRow(1, t + 0.1, invert: true);
			t += 2.0;
			HorizontalSlowPattern(t, 8, out var finalT);
			t += 1.0;
			AttackPlayerPanel(t);
			t += 2.0;
			AttackPlayerPanel(t);
			t += 1.8;
			AttackPlayerPanel(t);
			t += 1.5;
			AttackPlayerPanel(t);
			t += 1.0;
			AttackPlayerPanel(t);
			t += 0.8;
			AttackPlayerPanel(t);
			t += 0.5;
			AttackPlayerPanel(t);
			t += 0.5;
			AttackPlayerPanel(t);
			t += 0.5;
			AttackPlayerPanel(t);
			t += 0.5;
			AttackPlayerPanel(t);
			RandomFallPattern(t, out finalT);
			t += 0.5;
			AttackPlayerPanel(t);
			t += 0.5;
			AttackPlayerPanel(t);
			t += 0.5;
			AttackPlayerPanel(t);
			t += 0.5;
			AttackPlayerPanel(t);
			SlashPlayerPanel(t);
			t += 1.0;
			TelegraphAll(t);
		}

		public override async Task TransitionOut()
		{
			if (base._hp > 0)
			{
				Control overlay2 = UIUtil.CreateOverlay(Colors.White);
				GetParent().AddChild(overlay2);
				FadeInAnimation transition = new FadeInAnimation(overlay2, 1f);
				Game.Animations.Play(transition);
				await transition.WaitUntilFinished();
				base.Visible = false;
				Game.Room.Visible = true;
				Game.Variables.SetFlag("ch1.mk_clear_noskip_pt1", value: true);
				await this.DelaySeconds(1.0);
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

		public async void FightStartSlow()
		{
			await this.DelaySeconds(1.0);
			LowerBgOpacity();
			WaitUntilEnd();
		}

		public async void WaitUntilEnd()
		{
			await this.DelaySeconds(57.5);
			if (this.IsValid())
			{
				RestoreBgOpacity();
				await this.DelaySeconds(0.5);
				TimeoutTime = 0f;
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
