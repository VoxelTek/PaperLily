using System;
using System.Collections.Generic;

namespace LacieEngine.StoryPlayer
{
	[Serializable]
	public class StoryPlayerEvent
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string Path { get; set; }

		public string Room { get; set; }

		public bool ShouldTrackRepeats { get; set; }

		public bool AffectsPersistent { get; set; }

		public StoryPlayerCommand[] Dialogue { get; set; }

		public IEnumerable<string> GetDependencies()
		{
			ISet<string> dependencies = new HashSet<string>();
			StoryPlayerCommand[] dialogue = Dialogue;
			foreach (StoryPlayerCommand block in dialogue)
			{
				dependencies.UnionWith(block.GetDependencies());
			}
			return dependencies;
		}
	}
}
