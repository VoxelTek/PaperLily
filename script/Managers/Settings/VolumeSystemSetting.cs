// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.VolumeSystemSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.Settings
{
  internal class VolumeSystemSetting : Setting<Decimal>
  {
    private const Decimal Max = 1M;
    private const Decimal Min = 0M;
    private const Decimal Step = 0.05M;
    private Decimal value;

    public VolumeSystemSetting()
    {
      this.Name = "system.settings.volume.system";
      this.OwnSound = true;
      this.value = Game.Settings.VolumeSystem;
    }

    public override string ValueLabel() => ((int) (this.value * 100M)).ToString() + "%";

    public override void Decrement()
    {
      this.value -= 0.05M;
      if (!(this.value < 0M))
        return;
      this.value = 0M;
    }

    public override void Increment()
    {
      this.value += 0.05M;
      if (!(this.value > 1M))
        return;
      this.value = 1M;
    }

    public override void Apply()
    {
      Game.Settings.SetVolumeSystem(this.value);
      Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation.ogg");
    }
  }
}
