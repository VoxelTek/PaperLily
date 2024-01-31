// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.GammaSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Settings
{
  internal class GammaSetting : Setting<Decimal>
  {
    private const Decimal Max = 1M;
    private const Decimal Min = -1M;
    private const Decimal Step = 0.01M;
    private Decimal value;

    public GammaSetting()
    {
      this.Name = "system.settings.gamma";
      this.value = Game.Settings.Gamma;
    }

    public override string ValueLabel()
    {
      return this.value <= 0M ? this.value.ToString("0.00") : "+" + this.value.ToString("0.00");
    }

    public override void Decrement()
    {
      this.value -= 0.01M;
      if (!(this.value < -1M))
        return;
      this.value = -1M;
    }

    public override void Increment()
    {
      this.value += 0.01M;
      if (!(this.value > 1M))
        return;
      this.value = 1M;
    }

    public override void Apply() => Game.Settings.SetGamma(this.value);
  }
}
