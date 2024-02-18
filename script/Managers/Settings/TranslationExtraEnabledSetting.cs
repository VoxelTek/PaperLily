// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.TranslationExtraEnabledSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;

#nullable disable
namespace LacieEngine.Settings
{
    internal class TranslationExtraEnabledSetting : Setting<bool>
    {
        private static readonly Lazy<TranslationExtraEnabledSetting> _lazyInstance = new Lazy<TranslationExtraEnabledSetting>((Func<TranslationExtraEnabledSetting>)(() => new TranslationExtraEnabledSetting()));

        public static TranslationExtraEnabledSetting Instance {
            get => TranslationExtraEnabledSetting._lazyInstance.Value;
        }

        private TranslationExtraEnabledSetting()
        {
            this.Path = "lacie_engine/core/translation_extra_enabled";
            this.Value = this.ReadValue();
        }

        public override void Decrement() => this.Value = !this.Value;

        public override void Increment() => this.Value = !this.Value;

        public override void Apply()
        {
        }
    }
}
