using System.Diagnostics;
using Godot;
using LacieEngine.Core;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface ICoreFunctions
	{
		GameState State { get; }

		PersistentState PersistentState { get; }

		Stopwatch PlaytimeStopwatch { get; }

		void Quit();

		void InitPersistentState();

		void PauseGame();

		void StartGameFromEvent(string eventId);

		void StartGameFromRoom(string room, string point, Vector2 pos, string dir);

		void StartGameFromSave(string saveFile);

		void SwitchToScreen(PackedScene scene);

		void SwitchToScreen(PackedScene scene, bool keepState);

		void OpenScreen(PackedScene scene);

		void AssignInputProcessor();
	}
}
