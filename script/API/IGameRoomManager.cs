using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using LacieEngine.Nodes;
using LacieEngine.Rooms;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IGameRoomManager : IModule
	{
		IPlayer Player { get; }

		GameRoom CurrentRoom { get; }

		IGameCamera Camera { get; }

		bool Cutscene { get; }

		bool Visible { get; set; }

		List<IReflectable> MirrorReflections { get; }

		List<IAction> RegisteredRoomUpdates { get; }

		Dictionary<string, IAction> RegisteredRoomActions { get; }

		Dictionary<string, Node2D> RegisteredNPCs { get; }

		Dictionary<string, SpawnPoint> RegisteredPoints { get; }

		Color Modulate { get; set; }

		Task ChangeRoom(string room, string point, Vector2 pos, string dir, float? time, bool cutscene);

		Task ChangeRoom(string room, string point, Vector2 pos, string dir, float? time);

		Task ChangeRoom(string room, string point, Vector2 pos, string dir);

		void RoomFunction(string function);

		void UpdateRoomState();

		Node FindNodeInRoom(string nodeName);

		Path2D GetPath(string pathName);

		void ApplyLighting(Resource lighting);

		void ResetLighting();

		void ApplyRoomShader(Material material);

		void RegisterMirrorReflection(IReflectable reflectable);

		void RefreshPixelScaling();
	}
}
