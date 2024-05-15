// Decompiled with JetBrains decompiler
// Type: LacieEngine.Subgame.Chapter1.HidingBush
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

#nullable disable
namespace LacieEngine.Subgame.Chapter1
{
  public class HidingBush : Node2D
  {
    [LacieEngine.API.GetNode("Particles")]
    private Particles2D nParticles;
    [LacieEngine.API.GetNode("Spawn")]
    private SpawnPoint nPoint;
    private Timer nCanLeaveTimer;
    private Timer nCloseCallTimer;
    private bool _canLeave;
    private bool _closeCallDanger;

    public bool CloseCall { get; set; }

    public string CloseCallSuccessEvent { get; set; }

    public string CloseCallFailureEvent { get; set; }

    public AudioStream SfxBush { get; set; }

    public override void _Ready()
    {
      this.SetProcessInput(false);
      this.InjectNodes();
    }

    public void Activate()
    {
      Game.Audio.PlaySfx(this.SfxBush);
      this.LightMask = 0;
      this.Material = GD.Load<Material>("res://resources/material/animation_shake_unshaded.tres").Duplicate() as Material;
      ((ShaderMaterial) this.Material).SetShaderParam("shakes", (object) 2);
      Game.Animations.Play((LacieAnimation) new ShaderProgressAnimation((CanvasItem) this, 0.4f));
      Game.InputProcessor = Inputs.Processor.PlayerInObject;
      Game.Player.Node.Visible = false;
      Game.Player.SetPlayerState(CharacterState.InObject);
      GameCamera camera = Game.Camera as GameCamera;
      camera.ZoomLevel = 0.5f;
      camera.Unlock();
      camera.GlobalPosition = this.GlobalPosition;
      this.nParticles.Emitting = false;
      this.nParticles.Restart();
      this.nParticles.Emitting = true;
      this.nParticles.LightMask = 0;
      this.nParticles.Material = GD.Load<Material>("res://resources/material/unshaded.tres");
      this._canLeave = false;
      this.nCanLeaveTimer = GDUtil.MakeNode<Timer>("CanLeaveTimer");
      this.nCanLeaveTimer.Autostart = true;
      this.nCanLeaveTimer.WaitTime = 1f;
      this.nCanLeaveTimer.OneShot = true;
      int num1 = (int) this.nCanLeaveTimer.Connect("timeout", (Object) this, "AllowLeave");
      this.AddChild((Node) this.nCanLeaveTimer);
      if (this.CloseCall)
      {
        this._closeCallDanger = true;
        this.nCloseCallTimer = GDUtil.MakeNode<Timer>("CloseCallTimer");
        this.nCloseCallTimer.Autostart = true;
        this.nCloseCallTimer.WaitTime = 5f;
        this.nCloseCallTimer.OneShot = true;
        int num2 = (int) this.nCloseCallTimer.Connect("timeout", (Object) this, "CloseCallDisengage");
        this.AddChild((Node) this.nCloseCallTimer);
      }
      this.SetProcessInput(true);
    }

    public void Deactivate()
    {
      if (!this._canLeave)
        return;
      Game.Audio.PlaySfx(this.SfxBush);
      this.LightMask = 1;
      this.Material = GD.Load<Material>("res://resources/material/animation_shake.tres").Duplicate() as Material;
      ((ShaderMaterial) this.Material).SetShaderParam("shakes", (object) 2);
      Game.Animations.Play((LacieAnimation) new ShaderProgressAnimation((CanvasItem) this, 0.4f));
      Game.Player.Node.Visible = true;
      Game.Player.SetDirection(Direction.Down);
      Game.Player.Node.GlobalPosition = this.nPoint.GlobalPosition;
      Game.Player.Turn((Direction) this.nPoint.Direction);
      Game.InputProcessor = Inputs.Processor.Player;
      IGameCamera camera = Game.Camera;
      camera.TrackPlayer();
      camera.ApplyRoomSettings();
      this.nParticles.Emitting = false;
      this.nParticles.Restart();
      this.nParticles.Emitting = true;
      this.nParticles.LightMask = 1;
      this.nParticles.Material = (Material) null;
      string evt = (string) null;
      if (this.CloseCall)
        evt = this._closeCallDanger ? this.CloseCallFailureEvent : this.CloseCallSuccessEvent;
      this.nCanLeaveTimer.DeleteIfValid();
      this.nCloseCallTimer.DeleteIfValid();
      this.CloseCall = false;
      this.CloseCallFailureEvent = (string) null;
      this.CloseCallSuccessEvent = (string) null;
      this.SetProcessInput(false);
      if (evt == null)
        return;
      Game.Events.PlayEvent(evt);
    }

    public override void _Input(InputEvent @event)
    {
      if (!Inputs.HandleSingle(@event, Inputs.Processor.PlayerInObject, "input_action"))
        return;
      this.Deactivate();
    }

    public void AllowLeave() => this._canLeave = true;

    public void CloseCallDisengage() => this._closeCallDanger = false;
  }
}
