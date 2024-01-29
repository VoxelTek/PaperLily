using Godot;
using LacieEngine.Core;
using Steamworks;

namespace LacieEngine.Nodes
{
	public class SteamUpdater : Node
	{
		protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

		public SteamUpdater()
		{
			base.Name = "SteamUpdater";
			base.PauseMode = PauseModeEnum.Process;
		}

		public override void _Ready()
		{
			m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
		}

		public override void _Process(float delta)
		{
			SteamAPI.RunCallbacks();
		}

		public override void _Notification(int what)
		{
			if (what == 1006)
			{
				SteamAPI.Shutdown();
			}
		}

		public void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
		{
			if (pCallback.m_bActive != 0)
			{
				if (InputMapper.CurrentProfile.Type == InputProfile.InputType.Controller && Game.InputProcessor == Inputs.Processor.Player)
				{
					Game.Core.PauseGame();
				}
			}
			else
			{
				Game.Tree.Paused = false;
			}
		}
	}
}
