// Decompiled with JetBrains decompiler
// Type: LacieEngine.Nodes.LightFlickering
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;

#nullable disable
namespace LacieEngine.Nodes
{
  [Tool]
  public class LightFlickering : Light
  {
    private const double FPS = 4.0;
    private const float LOW_ENERGY = 0.96f;
    private float _time;
    private double _frequency;
    private double _halfAmplitude;
    private float _initialEnergy = 1f;

    public override void _Ready()
    {
      base._Ready();
      this._frequency = 8.0 * Math.PI;
      this._initialEnergy = this.Energy;
      this._halfAmplitude = ((double) this.Energy - (double) this.Energy * 0.95999997854232788) / 2.0;
    }

    public override void _Process(float delta)
    {
      if (Engine.EditorHint)
        return;
      this._time += delta;
      this.Energy = (float) (Math.Cos((double) this._time * this._frequency) * this._halfAmplitude + ((double) this._initialEnergy - this._halfAmplitude));
    }
  }
}
