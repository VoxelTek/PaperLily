// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerCameraCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;
using System;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerCameraCommand : StoryPlayerCommand
  {
    public StoryPlayerCameraCommand.CameraOperation Operation { get; set; }

    public Vector2 Distance { get; set; }

    public float? Time { get; set; }

    public bool ContinueImmediately { get; set; }

    public string TrackingTarget { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      storyPlayer.SetNextBlockContinue();
      if (Game.Camera != null)
      {
        IGameCamera camera = Game.Camera;
        if (this.Operation == StoryPlayerCameraCommand.CameraOperation.Move)
        {
          float valueOrDefault = this.Time.GetValueOrDefault();
          Vector2 position = camera.Node.Position;
          Vector2 to = camera.Node.Position + this.Distance;
          if ((double) valueOrDefault > 0.0)
          {
            LacieAnimation animation = (LacieAnimation) new PanAnimationNode2D((Node2D) camera.Node, position, to, valueOrDefault);
            Game.Animations.Play(animation);
            if (this.ContinueImmediately)
              storyPlayer.Next();
            else
              animation.Callback = (Action) (() => storyPlayer.Next());
          }
          else
          {
            camera.Node.Position = to;
            storyPlayer.Next();
          }
        }
        else if (this.Operation == StoryPlayerCameraCommand.CameraOperation.Reset)
        {
          if (Game.Player != null && Game.Player.Node.IsValid())
            Game.Camera.TrackPlayer();
          camera.ApplyRoomSettings();
          storyPlayer.Next();
        }
        else if (this.Operation == StoryPlayerCameraCommand.CameraOperation.Shake)
        {
          float valueOrDefault = this.Time.GetValueOrDefault();
          if ((double) valueOrDefault > 0.0)
            Game.Camera.Shake(valueOrDefault);
          else
            Game.Camera.Shake();
          if (this.ContinueImmediately)
            storyPlayer.Next();
          else
            storyPlayer.NextAfterSeconds(valueOrDefault);
        }
        else if (this.Operation == StoryPlayerCameraCommand.CameraOperation.Unlock)
        {
          Game.Camera.Unlock();
          storyPlayer.Next();
        }
        else
        {
          if (this.Operation != StoryPlayerCameraCommand.CameraOperation.Track)
            return;
          if (this.TrackingTarget == "Player")
            Game.Camera.TrackPlayer();
          else if (this.TrackingTarget == null)
          {
            camera.Unlock();
          }
          else
          {
            Node nodeInRoom = Game.Room.FindNodeInRoom(this.TrackingTarget);
            if (nodeInRoom == null)
            {
              Log.Error((object) "Camera tracking target not fouund: ", (object) this.TrackingTarget);
              storyPlayer.Next();
              return;
            }
            Game.Camera.TrackNode((Node2D) nodeInRoom);
          }
          storyPlayer.Next();
        }
      }
      else
      {
        Log.Error((object) "Game camera does not exist. Cannot execute CAMERA block.");
        storyPlayer.Next();
      }
    }

    public enum CameraOperation
    {
      Move,
      Reset,
      Shake,
      Unlock,
      Track,
    }
  }
}
