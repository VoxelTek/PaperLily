// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.SimpleTimer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System;

#nullable disable
namespace LacieEngine.Core
{
  [ExportType(icon = "stopwatch")]
  public class SimpleTimer : Node
  {
    private bool _started;
    private float _curTimeLeft;

    [Export(PropertyHint.None, "")]
    public float WaitTime { get; set; }

    [Export(PropertyHint.None, "")]
    public bool OneShot { get; set; }

    [Export(PropertyHint.None, "")]
    public bool Autostart { get; set; }

    public Action Callback { get; set; } = (Action) (() => Log.Warn((object) "No callback specified for timer!"));

    public override void _Ready()
    {
      if (!this.Autostart)
        return;
      this.Start();
    }

    public override void _Process(float delta)
    {
      if (!this._started)
        return;
      this._curTimeLeft -= delta;
      if ((double) this._curTimeLeft > 0.0)
        return;
      this.Callback();
      this._curTimeLeft = this.WaitTime;
      if (!this.OneShot)
        return;
      this._started = false;
    }

    public void Start()
    {
      this._curTimeLeft = this.WaitTime;
      this._started = true;
    }

    public void Stop()
    {
      this._started = false;
      this._curTimeLeft = this.WaitTime;
    }
  }
}
