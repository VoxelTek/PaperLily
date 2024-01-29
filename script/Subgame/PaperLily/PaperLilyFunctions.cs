using System.Collections.Generic;
using LacieEngine.API;
using LacieEngine.Core;
using Newtonsoft.Json;

namespace LacieEngine.Subgame.PaperLily
{
	public static class PaperLilyFunctions
	{
		public static void RemoveInventory(PVar inventoryVar)
		{
			List<IItem> ownedItems = Game.Items.GetOwnedItems();
			List<string> listToSerialize = new List<string>(ownedItems.Count);
			foreach (IItem item in ownedItems)
			{
				if (!item.Persistent)
				{
					listToSerialize.Add(item.Id + "::" + Game.Items.GetOwnedAmount(item.Id));
				}
			}
			string serializedContent = JsonConvert.SerializeObject(listToSerialize);
			inventoryVar.NewValue = serializedContent;
			Log.Debug("Storing inventory: ", inventoryVar.Value);
			Game.Items.ClearInventory();
		}

		public static void RestoreInventory(PVar inventoryVar)
		{
			Game.Items.ClearInventory();
			string storedItems = inventoryVar.Value;
			Log.Debug("Restoring inventory: ", storedItems);
			if (storedItems.IsNullOrEmpty())
			{
				return;
			}
			foreach (string item in JsonConvert.DeserializeObject<List<string>>(storedItems)!)
			{
				string[] itemAndQuantity = item.Split("::");
				Game.Items.AddItem(itemAndQuantity[0], int.Parse(itemAndQuantity[1]));
			}
			inventoryVar.NewValue = null;
		}
	}
}
