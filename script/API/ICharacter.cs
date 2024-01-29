using System.Collections.Generic;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.API
{
	public interface ICharacter
	{
		string Id { get; set; }

		string Name { get; set; }

		string OriginalName { get; set; }

		bool IdleAnimation { get; set; }

		List<string> Costumes { get; set; }

		List<string> Moods { get; set; }

		void UpdateSpriteState(Node node, string state);

		void UpdateSpriteDirection(Node node, Direction direction, string state);
	}
}
