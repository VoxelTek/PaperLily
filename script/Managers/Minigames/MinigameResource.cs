// Decompiled with JetBrains decompiler
// Type: LacieEngine.Minigames.MinigameResource
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;

#nullable disable
namespace LacieEngine.Minigames
{
  [ExportType]
  public class MinigameResource : Resource
  {
    [Export(PropertyHint.None, "")]
    public PackedScene MinigameScene { get; private set; }

    public IMinigame MakeMinigame()
    {
      IMinigame minigame = this.MinigameScene.Instance<IMinigame>();
      Injector.Resolve((object) minigame);
      minigame.Node.InjectNodes();
      minigame.Init();
      return minigame;
    }
  }
}
