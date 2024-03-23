// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerSystemCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.UI;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
    [Serializable]
    public class StoryPlayerSystemCommand : StoryPlayerCommand
    {
        public const string MINIGAME_END = "end";
        [Inject]
        [NonSerialized]
        private readonly IAchievementManager Achievements;

        public SpecialOperation Operation { get; set; }

        public string Value { get; set; }

        public string Value2 { get; set; }

        public string SaveEventLabel { get; set; }

        public string SaveLocation { get; set; }

        public string SaveImage { get; set; }

        public override void Execute(StoryPlayer storyPlayer)
        {
            if (Operation == SpecialOperation.RefreshRoom)
                Game.Room.UpdateRoomState();
            else if (Operation == SpecialOperation.CallRoomFunction)
            {
                var flag = false;
                if (Game.Minigames.Running)
                    flag = Game.Minigames.Function(Value);
                if (!flag)
                    Game.Room.RoomFunction(Value);
            }
            else if (Operation == SpecialOperation.MiniGame)
            {
                if (Value != "end")
                    Game.Minigames.Start(Value);
                else
                    Game.Minigames.End();
            }
            else if (Operation == SpecialOperation.Screen)
                Game.Core.OpenScreen(GD.Load<ScreenProxyResource>("res://resources/screen/" + Value + ".tres").Scene);
            else if (Operation == SpecialOperation.EndGame)
            {
                Game.Core.SwitchToScreen(Game.Settings.SceneProvider.CreditsScreen, true);
                GameState.Delete("retrysave");
            }
            else if (Operation == SpecialOperation.Death)
            {
                Game.Core.SwitchToScreen(Game.Settings.SceneProvider.DeathScreen);
            }
            else
            {
                if (Operation == SpecialOperation.TitleScreen)
                {
                    storyPlayer.ForceEndDialogue();
                    Game.Core.SwitchToScreen(Game.Settings.SceneProvider.TitleScreen);
                    return;
                }
                if (Operation == SpecialOperation.ClearInventory)
                    Game.Items.ClearInventory();
                else if (Operation == SpecialOperation.AutoSave)
                    GameState.Save("autosave");
                else if (Operation == SpecialOperation.RetrySave)
                    GameState.Save("retrysave", true);
                else if (Operation == SpecialOperation.RetryLoad)
                {
                    if (!SaveFileInformation.CanLoadRetrySave())
                        Game.Core.SwitchToScreen(Game.Settings.SceneProvider.CreditsScreen, true);
                    else
                        Game.Core.StartGameFromSave("retrysave");
                }
                else if (Operation == SpecialOperation.RetryClear)
                    GameState.Delete("retrysave");
                else if (Operation == SpecialOperation.SaveCopy)
                {
                    if (!Value.IsNullOrEmpty() && !Value2.IsNullOrEmpty() && GameState.SaveExists(Value))
                    {
                        new Directory().Copy("user://save/" + Value + ".sav", "user://save/" + Value2 + ".sav");
                    }
                }
                else
                {
                    if (Operation == SpecialOperation.Save)
                    {
                        if (!Value.IsNullOrEmpty())
                        {
                            Game.State.Event = Value;
                            Game.State.EventLabel = SaveEventLabel;
                            Game.State.LocationStr = SaveLocation;
                            Game.State.LocationImg = SaveImage;
                        }
                        else
                        {
                            Game.State.LocationStr = Game.CurrentRoom.SaveLocation;
                            Game.State.LocationImg = Game.CurrentRoom.SaveImage;
                        }
                        storyPlayer.HideAllUI();
                        Game.InputProcessor = Inputs.Processor.Menu;
                        var saveContainer = GDUtil.MakeNode<SaveScreenMenuContainer>("SaveContainer");
                        saveContainer.OnClose = () => ResumeAfterSave(saveContainer);
                        Game.Screen.AddToLayer(IScreenManager.Layer.UI, saveContainer);
                        saveContainer.Menu.ResetSelection();
                        storyPlayer.SetNextBlockContinue();
                        return;
                    }
                    if (Operation == SpecialOperation.Achievement)
                        Achievements.Unlock(Value);
                    else if (Operation == SpecialOperation.DisableMenu)
                        Game.State.MenuDisabled = true;
                    else if (Operation == SpecialOperation.EnableMenu)
                        Game.State.MenuDisabled = false;
                    else if (Operation == SpecialOperation.DisableRunning)
                        Game.Player.DisableRunning();
                    else if (Operation == SpecialOperation.EnableRunning)
                        Game.Player.EnableRunning();
                }
            }
            storyPlayer.SetNextBlockContinue();
            storyPlayer.Next();
        }

        public override void Load()
        {
            if (Operation == SpecialOperation.MiniGame && Value != "end")
            {
                Game.Memory.Cache("res://resources/minigame/" + Value + ".tres");
            }
            else
            {
                if (Operation != SpecialOperation.Screen)
                    return;
                Game.Memory.Cache("res://resources/screen/" + Value + ".tres");
            }
        }

        public override IList<string> GetDependencies()
        {
            if (Operation == SpecialOperation.MiniGame && Value != "end")
                return new List<string> { "res://resources/minigame/" + Value + ".tres" };
            if (Operation != SpecialOperation.Screen)
                return Array.Empty<string>();
            return new List<string> { "res://resources/screen/" + Value + ".tres" };
        }

        private void ResumeAfterSave(Control saveMenu)
        {
            Game.InputProcessor = Inputs.Processor.StoryPlayer;
            Game.State.Event = null;
            Game.State.EventLabel = null;
            saveMenu.Delete();
            Game.StoryPlayer.Next();
        }

        public enum SpecialOperation
        {
            RefreshRoom,
            CallRoomFunction,
            MiniGame,
            EndGame,
            Death,
            Screen,
            TitleScreen,
            ClearInventory,
            AutoSave,
            RetrySave,
            RetryLoad,
            RetryClear,
            Save,
            SaveCopy,
            Achievement,
            EnableMenu,
            DisableMenu,
            EnableRunning,
            DisableRunning,
        }
    }
}
