using System;
using Godot;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerActionCommand : StoryPlayerCommand
	{
		public enum ActionOperation
		{
			Walk,
			Turn,
			Teleport,
			IdleStart,
			IdleStop,
			State
		}

		public ActionOperation Operation { get; set; }

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

		public override void Execute(StoryPlayer storyPlayer)
		{
			storyPlayer.SetNextBlockContinue();
			switch (Operation)
			{
			case ActionOperation.Walk:
				if (Game.Room.FindNodeInRoom(TargetNode) is IWalker walker)
				{
					walker.Backwards = WalkBackwards;
					if (Point.HasValue || !PointName.IsNullOrEmpty())
					{
						if (!PointName.IsNullOrEmpty())
						{
							Point = Game.CurrentRoom.GetPoint(PointName);
						}
						if (!ContinueImmediately)
						{
							walker.WalkToPoint(Point.Value, delegate
							{
								storyPlayer.Next();
							});
						}
						else
						{
							walker.WalkToPoint(Point.Value);
							storyPlayer.Next();
						}
					}
					else if (!Path.IsNullOrEmpty())
					{
						if (!ContinueImmediately)
						{
							walker.WalkPath(Path, WalkSpeed, delegate
							{
								storyPlayer.Next();
							});
						}
						else
						{
							walker.WalkPath(Path, WalkSpeed);
							storyPlayer.Next();
						}
					}
					else if (!ContinueImmediately)
					{
						walker.Walk(DistanceX, DistanceY, Direction, WalkSpeed, delegate
						{
							storyPlayer.Next();
						});
					}
					else
					{
						walker.Walk(DistanceX, DistanceY, Direction, WalkSpeed);
						storyPlayer.Next();
					}
				}
				else
				{
					Log.Warn("IWalker node not found: ", TargetNode);
				}
				break;
			case ActionOperation.Turn:
				if (Game.Room.FindNodeInRoom(TargetNode) is ITurnable turnable)
				{
					turnable.Turn(Direction);
				}
				else
				{
					Log.Warn("ITurnable node not found: ", TargetNode);
				}
				storyPlayer.Next();
				break;
			case ActionOperation.Teleport:
				if (Point.HasValue || !PointName.IsNullOrEmpty())
				{
					if (!PointName.IsNullOrEmpty())
					{
						Point = Game.CurrentRoom.GetPoint(PointName);
					}
					if (Game.Room.FindNodeInRoom(TargetNode) is Node2D node2d)
					{
						node2d.Position = Point.Value;
					}
					else
					{
						Log.Warn("Node2D not found: ", TargetNode);
					}
				}
				storyPlayer.Next();
				break;
			case ActionOperation.IdleStart:
				if (Game.Room.FindNodeInRoom(TargetNode) is IIdle idle)
				{
					idle.StartIdleAnimation();
				}
				else
				{
					Log.Warn("IIdle node not found: ", TargetNode);
				}
				storyPlayer.Next();
				break;
			case ActionOperation.IdleStop:
				if (Game.Room.FindNodeInRoom(TargetNode) is IIdle idleStop)
				{
					idleStop.StopIdleAnimation();
				}
				else
				{
					Log.Warn("IIdle node not found: ", TargetNode);
				}
				storyPlayer.Next();
				break;
			case ActionOperation.State:
			{
				Node node = Game.Room.FindNodeInRoom(TargetNode);
				if (node is CharacterSprite characterSprite)
				{
					characterSprite.State = State;
				}
				else if (node is WalkingCharacter walkingCharacter)
				{
					walkingCharacter.OverrideSpriteState = true;
					walkingCharacter.SpriteState = State;
				}
				else
				{
					Log.Warn("CharacterSprite or WalkingCharacter node not found: ", TargetNode);
				}
				storyPlayer.Next();
				break;
			}
			}
		}
	}
}
