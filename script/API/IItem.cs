namespace LacieEngine.API
{
	public interface IItem
	{
		string Id { get; set; }

		string Name { get; set; }

		string Description { get; set; }

		string Icon { get; set; }

		string[] Tags { get; set; }

		string Event { get; set; }

		string Group { get; set; }

		bool Hidden { get; set; }

		bool Persistent { get; set; }
	}
}
