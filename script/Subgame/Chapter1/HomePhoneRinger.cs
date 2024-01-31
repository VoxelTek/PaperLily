// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.HomePhoneRinger
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class HomePhoneRinger : Node
  {
    private PVar varPhoneCallRinging = (PVar) "ch1.home_phone_call_ringing";
    public const float RingerTimer = 3f;
    private float _timer = 1.5f;
    private AudioStream _sfx;
    private float _volume;

    private HomePhoneRinger()
    {
    }

    public HomePhoneRinger(AudioStream sfx, float volume)
    {
      this._sfx = sfx;
      this._volume = volume;
    }

    public override void _Process(float delta)
    {
      this._timer -= delta;
      if ((double) this._timer > 0.0)
        return;
      if (!(bool) this.varPhoneCallRinging.Value)
      {
        this.Delete();
      }
      else
      {
        Game.Audio.PlaySfx(this._sfx, this._volume);
        this._timer = 3f;
      }
    }
  }
}
