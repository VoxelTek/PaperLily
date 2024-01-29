using System;
using LacieEngine.Nodes;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IExtraFunctions
	{
		void StartTimer(float seconds, Action onTimeout, bool destroyTimerOnTimeout = false);

		void StopTimer();

		EventTimer GetTimer();
	}
}
