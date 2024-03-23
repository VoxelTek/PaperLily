// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.TitleSettingsMenu
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
    public class TitleSettingsMenu : SimpleVerticalMenu
    {
        public TitleSettingsMenu(IMenuEntryList parentMenu, IMenuEntryContainer container)
        {
            Parent = parentMenu;
            Container = container;
            Entries = new List<IMenuEntry>();
            if (Game.Language.GetLanguageCount() > 1)
                Entries.Add(new SimpleMenuEntry("system.settings.game.language", new Action(ShowLanguageSelection), this));
            var subMenuEntry1 = new SubMenuEntry("system.settings.game", this, container);
            subMenuEntry1.AddEntry(new SettingMenuEntry<SkipEnabledSetting, bool>(subMenuEntry1));
            subMenuEntry1.AddEntry(new SettingMenuEntry<ShowObjectivesNotificationsSetting, bool>(subMenuEntry1));
            subMenuEntry1.AddEntry(new SettingMenuEntry2<ShowSkipOnDeathSetting, bool>(subMenuEntry1));
            subMenuEntry1.AddEntry(new BackMenuEntry("system.common.back", subMenuEntry1));
            Entries.Add(subMenuEntry1);
            var subMenuEntry2 = new SubMenuEntry("system.settings.audio", this, container);
            subMenuEntry2.AddEntry(new SettingMenuEntry<VolumeMasterSetting, decimal>(subMenuEntry2));
            subMenuEntry2.AddEntry(new SettingMenuEntry<VolumeBgmSetting, decimal>(subMenuEntry2));
            subMenuEntry2.AddEntry(new SettingMenuEntry<VolumeSfxSetting, decimal>(subMenuEntry2));
            subMenuEntry2.AddEntry(new SettingMenuEntry<VolumeSystemSetting, decimal>(subMenuEntry2));
            subMenuEntry2.AddEntry(new SettingMenuEntry<VolumeTextSetting, decimal>(subMenuEntry2));
            subMenuEntry2.AddEntry(new SettingMenuEntry<MuteAudioSetting, bool>(subMenuEntry2));
            subMenuEntry2.AddEntry(new BackMenuEntry("system.common.back", subMenuEntry2));
            Entries.Add(subMenuEntry2);
            var subMenuEntry3 = new SubMenuEntry("system.settings.video", this, container);
            subMenuEntry3.AddEntry(new FullScreenMenuEntry(subMenuEntry3));
            subMenuEntry3.AddEntry(new ResolutionMenuEntry(subMenuEntry3));
            subMenuEntry3.AddEntry(new SettingMenuEntry2<ResolutionScaleSetting, int>(subMenuEntry3));
            subMenuEntry3.AddEntry(new SettingMenuEntry2<FpsLimitSetting, string>(subMenuEntry3));
            subMenuEntry3.AddEntry(new SettingMenuEntry<BrightnessSetting, decimal>(subMenuEntry3));
            subMenuEntry3.AddEntry(new SettingMenuEntry<ContrastSetting, decimal>(subMenuEntry3));
            subMenuEntry3.AddEntry(new SettingMenuEntry<GammaSetting, decimal>(subMenuEntry3));
            subMenuEntry3.AddEntry(new SettingMenuEntry<HideCursorSetting, bool>(subMenuEntry3));
            subMenuEntry3.AddEntry(new BackMenuEntry("system.common.back", subMenuEntry3));
            Entries.Add(subMenuEntry3);
            var subMenuEntry4 = new SubMenuEntry("system.settings.input", this, container);
            subMenuEntry4.AddEntry(new SettingMenuEntry<InputTypeSetting, string>(subMenuEntry4));
            subMenuEntry4.AddEntry(new SettingMenuEntry2<DeadzoneSetting, float>(subMenuEntry4));
            subMenuEntry4.AddEntry(new SettingMenuEntry<InputAutoSwitchSetting, bool>(subMenuEntry4));
            subMenuEntry4.AddEntry(new InputBindingMenuEntry(subMenuEntry4, "system.actions.up", "input_up"));
            subMenuEntry4.AddEntry(new InputBindingMenuEntry(subMenuEntry4, "system.actions.down", "input_down"));
            subMenuEntry4.AddEntry(new InputBindingMenuEntry(subMenuEntry4, "system.actions.left", "input_left"));
            subMenuEntry4.AddEntry(new InputBindingMenuEntry(subMenuEntry4, "system.actions.right", "input_right"));
            subMenuEntry4.AddEntry(new InputBindingMenuEntry(subMenuEntry4, "system.actions.action", "input_action"));
            subMenuEntry4.AddEntry(new InputBindingMenuEntry(subMenuEntry4, "system.actions.cancel", "input_cancel"));
            subMenuEntry4.AddEntry(new InputBindingMenuEntry(subMenuEntry4, "system.actions.run", "input_run"));
            subMenuEntry4.AddEntry(new InputBindingMenuEntry(subMenuEntry4, "system.actions.special", "input_special"));
            subMenuEntry4.AddEntry(new InputBindingMenuEntry(subMenuEntry4, "system.actions.menu", "input_menu"));
            subMenuEntry4.AddEntry(new BackMenuEntry("system.common.back", subMenuEntry4));
            Entries.Add(subMenuEntry4);
        }

        public override void Back()
        {
            if (Game.Settings.InputProfileCustom && !Inputs.Profiles["custom"].IsComplete())
            {
                Log.Info("Input profile not complete!");
                Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
                var errorMsg = new MessageContainer();
                errorMsg.OnClose = () => errorMsg.Delete();
                errorMsg.Text = "system.settings.input.error";
                Game.Screen.AddToLayer(IScreenManager.Layer.Screen, errorMsg);
            }
            else
            {
                Game.Settings.SaveSettings();
                Game.Settings.RevertSettings();
                if (OnBack != null)
                {
                    OnBack();
                }
                else
                {
                    if (Parent == null)
                        return;
                    Parent.Root();
                }
            }
        }

        private void ShowLanguageSelection()
        {
            Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
            var languageSelector = GDUtil.MakeNode<TitleLanguageMenuContainer>("LanguageSelector");
            languageSelector.OnClose = () => {
                languageSelector.Delete();
                Root();
            };
            Game.Screen.AddToLayer(IScreenManager.Layer.Screen, languageSelector);
            languageSelector.Menu.ResetSelection();
        }
    }
}
