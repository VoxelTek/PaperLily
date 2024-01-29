using System;
using System.Collections.Generic;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

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
			{
				return characters[id];
			}
			Log.Error("Character ", id, " not found!");
			return null;
		}

		public bool Exists(string id)
		{
			return characters.ContainsKey(id);
		}

		public bool IsMoodValid(string id, string mood)
		{
			if (!characters.ContainsKey(id))
			{
				return false;
			}
			return characters[id].Moods.Contains(mood);
		}

		public void Init()
		{
			Log.Info("Loading characters...");
			characters = new Dictionary<string, Character>();
			lastLoadDate = new Dictionary<string, DateTime>();
			characterResources = new Dictionary<string, IList<string>>();
			foreach (string filename in GDUtil.ListFilesInPath("res://definitions/characters/", null, ".json", fullPath: false))
			{
				string id = filename.Substring(0, filename.LastIndexOf(".json"));
				CharacterDTO characterDto = GDUtil.ReadJsonFile<CharacterDTO>("res://definitions/characters/" + filename);
				Character character = new Character();
				character.Id = id;
				string text2 = (character.Name = (character.OriginalName = characterDto.Name));
				character.Costumes.AddRange(characterDto.Costumes);
				character.Moods.AddRange(FindMoodsForCharacter(id));
				GatherResourcesForCharacter(character.Id);
				LoadCharacterSpriteData(character, characterDto);
				characters.Add(id, character);
			}
		}

		public IList<string> GetDependencies(string characterId)
		{
			if (characterResources.ContainsKey(characterId))
			{
				return characterResources[characterId];
			}
			return Array.Empty<string>();
		}

		public void LoadResourcesForCharacter(string characterId)
		{
			if (!characters.ContainsKey(characterId) || !characterResources.ContainsKey(characterId) || (lastLoadDate.ContainsKey(characterId) && lastLoadDate[characterId].AddSeconds(5.0) > DateTime.Now))
			{
				return;
			}
			lastLoadDate[characterId] = DateTime.Now;
			Log.Trace("Loading character resources for ", characterId);
			foreach (string path in characterResources[characterId])
			{
				if (!path.EndsWith(".json"))
				{
					Game.Memory.Cache(path);
				}
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
			{
				characters[characterId].Name = Game.Language.GetCaption(characters[characterId].OriginalName);
			}
			else
			{
				characters[characterId].Name = characters[characterId].OriginalName;
			}
		}

		public void OverrideMood(string characterId, string mood, string newMood)
		{
			Game.State.Overrides["chara." + characterId + ".busts." + mood] = newMood;
		}

		public void RemoveOverrideMoods(string characterId)
		{
			Game.State.Overrides.RemoveAll((string k, string v) => k.StartsWith("chara." + characterId + ".busts."));
		}

		public Texture GetMoodTexture(string characterId, string mood)
		{
			if (Game.State.Overrides.ContainsKey("chara." + characterId + ".busts." + mood))
			{
				mood = Game.State.Overrides["chara." + characterId + ".busts." + mood];
			}
			else if (Game.State.Overrides.ContainsKey("chara." + characterId + ".busts.all"))
			{
				mood = Game.State.Overrides["chara." + characterId + ".busts.all"];
			}
			return GD.Load<Texture>("res://assets/img/bust/" + characterId + "/" + mood + ".png");
		}

		private void GatherResourcesForCharacter(string characterId)
		{
			List<string> resources = new List<string>();
			resources.Add("res://definitions/characters/" + characterId + ".json");
			if (GDUtil.FolderExists("res://assets/sprite/common/character/" + characterId))
			{
				resources.AddRange(GDUtil.ListFilesInPath("res://assets/sprite/common/character/" + characterId + "/", ".png"));
			}
			if (GDUtil.FolderExists("res://assets/img/bust/" + characterId))
			{
				resources.AddRange(GDUtil.ListFilesInPath("res://assets/img/bust/" + characterId + "/", ".png"));
			}
			characterResources[characterId] = resources;
		}

		private IList<string> FindMoodsForCharacter(string characterId)
		{
			if (GDUtil.FolderExists("res://assets/img/bust/" + characterId))
			{
				List<string> moods = new List<string>();
				{
					foreach (string bustFile in GDUtil.ListFilesInPath("res://assets/img/bust/" + characterId + "/", null, ".png", fullPath: false))
					{
						moods.Add(bustFile.StripSuffix(".png"));
					}
					return moods;
				}
			}
			return Array.Empty<string>();
		}

		private void LoadCharacterSpriteData(Character character, CharacterDTO characterDto)
		{
			if (!characterDto.NoDefaultSprites)
			{
				character.StateSprites["stand"] = new Character.CharacterStateSprites(Character.DefaultStand);
				character.StateSprites["walk"] = new Character.CharacterStateSprites(Character.DefaultWalk);
				character.StateSprites["run"] = new Character.CharacterStateSprites(Character.DefaultRun);
			}
			foreach (CharacterDTO.CharacterSpriteDTO spriteDto2 in characterDto.Sprites)
			{
				if (spriteDto2.State == "idle")
				{
					character.IdleAnimation = true;
				}
				Character.CharacterStateSprites stateSprite2 = new Character.CharacterStateSprites();
				stateSprite2.TextureVariation = (spriteDto2.TextureSuffix.IsNullOrEmpty() ? "" : ("_" + spriteDto2.TextureSuffix));
				stateSprite2.Hframes = spriteDto2.Hframes;
				stateSprite2.Vframes = spriteDto2.Vframes;
				if (!spriteDto2.Offset.IsNullOrEmpty())
				{
					stateSprite2.OffsetOverride = GDUtil.StringToVector2(spriteDto2.Offset);
				}
				if (IsAnimatedSprite(spriteDto2))
				{
					stateSprite2.Animated = true;
					stateSprite2.AnimationFps = spriteDto2.Fps;
					stateSprite2.AnimationLeftFrames = spriteDto2.LeftFrames;
					stateSprite2.AnimationUpFrames = spriteDto2.UpFrames;
					stateSprite2.AnimationRightFrames = spriteDto2.RightFrames;
					stateSprite2.AnimationDownFrames = spriteDto2.DownFrames;
					stateSprite2.Loop = spriteDto2.Loop;
				}
				else
				{
					int.TryParse(spriteDto2.LeftFrames, out stateSprite2.LeftFrame);
					int.TryParse(spriteDto2.UpFrames, out stateSprite2.UpFrame);
					int.TryParse(spriteDto2.RightFrames, out stateSprite2.RightFrame);
					int.TryParse(spriteDto2.DownFrames, out stateSprite2.DownFrame);
				}
				character.StateSprites[spriteDto2.State] = stateSprite2;
			}
			string p = "res://assets/sprite/common/character/" + character.Id + "/" + character.Id;
			foreach (Character.CharacterStateSprites stateSprite in character.StateSprites.Values)
			{
				string s = stateSprite.TextureVariation + ".png";
				string i = stateSprite.TextureVariation + "_n.png";
				stateSprite.Texture = (GDUtil.FileExists(p + s) ? GD.Load<Texture>(p + s) : null);
				stateSprite.NormalMap = (GDUtil.FileExists(p + i) ? GD.Load<Texture>(p + i) : null);
				if (stateSprite.Texture == null)
				{
					Log.Error("[CharacterManager] [", character.Id, "] No texture variation found ", stateSprite.TextureVariation);
				}
			}
			static bool IsAnimatedFrames(string framesStr)
			{
				if (!framesStr.IsNullOrEmpty())
				{
					return framesStr.Split(",").Length > 1;
				}
				return false;
			}
			static bool IsAnimatedSprite(CharacterDTO.CharacterSpriteDTO spriteDto)
			{
				if (!IsAnimatedFrames(spriteDto.LeftFrames) && !IsAnimatedFrames(spriteDto.UpFrames) && !IsAnimatedFrames(spriteDto.RightFrames))
				{
					return IsAnimatedFrames(spriteDto.DownFrames);
				}
				return true;
			}
		}

		public void ApplyTranslationOverrides()
		{
			foreach (Character character in characters.Values)
			{
				character.Name = Game.Language.GetCaption(character.OriginalName);
			}
		}

		public void ApplyStateOverrides()
		{
			foreach (Character character in characters.Values)
			{
				if (Game.State.OverrideCharacterNames.ContainsKey(character.Id))
				{
					character.Name = Game.State.OverrideCharacterNames[character.Id];
				}
			}
		}

		public void Clean()
		{
			foreach (Character value in characters.Values)
			{
				value.Name = value.OriginalName;
			}
		}

		private string CharacterNameContext(ICharacter character)
		{
			return "characters.name." + character.Id;
		}
	}
}
