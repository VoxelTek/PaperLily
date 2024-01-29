using System.Collections.Generic;

namespace LacieEngine.API
{
	public interface IObjective
	{
		string Id { get; set; }

		string Group { get; set; }

		string Name { get; set; }

		string Description { get; set; }

		int Order { get; set; }

		bool Hidden { get; set; }

		List<string> OnComplete { get; set; }

		IObjective Parent { get; set; }

		List<IObjective> Children { get; set; }

		bool HasParent();

		bool HasChildren();
	}
}
