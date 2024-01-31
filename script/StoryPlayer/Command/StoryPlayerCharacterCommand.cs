// Decompiled with JetBrains decompiler
// Type: LacieEngine.StoryPlayer.StoryPlayerCharacterCommand
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Nodes;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.StoryPlayer
{
  [Serializable]
  public class StoryPlayerCharacterCommand : StoryPlayerCommand
  {
    [Inject]
    [NonSerialized]
    private ICharacterManager Characters;

    public StoryPlayerCharacterCommand.CharacterOperation Operation { get; set; }

    public string Character { get; set; }

    public string Position { get; set; }

    public string Mood { get; set; }

    public string NewName { get; set; }

    public string OldMood { get; set; }

    public string NewMood { get; set; }

    public string Point { get; set; }

    public override void Execute(LacieEngine.StoryPlayer.StoryPlayer storyPlayer)
    {
      switch (this.Operation)
      {
        case StoryPlayerCharacterCommand.CharacterOperation.Show:
          if (!storyPlayer.Characters.IsCharacterSet(this.Character) || !this.Position.IsNullOrEmpty())
            storyPlayer.Characters.SetCharacter(this.Character, this.Position);
          storyPlayer.Characters.ShowCharacter(this.Character, this.Mood);
          break;
        case StoryPlayerCharacterCommand.CharacterOperation.Set:
          storyPlayer.Characters.SetCharacter(this.Character, this.Position);
          break;
        case StoryPlayerCharacterCommand.CharacterOperation.Hide:
          storyPlayer.Characters.HideCharacter(this.Character);
          break;
        case StoryPlayerCharacterCommand.CharacterOperation.PartySwitch:
          Game.State.Party.Clear();
          Game.State.Party.Add(this.Character);
          break;
        case StoryPlayerCharacterCommand.CharacterOperation.PartyAdd:
          if (!Game.State.Party.Contains(this.Character))
          {
            Game.State.Party.Add(this.Character);
            break;
          }
          break;
        case StoryPlayerCharacterCommand.CharacterOperation.PartyRemove:
          Game.State.Party.Remove(this.Character);
          break;
        case StoryPlayerCharacterCommand.CharacterOperation.Lock:
          storyPlayer.Characters.Locked = true;
          break;
        case StoryPlayerCharacterCommand.CharacterOperation.Unlock:
          storyPlayer.Characters.Locked = false;
          break;
        case StoryPlayerCharacterCommand.CharacterOperation.Rename:
          if (!this.NewName.IsNullOrEmpty())
          {
            this.Characters.OverrideCharacterName(this.Character, this.NewName);
            break;
          }
          this.Characters.RemoveOverrideCharacterName(this.Character);
          break;
        case StoryPlayerCharacterCommand.CharacterOperation.Spawn:
          NPCWalker npcWalker = new NPCWalker(this.Character);
          Game.CurrentRoom.GetMainLayer().AddChild((Node) npcWalker);
          npcWalker.Position = Game.CurrentRoom.GetPoint(this.Point);
          npcWalker.Teleport();
          npcWalker.Turn((Direction) Game.CurrentRoom.GetSpawnPoint(this.Point).Direction);
          break;
        case StoryPlayerCharacterCommand.CharacterOperation.Despawn:
          string pascalCase = this.Character.ToPascalCase();
          Game.CurrentRoom.GetMainLayer().GetNode((NodePath) pascalCase).DeleteIfValid();
          break;
        case StoryPlayerCharacterCommand.CharacterOperation.ReMood:
          if (!this.NewMood.IsNullOrEmpty())
          {
            this.Characters.OverrideMood(this.Character, this.OldMood, this.NewMood);
            break;
          }
          this.Characters.RemoveOverrideMoods(this.Character);
          break;
      }
      storyPlayer.SetNextBlockContinue();
      storyPlayer.Next();
    }

    public override void Load()
    {
      if (this.Character.IsNullOrEmpty())
        return;
      this.Characters.LoadResourcesForCharacter(this.Character);
    }

    public override IList<string> GetDependencies()
    {
      return !this.Character.IsNullOrEmpty() ? this.Characters.GetDependencies(this.Character) : (IList<string>) Array.Empty<string>();
    }

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
      ReMood,
    }

    public enum CharacterPosition
    {
      Left,
      Right,
    }
  }
}
