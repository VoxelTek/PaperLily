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
    private IAchievementManager Achievements;

    public StoryPlayerSystemCommand.SpecialOperation Operation { get; set; }

    public string Value { get; set; }

    public string SaveEventLabel { get; set; }

    public string SaveLocation { get; set; }

    public string SaveImage { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.RefreshRoom)
        Game.Room.UpdateRoomState();
      else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.CallRoomFunction)
      {
        bool flag = false;
        if (Game.Minigames.Running)
          flag = Game.Minigames.Function(this.Value);
        if (!flag)
          Game.Room.RoomFunction(this.Value);
      }
      else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.MiniGame)
      {
        if (this.Value != "end")
          Game.Minigames.Start(this.Value);
        else
          Game.Minigames.End();
      }
      else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.Screen)
        Game.Core.OpenScreen(GD.Load<ScreenProxyResource>("res://resources/screen/" + this.Value + ".tres").Scene);
      else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.EndGame)
        Game.Core.SwitchToScreen(Game.Settings.SceneProvider.CreditsScreen, true);
      else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.Death)
      {
        Game.Core.SwitchToScreen(Game.Settings.SceneProvider.DeathScreen);
      }
      else
      {
        if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.TitleScreen)
        {
          storyPlayer.ForceEndDialogue();
          Game.Core.SwitchToScreen(Game.Settings.SceneProvider.TitleScreen);
          return;
        }
        if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.ClearInventory)
          Game.Items.ClearInventory();
        else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.AutoSave)
          GameState.Save("autosave");
        else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.RetrySave)
          GameState.Save("retrysave", true);
        else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.RetryLoad)
        {
          Game.Core.StartGameFromSave("retrysave");
        }
        else
        {
          if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.Save)
          {
            if (!this.Value.IsNullOrEmpty())
            {
              Game.State.Event = this.Value;
              Game.State.EventLabel = this.SaveEventLabel;
              Game.State.LocationStr = this.SaveLocation;
              Game.State.LocationImg = this.SaveImage;
            }
            else
            {
              Game.State.LocationStr = Game.CurrentRoom.SaveLocation;
              Game.State.LocationImg = Game.CurrentRoom.SaveImage;
            }
            storyPlayer.HideAllUI();
            Game.InputProcessor = Inputs.Processor.Menu;
            SaveScreenMenuContainer saveContainer = GDUtil.MakeNode<SaveScreenMenuContainer>("SaveContainer");
            saveContainer.OnClose = (Action) (() => this.ResumeAfterSave((Control) saveContainer));
            Game.Screen.AddToLayer(IScreenManager.Layer.UI, (Node) saveContainer);
            saveContainer.Menu.ResetSelection();
            storyPlayer.SetNextBlockContinue();
            return;
          }
          if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.Achievement)
            this.Achievements.Unlock(this.Value);
          else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.DisableMenu)
            Game.State.MenuDisabled = true;
          else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.EnableMenu)
            Game.State.MenuDisabled = false;
          else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.DisableRunning)
            Game.Player.DisableRunning();
          else if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.EnableRunning)
            Game.Player.EnableRunning();
        }
      }
      storyPlayer.SetNextBlockContinue();
      storyPlayer.Next();
    }

    public override void Load()
    {
      if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.MiniGame && this.Value != "end")
      {
        Game.Memory.Cache("res://resources/minigame/" + this.Value + ".tres");
      }
      else
      {
        if (this.Operation != StoryPlayerSystemCommand.SpecialOperation.Screen)
          return;
        Game.Memory.Cache("res://resources/screen/" + this.Value + ".tres");
      }
    }

    public override IList<string> GetDependencies()
    {
      if (this.Operation == StoryPlayerSystemCommand.SpecialOperation.MiniGame && this.Value != "end")
        return (IList<string>) new List<string>(1)
        {
          "res://resources/minigame/" + this.Value + ".tres"
        };
      if (this.Operation != StoryPlayerSystemCommand.SpecialOperation.Screen)
        return (IList<string>) Array.Empty<string>();
      return (IList<string>) new List<string>(1)
      {
        "res://resources/screen/" + this.Value + ".tres"
      };
    }

    private void ResumeAfterSave(Control saveMenu)
    {
      Game.InputProcessor = Inputs.Processor.StoryPlayer;
      Game.State.Event = (string) null;
      Game.State.EventLabel = (string) null;
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
      Save,
      Achievement,
      EnableMenu,
      DisableMenu,
      EnableRunning,
      DisableRunning,
    }
  }
}
