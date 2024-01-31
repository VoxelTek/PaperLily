// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.ExtraFunctions
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Nodes;
using System;

#nullable disable
namespace LacieEngine.Core
{
  [Injectable]
  public class ExtraFunctions : IExtraFunctions
  {
    private EventTimer nEventTimer;

    public void StartTimer(float seconds, Action onTimeout, bool destroyTimerOnTimeout = false)
    {
      this.StopTimer();
      this.nEventTimer = new EventTimer();
      this.nEventTimer.SelfDestructWhenFinished = destroyTimerOnTimeout;
      Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, (Node) this.nEventTimer);
      this.nEventTimer.StartTimer(seconds, onTimeout);
    }

    public void StopTimer()
    {
      if (!this.nEventTimer.IsValid())
        return;
      this.nEventTimer.StopTimer();
      this.nEventTimer.Delete();
      this.nEventTimer = (EventTimer) null;
    }

    public EventTimer GetTimer() => this.nEventTimer;
  }
}
