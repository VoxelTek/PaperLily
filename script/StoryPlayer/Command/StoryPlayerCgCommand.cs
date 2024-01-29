using System;
using System.Collections.Generic;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerCgCommand : StoryPlayerCommand
	{
		public enum CGOperation
		{
			Show,
			Hide,
			Pan
		}

		public enum CGPosition
		{
			Fill,
			Top,
			Bottom,
			AboveText
		}

		public enum TransitionType
		{
			Fade,
			Instant
		}

		public CGOperation Operation { get; set; }

		public CgDisplayManager.CgLayer Layer { get; set; }

		public CGPosition Position { get; set; }

		public string Image { get; set; }

		public float? Time { get; set; }

		public bool Mini { get; set; }

		public bool Scene { get; set; }

		public Direction Direction { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			storyPlayer.CG.Execute(this);
			storyPlayer.SetNextBlockContinue();
			storyPlayer.Next();
		}

		public override IList<string> GetDependencies()
		{
			if (Operation == CGOperation.Show)
			{
				if (Scene)
				{
					return new List<string>(1) { "res://resources/nodes/cg/" + Image + ".tscn" };
				}
				return new List<string>(1) { "res://assets/img/cg/" + Image + ".png" };
			}
			return Array.Empty<string>();
		}
	}
}
