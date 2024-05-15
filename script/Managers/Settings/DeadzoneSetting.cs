// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.DeadzoneSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;

#nullable disable
namespace LacieEngine.Settings
{
    internal class DeadzoneSetting : Setting<float>
    {
        private static readonly Lazy<DeadzoneSetting> _lazyInstance = new Lazy<DeadzoneSetting>((Func<DeadzoneSetting>)(() => new DeadzoneSetting()));
        private const Decimal Max = 0.95M;
        private const Decimal Min = 0M;
        private const Decimal Step = 0.05M;
        private Decimal _value;

        public static DeadzoneSetting Instance => DeadzoneSetting._lazyInstance.Value;

        private DeadzoneSetting()
        {
            this.Name = "system.settings.input.deadzone";
            this.Path = "lacie_engine/input/joystick_deadzone";
            this.Value = this.ReadValue();
            this._value = (Decimal)this.Value;
        }

        public override string ValueLabel() => ((int)((double)this.Value * 100.0)).ToString() + "%";

        public override void Decrement()
        {
            this._value -= 0.05M;
            if (this._value < 0M)
                this._value = 0M;
            this.Value = (float)this._value;
        }

        public override void Increment()
        {
            this._value += 0.05M;
            if (this._value > 0.95M)
                this._value = 0.95M;
            this.Value = (float)this._value;
        }

        public override void Apply()
        {
        }
    }
}
