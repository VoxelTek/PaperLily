using System.Collections.Generic;

namespace LacieEngine.API
{
	[InjectableInterface(unique = true)]
	public interface IItemManager : IModule
	{
		IItem Get(string id);

		List<IItem> GetOwnedItems();

		List<IItem> GetAllItems();

		int GetOwnedAmount(string itemId);

		void AddItem(string itemId);

		void AddItem(string itemId, int amount);

		void RemoveItem(string itemId, int amount);

		void ClearInventory();

		bool IsItemValid(string itemId);

		void OverrideItemNameAndDescription(string itemId, string newName, string newDescription);
	}
}
