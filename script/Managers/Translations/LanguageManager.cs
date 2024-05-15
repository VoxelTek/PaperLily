// Decompiled with JetBrains decompiler
// Type: LacieEngine.Translations.LanguageManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using Karambolo.PO;
using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
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
            _translationList = new Dictionary<string, TranslationDTO> {
                [Game.Settings.TranslationBaseLocale] = new TranslationDTO {
                    Name = Game.Settings.TranslationBaseName,
                    Locale = Game.Settings.TranslationBaseLocale
                }
            };
            var dictionary = new Dictionary<string, List<string>>();
            foreach (var key in (IEnumerable<string>)Game.Settings.TranslationRemaps.Keys)
                dictionary.Add(key, new List<string>(Game.Settings.TranslationRemaps[key]));
            foreach (var str in GDUtil.ListFilesInPath("res://definitions/translations/", ".json"))
            {
                var translationDto = GDUtil.ReadJsonFile<TranslationDTO>(str);
                if (Game.Settings.ProductName.In(translationDto.Compatibility))
                {
                    Log.Info("External language found: ", translationDto.Name);
                    var key1 = str.StripPrefix("res://definitions/translations/").StripSuffix(".json");
                    _translationList[key1] = translationDto;
                    LanguageList.Add(key1);
                    LanguageNames[key1] = translationDto.Name;
                    if (!translationDto.FallbackFonts.IsNullOrEmpty())
                    {
                        foreach (var key2 in translationDto.FallbackFonts.Keys)
                        {
                            if (GDUtil.FileExists("res://resources/font/" + key2 + ".tres"))
                                GD.Load<DynamicFont>("res://resources/font/" + key2 + ".tres").AddFallback(GD.Load<DynamicFontData>(translationDto.FallbackFonts[key2]));
                        }
                    }
                }
            }
            if (!LanguageList.Contains(Game.Settings.TranslationSelected))
                Game.Settings.SetTranslationLocale("");
            if (LanguageList.Count == 1)
                Game.Settings.SetTranslationLocale(Game.Settings.TranslationBaseLocale);
            baseCaptions = new Dictionary<string, string>();
            foreach (var translationFile in GDUtil.ListFilesInPath("res://definitions/captions/", ".csv"))
            {
                foreach (var keyValuePair in GDUtil.ReadCaptionFile(translationFile, Game.Settings.TranslationBaseLocale))
                    baseCaptions[keyValuePair.Key] = keyValuePair.Value;
            }
        }

        public void LoadLanguage(string translationId)
        {
            if (translationId.IsNullOrEmpty())
                translationId = Game.Settings.TranslationBaseLocale;
            Log.Info("Setting language to ", translationId);
            InitLanguage();
            TranslationEnabled = translationId != Game.Settings.TranslationBaseLocale;
            foreach (var baseCaption in baseCaptions)
                AddKey(baseCaption.Key, baseCaption.Value);
            _agent.ClearOverrides();
            if (TranslationEnabled)
            {
                _curTranslationId = translationId;
                var translation1 = _translationList[translationId];
                currentTranslation.Locale = translation1.Locale;
                var str1 = "res://definitions/locale/" + translation1.Path + "/";
                foreach (var str2 in GDUtil.ListFilesInPath(str1, ".po"))
                {
                    var str3 = str2.StripPrefix(str1).StripSuffix(".po");
                    Log.Debug("Reading translation file: ", str2);
                    var poParseResult = GDUtil.ReadPoFile(str2);
                    foreach (var poEntry in poParseResult.Catalog.Values)
                    {
                        if (str3.StartsWith("events_"))
                        {
                            var room = str3.StripPrefix("events_");
                            var key = poEntry.Key;
                            var id = key.Id;
                            var translation2 = poParseResult.Catalog.GetTranslation(poEntry.Key);
                            key = poEntry.Key;
                            var contextId = key.ContextId;
                            var caption = new LanguageCaption(id, translation2, contextId);
                            AddStoryKey(room, caption);
                        }
                        else
                            AddKey(poEntry.Key.Id, poParseResult.Catalog.GetTranslation(poEntry.Key), poEntry.Key.ContextId);
                    }
                }
                foreach (var baseCaption in baseCaptions)
                {
                    if (GetCaption(baseCaption.Value, baseCaption.Key) != baseCaption.Value)
                        AddKey(baseCaption.Key, GetCaption(baseCaption.Value, baseCaption.Key));
                }
                foreach (string key in translation1.ResourceRemaps.Keys)
                    _agent.OverrideResource(translation1.ResourceRemaps[key], key);
            }
            TranslationServer.AddTranslation(currentTranslation);
            TranslationServer.SetLocale(translationId);
        }

        public bool IsCaptionKey(string key)
        {
            return !key.IsNullOrEmpty() && key.StartsWith(CaptionPrefixSystem) || key.StartsWith(CaptionPrefixObjectives) || key.StartsWith(CaptionPrefixCharacters) || key.StartsWith(CaptionPrefixItems) || key.StartsWith(CaptionPrefixTutorials) || key.StartsWith(CaptionPrefixMisc) || key.StartsWith(CaptionPrefixStory);
        }

        public string GetCaption(string key) => translationCaptions.Get(key);

        public string GetCaption(string key, string context)
        {
            return translationCaptions.Get(key, context);
        }

        public string GetFormattedCaption(string key, params string[] args)
        {
            var caption = GetCaption(key);
            if (args != null && args.Length != 0)
            {
                try
                {
                    return string.Format(caption, args);
                }
                catch
                {
                    Log.Error("Expected formatted caption on: ", key, " but got: ", caption);
                    return caption;
                }
            }
            return caption;
        }

        public int GetLanguageCount()
        {
            return _translationList.Count;
        }

        public ICaptionSet GetStoryCaptions(string room)
        {
            return !storyCaptions.ContainsKey(room) ? null : (ICaptionSet)storyCaptions[room];
        }

        public List<string> GetAvailableLanguages()
        {
            var availableLanguages = new List<string>();
            foreach (var key in _translationList.Keys)
            {
                if (!_translationList[key].Disclaimer)
                    availableLanguages.Add(key);
            }
            return availableLanguages;
        }

        public List<string> GetExtraLanguages()
        {
            var extraLanguages = new List<string>();
            foreach (var key in _translationList.Keys)
            {
                if (_translationList[key].Disclaimer)
                    extraLanguages.Add(key);
            }
            return extraLanguages;
        }

        public string GetTranslatorCredit() => _translationList[_curTranslationId]?.Credit;

        public string GetTranslatorWebsite() => _translationList[_curTranslationId]?.Website;

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
            if (caption.IsNullOrEmpty())
                return;
            currentTranslation.AddMessage(key, caption);
            translationCaptions.AddCaption(new LanguageCaption(key, caption, context));
        }

        private void AddStoryKey(string room, LanguageCaption caption)
        {
            if (caption.Str.IsNullOrEmpty())
                return;
            if (room.EndsWith("Global"))
                room = "Global";
            if (!storyCaptions.ContainsKey(room))
                storyCaptions[room] = new LanguageCaptionSet();
            storyCaptions[room].AddCaption(caption);
        }
    }
}
