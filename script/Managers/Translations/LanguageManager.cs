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
      this._agent = new LanguageAgent();
      Game.Root.AddChild((Node) this._agent);
      this.LanguageList = new List<string>();
      this.LanguageNames = new Dictionary<string, string>();
      this.LanguageList.Add(Game.Settings.TranslationBaseLocale);
      this.LanguageNames[Game.Settings.TranslationBaseLocale] = Game.Settings.TranslationBaseName;
      this._translationList = new Dictionary<string, TranslationDTO>();
      this._translationList[Game.Settings.TranslationBaseLocale] = new TranslationDTO()
      {
        Name = Game.Settings.TranslationBaseName,
        Locale = Game.Settings.TranslationBaseLocale
      };
      Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
      foreach (string key in (IEnumerable<string>) Game.Settings.TranslationRemaps.Keys)
        dictionary.Add(key, new List<string>((IEnumerable<string>) Game.Settings.TranslationRemaps[key]));
      foreach (string str in GDUtil.ListFilesInPath("res://definitions/translations/", ".json"))
      {
        TranslationDTO translationDto = GDUtil.ReadJsonFile<TranslationDTO>(str);
        if (Game.Settings.ProductName.In(translationDto.Compatibility))
        {
          Log.Info((object) "External language found: ", (object) translationDto.Name);
          string key1 = str.StripPrefix("res://definitions/translations/").StripSuffix(".json");
          this._translationList[key1] = translationDto;
          this.LanguageList.Add(key1);
          this.LanguageNames[key1] = translationDto.Name;
          if (!translationDto.FallbackFonts.IsNullOrEmpty<KeyValuePair<string, string>>())
          {
            foreach (string key2 in translationDto.FallbackFonts.Keys)
            {
              if (GDUtil.FileExists("res://resources/font/" + key2 + ".tres"))
                GD.Load<DynamicFont>("res://resources/font/" + key2 + ".tres").AddFallback(GD.Load<DynamicFontData>(translationDto.FallbackFonts[key2]));
            }
          }
        }
      }
      if (!this.LanguageList.Contains(Game.Settings.TranslationSelected))
        Game.Settings.SetTranslationLocale("");
      if (this.LanguageList.Count == 1)
        Game.Settings.SetTranslationLocale(Game.Settings.TranslationBaseLocale);
      this.baseCaptions = new Dictionary<string, string>();
      foreach (string translationFile in GDUtil.ListFilesInPath("res://definitions/captions/", ".csv"))
      {
        foreach (KeyValuePair<string, string> keyValuePair in GDUtil.ReadCaptionFile(translationFile, Game.Settings.TranslationBaseLocale))
          this.baseCaptions[keyValuePair.Key] = keyValuePair.Value;
      }
    }

    public void LoadLanguage(string translationId)
    {
      if (translationId.IsNullOrEmpty())
        translationId = Game.Settings.TranslationBaseLocale;
      Log.Info((object) "Setting language to ", (object) translationId);
      this.InitLanguage();
      this.TranslationEnabled = translationId != Game.Settings.TranslationBaseLocale;
      foreach (KeyValuePair<string, string> baseCaption in this.baseCaptions)
        this.AddKey(baseCaption.Key, baseCaption.Value);
      this._agent.ClearOverrides();
      if (this.TranslationEnabled)
      {
        this._curTranslationId = translationId;
        TranslationDTO translation1 = this._translationList[translationId];
        this.currentTranslation.Locale = translation1.Locale;
        string str1 = "res://definitions/locale/" + translation1.Path + "/";
        foreach (string str2 in GDUtil.ListFilesInPath(str1, ".po"))
        {
          string str3 = str2.StripPrefix(str1).StripSuffix(".po");
          Log.Debug((object) "Reading translation file: ", (object) str2);
          POParseResult poParseResult = GDUtil.ReadPoFile(str2);
          foreach (IPOEntry poEntry in poParseResult.Catalog.Values)
          {
            if (str3.StartsWith("events_"))
            {
              string room = str3.StripPrefix("events_");
              POKey key = poEntry.Key;
              string id = key.Id;
              string translation2 = poParseResult.Catalog.GetTranslation(poEntry.Key);
              key = poEntry.Key;
              string contextId = key.ContextId;
              LanguageCaption caption = new LanguageCaption(id, translation2, contextId);
              this.AddStoryKey(room, caption);
            }
            else
              this.AddKey(poEntry.Key.Id, poParseResult.Catalog.GetTranslation(poEntry.Key), poEntry.Key.ContextId);
          }
        }
        foreach (KeyValuePair<string, string> baseCaption in this.baseCaptions)
        {
          if (this.GetCaption(baseCaption.Value, baseCaption.Key) != baseCaption.Value)
            this.AddKey(baseCaption.Key, this.GetCaption(baseCaption.Value, baseCaption.Key));
        }
        foreach (string key in translation1.ResourceRemaps.Keys)
          this._agent.OverrideResource(translation1.ResourceRemaps[key], key);
      }
      TranslationServer.AddTranslation(this.currentTranslation);
      TranslationServer.SetLocale(translationId);
    }

    public bool IsCaptionKey(string key)
    {
      return !key.IsNullOrEmpty() && key.StartsWith("system.") || key.StartsWith("objectives.") || key.StartsWith("characters.") || key.StartsWith("items.") || key.StartsWith("tutorials.") || key.StartsWith("misc.") || key.StartsWith("story.");
    }

    public string GetCaption(string key) => this.translationCaptions.Get(key);

    public string GetCaption(string key, string context)
    {
      return this.translationCaptions.Get(key, context);
    }

    public string GetFormattedCaption(string key, params string[] args)
    {
      string caption = this.GetCaption(key);
      if (args != null)
      {
        if (args.Length != 0)
        {
          try
          {
            return string.Format(caption, (object[]) args);
          }
          catch (FormatException ex)
          {
            Log.Error((object) "Expected formatted caption on: ", (object) key, (object) " but got: ", (object) caption);
            return caption;
          }
        }
      }
      return caption;
    }

    public ICaptionSet GetStoryCaptions(string room)
    {
      return !this.storyCaptions.ContainsKey(room) ? (ICaptionSet) null : (ICaptionSet) this.storyCaptions[room];
    }

    public List<string> GetAvailableLanguages()
    {
      List<string> availableLanguages = new List<string>();
      foreach (string key in this._translationList.Keys)
      {
        if (!this._translationList[key].Disclaimer)
          availableLanguages.Add(key);
      }
      return availableLanguages;
    }

    public List<string> GetExtraLanguages()
    {
      List<string> extraLanguages = new List<string>();
      foreach (string key in this._translationList.Keys)
      {
        if (this._translationList[key].Disclaimer)
          extraLanguages.Add(key);
      }
      return extraLanguages;
    }

    public string GetTranslatorCredit() => this._translationList[this._curTranslationId]?.Credit;

    public string GetTranslatorWebsite() => this._translationList[this._curTranslationId]?.Website;

    private void InitLanguage()
    {
      this._curTranslationId = Game.Settings.TranslationBaseLocale;
      this.currentTranslation = new Translation();
      this.currentTranslation.Locale = Game.Settings.TranslationBaseLocale;
      this.translationCaptions = new LanguageCaptionSet();
      this.storyCaptions = new Dictionary<string, LanguageCaptionSet>();
    }

    private void AddKey(string key, string caption, string context = null)
    {
      if (caption.IsNullOrEmpty())
        return;
      this.currentTranslation.AddMessage(key, caption);
      this.translationCaptions.AddCaption(new LanguageCaption(key, caption, context));
    }

    private void AddStoryKey(string room, LanguageCaption caption)
    {
      if (caption.Str.IsNullOrEmpty())
        return;
      if (room.EndsWith("Global"))
        room = "Global";
      if (!this.storyCaptions.ContainsKey(room))
        this.storyCaptions[room] = new LanguageCaptionSet();
      this.storyCaptions[room].AddCaption(caption);
    }
  }
}
