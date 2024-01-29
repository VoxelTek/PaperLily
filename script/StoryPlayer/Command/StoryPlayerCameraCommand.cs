using System;
using Godot;
using LacieEngine.Animation;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerCameraCommand : StoryPlayerCommand
	{
		public enum CameraOperation
		{
			Move,
			Reset,
			Shake,
			Unlock,
			Track
		}

		public CameraOperation Operation { get; set; }

		public Vector2 Distance { get; set; }

		public float? Time { get; set; }

		public bool ContinueImmediately { get; set; }

		public string TrackingTarget { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			storyPlayer.SetNextBlockContinue();
			if (Game.Camera != null)
			{
				IGameCamera camera = Game.Camera;
				if (Operation == CameraOperation.Move)
				{
					float time2 = Time.GetValueOrDefault();
					Vector2 from = camera.Node.Position;
					Vector2 to = camera.Node.Position + Distance;
					if (time2 > 0f)
					{
						LacieAnimation animation = new PanAnimationNode2D(camera.Node, from, to, time2);
						Game.Animations.Play(animation);
						if (ContinueImmediately)
						{
							storyPlayer.Next();
							return;
						}
						animation.Callback = delegate
						{
							storyPlayer.Next();
						};
					}
					else
					{
						camera.Node.Position = to;
						storyPlayer.Next();
					}
				}
				else if (Operation == CameraOperation.Reset)
				{
					if (Game.Player != null && Game.Player.Node.IsValid())
					{
						Game.Camera.TrackPlayer();
					}
					camera.ApplyRoomSettings();
					storyPlayer.Next();
				}
				else if (Operation == CameraOperation.Shake)
				{
					float time = Time.GetValueOrDefault();
					if (time > 0f)
					{
						Game.Camera.Shake(time);
					}
					else
					{
						Game.Camera.Shake();
					}
					if (ContinueImmediately)
					{
						storyPlayer.Next();
					}
					else
					{
						storyPlayer.NextAfterSeconds(time);
					}
				}
				else if (Operation == CameraOperation.Unlock)
				{
					Game.Camera.Unlock();
					storyPlayer.Next();
				}
				else
				{
					if (Operation != CameraOperation.Track)
					{
						return;
					}
					if (TrackingTarget == "Player")
					{
						Game.Camera.TrackPlayer();
					}
					else if (TrackingTarget == null)
					{
						camera.Unlock();
					}
					else
					{
						Node target = Game.Room.FindNodeInRoom(TrackingTarget);
						if (target == null)
						{
							Log.Error("Camera tracking target not fouund: ", TrackingTarget);
							storyPlayer.Next();
							return;
						}
						Game.Camera.TrackNode((Node2D)target);
					}
					storyPlayer.Next();
				}
			}
			else
			{
				Log.Error("Game camera does not exist. Cannot execute CAMERA block.");
				storyPlayer.Next();
			}
		}
	}
}
