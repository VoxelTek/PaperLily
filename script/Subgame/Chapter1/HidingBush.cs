using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.Subgame.Chapter1
{
	public class HidingBush : Node2D
	{
		[GetNode("Particles")]
		private Particles2D nParticles;

		[GetNode("Spawn")]
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
			SetProcessInput(enable: false);
			this.InjectNodes();
		}

		public void Activate()
		{
			Game.Audio.PlaySfx(SfxBush);
			base.LightMask = 0;
			base.Material = GD.Load<Material>("res://resources/material/animation_shake_unshaded.tres").Duplicate() as Material;
			((ShaderMaterial)base.Material).SetShaderParam("shakes", 2);
			Game.Animations.Play(new ShaderProgressAnimation(this, 0.4f));
			Game.InputProcessor = Inputs.Processor.PlayerInObject;
			Game.Player.Node.Visible = false;
			Game.Player.SetPlayerState(CharacterState.InObject);
			GameCamera obj = Game.Camera as GameCamera;
			obj.ZoomLevel = 0.5f;
			obj.Unlock();
			obj.GlobalPosition = base.GlobalPosition;
			nParticles.Emitting = false;
			nParticles.Restart();
			nParticles.Emitting = true;
			nParticles.LightMask = 0;
			nParticles.Material = GD.Load<Material>("res://resources/material/unshaded.tres");
			_canLeave = false;
			nCanLeaveTimer = GDUtil.MakeNode<Timer>("CanLeaveTimer");
			nCanLeaveTimer.Autostart = true;
			nCanLeaveTimer.WaitTime = 1f;
			nCanLeaveTimer.OneShot = true;
			nCanLeaveTimer.Connect("timeout", this, "AllowLeave");
			AddChild(nCanLeaveTimer);
			if (CloseCall)
			{
				_closeCallDanger = true;
				nCloseCallTimer = GDUtil.MakeNode<Timer>("CloseCallTimer");
				nCloseCallTimer.Autostart = true;
				nCloseCallTimer.WaitTime = 5f;
				nCloseCallTimer.OneShot = true;
				nCloseCallTimer.Connect("timeout", this, "CloseCallDisengage");
				AddChild(nCloseCallTimer);
			}
			SetProcessInput(enable: true);
		}

		public void Deactivate()
		{
			if (_canLeave)
			{
				Game.Audio.PlaySfx(SfxBush);
				base.LightMask = 1;
				base.Material = GD.Load<Material>("res://resources/material/animation_shake.tres").Duplicate() as Material;
				((ShaderMaterial)base.Material).SetShaderParam("shakes", 2);
				Game.Animations.Play(new ShaderProgressAnimation(this, 0.4f));
				Game.Player.Node.Visible = true;
				Game.Player.SetDirection(Direction.Down);
				Game.Player.Node.GlobalPosition = nPoint.GlobalPosition;
				Game.Player.Turn(nPoint.Direction);
				Game.InputProcessor = Inputs.Processor.Player;
				IGameCamera camera = Game.Camera;
				camera.ZoomLevel = 1f;
				camera.TrackPlayer();
				camera.ApplyRoomSettings();
				camera.Node.GlobalPosition = Game.Player.Node.GlobalPosition;
				nParticles.Emitting = false;
				nParticles.Restart();
				nParticles.Emitting = true;
				nParticles.LightMask = 1;
				nParticles.Material = null;
				string eventToCall = null;
				if (CloseCall)
				{
					eventToCall = (_closeCallDanger ? CloseCallFailureEvent : CloseCallSuccessEvent);
				}
				nCanLeaveTimer.DeleteIfValid();
				nCloseCallTimer.DeleteIfValid();
				CloseCall = false;
				CloseCallFailureEvent = null;
				CloseCallSuccessEvent = null;
				SetProcessInput(enable: false);
				if (eventToCall != null)
				{
					Game.Events.PlayEvent(eventToCall);
				}
			}
		}

		public override void _Input(InputEvent @event)
		{
			if (Inputs.HandleSingle(@event, Inputs.Processor.PlayerInObject, "input_action"))
			{
				Deactivate();
			}
		}

		public void AllowLeave()
		{
			_canLeave = true;
		}

		public void CloseCallDisengage()
		{
			_closeCallDanger = false;
		}
	}
}
