using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Minigames
{
	[ExportType]
	public class MinigameResource : Resource
	{
		[Export(PropertyHint.None, "")]
		public PackedScene MinigameScene { get; private set; }

		public IMinigame MakeMinigame()
		{
			IMinigame minigame = MinigameScene.Instance<IMinigame>();
			Injector.Resolve(minigame);
			minigame.Node.InjectNodes();
			minigame.Init();
			return minigame;
		}
	}
}
