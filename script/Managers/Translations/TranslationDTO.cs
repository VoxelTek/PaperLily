using System.Collections.Generic;

namespace LacieEngine.Translations
{
	public class TranslationDTO
	{
		public string Locale;

		public string Name;

		public string Credit;

		public string Website;

		public string Path;

		public bool Disclaimer;

		public bool UseContext;

		public Dictionary<string, string> FallbackFonts;

		public Dictionary<string, string> ResourceRemaps;

		public string[] Compatibility;
	}
}
