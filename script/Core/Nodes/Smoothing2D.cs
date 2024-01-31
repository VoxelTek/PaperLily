// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.Smoothing2D
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;

#nullable disable
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
      get => this._enabled;
      set
      {
        this._enabled = value;
        this._SetProcessing();
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
      bool translate = this.translate;
      bool rotate = this.rotate;
      bool scale = this.scale;
      this.translate = this.rotate = this.scale = true;
      this._RefreshTransform();
      this.m_Pos_prev = this.m_Pos_curr;
      this.m_Angle_prev = this.m_Angle_curr;
      this.m_Scale_prev = this.m_Scale_curr;
      this._Process(0.0f);
      this.translate = translate;
      this.rotate = rotate;
      this.scale = scale;
    }

    public override void _Ready()
    {
      this.m_Angle_curr = 0.0f;
      this.m_Angle_prev = 0.0f;
    }

    private void _SetProcessing()
    {
      this.SetProcess(this.enabled);
      this.SetPhysicsProcess(this.enabled);
    }

    public override void _EnterTree() => this._FindTarget();

    private void _RefreshTransform()
    {
      this._dirty = false;
      if (!this._HasTarget())
        return;
      if (this.globalIn)
      {
        if (this.translate)
        {
          this.m_Pos_prev = this.m_Pos_curr;
          this.m_Pos_curr = this._m_Target.GlobalPosition;
        }
        if (this.rotate)
        {
          this.m_Angle_prev = this.m_Angle_curr;
          this.m_Angle_curr = this._m_Target.GlobalRotation;
        }
        if (!this.scale)
          return;
        this.m_Scale_prev = this.m_Scale_curr;
        this.m_Scale_curr = this._m_Target.GlobalScale;
      }
      else
      {
        if (this.translate)
        {
          this.m_Pos_prev = this.m_Pos_curr;
          this.m_Pos_curr = this._m_Target.Position;
        }
        if (this.rotate)
        {
          this.m_Angle_prev = this.m_Angle_curr;
          this.m_Angle_curr = this._m_Target.Rotation;
        }
        if (!this.scale)
          return;
        this.m_Scale_prev = this.m_Scale_curr;
        this.m_Scale_curr = this._m_Target.Scale;
      }
    }

    private void _FindTarget()
    {
      this._m_Target = (Node2D) null;
      if (!this.Target.IsValid())
        return;
      this._m_Target = this.Target;
    }

    private bool _HasTarget()
    {
      if (this._m_Target == null)
        return false;
      if (Object.IsInstanceValid((Object) this._m_Target))
        return true;
      this._m_Target = (Node2D) null;
      return false;
    }

    public override void _Process(float delta)
    {
      if (this._dirty)
        this._RefreshTransform();
      float interpolationFraction = Engine.GetPhysicsInterpolationFraction();
      if (this.globalOut)
      {
        if (this.translate)
          this.GlobalPosition = this.m_Pos_prev.LinearInterpolate(this.m_Pos_curr, interpolationFraction);
        if (this.rotate)
          this.GlobalRotation = this._LerpAngle(this.m_Angle_prev, this.m_Angle_curr, interpolationFraction);
        if (!this.scale)
          return;
        this.GlobalScale = this.m_Scale_prev.LinearInterpolate(this.m_Scale_curr, interpolationFraction);
      }
      else
      {
        if (this.translate)
          this.Position = this.m_Pos_prev.LinearInterpolate(this.m_Pos_curr, interpolationFraction);
        if (this.rotate)
          this.Rotation = this._LerpAngle(this.m_Angle_prev, this.m_Angle_curr, interpolationFraction);
        if (!this.scale)
          return;
        this.Scale = this.m_Scale_prev.LinearInterpolate(this.m_Scale_curr, interpolationFraction);
      }
    }

    public override void _PhysicsProcess(float delta)
    {
      if (this._dirty)
        this._RefreshTransform();
      this._dirty = true;
    }

    private float _LerpAngle(float from, float to, float weight)
    {
      return from + this._ShortAngleDist(from, to) * weight;
    }

    private float _ShortAngleDist(float from, float to)
    {
      float num1 = 6.28318548f;
      float num2 = (to - from) % num1;
      return 2f * num2 % num1 - num2;
    }
  }
}
