// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1ForestLakesideLake
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1ForestLakesideLake : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private AudioStream sfxPaddle1;
    [Export(PropertyHint.None, "")]
    private AudioStream sfxPaddle2;
    [Export(PropertyHint.None, "")]
    private AudioStream sfxPaddle3;
    [Export(PropertyHint.None, "")]
    private AudioStream sfxPaddle4;
    [LacieEngine.API.GetNode("Background/LacieRowing")]
    private IAnimation2D nLaciePaddle;
    private Random random = new Random();

    public void Paddle()
    {
      AudioStream track;
      switch (this.random.Next(4))
      {
        case 0:
          track = this.sfxPaddle1;
          break;
        case 1:
          track = this.sfxPaddle2;
          break;
        case 2:
          track = this.sfxPaddle3;
          break;
        case 3:
          track = this.sfxPaddle4;
          break;
        default:
          track = this.sfxPaddle1;
          break;
      }
      Game.Audio.PlaySfx(track, 0.3f);
      this.nLaciePaddle.Play();
    }
  }
}
