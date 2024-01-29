using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Subgame.Chapter1
{
	[Tool]
	public class Ch1ForestBusStop : GameRoom
	{
		[GetNode("Foreground/Bus")]
		private Sprite nBus;

		public override void _UpdateRoom()
		{
			nBus.Visible = !Game.Variables.GetFlag("ch1.bus_got_off_cutscene_end");
		}
	}
}
