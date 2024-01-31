// Decompiled with JetBrains decompiler
// Type: LacieEngine.Animation.AudioVolumeFadeAnimation
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;

#nullable disable
namespace LacieEngine.Animation
{
  public class AudioVolumeFadeAnimation : LacieAnimation
  {
    private float _initialVolume;
    private float _finalVolume;
    private AudioStreamPlayer _node;
    private float _track;

    public AudioVolumeFadeAnimation(
      AudioStreamPlayer node,
      float duration,
      float initialVolume,
      float finalVolume)
    {
      this.Node = (Node) (this._node = node);
      this.Duration = duration;
      this._initialVolume = initialVolume;
      this._finalVolume = finalVolume;
    }

    public override void InitialCalculations()
    {
      this._track = this._finalVolume - this._initialVolume;
    }

    public override void UpdateToFirstFrame()
    {
      this._node.VolumeDb = GD.Linear2Db(this._initialVolume);
    }

    public override void UpdateToCurrentFrame()
    {
      this._node.VolumeDb = GD.Linear2Db(this._initialVolume + this._track * (this.Elapsed / this.Duration));
    }

    public override void UpdateToFinalFrame()
    {
      this._node.VolumeDb = GD.Linear2Db(this._finalVolume);
    }
  }
}
