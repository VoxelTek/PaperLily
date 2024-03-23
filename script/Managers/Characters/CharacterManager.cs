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
            if (characters.ContainsKey(id))
                return characters[id];
            Log.Error("Character ", id, " not found!");
            return null;
        }

        public bool Exists(string id) => characters.ContainsKey(id);

        public bool IsMoodValid(string id, string mood)
        {
            return characters.ContainsKey(id) && characters[id].Moods.Contains(mood);
        }

        public void Init()
        {
            Log.Info("Loading characters...");
            characters = new Dictionary<string, Character>();
            lastLoadDate = new Dictionary<string, DateTime>();
            characterResources = new Dictionary<string, IList<string>>();
            foreach (var str1 in GDUtil.ListFilesInPath("res://definitions/characters/", suffix: ".json", fullPath: false))
            {
                var str2 = str1.Substring(0, str1.LastIndexOf(".json"));
                var characterDto = GDUtil.ReadJsonFile<CharacterDTO>("res://definitions/characters/" + str1);
                var character = new Character() { Id = str2 };
                character.Name = character.OriginalName = characterDto.Name;
                character.Costumes.AddRange(characterDto.Costumes);
                character.Moods.AddRange(FindMoodsForCharacter(str2));
                GatherResourcesForCharacter(character.Id);
                LoadCharacterSpriteData(character, characterDto);
                characters.Add(str2, character);
            }
        }

        public IList<string> GetDependencies(string characterId)
        {
            return characterResources.ContainsKey(characterId) ? characterResources[characterId] : Array.Empty<string>();
        }

        public void LoadResourcesForCharacter(string characterId)
        {
            if (!characters.ContainsKey(characterId) || !characterResources.ContainsKey(characterId) || lastLoadDate.ContainsKey(characterId) && lastLoadDate[characterId].AddSeconds(AssetReloadPeriod) > DateTime.Now)
                return;
            lastLoadDate[characterId] = DateTime.Now;
            Log.Trace("Loading character resources for ", characterId);
            foreach (var path in (IEnumerable<string>)characterResources[characterId])
            {
                if (!path.EndsWith(".json"))
                    Game.Memory.Cache(path);
            }
        }

        public void OverrideCharacterName(string characterId, string newName)
        {
            Game.State.OverrideCharacterNames[characterId] = newName;
            characters[characterId].Name = newName;
        }

        public void RemoveOverrideCharacterName(string characterId)
        {
            Game.State.OverrideCharacterNames.Remove(characterId);
            if (Game.Language.TranslationEnabled)
                characters[characterId].Name = Game.Language.GetCaption(characters[characterId].OriginalName);
            else
                characters[characterId].Name = characters[characterId].OriginalName;
        }

        public void OverrideMood(string characterId, string mood, string newMood)
        {
            Game.State.Overrides["chara." + characterId + ".busts." + mood] = newMood;
        }

        public void RemoveOverrideMoods(string characterId)
        {
            Game.State.Overrides.RemoveAll((k, v) => k.StartsWith("chara." + characterId + ".busts."));
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
            var stringList = new List<string> {
                "res://definitions/characters/" + characterId + ".json"
            };
            if (GDUtil.FolderExists("res://assets/sprite/common/character/" + characterId))
                stringList.AddRange(GDUtil.ListFilesInPath("res://assets/sprite/common/character/" + characterId + "/", ".png"));
            if (GDUtil.FolderExists("res://assets/img/bust/" + characterId))
                stringList.AddRange(GDUtil.ListFilesInPath("res://assets/img/bust/" + characterId + "/", ".png"));
            characterResources[characterId] = stringList;
        }

        private IList<string> FindMoodsForCharacter(string characterId)
        {
            if (!GDUtil.FolderExists("res://assets/img/bust/" + characterId))
                return Array.Empty<string>();
            var moodsForCharacter = new List<string>();
            foreach (var str in GDUtil.ListFilesInPath("res://assets/img/bust/" + characterId + "/", suffix: ".png", fullPath: false))
                moodsForCharacter.Add(str.StripSuffix(".png"));
            return moodsForCharacter;
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
                var characterStateSprites = new Character.CharacterStateSprites {
                    TextureVariation = sprite.TextureSuffix.IsNullOrEmpty() ? "" : "_" + sprite.TextureSuffix,
                    Hframes = sprite.Hframes,
                    Vframes = sprite.Vframes
                };
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
            var str1 = "res://assets/sprite/common/character/" + character.Id + "/" + character.Id;
            foreach (Character.CharacterStateSprites characterStateSprites in character.StateSprites.Values)
            {
                var str2 = characterStateSprites.TextureVariation + ".png";
                var str3 = characterStateSprites.TextureVariation + "_n.png";
                characterStateSprites.Texture = GDUtil.FileExists(str1 + str2) ? GD.Load<Texture>(str1 + str2) : null;
                characterStateSprites.NormalMap = GDUtil.FileExists(str1 + str3) ? GD.Load<Texture>(str1 + str3) : null;
                if (characterStateSprites.Texture == null)
                    Log.Error("[CharacterManager] [", character.Id, "] No texture variation found ", characterStateSprites.TextureVariation);
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
            foreach (var character in characters.Values)
                character.Name = Game.Language.GetCaption(character.OriginalName, CharacterNameContext(character));
        }

        public void ApplyStateOverrides()
        {
            foreach (var character in characters.Values)
            {
                if (Game.State.OverrideCharacterNames.ContainsKey(character.Id))
                    character.Name = Game.State.OverrideCharacterNames[character.Id];
            }
        }

        public void Clean()
        {
            foreach (var character in characters.Values)
                character.Name = character.OriginalName;
        }

        private string CharacterNameContext(ICharacter character) => "characters.name." + character.Id;
    }
}
