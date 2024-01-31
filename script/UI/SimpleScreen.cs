// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.SimpleScreen
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.UI
{
  public class SimpleScreen : Control
  {
    private const float DEFAULT_WAIT_TIME = 4f;
    [Export(PropertyHint.None, "")]
    public float WaitTime = 4f;
    [Export(PropertyHint.None, "")]
    public bool NoSkip;
    [Export(PropertyHint.None, "")]
    public bool KeepState;
    [Export(PropertyHint.None, "")]
    public PackedScene NextScreen;
    private string[] HandledInputs = new string[1]
    {
      "input_action"
    };
    private Timer nTimer;

    public override void _EnterTree()
    {
      if ((double) this.WaitTime <= 0.0)
        return;
      this.nTimer = GDUtil.MakeNode<Timer>("Timer");
      this.nTimer.WaitTime = this.WaitTime;
      this.nTimer.OneShot = true;
      int num = (int) this.nTimer.Connect("timeout", (Object) this, "GoToNextScreen");
      this.AddChild((Node) this.nTimer);
    }

    public override void _Ready()
    {
      if ((double) this.WaitTime <= 0.0)
        return;
      this.nTimer.Start();
    }

    public override void _Input(InputEvent @event)
    {
      if (this.NoSkip || !(Inputs.Handle(@event, Inputs.Processor.Menu, this.HandledInputs) == "input_action"))
        return;
      this.nTimer?.Stop();
      this.GoToNextScreen();
    }

    public virtual void GoToNextScreen()
    {
      if (this.NextScreen == null)
        Log.Error((object) "Next screen not set in ", (object) this.Name);
      else
        Game.Core.SwitchToScreen(this.NextScreen, this.KeepState);
    }
  }
}
