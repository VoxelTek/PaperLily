// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.IGameRoomManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Nodes;
using LacieEngine.Rooms;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
namespace LacieEngine.API
{
  [InjectableInterface(unique = true)]
  public interface IGameRoomManager : IModule
  {
    IPlayer Player { get; }

    GameRoom CurrentRoom { get; }

    IGameCamera Camera { get; }

    bool Cutscene { get; }

    bool Visible { get; set; }

    List<IReflectable> MirrorReflections { get; }

    List<IAction> RegisteredRoomUpdates { get; }

    Dictionary<string, IAction> RegisteredRoomActions { get; }

    Dictionary<string, Node2D> RegisteredNPCs { get; }

    Dictionary<string, SpawnPoint> RegisteredPoints { get; }

    Color Modulate { get; set; }

    Task ChangeRoom(
      string room,
      string point,
      Vector2 pos,
      string dir,
      float? time,
      bool cutscene);

    Task ChangeRoom(string room, string point, Vector2 pos, string dir, float? time);

    Task ChangeRoom(string room, string point, Vector2 pos, string dir);

    void RoomFunction(string function);

    void UpdateRoomState();

    Node FindNodeInRoom(string nodeName);

    Path2D GetPath(string pathName);

    void ApplyLighting(Resource lighting);

    void ResetLighting();

    void ApplyRoomShader(Material material);

    void RegisterMirrorReflection(IReflectable reflectable);

    void RefreshPixelScaling();
  }
}
