// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.ILanguageManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System.Collections.Generic;

#nullable disable
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

        int GetLanguageCount();

        ICaptionSet GetStoryCaptions(string room);

        string GetTranslatorCredit();

        string GetTranslatorWebsite();
    }
}
