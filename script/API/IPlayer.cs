// Decompiled with JetBrains decompiler
// Type: LacieEngine.API.IPlayer
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

#nullable disable
namespace LacieEngine.API
{
  public interface IPlayer : IPhysicsInterpolated
  {
    bool Exists => this.Node != null && this.Node.IsValid();

    bool SneakingEnabled { get; set; }

    KinematicBody2D Node { get; }

    string SpriteState { get; set; }

    Direction SpriteDirection { get; set; }

    bool CollisionEnabled { get; set; }

    void SetLight(Light2D light);

    Light2D GetLight();

    Vector2 GetCenter();

    EventTrigger GetInteractingNode();

    EventTrigger GetItemInteractingNode(string itemId);

    Direction GetDirection();

    void SetDirection(Direction dir);

    void DisableRunning();

    void EnableRunning();

    CharacterState GetPlayerState();

    void SetPlayerState(CharacterState state);

    void Turn(Direction direction);
  }
}
