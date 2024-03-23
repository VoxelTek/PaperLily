// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.TitleMenu
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
    public class TitleMenu : SimpleVerticalMenu
    {
        private TitleSettingsMenuContainer nSettingsContainer;
        private TitleLoadMenuContainer nLoadContainer;

        public TitleMenu(IMenuEntryContainer container) => Container = container;

        public void Continue()
        {
            Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
            if (TryRetrySave())
                return;
            Container.Control.Visible = false;
            nLoadContainer = GDUtil.MakeNode<TitleLoadMenuContainer>("LoadMenuContainer");
            nLoadContainer.OnClose = () => CloseLoad();
            Game.Screen.AddToLayer(IScreenManager.Layer.Screen, nLoadContainer);
            nLoadContainer.Menu.ResetSelection();
        }

        private bool TryRetrySave()
        {
            if (!SaveFileInformation.CanLoadRetrySave())
                return false;
            Game.Core.StartGameFromSave("retrysave");
            return true;
        }

        public void NewGame()
        {
            Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
            Game.Core.StartGameFromEvent(Game.Settings.NewGameEvent);
        }

        public void DebugRoom()
        {
            Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
            Game.Core.StartGameFromRoom(Game.Settings.DebugRoom, null, Vector2.Zero, "down");
        }

        public void Settings()
        {
            Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
            Container.Control.Visible = false;
            nSettingsContainer = GDUtil.MakeNode<TitleSettingsMenuContainer>("SettingsContainer");
            nSettingsContainer.OnClose = () => CloseSettings();
            Game.Screen.AddToLayer(IScreenManager.Layer.Screen, nSettingsContainer);
        }

        public void CloseSettings()
        {
            nSettingsContainer.Delete();
            nSettingsContainer = null;
            Container.Control.Visible = true;
            Root();
            if (!(Container is TitleScreen container))
                return;
            container.UpdateExtraInfo();
        }

        public void CloseLoad()
        {
            nLoadContainer.Delete();
            nLoadContainer = null;
            Container.Control.Visible = true;
            Root();
        }

        public void HomePage()
        {
            Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
            OS.ShellOpen(Game.Settings.WebsiteLink);
        }

        public void TranslatorPage()
        {
            Game.Audio.PlaySystemSound("res://assets/sfx/ui_start.ogg");
            OS.ShellOpen(Game.Language.GetTranslatorWebsite());
        }

        public void Quit()
        {
            Game.Audio.PlaySystemSound("res://assets/sfx/ui_navigation2.ogg");
            Game.InputProcessor = Inputs.Processor.None;
            Game.Tree.Quit();
        }

        public override Control DrawContent()
        {
            Entries = new List<IMenuEntry>();
            if (GameState.AnySaveExists())
                Entries.Add(new SimpleMenuEntry("system.menu.continue", () => Continue(), this));
            Entries.Add(new SimpleMenuEntry("system.menu.newgame", () => NewGame(), this));
            if (OS.IsDebugBuild())
                Entries.Add(new SimpleMenuEntry("system.menu.debugroom", () => DebugRoom(), this));
            Entries.Add(new SimpleMenuEntry("system.menu.settings", () => Settings(), this));
            if (Game.Settings.WebsiteEnabled)
                Entries.Add(new SimpleMenuEntry(Game.Settings.WebsiteCaption, () => HomePage(), this));
            if (!Game.Language.GetTranslatorWebsite().IsNullOrEmpty())
                Entries.Add(new SimpleMenuEntry("system.menu.website.translator", () => TranslatorPage(), this));
            Entries.Add(new SimpleMenuEntry("system.menu.quit", () => Quit(), this));
            return UIUtil.CreateVerticalEntryList(Entries, out _selectBgs, 2);
        }
    }
}
