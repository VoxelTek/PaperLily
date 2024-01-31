// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.InputAutoSwitchSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Settings
{
  internal class InputAutoSwitchSetting : Setting<bool>
  {
    private bool value;

    public InputAutoSwitchSetting()
    {
      this.Name = "system.settings.input.autoswitch";
      this.value = Game.Settings.InputAutoSwitch;
    }

    public override string ValueLabel() => !this.value ? "system.common.no" : "system.common.yes";

    public override void Decrement() => this.value = !this.value;

    public override void Increment() => this.value = !this.value;

    public override void Apply() => Game.Settings.SetInputAutoSwitch(this.value);
  }
}
