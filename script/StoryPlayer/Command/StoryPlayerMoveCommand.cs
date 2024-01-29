using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerMoveCommand : StoryPlayerCommand
	{
		public string Room { get; set; }

		public string Point { get; set; }

		public Vector2? Coordinates { get; set; }

		public string Direction { get; set; }

		public float? Time { get; set; }

		public bool Cutscene { get; set; }

		public override void Load()
		{
		}

		public override IList<string> GetDependencies()
		{
			return new List<string>(1) { "res://resources/nodes/rooms/" + Room + ".tscn" };
		}

		public override void Execute(StoryPlayer storyPlayer)
		{
			storyPlayer.UI.HideDialogueBox();
			storyPlayer.Characters.HideAllCharacters();
			ChangeRoom(storyPlayer);
		}

		private void ChangeRoom(StoryPlayer storyPlayer)
		{
			if (!Point.IsNullOrEmpty())
			{
				Game.Room.ChangeRoom(Room, Point, Vector2.Zero, Direction, Time ?? null, storyPlayer.Event || Cutscene);
			}
			else
			{
				Game.Room.ChangeRoom(Room, null, Coordinates.Value, Direction, Time ?? null, storyPlayer.Event || Cutscene);
			}
		}
	}
}
