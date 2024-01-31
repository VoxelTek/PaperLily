// Decompiled with JetBrains decompiler
// Type: LacieEngine.Rooms.Ch1MkB2fCages
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Rooms
{
  [Tool]
  public class Ch1MkB2fCages : GameRoom
  {
    [Export(PropertyHint.None, "")]
    private AudioStream bgmQuiet;
    [Export(PropertyHint.None, "")]
    private AudioStream bgmDynamic;
    private PVar varSerious = (PVar) "general.serious";
    private PVar varCrowDead = (PVar) "ch1.mk_crow_dead";

    public override void _BeforeFadeIn()
    {
      if ((bool) this.varSerious.Value || !(Game.State.OverrideBgm != "res://assets/bgm/silent.ogg"))
        return;
      if (!(bool) this.varCrowDead.Value)
        Game.Audio.PlayBgm(this.bgmQuiet, 1f, true);
      else
        Game.Audio.PlayBgm(this.bgmDynamic, 1f, true);
    }
  }
}
