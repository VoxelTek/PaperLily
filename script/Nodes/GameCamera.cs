// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.GameCamera
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
	public class GameCamera : Camera2D, IGameCamera
	{
		private const int DEFAULT_CAMERA_LIMIT = 10000000;
		private const float DEFAULT_SHAKE_POWER = 8f;
		private const float DEFAULT_SHAKE_TIME = 0.2f;
		private static Random _rnd = new Random();
		private float _baseZoom;
		private bool _shaking;
		private float _shakePower;
		private float _shakeTime;
		private Vector2 _shakeInitialPos;
		private float _shakeElapsedTime;

		public Camera2D Node => (Camera2D)this;

		public float ZoomLevel {
			get => this._baseZoom;
			set {
				this._baseZoom = value;
				this.RefreshZoom();
			}
		}

		public override void _EnterTree() => this.Name = nameof(GameCamera);

		public override void _Process(float delta)
		{
			if (!this._shaking)
				return;
			this.ExecuteShake(delta);
		}

		public void Shake(float time, float power)
		{
			this._shakeInitialPos = this.Offset;
			this._shakeElapsedTime = 0.0f;
			this._shakeTime = time;
			this._shakePower = power;
			this._shaking = true;
		}

		public void Shake(float time) => this.Shake(time, 8f);

		public void Shake() => this.Shake(0.2f, 8f);

		public void ApplyRoomSettings()
		{
			this.LimitLeft = Game.CurrentRoom.CameraLimitLeft;
			this.LimitTop = Game.CurrentRoom.CameraLimitTop;
			this.LimitRight = Game.CurrentRoom.CameraLimitRight;
			this.LimitBottom = Game.CurrentRoom.CameraLimitBottom;
			this.ZoomLevel = Game.CurrentRoom.CameraZoom;
		}

		public void TrackNode(Node2D node)
		{
			this.Position = Vector2.Zero;
			if (node is IPhysicsInterpolated physicsInterpolated)
				this.Reparent((Godot.Node)physicsInterpolated.VisualNode);
			else
				this.Reparent((Godot.Node)node);
		}

		public void TrackPlayer() => this.TrackNode((Node2D)Game.Player.Node);

		public void Unlock()
		{
			Vector2 cameraScreenCenter = this.GetCameraScreenCenter();
			this.Reparent((Godot.Node)Game.CurrentRoom);
			this.Position = cameraScreenCenter;
			this.LimitLeft = -10000000;
			this.LimitTop = -10000000;
			this.LimitRight = 10000000;
			this.LimitBottom = 10000000;
		}

		public void RefreshZoom()
		{
			float num = this._baseZoom / (float)Game.Settings.ResolutionScalePixel;
			this.Zoom = new Vector2(num, num);
		}

		private void ExecuteShake(float delta)
		{
			this._shakeElapsedTime += delta;
			if ((double)this._shakeElapsedTime < (double)this._shakeTime)
			{
				this.Offset = GameCamera._rnd.NextVector2() * this._shakePower;
			}
			else
			{
				this._shaking = false;
				this._shakeElapsedTime = 0.0f;
				this.Offset = this._shakeInitialPos;
			}
		}
	}
}
