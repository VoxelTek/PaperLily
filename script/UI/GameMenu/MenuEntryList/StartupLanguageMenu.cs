using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Settings;

namespace LacieEngine.UI
{
	public class StartupLanguageMenu : SimpleVerticalMenu
	{
		public class LanguageChangeMenuEntry : IMenuEntry
		{
			private string _languageId;

			private string _caption;

			private bool _extraLanguages;

			public IMenuEntryList Parent { get; set; }

			public LanguageChangeMenuEntry(string languageId, bool extraLanguages, IMenuEntryList parent)
			{
				Parent = parent;
				_languageId = languageId;
				_caption = Game.Language.LanguageNames[languageId];
				_extraLanguages = extraLanguages;
			}

			public Control DrawEntry()
			{
				return UIUtil.CreateSimpleEntry(_caption);
			}

			public void Accept()
			{
				Parent.Container.Control.Visible = false;
				Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
				if (_languageId != Game.Settings.TranslationBaseLocale)
				{
					Game.Language.LoadLanguage(Game.Settings.TranslationBaseLocale);
				}
				Game.Settings.SetTranslationLocale(_languageId);
				Game.Language.LoadLanguage(_languageId);
				if (_extraLanguages)
				{
					Parent.Parent.Back();
				}
				else
				{
					Parent.Back();
				}
			}

			public void Cancel()
			{
				if (_extraLanguages && Parent.Container.Control is TitledMenuFrame frame)
				{
					frame.TitleText = "system.settings.game.language.select";
				}
				Parent.Back();
			}
		}

		public class MoreLanguagesMenuEntry : IMenuEntry
		{
			public IMenuEntryList Parent { get; set; }

			public MoreLanguagesMenuEntry(IMenuEntryList parent)
			{
				Parent = parent;
			}

			public Control DrawEntry()
			{
				return UIUtil.CreateSimpleEntry("system.settings.game.language.extra");
			}

			public void Accept()
			{
				if (!Game.Settings.TranslationExtraEnabled)
				{
					Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
					AreYouSureContainer areYouSure = new AreYouSureContainer();
					areYouSure.OnYes = delegate
					{
						EnableExtraLanguages();
					};
					areYouSure.OnClose = delegate
					{
						areYouSure.Delete();
					};
					areYouSure.Text = "system.settings.game.language.extra.warning";
					Game.Screen.AddToLayer(IScreenManager.Layer.Screen, areYouSure);
				}
				else
				{
					Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
					ShowExtraLanguages();
				}
			}

			private void EnableExtraLanguages()
			{
				TranslationExtraEnabledSetting.Instance.Value = true;
				Game.Settings.SaveSettings();
				ShowExtraLanguages();
			}

			private void ShowExtraLanguages()
			{
				if (Parent.Container.Control is TitledMenuFrame frame)
				{
					frame.TitleText = "system.settings.game.language.select.extra";
				}
				((IMenuEntryList)new StartupLanguageMenu(extraLanguages: true, Parent, Parent.Container)).Root();
			}
		}

		private bool _extraLanguages;

		public StartupLanguageMenu(bool extraLanguages, IMenuEntryList parentMenu, IMenuEntryContainer container)
		{
			_extraLanguages = extraLanguages;
			base.Parent = parentMenu;
			base.Container = container;
		}

		public override Control DrawContent()
		{
			base.Entries = new List<IMenuEntry>();
			foreach (string languageId in _extraLanguages ? Game.Language.GetExtraLanguages() : Game.Language.GetAvailableLanguages())
			{
				base.Entries.Add(new LanguageChangeMenuEntry(languageId, _extraLanguages, this));
			}
			if (!_extraLanguages && !Game.Language.GetExtraLanguages().IsEmpty())
			{
				base.Entries.Add(new MoreLanguagesMenuEntry(this));
			}
			return UIUtil.CreateVerticalEntryList(base.Entries, out _selectBgs);
		}

		public override void Back()
		{
			Game.Settings.SaveSettings();
			Game.Settings.RevertSettings();
			if (base.OnBack != null)
			{
				base.OnBack();
			}
			else
			{
				base.Parent?.Root();
			}
		}

		public override void HandleInput(InputEvent @event)
		{
			string input = Inputs.Handle(@event, Inputs.Processor.Menu, Inputs.AllUi, allowEcho: true);
			if (input != null)
			{
				switch (input)
				{
				case "input_up":
				case "input_down":
				case "input_action":
					UIUtil.HandleVerticalNavigationInput(this, @event);
					break;
				case "input_cancel":
					Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
					base.Entries[base.Selection].Cancel();
					break;
				case "input_left":
					break;
				case "input_right":
					break;
				}
			}
		}
	}
}
