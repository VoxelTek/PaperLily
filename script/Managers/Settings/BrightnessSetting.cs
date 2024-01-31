// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.BrightnessSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Settings
{
  internal class BrightnessSetting : Setting<Decimal>
  {
    private const Decimal Max = 2M;
    private const Decimal Min = 0.1M;
    private const Decimal Step = 0.01M;
    private Decimal value;

    public BrightnessSetting()
    {
      this.Name = "system.settings.brightness";
      this.value = Game.Settings.Brightness;
    }

    public override string ValueLabel() => ((int) (this.value * 100M)).ToString() + "%";

    public override void Decrement()
    {
      this.value -= 0.01M;
      if (!(this.value < 0.1M))
        return;
      this.value = 0.1M;
    }

    public override void Increment()
    {
      this.value += 0.01M;
      if (!(this.value > 2M))
        return;
      this.value = 2M;
    }

    public override void Apply() => Game.Settings.SetBrightness(this.value);
  }
}
