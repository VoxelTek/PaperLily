// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerActionCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;
using System;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerActionCommand : StoryPlayerCommand
  {
    public StoryPlayerActionCommand.ActionOperation Operation { get; set; }

    public string TargetNode { get; set; }

    public Vector2? Point { get; set; }

    public string PointName { get; set; }

    public string Path { get; set; }

    public Direction Direction { get; set; }

    public int DistanceX { get; set; }

    public int DistanceY { get; set; }

    public bool ContinueImmediately { get; set; }

    public IWalker.MoveSpeed WalkSpeed { get; set; }

    public bool WalkBackwards { get; set; }

    public string State { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      storyPlayer.SetNextBlockContinue();
      switch (this.Operation)
      {
        case StoryPlayerActionCommand.ActionOperation.Walk:
          if (Game.Room.FindNodeInRoom(this.TargetNode) is IWalker nodeInRoom1)
          {
            nodeInRoom1.Backwards = this.WalkBackwards;
            Vector2? point1 = this.Point;
            if (point1.HasValue || !this.PointName.IsNullOrEmpty())
            {
              if (!this.PointName.IsNullOrEmpty())
                this.Point = new Vector2?(Game.CurrentRoom.GetPoint(this.PointName));
              if (!this.ContinueImmediately)
              {
                IWalker walker = nodeInRoom1;
                point1 = this.Point;
                Vector2 point2 = point1.Value;
                Action callback = (Action) (() => storyPlayer.Next());
                walker.WalkToPoint(point2, callback);
                break;
              }
              IWalker walker1 = nodeInRoom1;
              point1 = this.Point;
              Vector2 point3 = point1.Value;
              walker1.WalkToPoint(point3);
              storyPlayer.Next();
              break;
            }
            if (!this.Path.IsNullOrEmpty())
            {
              if (!this.ContinueImmediately)
              {
                nodeInRoom1.WalkPath(this.Path, this.WalkSpeed, (Action) (() => storyPlayer.Next()));
                break;
              }
              nodeInRoom1.WalkPath(this.Path, this.WalkSpeed);
              storyPlayer.Next();
              break;
            }
            if (!this.ContinueImmediately)
            {
              nodeInRoom1.Walk(this.DistanceX, this.DistanceY, this.Direction, this.WalkSpeed, (Action) (() => storyPlayer.Next()));
              break;
            }
            nodeInRoom1.Walk(this.DistanceX, this.DistanceY, this.Direction, this.WalkSpeed);
            storyPlayer.Next();
            break;
          }
          Log.Warn((object) "IWalker node not found: ", (object) this.TargetNode);
          break;
        case StoryPlayerActionCommand.ActionOperation.Turn:
          if (Game.Room.FindNodeInRoom(this.TargetNode) is ITurnable nodeInRoom2)
            nodeInRoom2.Turn(this.Direction);
          else
            Log.Warn((object) "ITurnable node not found: ", (object) this.TargetNode);
          storyPlayer.Next();
          break;
        case StoryPlayerActionCommand.ActionOperation.Teleport:
          if (this.Point.HasValue || !this.PointName.IsNullOrEmpty())
          {
            if (!this.PointName.IsNullOrEmpty())
              this.Point = new Vector2?(Game.CurrentRoom.GetPoint(this.PointName));
            if (Game.Room.FindNodeInRoom(this.TargetNode) is Node2D nodeInRoom3)
            {
              Vector2 vector2 = this.Point.Value;
              nodeInRoom3.Position = vector2;
            }
            else
              Log.Warn((object) "Node2D not found: ", (object) this.TargetNode);
          }
          storyPlayer.Next();
          break;
        case StoryPlayerActionCommand.ActionOperation.IdleStart:
          if (Game.Room.FindNodeInRoom(this.TargetNode) is IIdle nodeInRoom4)
            nodeInRoom4.StartIdleAnimation();
          else
            Log.Warn((object) "IIdle node not found: ", (object) this.TargetNode);
          storyPlayer.Next();
          break;
        case StoryPlayerActionCommand.ActionOperation.IdleStop:
          if (Game.Room.FindNodeInRoom(this.TargetNode) is IIdle nodeInRoom5)
            nodeInRoom5.StopIdleAnimation();
          else
            Log.Warn((object) "IIdle node not found: ", (object) this.TargetNode);
          storyPlayer.Next();
          break;
        case StoryPlayerActionCommand.ActionOperation.State:
          switch (Game.Room.FindNodeInRoom(this.TargetNode))
          {
            case CharacterSprite characterSprite:
              characterSprite.State = this.State;
              break;
            case WalkingCharacter walkingCharacter:
              walkingCharacter.OverrideSpriteState = true;
              walkingCharacter.SpriteState = this.State;
              break;
            default:
              Log.Warn((object) "CharacterSprite or WalkingCharacter node not found: ", (object) this.TargetNode);
              break;
          }
          storyPlayer.Next();
          break;
      }
    }

    public enum ActionOperation
    {
      Walk,
      Turn,
      Teleport,
      IdleStart,
      IdleStop,
      State,
    }
  }
}
