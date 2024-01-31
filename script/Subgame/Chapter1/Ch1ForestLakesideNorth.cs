﻿// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1ForestLakesideNorth
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1ForestLakesideNorth : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private AudioStream sfxRain;

    public override void _BeforeFadeIn() => Game.Audio.PlayAmbiance(this.sfxRain);

    public override void _BeforeFadeOut() => Game.Audio.StopAmbiance();
  }
}
