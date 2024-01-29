using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Injectable]
	public class MemoryManager : Node, IMemoryManager, IModule
	{
		private Dictionary<string, Resource> systemCache;

		private Dictionary<string, Resource> userCache;

		private Queue<string> _loadQueue;

		private Task _task;

		public void Init()
		{
			if (!IsInsideTree())
			{
				base.Name = "CacheAgent";
				systemCache = new Dictionary<string, Resource>();
				userCache = new Dictionary<string, Resource>();
				_loadQueue = new Queue<string>();
				Game.Root.AddChild(this);
			}
		}

		public void Cache(string path)
		{
			if (!userCache.ContainsKey(path))
			{
				_loadQueue.Enqueue(path);
			}
		}

		public void SystemCache(string path)
		{
			if (!systemCache.ContainsKey(path))
			{
				systemCache[path] = ResourceLoader.Load(path);
			}
		}

		public void Clean()
		{
			userCache.Clear();
			ForceGC();
		}

		public async Task WaitForCompletion()
		{
			StartBackgroundLoading();
			while (!_loadQueue.IsEmpty())
			{
				await GDUtil.DelayOneFrame();
			}
		}

		private void ForceGC()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}

		private void StartBackgroundLoading()
		{
			if (_task == null || _task.IsCompleted)
			{
				_task = Task.Run(delegate
				{
					BackgroundLoadingProc();
				});
			}
		}

		private void BackgroundLoadingProc()
		{
			while (!_loadQueue.IsEmpty())
			{
				if (!_loadQueue.IsEmpty())
				{
					string path = _loadQueue.Peek();
					userCache[path] = ResourceLoader.Load(path);
					_loadQueue.Dequeue();
				}
			}
		}
	}
}
