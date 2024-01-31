// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.EventTimer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Nodes
{
  public class EventTimer : Node
  {
    private Action _callback;
    private bool _visible = true;
    private Label nLabel;
    private Timer nTimer;

    public bool SelfDestructWhenFinished { get; set; }

    public bool Visible
    {
      get => !this.nLabel.IsValid() ? this._visible : this.nLabel.Visible;
      set
      {
        if (this.nLabel.IsValid())
          this.nLabel.Visible = value;
        else
          this._visible = value;
      }
    }

    public override void _EnterTree()
    {
      this.Name = "Timer";
      this.nLabel = GDUtil.MakeNode<Label>("TimerLabel");
      this.nLabel.SetDefaultFontAndColor();
      this.nLabel.RectPosition = new Vector2(300f, 5f);
      this.nLabel.Visible = this._visible;
      Game.Screen.AddToLayer(IScreenManager.Layer.HUD, (Node) this.nLabel);
    }

    public override void _ExitTree()
    {
      this.nTimer.DeleteIfValid();
      this.nLabel.DeleteIfValid();
    }

    public override void _Process(float delta)
    {
      if (!this.nTimer.IsValid())
        return;
      this.nLabel.Text = ((int) ((double) this.nTimer.TimeLeft / 60.0)).ToString().PadLeft(2, '0') + ":" + ((int) ((double) this.nTimer.TimeLeft % 60.0)).ToString().PadLeft(2, '0');
    }

    public void StartTimer(float seconds, Action timeoutCallback)
    {
      this.nTimer.DeleteIfValid();
      this.nTimer = GDUtil.MakeNode<Timer>("ActualTimer");
      this.nTimer.Autostart = true;
      this.nTimer.WaitTime = seconds;
      this.nTimer.OneShot = true;
      int num = (int) this.nTimer.Connect("timeout", (Godot.Object) this, "Timeout");
      this._callback = timeoutCallback;
      this.AddChild((Node) this.nTimer);
    }

    public void StopTimer()
    {
      this.nTimer.DeleteIfValid();
      this._callback = (Action) null;
    }

    public void PauseTimer()
    {
      if (!this.nTimer.IsValid())
        Log.Error((object) "Timer needs to be started first!");
      else
        this.nTimer.Paused = true;
    }

    public void ResumeTimer()
    {
      if (!this.nTimer.IsValid())
        Log.Error((object) "Timer needs to be started first!");
      else
        this.nTimer.Paused = false;
    }

    public void IncreaseTime(float seconds)
    {
      if (!this.nTimer.IsValid())
      {
        Log.Error((object) "Timer needs to be started first!");
      }
      else
      {
        this.nTimer.Stop();
        this.nTimer.WaitTime += seconds;
        this.nTimer.Start();
      }
    }

    public void DecreaseTime(float seconds)
    {
      if (!this.nTimer.IsValid())
      {
        Log.Error((object) "Timer needs to be started first!");
      }
      else
      {
        this.nTimer.Stop();
        if ((double) this.nTimer.WaitTime - (double) seconds > 0.0)
        {
          this.nTimer.WaitTime -= seconds;
          this.nTimer.Start();
        }
        else
          this.Timeout();
      }
    }

    public void Timeout()
    {
      Action callback = this._callback;
      if (callback != null)
        callback();
      if (!this.SelfDestructWhenFinished)
        return;
      this.DeleteIfValid();
    }
  }
}
