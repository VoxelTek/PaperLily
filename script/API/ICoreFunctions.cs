// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.ICoreFunctions
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using System.Diagnostics;

#nullable disable
namespace LacieEngine.API
{
  [InjectableInterface(unique = true)]
  public interface ICoreFunctions
  {
    GameState State { get; }

    PersistentState PersistentState { get; }

    Stopwatch PlaytimeStopwatch { get; }

    void Quit();

    void InitPersistentState();

    void PauseGame();

    void StartGameFromEvent(string eventId);

    void StartGameFromRoom(string room, string point, Vector2 pos, string dir);

    void StartGameFromSave(string saveFile);

    void SwitchToScreen(PackedScene scene);

    void SwitchToScreen(PackedScene scene, bool keepState);

    void OpenScreen(PackedScene scene);

    void AssignInputProcessor();
  }
}
