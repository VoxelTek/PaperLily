// Decompiled with JetBrains decompiler
// Type: LacieEngine.Characters.CharacterManager
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using System;
using System.Collections.Generic;

#nullable disable
namespace LacieEngine.Characters
{
  [Injectable]
  public class CharacterManager : ICharacterManager, IModule, ITranslatable, IStateOverridable
  {
    private const double AssetReloadPeriod = 5.0;
    private Dictionary<string, Character> characters;
    private Dictionary<string, DateTime> lastLoadDate;
    private Dictionary<string, IList<string>> characterResources;

    public ICharacter Get(string id)
    {
      if (this.characters.ContainsKey(id))
        return (ICharacter) this.characters[id];
      Log.Error((object) "Character ", (object) id, (object) " not found!");
      return (ICharacter) null;
    }

    public bool Exists(string id) => this.characters.ContainsKey(id);

    public bool IsMoodValid(string id, string mood)
    {
      return this.characters.ContainsKey(id) && this.characters[id].Moods.Contains(mood);
    }

    public void Init()
    {
      Log.Info((object) "Loading characters...");
      this.characters = new Dictionary<string, Character>();
      this.lastLoadDate = new Dictionary<string, DateTime>();
      this.characterResources = new Dictionary<string, IList<string>>();
      foreach (string str1 in GDUtil.ListFilesInPath("res://definitions/characters/", suffix: ".json", fullPath: false))
      {
        string str2 = str1.Substring(0, str1.LastIndexOf(".json"));
        CharacterDTO characterDto = GDUtil.ReadJsonFile<CharacterDTO>("res://definitions/characters/" + str1);
        Character character = new Character() { Id = str2 };
        character.Name = character.OriginalName = characterDto.Name;
        character.Costumes.AddRange((IEnumerable<string>) characterDto.Costumes);
        character.Moods.AddRange((IEnumerable<string>) this.FindMoodsForCharacter(str2));
        this.GatherResourcesForCharacter(character.Id);
        this.LoadCharacterSpriteData(character, characterDto);
        this.characters.Add(str2, character);
      }
    }

    public IList<string> GetDependencies(string characterId)
    {
      return this.characterResources.ContainsKey(characterId) ? this.characterResources[characterId] : (IList<string>) Array.Empty<string>();
    }

    public void LoadResourcesForCharacter(string characterId)
    {
      if (!this.characters.ContainsKey(characterId) || !this.characterResources.ContainsKey(characterId) || this.lastLoadDate.ContainsKey(characterId) && this.lastLoadDate[characterId].AddSeconds(5.0) > DateTime.Now)
        return;
      this.lastLoadDate[characterId] = DateTime.Now;
      Log.Trace((object) "Loading character resources for ", (object) characterId);
      foreach (string path in (IEnumerable<string>) this.characterResources[characterId])
      {
        if (!path.EndsWith(".json"))
          Game.Memory.Cache(path);
      }
    }

    public void OverrideCharacterName(string characterId, string newName)
    {
      Game.State.OverrideCharacterNames[characterId] = newName;
      this.characters[characterId].Name = newName;
    }

    public void RemoveOverrideCharacterName(string characterId)
    {
      Game.State.OverrideCharacterNames.Remove(characterId);
      if (Game.Language.TranslationEnabled)
        this.characters[characterId].Name = Game.Language.GetCaption(this.characters[characterId].OriginalName);
      else
        this.characters[characterId].Name = this.characters[characterId].OriginalName;
    }

    public void OverrideMood(string characterId, string mood, string newMood)
    {
      Game.State.Overrides["chara." + characterId + ".busts." + mood] = newMood;
    }

    public void RemoveOverrideMoods(string characterId)
    {
      Game.State.Overrides.RemoveAll<string, string>((Func<string, string, bool>) ((k, v) => k.StartsWith("chara." + characterId + ".busts.")));
    }

    public Texture GetMoodTexture(string characterId, string mood)
    {
      if (Game.State.Overrides.ContainsKey("chara." + characterId + ".busts." + mood))
        mood = Game.State.Overrides["chara." + characterId + ".busts." + mood];
      else if (Game.State.Overrides.ContainsKey("chara." + characterId + ".busts.all"))
        mood = Game.State.Overrides["chara." + characterId + ".busts.all"];
      return GD.Load<Texture>("res://assets/img/bust/" + characterId + "/" + mood + ".png");
    }

    private void GatherResourcesForCharacter(string characterId)
    {
      List<string> stringList = new List<string>();
      stringList.Add("res://definitions/characters/" + characterId + ".json");
      if (GDUtil.FolderExists("res://assets/sprite/common/character/" + characterId))
        stringList.AddRange((IEnumerable<string>) GDUtil.ListFilesInPath("res://assets/sprite/common/character/" + characterId + "/", ".png"));
      if (GDUtil.FolderExists("res://assets/img/bust/" + characterId))
        stringList.AddRange((IEnumerable<string>) GDUtil.ListFilesInPath("res://assets/img/bust/" + characterId + "/", ".png"));
      this.characterResources[characterId] = (IList<string>) stringList;
    }

    private IList<string> FindMoodsForCharacter(string characterId)
    {
      if (!GDUtil.FolderExists("res://assets/img/bust/" + characterId))
        return (IList<string>) Array.Empty<string>();
      List<string> moodsForCharacter = new List<string>();
      foreach (string str in GDUtil.ListFilesInPath("res://assets/img/bust/" + characterId + "/", suffix: ".png", fullPath: false))
        moodsForCharacter.Add(str.StripSuffix(".png"));
      return (IList<string>) moodsForCharacter;
    }

    private void LoadCharacterSpriteData(Character character, CharacterDTO characterDto)
    {
      if (!characterDto.NoDefaultSprites)
      {
        character.StateSprites["stand"] = new Character.CharacterStateSprites(Character.DefaultStand);
        character.StateSprites["walk"] = new Character.CharacterStateSprites(Character.DefaultWalk);
        character.StateSprites["run"] = new Character.CharacterStateSprites(Character.DefaultRun);
      }
      foreach (CharacterDTO.CharacterSpriteDTO sprite in characterDto.Sprites)
      {
        if (sprite.State == "idle")
          character.IdleAnimation = true;
        Character.CharacterStateSprites characterStateSprites = new Character.CharacterStateSprites();
        characterStateSprites.TextureVariation = sprite.TextureSuffix.IsNullOrEmpty() ? "" : "_" + sprite.TextureSuffix;
        characterStateSprites.Hframes = sprite.Hframes;
        characterStateSprites.Vframes = sprite.Vframes;
        if (!sprite.Offset.IsNullOrEmpty())
          characterStateSprites.OffsetOverride = new Vector2?(GDUtil.StringToVector2(sprite.Offset));
        if (IsAnimatedSprite(sprite))
        {
          characterStateSprites.Animated = true;
          characterStateSprites.AnimationFps = sprite.Fps;
          characterStateSprites.AnimationLeftFrames = sprite.LeftFrames;
          characterStateSprites.AnimationUpFrames = sprite.UpFrames;
          characterStateSprites.AnimationRightFrames = sprite.RightFrames;
          characterStateSprites.AnimationDownFrames = sprite.DownFrames;
          characterStateSprites.Loop = sprite.Loop;
        }
        else
        {
          int.TryParse(sprite.LeftFrames, out characterStateSprites.LeftFrame);
          int.TryParse(sprite.UpFrames, out characterStateSprites.UpFrame);
          int.TryParse(sprite.RightFrames, out characterStateSprites.RightFrame);
          int.TryParse(sprite.DownFrames, out characterStateSprites.DownFrame);
        }
        character.StateSprites[sprite.State] = characterStateSprites;
      }
      string str1 = "res://assets/sprite/common/character/" + character.Id + "/" + character.Id;
      foreach (Character.CharacterStateSprites characterStateSprites in character.StateSprites.Values)
      {
        string str2 = characterStateSprites.TextureVariation + ".png";
        string str3 = characterStateSprites.TextureVariation + "_n.png";
        characterStateSprites.Texture = GDUtil.FileExists(str1 + str2) ? GD.Load<Texture>(str1 + str2) : (Texture) null;
        characterStateSprites.NormalMap = GDUtil.FileExists(str1 + str3) ? GD.Load<Texture>(str1 + str3) : (Texture) null;
        if (characterStateSprites.Texture == null)
          Log.Error((object) "[CharacterManager] [", (object) character.Id, (object) "] No texture variation found ", (object) characterStateSprites.TextureVariation);
      }

      static bool IsAnimatedSprite(CharacterDTO.CharacterSpriteDTO spriteDto)
      {
        return IsAnimatedFrames(spriteDto.LeftFrames) || IsAnimatedFrames(spriteDto.UpFrames) || IsAnimatedFrames(spriteDto.RightFrames) || IsAnimatedFrames(spriteDto.DownFrames);
      }

      static bool IsAnimatedFrames(string framesStr)
      {
        return !framesStr.IsNullOrEmpty() && framesStr.Split(",").Length > 1;
      }
    }

    public void ApplyTranslationOverrides()
    {
      foreach (Character character in this.characters.Values)
        character.Name = Game.Language.GetCaption(character.OriginalName);
    }

    public void ApplyStateOverrides()
    {
      foreach (Character character in this.characters.Values)
      {
        if (Game.State.OverrideCharacterNames.ContainsKey(character.Id))
          character.Name = Game.State.OverrideCharacterNames[character.Id];
      }
    }

    public void Clean()
    {
      foreach (Character character in this.characters.Values)
        character.Name = character.OriginalName;
    }

    private string CharacterNameContext(ICharacter character) => "characters.name." + character.Id;
  }
}
