using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.Animation
{
	public abstract class LacieAnimation
	{
		private object _waitUntilFinishedLock = new object();

		public Node Node { get; protected set; }

		public bool Playing { get; protected set; }

		public bool Finished { get; protected set; }

		public float Duration { get; protected set; }

		public float Elapsed { get; protected set; }

		public Action Callback { get; set; }

		public void Begin()
		{
			if (Node.IsValid())
			{
				InitialCalculations();
				UpdateToFirstFrame();
			}
			Playing = true;
		}

		public void Play(float delta)
		{
			Elapsed += delta;
			if (Elapsed < Duration && Node.IsValid())
			{
				UpdateToCurrentFrame();
			}
			else
			{
				Finish();
			}
		}

		public void Finish()
		{
			Playing = false;
			if (Node.IsValid())
			{
				UpdateToFinalFrame();
			}
			Monitor.Enter(_waitUntilFinishedLock);
			Finished = true;
			Monitor.PulseAll(_waitUntilFinishedLock);
			Monitor.Exit(_waitUntilFinishedLock);
			Callback?.Invoke();
		}

		public virtual void InitialCalculations()
		{
		}

		public abstract void UpdateToFirstFrame();

		public abstract void UpdateToCurrentFrame();

		public abstract void UpdateToFinalFrame();

		public async Task WaitUntilFinished()
		{
			await Task.Run((Action)WaitUntilFinishedProc);
		}

		private void WaitUntilFinishedProc()
		{
			Monitor.Enter(_waitUntilFinishedLock);
			while (!Finished)
			{
				Monitor.Wait(_waitUntilFinishedLock);
			}
			Monitor.Exit(_waitUntilFinishedLock);
		}
	}
}
