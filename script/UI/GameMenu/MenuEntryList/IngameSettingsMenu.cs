// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.IngameSettingsMenu
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
  public class IngameSettingsMenu : SimpleVerticalMenu
  {
    public IngameSettingsMenu(IMenuEntryList parentMenu, IMenuEntryContainer container)
    {
      this.Parent = parentMenu;
      this.Container = container;
      this.Entries = new List<IMenuEntry>();
      SubMenuEntry subMenuEntry1 = new SubMenuEntry("system.settings.game", (IMenuEntryList) this, container);
      subMenuEntry1.AddEntry((IMenuEntry) new SettingMenuEntry<SkipEnabledSetting, bool>((IMenuEntryList) subMenuEntry1));
      subMenuEntry1.AddEntry((IMenuEntry) new SettingMenuEntry<ShowObjectivesNotificationsSetting, bool>((IMenuEntryList) subMenuEntry1));
      subMenuEntry1.AddEntry((IMenuEntry) new BackMenuEntry("system.common.back", (IMenuEntryList) subMenuEntry1));
      SubMenuEntry subMenuEntry2 = new SubMenuEntry("system.settings.audio", (IMenuEntryList) this, container);
      subMenuEntry2.AddEntry((IMenuEntry) new SettingMenuEntry<VolumeMasterSetting, Decimal>((IMenuEntryList) subMenuEntry2));
      subMenuEntry2.AddEntry((IMenuEntry) new SettingMenuEntry<VolumeBgmSetting, Decimal>((IMenuEntryList) subMenuEntry2));
      subMenuEntry2.AddEntry((IMenuEntry) new SettingMenuEntry<VolumeSfxSetting, Decimal>((IMenuEntryList) subMenuEntry2));
      subMenuEntry2.AddEntry((IMenuEntry) new SettingMenuEntry<VolumeSystemSetting, Decimal>((IMenuEntryList) subMenuEntry2));
      subMenuEntry2.AddEntry((IMenuEntry) new SettingMenuEntry<VolumeTextSetting, Decimal>((IMenuEntryList) subMenuEntry2));
      subMenuEntry2.AddEntry((IMenuEntry) new SettingMenuEntry<MuteAudioSetting, bool>((IMenuEntryList) subMenuEntry2));
      subMenuEntry2.AddEntry((IMenuEntry) new BackMenuEntry("system.common.back", (IMenuEntryList) subMenuEntry2));
      SubMenuEntry subMenuEntry3 = new SubMenuEntry("system.settings.video", (IMenuEntryList) this, container);
      subMenuEntry3.AddEntry((IMenuEntry) new FullScreenMenuEntry((IMenuEntryList) subMenuEntry3));
      subMenuEntry3.AddEntry((IMenuEntry) new ResolutionMenuEntry((IMenuEntryList) subMenuEntry3));
      subMenuEntry3.AddEntry((IMenuEntry) new SettingMenuEntry2<ResolutionScaleSetting, int>((IMenuEntryList) subMenuEntry3));
      subMenuEntry3.AddEntry((IMenuEntry) new SettingMenuEntry2<FpsLimitSetting, string>((IMenuEntryList) subMenuEntry3));
      subMenuEntry3.AddEntry((IMenuEntry) new SettingMenuEntry<BrightnessSetting, Decimal>((IMenuEntryList) subMenuEntry3));
      subMenuEntry3.AddEntry((IMenuEntry) new SettingMenuEntry<ContrastSetting, Decimal>((IMenuEntryList) subMenuEntry3));
      subMenuEntry3.AddEntry((IMenuEntry) new SettingMenuEntry<GammaSetting, Decimal>((IMenuEntryList) subMenuEntry3));
      subMenuEntry3.AddEntry((IMenuEntry) new SettingMenuEntry<HideCursorSetting, bool>((IMenuEntryList) subMenuEntry3));
      subMenuEntry3.AddEntry((IMenuEntry) new BackMenuEntry("system.common.back", (IMenuEntryList) subMenuEntry3));
      SubMenuEntry subMenuEntry4 = new SubMenuEntry("system.settings.input", (IMenuEntryList) this, container);
      subMenuEntry4.AddEntry((IMenuEntry) new SettingMenuEntry<InputTypeSetting, string>((IMenuEntryList) subMenuEntry4));
      subMenuEntry4.AddEntry((IMenuEntry) new SettingMenuEntry2<DeadzoneSetting, float>((IMenuEntryList) subMenuEntry4));
      subMenuEntry4.AddEntry((IMenuEntry) new SettingMenuEntry<InputAutoSwitchSetting, bool>((IMenuEntryList) subMenuEntry4));
      subMenuEntry4.AddEntry((IMenuEntry) new InputBindingMenuEntry((IMenuEntryList) subMenuEntry4, "system.actions.up", "input_up"));
      subMenuEntry4.AddEntry((IMenuEntry) new InputBindingMenuEntry((IMenuEntryList) subMenuEntry4, "system.actions.down", "input_down"));
      subMenuEntry4.AddEntry((IMenuEntry) new InputBindingMenuEntry((IMenuEntryList) subMenuEntry4, "system.actions.left", "input_left"));
      subMenuEntry4.AddEntry((IMenuEntry) new InputBindingMenuEntry((IMenuEntryList) subMenuEntry4, "system.actions.right", "input_right"));
      subMenuEntry4.AddEntry((IMenuEntry) new InputBindingMenuEntry((IMenuEntryList) subMenuEntry4, "system.actions.action", "input_action"));
      subMenuEntry4.AddEntry((IMenuEntry) new InputBindingMenuEntry((IMenuEntryList) subMenuEntry4, "system.actions.cancel", "input_cancel"));
      subMenuEntry4.AddEntry((IMenuEntry) new InputBindingMenuEntry((IMenuEntryList) subMenuEntry4, "system.actions.run", "input_run"));
      subMenuEntry4.AddEntry((IMenuEntry) new InputBindingMenuEntry((IMenuEntryList) subMenuEntry4, "system.actions.special", "input_special"));
      subMenuEntry4.AddEntry((IMenuEntry) new InputBindingMenuEntry((IMenuEntryList) subMenuEntry4, "system.actions.menu", "input_menu"));
      subMenuEntry4.AddEntry((IMenuEntry) new BackMenuEntry("system.common.back", (IMenuEntryList) subMenuEntry4));
      this.Entries.Add((IMenuEntry) subMenuEntry1);
      this.Entries.Add((IMenuEntry) subMenuEntry2);
      this.Entries.Add((IMenuEntry) subMenuEntry3);
      this.Entries.Add((IMenuEntry) subMenuEntry4);
    }

    public override void Back()
    {
      if (Game.Settings.InputProfileCustom && !Inputs.Profiles["custom"].IsComplete())
      {
        Log.Info((object) "Input profile not complete!");
        Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
        MessageContainer errorMsg = new MessageContainer();
        errorMsg.OnClose = (Action) (() => errorMsg.Delete());
        errorMsg.Text = "system.settings.input.error";
        Game.Screen.AddToLayer(IScreenManager.Layer.Screen, (Node) errorMsg);
      }
      else
      {
        Game.Settings.SaveSettings();
        Game.Settings.RevertSettings();
        if (this.OnBack != null)
        {
          this.OnBack();
        }
        else
        {
          if (this.Parent == null)
            return;
          this.Parent.Root();
        }
      }
    }
  }
}
