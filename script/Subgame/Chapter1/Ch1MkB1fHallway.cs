using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1MkB1fHallway : GameRoom
	{
		[GetNode("TilesFgTrapped")]
		private TileMap nTilesTrapped;

		[GetNode("Other/Events/misc_wall_trapped")]
		private EventTrigger nWallTrapped;

		public override void _UpdateRoom()
		{
			if (Game.Variables.GetFlag("ch1.mk_trapped"))
			{
				nTilesTrapped.Visible = true;
				nTilesTrapped.CollisionLayer = 2u;
				nWallTrapped.Enabled = true;
			}
		}
	}
}
