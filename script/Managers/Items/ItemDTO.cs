using System.Collections.Generic;

namespace LacieEngine.Items
{
	internal class ItemDTO
	{
		public class Item
		{
			public string Id;

			public string Name;

			public string Description;

			public string Icon;

			public string Event;

			public string[] Tags;

			public bool Hidden;

			public bool Persistent;
		}

		public List<Item> Items;
	}
}
