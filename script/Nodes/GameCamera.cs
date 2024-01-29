using System;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

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

		public Camera2D Node => this;

		public float ZoomLevel
		{
			get
			{
				return _baseZoom;
			}
			set
			{
				_baseZoom = value;
				RefreshZoom();
			}
		}

		public override void _EnterTree()
		{
			base.Name = "GameCamera";
		}

		public override void _Process(float delta)
		{
			if (_shaking)
			{
				ExecuteShake(delta);
			}
		}

		public void Shake(float time, float power)
		{
			_shakeInitialPos = base.Offset;
			_shakeElapsedTime = 0f;
			_shakeTime = time;
			_shakePower = power;
			_shaking = true;
		}

		public void Shake(float time)
		{
			Shake(time, 8f);
		}

		public void Shake()
		{
			Shake(0.2f, 8f);
		}

		public void ApplyRoomSettings()
		{
			base.LimitLeft = Game.CurrentRoom.CameraLimitLeft;
			base.LimitTop = Game.CurrentRoom.CameraLimitTop;
			base.LimitRight = Game.CurrentRoom.CameraLimitRight;
			base.LimitBottom = Game.CurrentRoom.CameraLimitBottom;
			ZoomLevel = Game.CurrentRoom.CameraZoom;
		}

		public void TrackNode(Node2D node)
		{
			base.Position = Vector2.Zero;
			if (node is IPhysicsInterpolated nodePi)
			{
				this.Reparent(nodePi.VisualNode);
			}
			else
			{
				this.Reparent(node);
			}
		}

		public void TrackPlayer()
		{
			TrackNode(Game.Player.Node);
		}

		public void Unlock()
		{
			Vector2 curPos = GetCameraScreenCenter();
			this.Reparent(Game.CurrentRoom);
			base.Position = curPos;
			base.LimitLeft = -10000000;
			base.LimitTop = -10000000;
			base.LimitRight = 10000000;
			base.LimitBottom = 10000000;
		}

		public void RefreshZoom()
		{
			float zoomFactor = _baseZoom / (float)Game.Settings.ResolutionScalePixel;
			base.Zoom = new Vector2(zoomFactor, zoomFactor);
		}

		private void ExecuteShake(float delta)
		{
			_shakeElapsedTime += delta;
			if (_shakeElapsedTime < _shakeTime)
			{
				base.Offset = _rnd.NextVector2() * _shakePower;
				return;
			}
			_shaking = false;
			_shakeElapsedTime = 0f;
			base.Offset = _shakeInitialPos;
		}

		bool IGameCamera.get_Current()
		{
			return base.Current;
		}

		void IGameCamera.set_Current(bool value)
		{
			base.Current = value;
		}
	}
}
