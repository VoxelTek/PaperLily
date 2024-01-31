// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.StartupLanguageMenu
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Settings;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
  public class StartupLanguageMenu : SimpleVerticalMenu
  {
    private bool _extraLanguages;

    public StartupLanguageMenu(
      bool extraLanguages,
      IMenuEntryList parentMenu,
      IMenuEntryContainer container)
    {
      this._extraLanguages = extraLanguages;
      this.Parent = parentMenu;
      this.Container = container;
    }

    public override Control DrawContent()
    {
      this.Entries = new List<IMenuEntry>();
      foreach (string languageId in this._extraLanguages ? Game.Language.GetExtraLanguages() : Game.Language.GetAvailableLanguages())
        this.Entries.Add((IMenuEntry) new StartupLanguageMenu.LanguageChangeMenuEntry(languageId, this._extraLanguages, (IMenuEntryList) this));
      if (!this._extraLanguages && !Game.Language.GetExtraLanguages().IsEmpty<string>())
        this.Entries.Add((IMenuEntry) new StartupLanguageMenu.MoreLanguagesMenuEntry((IMenuEntryList) this));
      return UIUtil.CreateVerticalEntryList(this.Entries, out this._selectBgs);
    }

    public override void Back()
    {
      Game.Settings.SaveSettings();
      Game.Settings.RevertSettings();
      if (this.OnBack != null)
        this.OnBack();
      else
        this.Parent?.Root();
    }

    public override void HandleInput(InputEvent @event)
    {
      switch (Inputs.Handle(@event, Inputs.Processor.Menu, Inputs.AllUi, true))
      {
        case "input_up":
        case "input_down":
        case "input_action":
          UIUtil.HandleVerticalNavigationInput((IMenuEntryList) this, @event);
          break;
        case "input_cancel":
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
          this.Entries[this.Selection].Cancel();
          break;
      }
    }

    public class LanguageChangeMenuEntry : IMenuEntry
    {
      private string _languageId;
      private string _caption;
      private bool _extraLanguages;

      public IMenuEntryList Parent { get; set; }

      public LanguageChangeMenuEntry(string languageId, bool extraLanguages, IMenuEntryList parent)
      {
        this.Parent = parent;
        this._languageId = languageId;
        this._caption = Game.Language.LanguageNames[languageId];
        this._extraLanguages = extraLanguages;
      }

      public Control DrawEntry() => UIUtil.CreateSimpleEntry(this._caption);

      public void Accept()
      {
        this.Parent.Container.Control.Visible = false;
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
        if (this._languageId != Game.Settings.TranslationBaseLocale)
          Game.Language.LoadLanguage(Game.Settings.TranslationBaseLocale);
        Game.Settings.SetTranslationLocale(this._languageId);
        Game.Language.LoadLanguage(this._languageId);
        if (this._extraLanguages)
          this.Parent.Parent.Back();
        else
          this.Parent.Back();
      }

      public void Cancel()
      {
        if (this._extraLanguages && this.Parent.Container.Control is TitledMenuFrame control)
          control.TitleText = "system.settings.game.language.select";
        this.Parent.Back();
      }
    }

    public class MoreLanguagesMenuEntry : IMenuEntry
    {
      public IMenuEntryList Parent { get; set; }

      public MoreLanguagesMenuEntry(IMenuEntryList parent) => this.Parent = parent;

      public Control DrawEntry() => UIUtil.CreateSimpleEntry("system.settings.game.language.extra");

      public void Accept()
      {
        if (!Game.Settings.TranslationExtraEnabled)
        {
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
          AreYouSureContainer areYouSure = new AreYouSureContainer()
          {
            OnYes = (Action) (() => this.EnableExtraLanguages())
          };
          areYouSure.OnClose = (Action) (() => areYouSure.Delete());
          areYouSure.Text = "system.settings.game.language.extra.warning";
          Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) areYouSure);
        }
        else
        {
          Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
          this.ShowExtraLanguages();
        }
      }

      private void EnableExtraLanguages()
      {
        TranslationExtraEnabledSetting.Instance.Value = true;
        Game.Settings.SaveSettings();
        this.ShowExtraLanguages();
      }

      private void ShowExtraLanguages()
      {
        if (this.Parent.Container.Control is TitledMenuFrame control)
          control.TitleText = "system.settings.game.language.select.extra";
        new StartupLanguageMenu(true, this.Parent, this.Parent.Container).Root();
      }
    }
  }
}
