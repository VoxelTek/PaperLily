using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerBubbleCommand : StoryPlayerCommand
	{
		private const float DefaultWaitTime = 1.5f;

		private static readonly Vector2 DEFAULT_OFFSET_NPC = new Vector2(0f, -65f);

		private static readonly Vector2 DEFAULT_OFFSET_POINT = Vector2.Zero;

		public string Bubble { get; set; }

		public string TargetNode { get; set; }

		public Vector2? Offset { get; set; }

		public float? Time { get; set; }

		public bool ContinueImmediately { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			if (TargetNode == null)
			{
				string text2 = (TargetNode = "Player");
			}
			Vector2? offset = Offset;
			Vector2 valueOrDefault = offset.GetValueOrDefault();
			if (!offset.HasValue)
			{
				valueOrDefault = DEFAULT_OFFSET_NPC;
				Vector2? vector2 = (Offset = valueOrDefault);
			}
			if (Game.Room.FindNodeInRoom(TargetNode) == null)
			{
				Log.Error("Invalid parent node for bubble: ", TargetNode);
				storyPlayer.SetNextBlockContinue();
				storyPlayer.Next();
				return;
			}
			Node2D root = Game.Room.FindNodeInRoom(TargetNode) as Node2D;
			Node2D bubble = GDUtil.Instance<Node2D>("res://resources/nodes/common/bubble/" + Bubble + ".tscn");
			storyPlayer.HideAllUI();
			bubble.Position = root.GlobalPosition + Offset.Value;
			Game.CurrentRoom.AddChild(bubble);
			float time = Time ?? 1.5f;
			storyPlayer.SetNextBlockContinue();
			if (ContinueImmediately || time <= 0f)
			{
				storyPlayer.Next();
			}
			else
			{
				storyPlayer.NextAfterSeconds(time);
			}
		}

		public override IList<string> GetDependencies()
		{
			return new List<string>(1) { "res://resources/nodes/common/bubble/" + Bubble + ".tscn" };
		}
	}
}
