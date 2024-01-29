using Godot;

namespace LacieEngine.Core
{
	public class SceneProvider : Resource
	{
		[Export(PropertyHint.None, "")]
		public PackedScene FirstScreen { get; private set; }

		[Export(PropertyHint.None, "")]
		public PackedScene TitleScreen { get; private set; }

		[Export(PropertyHint.None, "")]
		public PackedScene CreditsScreen { get; private set; }

		[Export(PropertyHint.None, "")]
		public PackedScene DeathScreen { get; private set; }

		[Export(PropertyHint.None, "")]
		public PackedScene LoadingScreen { get; private set; }

		[Export(PropertyHint.None, "")]
		public PackedScene SavingScreen { get; private set; }
	}
}
