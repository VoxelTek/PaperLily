using System;
using System.Collections.Generic;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerCharacterCommand : StoryPlayerCommand
	{
		public enum CharacterOperation
		{
			Show,
			Set,
			Hide,
			PartySwitch,
			PartyAdd,
			PartyRemove,
			Lock,
			Unlock,
			Rename,
			Spawn,
			Despawn,
			ReMood
		}

		public enum CharacterPosition
		{
			Left,
			Right
		}

		[NonSerialized]
		[Inject]
		private ICharacterManager Characters;

		public CharacterOperation Operation { get; set; }

		public string Character { get; set; }

		public string Position { get; set; }

		public string Mood { get; set; }

		public string NewName { get; set; }

		public string OldMood { get; set; }

		public string NewMood { get; set; }

		public string Point { get; set; }

		public override void Execute(StoryPlayer storyPlayer)
		{
			switch (Operation)
			{
			case CharacterOperation.Show:
				if (!storyPlayer.Characters.IsCharacterSet(Character) || !Position.IsNullOrEmpty())
				{
					storyPlayer.Characters.SetCharacter(Character, Position);
				}
				storyPlayer.Characters.ShowCharacter(Character, Mood);
				break;
			case CharacterOperation.Set:
				storyPlayer.Characters.SetCharacter(Character, Position);
				break;
			case CharacterOperation.Hide:
				storyPlayer.Characters.HideCharacter(Character);
				break;
			case CharacterOperation.PartySwitch:
				Game.State.Party.Clear();
				Game.State.Party.Add(Character);
				break;
			case CharacterOperation.PartyAdd:
				if (!Game.State.Party.Contains(Character))
				{
					Game.State.Party.Add(Character);
				}
				break;
			case CharacterOperation.PartyRemove:
				Game.State.Party.Remove(Character);
				break;
			case CharacterOperation.Lock:
				storyPlayer.Characters.Locked = true;
				break;
			case CharacterOperation.Unlock:
				storyPlayer.Characters.Locked = false;
				break;
			case CharacterOperation.Rename:
				if (!NewName.IsNullOrEmpty())
				{
					Characters.OverrideCharacterName(Character, NewName);
				}
				else
				{
					Characters.RemoveOverrideCharacterName(Character);
				}
				break;
			case CharacterOperation.ReMood:
				if (!NewMood.IsNullOrEmpty())
				{
					Characters.OverrideMood(Character, OldMood, NewMood);
				}
				else
				{
					Characters.RemoveOverrideMoods(Character);
				}
				break;
			case CharacterOperation.Spawn:
			{
				NPCWalker npc = new NPCWalker(Character);
				Game.CurrentRoom.GetMainLayer().AddChild(npc);
				npc.Position = Game.CurrentRoom.GetPoint(Point);
				npc.Teleport();
				npc.Turn(Game.CurrentRoom.GetSpawnPoint(Point).Direction);
				break;
			}
			case CharacterOperation.Despawn:
			{
				string nodeName = Character.ToPascalCase();
				Game.CurrentRoom.GetMainLayer().GetNode(nodeName).DeleteIfValid();
				break;
			}
			}
			storyPlayer.SetNextBlockContinue();
			storyPlayer.Next();
		}

		public override void Load()
		{
			if (!Character.IsNullOrEmpty())
			{
				Characters.LoadResourcesForCharacter(Character);
			}
		}

		public override IList<string> GetDependencies()
		{
			if (!Character.IsNullOrEmpty())
			{
				return Characters.GetDependencies(Character);
			}
			return Array.Empty<string>();
		}
	}
}
