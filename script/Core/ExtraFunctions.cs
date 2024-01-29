using System;
using LacieEngine.API;
using LacieEngine.Nodes;

namespace LacieEngine.Core
{
	[Injectable]
	public class ExtraFunctions : IExtraFunctions
	{
		private EventTimer nEventTimer;

		public void StartTimer(float seconds, Action onTimeout, bool destroyTimerOnTimeout = false)
		{
			StopTimer();
			nEventTimer = new EventTimer();
			nEventTimer.SelfDestructWhenFinished = destroyTimerOnTimeout;
			Game.Screen.AddToLayer(IScreenManager.Layer.Pixel, nEventTimer);
			nEventTimer.StartTimer(seconds, onTimeout);
		}

		public void StopTimer()
		{
			if (nEventTimer.IsValid())
			{
				nEventTimer.StopTimer();
				nEventTimer.Delete();
				nEventTimer = null;
			}
		}

		public EventTimer GetTimer()
		{
			return nEventTimer;
		}
	}
}
