// Decompiled with JetBrains decompiler
// Type: LacieEngine.UI.DeathScreen
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.UI
{
    public class DeathScreen : Control
    {
        [LacieEngine.API.GetNode("MenuContainer")]
        private readonly Control nMenuContainer;

        public override void _Ready() => ShowMenu();

        public async void ShowMenu()
        {
            DeathScreen deathScreen = this;
            await DrkieUtil.DelaySeconds(1.0);
            SimpleChoicesMenuContainer choicesMenuContainer = new SimpleChoicesMenuContainer(new List<IMenuEntry> {
                 new SimpleMenuEntry(Game.Language.GetCaption("system.screen.death.retry"), new Action(deathScreen.Retry),  null),
                 new SimpleMenuEntry(Game.Language.GetCaption("system.screen.death.giveup"), new Action(deathScreen.GiveUp),  null)
            });
            deathScreen.nMenuContainer.AddChild(choicesMenuContainer);
            Game.InputProcessor = Inputs.Processor.Menu;
        }

        public void Retry()
        {
            if (!SaveFileInformation.CanLoadRetrySave())
            {
                Game.InputProcessor = Inputs.Processor.None;
                Game.Core.StartGameFromSave("retrysave");
            }
            else
                Game.Audio.PlaySystemSound("res://assets/sfx/ui_bad.ogg");
        }

        public void GiveUp()
        {
            GameState.Delete("retrysave");
            Game.Core.SwitchToScreen(Game.Settings.SceneProvider.TitleScreen);
        }
    }
}
