// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.BatterySaverSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Settings
{
  internal class BatterySaverSetting : Setting<bool>
  {
    private bool value;

    public BatterySaverSetting()
    {
      this.Name = "system.settings.batterysaver";
      this.value = Game.Settings.BatterySaver;
    }

    public override string ValueLabel() => !this.value ? "system.common.no" : "system.common.yes";

    public override void Decrement() => this.value = !this.value;

    public override void Increment() => this.value = !this.value;

    public override void Apply() => Game.Settings.SetBatterySaver(this.value);
  }
}
