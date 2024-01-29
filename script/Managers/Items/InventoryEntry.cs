using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Items
{
	public class InventoryEntry
	{
		public IItem Item;

		public int Amount;

		public InventoryEntry(string id)
		{
			Item = Game.Items.Get(id);
			Amount = 1;
		}

		public InventoryEntry(string id, int amount)
		{
			Item = Game.Items.Get(id);
			Amount = amount;
		}
	}
}
