using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1ForestPath3 : GameRoom
	{
		[GetNode("Ground/Carpet")]
		private Sprite nCarpet;

		[GetNode("Ground/Carpet/Shiny")]
		private Sprite nKeyShiny;

		private PVar varRevealedKey = "ch1.forest_lockedsite_revealed_key_3";

		private PVar varTookKey = "ch1.forest_lockedsite_took_key_3";

		public override void _UpdateRoom()
		{
			nCarpet.Frame = (((bool)varRevealedKey.Value) ? 1 : 0);
			nKeyShiny.Visible = (bool)varRevealedKey.Value && !varTookKey.Value;
		}
	}
}
