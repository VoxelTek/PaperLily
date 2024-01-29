using System.Collections.Generic;
using LacieEngine.Core;

namespace LacieEngine.Settings
{
	internal class LanguageSetting : Setting<string>
	{
		private List<string> Options;

		private string value;

		private int selection;

		private const string EXTRA_LANGUAGES = "more";

		public override string CaptionLabel()
		{
			return "system.settings.game.language";
		}

		public LanguageSetting()
		{
			Options = Game.Language.GetAvailableLanguages();
			if (!Game.Language.GetExtraLanguages().IsEmpty())
			{
				Options.Add("more");
			}
			value = Game.Settings.TranslationSelected ?? Game.Settings.TranslationBaseLocale;
			for (int i = 0; i < Options.Count; i++)
			{
				if (Options[i] == value)
				{
					selection = i;
				}
			}
		}

		public override string ValueLabel()
		{
			if (value == "more")
			{
				return "system.settings.game.language.extra";
			}
			return Game.Language.LanguageNames[value];
		}

		public override void Decrement()
		{
			selection--;
			if (selection < 0)
			{
				selection = Options.Count - 1;
			}
			value = Options[selection];
		}

		public override void Increment()
		{
			selection++;
			if (selection >= Options.Count)
			{
				selection = 0;
			}
			value = Options[selection];
		}

		public override void Apply()
		{
			if (!(value == "more"))
			{
				if (value != Game.Settings.TranslationBaseLocale)
				{
					Game.Language.LoadLanguage(Game.Settings.TranslationBaseLocale);
				}
				Game.Settings.SetTranslationLocale(value);
				Game.Language.LoadLanguage(value);
			}
		}
	}
}
