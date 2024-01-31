// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1CutsceneMkBeforeFight
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Audio;
using LacieEngine.Core;

#nullable disable
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
      Game.Memory.Cache(this.resBattle.ResourcePath);
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
      this.nMusicPlayer = Game.Screen.GetFromLayer(IScreenManager.Layer.Pixel, "AdvancedMusicPlayer") as AdvancedMusicPlayer;
      if (this.nMusicPlayer.IsValid())
        return;
      this.nMusicPlayer = new AdvancedMusicPlayer();
      Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, (Node) this.nMusicPlayer);
    }

    public void PlayBattleMusic()
    {
      this.nMusicPlayer.Queue(this.introTrack);
      this.nMusicPlayer.Queue(this.partATrack);
      this.nMusicPlayer.Queue(this.partBTrack);
      this.nMusicPlayer.Loop = true;
    }
  }
}
