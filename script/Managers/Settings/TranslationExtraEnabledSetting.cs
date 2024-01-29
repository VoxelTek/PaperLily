using System;

namespace LacieEngine.Settings
{
	internal class TranslationExtraEnabledSetting : Setting<bool>
	{
		private static readonly Lazy<TranslationExtraEnabledSetting> _lazyInstance = new Lazy<TranslationExtraEnabledSetting>(() => new TranslationExtraEnabledSetting());

		public static TranslationExtraEnabledSetting Instance => _lazyInstance.Value;

		public TranslationExtraEnabledSetting()
		{
			base.Path = "lacie_engine/core/translation_extra_enabled";
			base.Value = ReadValue();
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
	}
}
