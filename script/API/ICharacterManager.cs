using System.Collections.Generic;
using Godot;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface ICharacterManager : IModule
	{
		ICharacter Get(string id);

		bool Exists(string id);

		bool IsMoodValid(string id, string mood);

		void LoadResourcesForCharacter(string characterId);

		void OverrideCharacterName(string characterId, string newName);

		void RemoveOverrideCharacterName(string characterId);

		void OverrideMood(string characterId, string mood, string newMood);

		void RemoveOverrideMoods(string characterId);

		Texture GetMoodTexture(string characterId, string mood);

		IList<string> GetDependencies(string characterId);
	}
}
