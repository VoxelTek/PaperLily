using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1ForestLakesideTentInterior : GameRoom
	{
		[GetNode("Main/Terrarium")]
		private Node2D nTerrarium;

		[GetNode("Other/Events/misc_terrarium")]
		private EventTrigger nTerrariumEvt;

		private PVar varTookCaterpillar = "ch1.forest_moths_took_caterpillar";

		public override void _UpdateRoom()
		{
			nTerrarium.Visible = !varTookCaterpillar.Value;
			nTerrariumEvt.Enabled = !varTookCaterpillar.Value;
		}
	}
}
