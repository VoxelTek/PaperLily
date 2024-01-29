using Godot;
using LacieEngine.API;
using LacieEngine.Nodes;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1Ending4ParkingLot : GameRoom
	{
		[GetNode("Lighting/room_light_red")]
		private RoomLighting nLightingR;

		[GetNode("Lighting/room_light_yellow")]
		private RoomLighting nLightingY;

		[GetNode("Lighting/room_light_green")]
		private RoomLighting nLightingG;

		private int _curLight;

		public void RotateLight()
		{
			switch (_curLight++)
			{
			case 0:
				nLightingR.Apply();
				break;
			case 1:
				nLightingY.Apply();
				break;
			default:
				nLightingG.Apply();
				_curLight = 0;
				break;
			}
		}
	}
}
