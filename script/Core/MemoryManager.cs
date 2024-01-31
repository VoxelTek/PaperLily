// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.MemoryManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
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
      if (this.IsInsideTree())
        return;
      this.Name = "CacheAgent";
      this.systemCache = new Dictionary<string, Resource>();
      this.userCache = new Dictionary<string, Resource>();
      this._loadQueue = new Queue<string>();
      Game.Root.AddChild((Node) this);
    }

    public void Cache(string path)
    {
      if (this.userCache.ContainsKey(path))
        return;
      this._loadQueue.Enqueue(path);
    }

    public void SystemCache(string path)
    {
      if (this.systemCache.ContainsKey(path))
        return;
      this.systemCache[path] = ResourceLoader.Load(path);
    }

    public void Clean()
    {
      this.userCache.Clear();
      this.ForceGC();
    }

    public async Task WaitForCompletion()
    {
      this.StartBackgroundLoading();
      while (!this._loadQueue.IsEmpty<string>())
        await GDUtil.DelayOneFrame();
    }

    private void ForceGC()
    {
      GC.Collect();
      GC.WaitForPendingFinalizers();
      GC.Collect();
    }

    private void StartBackgroundLoading()
    {
      if (this._task != null && !this._task.IsCompleted)
        return;
      this._task = Task.Run((Action) (() => this.BackgroundLoadingProc()));
    }

    private void BackgroundLoadingProc()
    {
      while (!this._loadQueue.IsEmpty<string>())
      {
        if (!this._loadQueue.IsEmpty<string>())
        {
          string str = this._loadQueue.Peek();
          this.userCache[str] = ResourceLoader.Load(str);
          this._loadQueue.Dequeue();
        }
      }
    }
  }
}
