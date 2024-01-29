using Godot;
using LacieEngine.Core;
using LacieEngine.Subgame.PaperLily;

namespace LacieEngine.Rooms
{
	[Tool]
	public class Ch1FacilityB3fMk : GameRoom
	{
		private PVar varItemStorage = "ch1.temp_mk_item_storage";

		public void RemoveInventory()
		{
			PaperLilyFunctions.RemoveInventory(varItemStorage);
		}

		public void RestoreInventory()
		{
			PaperLilyFunctions.RestoreInventory(varItemStorage);
		}
	}
}
