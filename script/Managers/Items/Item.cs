using LacieEngine.API;

namespace LacieEngine.Items
{
	public class Item : IItem
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string OriginalName { get; set; }

		public string Description { get; set; }

		public string OriginalDescription { get; set; }

		public string Icon { get; set; }

		public string Event { get; set; }

		public string Group { get; set; }

		public string[] Tags { get; set; }

		public bool Hidden { get; set; }

		public bool Persistent { get; set; }
	}
}
