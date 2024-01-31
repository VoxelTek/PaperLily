// Decompiled with JetBrains decompiler
// Type: LacieEngine.Translations.TranslationDTO
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System.Collections.Generic;

#nullable disable
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
