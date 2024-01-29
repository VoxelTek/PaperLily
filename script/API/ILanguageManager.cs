using System.Collections.Generic;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface ILanguageManager : IModule
	{
		Dictionary<string, string> LanguageNames { get; }

		bool TranslationEnabled { get; }

		void LoadLanguage(string lang);

		bool IsCaptionKey(string key);

		List<string> GetAvailableLanguages();

		List<string> GetExtraLanguages();

		string GetCaption(string key);

		string GetCaption(string key, string context);

		string GetFormattedCaption(string key, params string[] args);

		ICaptionSet GetStoryCaptions(string room);

		string GetTranslatorCredit();

		string GetTranslatorWebsite();
	}
}
