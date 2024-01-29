using System;
using System.Collections.Generic;
using Godot;
using Karambolo.PO;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Translations
{
	[Injectable]
	public class LanguageManager : ILanguageManager, IModule
	{
		private const string CaptionPrefixSystem = "system.";

		private const string CaptionPrefixObjectives = "objectives.";

		private const string CaptionPrefixCharacters = "characters.";

		private const string CaptionPrefixItems = "items.";

		private const string CaptionPrefixTutorials = "tutorials.";

		private const string CaptionPrefixStory = "story.";

		private const string CaptionPrefixMisc = "misc.";

		private Dictionary<string, TranslationDTO> _translationList;

		private string _curTranslationId;

		private LanguageAgent _agent;

		private Translation currentTranslation;

		private Dictionary<string, string> baseCaptions;

		private LanguageCaptionSet translationCaptions;

		private Dictionary<string, LanguageCaptionSet> storyCaptions;

		public List<string> LanguageList { get; private set; }

		public Dictionary<string, string> LanguageNames { get; private set; }

		public bool TranslationEnabled { get; private set; }

		public void Init()
		{
			_agent = new LanguageAgent();
			Game.Root.AddChild(_agent);
			LanguageList = new List<string>();
			LanguageNames = new Dictionary<string, string>();
			LanguageList.Add(Game.Settings.TranslationBaseLocale);
			LanguageNames[Game.Settings.TranslationBaseLocale] = Game.Settings.TranslationBaseName;
			_translationList = new Dictionary<string, TranslationDTO>();
			_translationList[Game.Settings.TranslationBaseLocale] = new TranslationDTO
			{
				Name = Game.Settings.TranslationBaseName,
				Locale = Game.Settings.TranslationBaseLocale
			};
			Dictionary<string, List<string>> remaps = new Dictionary<string, List<string>>();
			foreach (string key in Game.Settings.TranslationRemaps.Keys)
			{
				remaps.Add(key, new List<string>(Game.Settings.TranslationRemaps[key]));
			}
			foreach (string langDefinitionFile in GDUtil.ListFilesInPath("res://definitions/translations/", ".json"))
			{
				TranslationDTO translationDto = GDUtil.ReadJsonFile<TranslationDTO>(langDefinitionFile);
				if (!Game.Settings.ProductName.In(translationDto.Compatibility))
				{
					continue;
				}
				Log.Info("External language found: ", translationDto.Name);
				string translationId = langDefinitionFile.StripPrefix("res://definitions/translations/").StripSuffix(".json");
				_translationList[translationId] = translationDto;
				LanguageList.Add(translationId);
				LanguageNames[translationId] = translationDto.Name;
				if (translationDto.FallbackFonts.IsNullOrEmpty())
				{
					continue;
				}
				foreach (string baseFont in translationDto.FallbackFonts.Keys)
				{
					if (GDUtil.FileExists("res://resources/font/" + baseFont + ".tres"))
					{
						GD.Load<DynamicFont>("res://resources/font/" + baseFont + ".tres").AddFallback(GD.Load<DynamicFontData>(translationDto.FallbackFonts[baseFont]));
					}
				}
			}
			if (!LanguageList.Contains(Game.Settings.TranslationSelected))
			{
				Game.Settings.SetTranslationLocale("");
			}
			if (LanguageList.Count == 1)
			{
				Game.Settings.SetTranslationLocale(Game.Settings.TranslationBaseLocale);
			}
			baseCaptions = new Dictionary<string, string>();
			foreach (string item in GDUtil.ListFilesInPath("res://definitions/captions/", ".csv"))
			{
				foreach (KeyValuePair<string, string> entry in GDUtil.ReadCaptionFile(item, Game.Settings.TranslationBaseLocale))
				{
					baseCaptions[entry.Key] = entry.Value;
				}
			}
		}

		public void LoadLanguage(string translationId)
		{
			if (translationId.IsNullOrEmpty())
			{
				translationId = Game.Settings.TranslationBaseLocale;
			}
			Log.Info("Setting language to ", translationId);
			InitLanguage();
			TranslationEnabled = translationId != Game.Settings.TranslationBaseLocale;
			foreach (KeyValuePair<string, string> entry2 in baseCaptions)
			{
				AddKey(entry2.Key, entry2.Value);
			}
			_agent.ClearOverrides();
			if (TranslationEnabled)
			{
				_curTranslationId = translationId;
				TranslationDTO translationMeta = _translationList[translationId];
				currentTranslation.Locale = translationMeta.Locale;
				string translationPath = "res://definitions/locale/" + translationMeta.Path + "/";
				foreach (string translationFile in GDUtil.ListFilesInPath(translationPath, ".po"))
				{
					string fileName = translationFile.StripPrefix(translationPath).StripSuffix(".po");
					Log.Debug("Reading translation file: ", translationFile);
					POParseResult po = GDUtil.ReadPoFile(translationFile);
					foreach (IPOEntry key in po.Catalog.Values)
					{
						if (fileName.StartsWith("events_"))
						{
							AddStoryKey(fileName.StripPrefix("events_"), new LanguageCaption(key.Key.Id, po.Catalog.GetTranslation(key.Key), key.Key.ContextId));
						}
						else
						{
							AddKey(key.Key.Id, po.Catalog.GetTranslation(key.Key), key.Key.ContextId);
						}
					}
				}
				foreach (KeyValuePair<string, string> entry in baseCaptions)
				{
					if (GetCaption(entry.Value, entry.Key) != entry.Value)
					{
						AddKey(entry.Key, GetCaption(entry.Value, entry.Key));
					}
				}
				foreach (string resource in translationMeta.ResourceRemaps.Keys)
				{
					_agent.OverrideResource(translationMeta.ResourceRemaps[resource], resource);
				}
			}
			TranslationServer.AddTranslation(currentTranslation);
			TranslationServer.SetLocale(translationId);
		}

		public bool IsCaptionKey(string key)
		{
			if ((key.IsNullOrEmpty() || !key.StartsWith("system.")) && !key.StartsWith("objectives.") && !key.StartsWith("characters.") && !key.StartsWith("items.") && !key.StartsWith("tutorials.") && !key.StartsWith("misc."))
			{
				return key.StartsWith("story.");
			}
			return true;
		}

		public string GetCaption(string key)
		{
			return translationCaptions.Get(key);
		}

		public string GetCaption(string key, string context)
		{
			return translationCaptions.Get(key, context);
		}

		public string GetFormattedCaption(string key, params string[] args)
		{
			string message = GetCaption(key);
			if (args == null || args.Length == 0)
			{
				return message;
			}
			try
			{
				return string.Format(message, args);
			}
			catch (FormatException)
			{
				Log.Error("Expected formatted caption on: ", key, " but got: ", message);
				return message;
			}
		}

		public ICaptionSet GetStoryCaptions(string room)
		{
			if (!storyCaptions.ContainsKey(room))
			{
				return null;
			}
			return storyCaptions[room];
		}

		public List<string> GetAvailableLanguages()
		{
			List<string> languages = new List<string>();
			foreach (string langId in _translationList.Keys)
			{
				if (!_translationList[langId].Disclaimer)
				{
					languages.Add(langId);
				}
			}
			return languages;
		}

		public List<string> GetExtraLanguages()
		{
			List<string> languages = new List<string>();
			foreach (string langId in _translationList.Keys)
			{
				if (_translationList[langId].Disclaimer)
				{
					languages.Add(langId);
				}
			}
			return languages;
		}

		public string GetTranslatorCredit()
		{
			return _translationList[_curTranslationId]?.Credit;
		}

		public string GetTranslatorWebsite()
		{
			return _translationList[_curTranslationId]?.Website;
		}

		private void InitLanguage()
		{
			_curTranslationId = Game.Settings.TranslationBaseLocale;
			currentTranslation = new Translation();
			currentTranslation.Locale = Game.Settings.TranslationBaseLocale;
			translationCaptions = new LanguageCaptionSet();
			storyCaptions = new Dictionary<string, LanguageCaptionSet>();
		}

		private void AddKey(string key, string caption, string context = null)
		{
			if (!caption.IsNullOrEmpty())
			{
				currentTranslation.AddMessage(key, caption);
				translationCaptions.AddCaption(new LanguageCaption(key, caption, context));
			}
		}

		private void AddStoryKey(string room, LanguageCaption caption)
		{
			if (!caption.Str.IsNullOrEmpty())
			{
				if (room.EndsWith("Global"))
				{
					room = "Global";
				}
				if (!storyCaptions.ContainsKey(room))
				{
					storyCaptions[room] = new LanguageCaptionSet();
				}
				storyCaptions[room].AddCaption(caption);
			}
		}
	}
}
