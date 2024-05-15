using System;

namespace LacieEngine.Settings
{
    internal class ShowSkipOnDeathSetting : Setting<bool>
    {
        public static ShowSkipOnDeathSetting Instance {
            get {
                return ShowSkipOnDeathSetting._lazyInstance.Value;
            }
        }

        private ShowSkipOnDeathSetting()
        {
            base.Name = "system.settings.game.showskipoption";
            base.Path = "lacie_engine/game/show_skip_option";
            base.Value = this.ReadValue();
        }

        public override string ValueLabel()
        {
            if (!base.Value)
            {
                return "system.common.no";
            }
            return "system.common.yes";
        }

        public override void Decrement()
        {
            base.Value = !base.Value;
        }

        public override void Increment()
        {
            base.Value = !base.Value;
        }

        public override void Apply()
        {
        }

        private static readonly Lazy<ShowSkipOnDeathSetting> _lazyInstance = new Lazy<ShowSkipOnDeathSetting>(() => new ShowSkipOnDeathSetting());
    }
}