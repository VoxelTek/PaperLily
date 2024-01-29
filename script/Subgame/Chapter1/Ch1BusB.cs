using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1BusB : GameRoom
	{
		[Inject]
		private IExtraFunctions Functions;

		[GetNode("Background/chr_juniper")]
		private Sprite nJuniper;

		[GetNode("Background/chr_sai")]
		private Sprite nSai;

		[GetNode("Background/curtains_b")]
		private SimpleAnimatedSprite nCurtains;

		private const float TIME_TO_GET_OFF = 10f;

		private const float TIME_LOST_PER_INTERACTION = 1f;

		public void StartGetOffTimer()
		{
			Functions.StartTimer(11f, delegate
			{
				Game.Events.PlayEvent("ch1_event_bustimeout");
			}, destroyTimerOnTimeout: true);
			Game.StoryPlayer.Connect("DialogueStarted", this, "PauseTimer");
			Game.StoryPlayer.Connect("DialogueEnded", this, "ResumeAndDecreaseTimer");
		}

		public void PauseTimer()
		{
			Functions.GetTimer().PauseTimer();
		}

		public void ResumeAndDecreaseTimer()
		{
			Functions.GetTimer().ResumeTimer();
			Functions.GetTimer().DecreaseTime(1f);
		}

		public void GetOffBus()
		{
			Game.Player.Node.Visible = false;
			Functions.StopTimer();
		}

		public void StopGetOffTimer()
		{
			Functions.StopTimer();
		}

		public void SaiGlance()
		{
			nSai.Frame = 1;
		}

		public void MoveJuniperHead(object body, int frame)
		{
			if (body == Game.Player)
			{
				nJuniper.Frame = frame;
			}
		}

		public void OpenCurtains()
		{
			nCurtains.Playing = true;
		}
	}
}
