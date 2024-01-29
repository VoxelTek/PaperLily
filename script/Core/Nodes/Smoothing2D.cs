using System;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	public class Smoothing2D : Node2D
	{
		private Node2D _m_Target;

		private Vector2 m_Pos_curr;

		private Vector2 m_Pos_prev;

		private float m_Angle_curr;

		private float m_Angle_prev;

		private Vector2 m_Scale_curr;

		private Vector2 m_Scale_prev;

		private bool _dirty;

		private bool _invisible;

		private bool _enabled;

		[Export(PropertyHint.None, "")]
		public Node2D Target;

		[Export(PropertyHint.None, "")]
		public bool enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				_enabled = value;
				_SetProcessing();
			}
		}

		[Export(PropertyHint.None, "")]
		public bool translate { get; set; }

		[Export(PropertyHint.None, "")]
		public bool rotate { get; set; }

		[Export(PropertyHint.None, "")]
		public bool scale { get; set; }

		[Export(PropertyHint.None, "")]
		public bool globalIn { get; set; }

		[Export(PropertyHint.None, "")]
		public bool globalOut { get; set; }

		public void Teleport()
		{
			bool tmpTranslate = translate;
			bool tmpRotate = rotate;
			bool tmpScale = scale;
			bool flag2 = (scale = true);
			bool flag4 = (rotate = flag2);
			translate = flag4;
			_RefreshTransform();
			m_Pos_prev = m_Pos_curr;
			m_Angle_prev = m_Angle_curr;
			m_Scale_prev = m_Scale_curr;
			_Process(0f);
			translate = tmpTranslate;
			rotate = tmpRotate;
			scale = tmpScale;
		}

		public override void _Ready()
		{
			m_Angle_curr = 0f;
			m_Angle_prev = 0f;
		}

		private void _SetProcessing()
		{
			SetProcess(enabled);
			SetPhysicsProcess(enabled);
		}

		public override void _EnterTree()
		{
			_FindTarget();
		}

		private void _RefreshTransform()
		{
			_dirty = false;
			if (!_HasTarget())
			{
				return;
			}
			if (globalIn)
			{
				if (translate)
				{
					m_Pos_prev = m_Pos_curr;
					m_Pos_curr = _m_Target.GlobalPosition;
				}
				if (rotate)
				{
					m_Angle_prev = m_Angle_curr;
					m_Angle_curr = _m_Target.GlobalRotation;
				}
				if (scale)
				{
					m_Scale_prev = m_Scale_curr;
					m_Scale_curr = _m_Target.GlobalScale;
				}
			}
			else
			{
				if (translate)
				{
					m_Pos_prev = m_Pos_curr;
					m_Pos_curr = _m_Target.Position;
				}
				if (rotate)
				{
					m_Angle_prev = m_Angle_curr;
					m_Angle_curr = _m_Target.Rotation;
				}
				if (scale)
				{
					m_Scale_prev = m_Scale_curr;
					m_Scale_curr = _m_Target.Scale;
				}
			}
		}

		private void _FindTarget()
		{
			_m_Target = null;
			if (Target.IsValid())
			{
				_m_Target = Target;
			}
		}

		private bool _HasTarget()
		{
			if (_m_Target == null)
			{
				return false;
			}
			if (Godot.Object.IsInstanceValid(_m_Target))
			{
				return true;
			}
			_m_Target = null;
			return false;
		}

		public override void _Process(float delta)
		{
			if (_dirty)
			{
				_RefreshTransform();
			}
			float f = Engine.GetPhysicsInterpolationFraction();
			if (globalOut)
			{
				if (translate)
				{
					base.GlobalPosition = m_Pos_prev.LinearInterpolate(m_Pos_curr, f);
				}
				if (rotate)
				{
					base.GlobalRotation = _LerpAngle(m_Angle_prev, m_Angle_curr, f);
				}
				if (scale)
				{
					base.GlobalScale = m_Scale_prev.LinearInterpolate(m_Scale_curr, f);
				}
			}
			else
			{
				if (translate)
				{
					base.Position = m_Pos_prev.LinearInterpolate(m_Pos_curr, f);
				}
				if (rotate)
				{
					base.Rotation = _LerpAngle(m_Angle_prev, m_Angle_curr, f);
				}
				if (scale)
				{
					base.Scale = m_Scale_prev.LinearInterpolate(m_Scale_curr, f);
				}
			}
		}

		public override void _PhysicsProcess(float delta)
		{
			if (_dirty)
			{
				_RefreshTransform();
			}
			_dirty = true;
		}

		private float _LerpAngle(float from, float to, float weight)
		{
			return from + _ShortAngleDist(from, to) * weight;
		}

		private float _ShortAngleDist(float from, float to)
		{
			float max_angle = MathF.PI * 2f;
			float diff = (to - from) % max_angle;
			return 2f * diff % max_angle - diff;
		}
	}
}
