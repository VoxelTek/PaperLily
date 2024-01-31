// Decompiled with JetBrains decompiler
// Type: LacieEngine.Translations.LanguageCaption
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using System;

#nullable disable
namespace LacieEngine.Translations
{
  public class LanguageCaption
  {
    public string Id { get; private set; }

    public string Str { get; private set; } = string.Empty;

    public string Context { get; private set; }

    public LanguageCaption(string id) => this.Id = id;

    public LanguageCaption(string id, string str)
    {
      this.Id = id;
      this.Str = str;
    }

    public LanguageCaption(string id, string str, string context)
    {
      this.Id = id;
      this.Str = str;
      this.Context = context;
    }

    public override bool Equals(object obj)
    {
      return obj is LanguageCaption languageCaption && this.Id == languageCaption.Id && this.Str == languageCaption.Str && this.Context == languageCaption.Context;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine<string, string, string>(this.Id, this.Str, this.Context);
    }
  }
}
