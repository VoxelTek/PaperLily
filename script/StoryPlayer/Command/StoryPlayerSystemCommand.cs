using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.UI;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerSystemCommand : StoryPlayerCommand
	{
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
			DisableRunning
		}

		public const string MINIGAME_END = "end";

		[NonSerialized]
		[Inject]
		private IAchievementManager Achievements;

		public SpecialOperation Operation { get; set; }

		public string Value { get; set; }

		public string SaveEventLabel { get; set; }

		public string SaveLocation { get; set; }

		public string SaveImage { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			if (Operation == SpecialOperation.RefreshRoom)
			{
				Game.Room.UpdateRoomState();
			}
			else if (Operation == SpecialOperation.CallRoomFunction)
			{
				bool called = false;
				if (Game.Minigames.Running)
				{
					called = Game.Minigames.Function(Value);
				}
				if (!called)
				{
					Game.Room.RoomFunction(Value);
				}
			}
			else if (Operation == SpecialOperation.MiniGame)
			{
				if (Value != "end")
				{
					Game.Minigames.Start(Value);
				}
				else
				{
					Game.Minigames.End();
				}
			}
			else if (Operation == SpecialOperation.Screen)
			{
				ScreenProxyResource proxy = GD.Load<ScreenProxyResource>("res://resources/screen/" + Value + ".tres");
				Game.Core.OpenScreen(proxy.Scene);
			}
			else if (Operation == SpecialOperation.EndGame)
			{
				Game.Core.SwitchToScreen(Game.Settings.SceneProvider.CreditsScreen, keepState: true);
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
				{
					Game.Items.ClearInventory();
				}
				else if (Operation == SpecialOperation.AutoSave)
				{
					GameState.Save("autosave");
				}
				else if (Operation == SpecialOperation.RetrySave)
				{
					GameState.Save("retrysave", quiet: true);
				}
				else if (Operation == SpecialOperation.RetryLoad)
				{
					Game.Core.StartGameFromSave("retrysave");
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
						SaveScreenMenuContainer saveContainer = GDUtil.MakeNode<SaveScreenMenuContainer>("SaveContainer");
						saveContainer.OnClose = delegate
						{
							ResumeAfterSave(saveContainer);
						};
						Game.Screen.AddToLayer(IScreenManager.Layer.UI, saveContainer);
						saveContainer.Menu.ResetSelection();
						storyPlayer.SetNextBlockContinue();
						return;
					}
					if (Operation == SpecialOperation.Achievement)
					{
						Achievements.Unlock(Value);
					}
					else if (Operation == SpecialOperation.DisableMenu)
					{
						Game.State.MenuDisabled = true;
					}
					else if (Operation == SpecialOperation.EnableMenu)
					{
						Game.State.MenuDisabled = false;
					}
					else if (Operation == SpecialOperation.DisableRunning)
					{
						Game.Player.DisableRunning();
					}
					else if (Operation == SpecialOperation.EnableRunning)
					{
						Game.Player.EnableRunning();
					}
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
			else if (Operation == SpecialOperation.Screen)
			{
				Game.Memory.Cache("res://resources/screen/" + Value + ".tres");
			}
		}

		public override IList<string> GetDependencies()
		{
			if (Operation == SpecialOperation.MiniGame && Value != "end")
			{
				return new List<string>(1) { "res://resources/minigame/" + Value + ".tres" };
			}
			if (Operation == SpecialOperation.Screen)
			{
				return new List<string>(1) { "res://resources/screen/" + Value + ".tres" };
			}
			return Array.Empty<string>();
		}

		private void ResumeAfterSave(Control saveMenu)
		{
			Game.InputProcessor = Inputs.Processor.StoryPlayer;
			Game.State.Event = null;
			Game.State.EventLabel = null;
			saveMenu.Delete();
			Game.StoryPlayer.Next();
		}
	}
}
