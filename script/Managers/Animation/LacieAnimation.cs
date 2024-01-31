// Decompiled with JetBrains decompiler
// Type: LacieEngine.Animation.LacieAnimation
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.Animation
{
  public abstract class LacieAnimation
  {
    private object _waitUntilFinishedLock = new object();

    public Node Node { get; protected set; }

    public bool Playing { get; protected set; }

    public bool Finished { get; protected set; }

    public float Duration { get; protected set; }

    public float Elapsed { get; protected set; }

    public Action Callback { get; set; }

    public void Begin()
    {
      if (this.Node.IsValid())
      {
        this.InitialCalculations();
        this.UpdateToFirstFrame();
      }
      this.Playing = true;
    }

    public void Play(float delta)
    {
      this.Elapsed += delta;
      if ((double) this.Elapsed < (double) this.Duration && this.Node.IsValid())
        this.UpdateToCurrentFrame();
      else
        this.Finish();
    }

    public void Finish()
    {
      this.Playing = false;
      if (this.Node.IsValid())
        this.UpdateToFinalFrame();
      System.Threading.Monitor.Enter(this._waitUntilFinishedLock);
      this.Finished = true;
      System.Threading.Monitor.PulseAll(this._waitUntilFinishedLock);
      System.Threading.Monitor.Exit(this._waitUntilFinishedLock);
      Action callback = this.Callback;
      if (callback == null)
        return;
      callback();
    }

    public virtual void InitialCalculations()
    {
    }

    public abstract void UpdateToFirstFrame();

    public abstract void UpdateToCurrentFrame();

    public abstract void UpdateToFinalFrame();

    public async Task WaitUntilFinished() => await Task.Run(new Action(this.WaitUntilFinishedProc));

    private void WaitUntilFinishedProc()
    {
      System.Threading.Monitor.Enter(this._waitUntilFinishedLock);
      while (!this.Finished)
        System.Threading.Monitor.Wait(this._waitUntilFinishedLock);
      System.Threading.Monitor.Exit(this._waitUntilFinishedLock);
    }
  }
}
