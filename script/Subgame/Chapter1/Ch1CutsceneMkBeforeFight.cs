using Godot;
using LacieEngine.API;
using LacieEngine.Audio;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1CutsceneMkBeforeFight : GameRoomNested
	{
		[Export(PropertyHint.None, "")]
		private Resource resBattle;

		[Export(PropertyHint.None, "")]
		private AudioStream bgmPart0;

		[Export(PropertyHint.None, "")]
		private AudioStream bgmPartA;

		[Export(PropertyHint.None, "")]
		private AudioStream bgmPartB;

		private AdvancedMusicPlayer nMusicPlayer;

		private AdvancedMusicPlayer.AudioInformation introTrack;

		private AdvancedMusicPlayer.AudioInformation partATrack;

		private AdvancedMusicPlayer.AudioInformation partBTrack;

		public override void _BeforeFadeIn()
		{
			Game.Memory.Cache(resBattle.ResourcePath);
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
			nMusicPlayer = Game.Screen.GetFromLayer(IScreenManager.Layer.Pixel, "AdvancedMusicPlayer") as AdvancedMusicPlayer;
			if (!nMusicPlayer.IsValid())
			{
				nMusicPlayer = new AdvancedMusicPlayer();
				Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, nMusicPlayer);
			}
		}

		public void PlayBattleMusic()
		{
			nMusicPlayer.Queue(introTrack);
			nMusicPlayer.Queue(partATrack);
			nMusicPlayer.Queue(partBTrack);
			nMusicPlayer.Loop = true;
		}
	}
}
