using Godot;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class GameRoomNested : GameRoom
	{
		[Export(PropertyHint.None, "")]
		public NodePath Subroom = new NodePath();

		protected GameRoom nRoom;

		public override void _Initialize()
		{
			nRoom = GetNode<GameRoom>(Subroom);
			Injector.Resolve(nRoom);
			nRoom.InjectNodes();
			_InitializeNested();
		}

		public virtual void _InitializeNested()
		{
		}

		public override void _AfterFadeIn()
		{
			nRoom._AfterFadeIn();
		}

		public override void _BeforeFadeOut()
		{
			nRoom._BeforeFadeOut();
		}

		public override void _AfterFadeOut()
		{
			nRoom._AfterFadeOut();
		}

		public override void _UpdateRoom()
		{
			nRoom._UpdateRoom();
		}

		public override void _RoomProcess(float delta)
		{
			nRoom._RoomProcess(delta);
		}

		public override Node2D GetMainLayer()
		{
			return nRoom.GetMainLayer();
		}

		public override Node FindNodeInRoom(string nodeName)
		{
			return nRoom.FindNodeInRoom(nodeName);
		}
	}
}
