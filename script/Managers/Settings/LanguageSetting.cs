// Decompiled with JetBrains decompiler
// Type: LacieEngine.Settings.LanguageSetting
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using LacieEngine.Core;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Settings
{
  internal class LanguageSetting : Setting<string>
  {
    private List<string> Options;
    private string value;
    private int selection;
    private const string EXTRA_LANGUAGES = "more";

    public override string CaptionLabel() => "system.settings.game.language";

    public LanguageSetting()
    {
      this.Options = Game.Language.GetAvailableLanguages();
      if (!Game.Language.GetExtraLanguages().IsEmpty<string>())
        this.Options.Add("more");
      this.value = Game.Settings.TranslationSelected ?? Game.Settings.TranslationBaseLocale;
      for (int index = 0; index < this.Options.Count; ++index)
      {
        if (this.Options[index] == this.value)
          this.selection = index;
      }
    }

    public override string ValueLabel()
    {
      return this.value == "more" ? "system.settings.game.language.extra" : Game.Language.LanguageNames[this.value];
    }

    public override void Decrement()
    {
      --this.selection;
      if (this.selection < 0)
        this.selection = this.Options.Count - 1;
      this.value = this.Options[this.selection];
    }

    public override void Increment()
    {
      ++this.selection;
      if (this.selection >= this.Options.Count)
        this.selection = 0;
      this.value = this.Options[this.selection];
    }

    public override void Apply()
    {
      if (this.value == "more")
        return;
      if (this.value != Game.Settings.TranslationBaseLocale)
        Game.Language.LoadLanguage(Game.Settings.TranslationBaseLocale);
      Game.Settings.SetTranslationLocale(this.value);
      Game.Language.LoadLanguage(this.value);
    }
  }
}
