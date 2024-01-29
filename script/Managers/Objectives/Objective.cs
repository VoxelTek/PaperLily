using System.Collections.Generic;
using LacieEngine.API;

namespace LacieEngine.Objectives
{
	public class Objective : IObjective
	{
		public string Id { get; set; }

		public string Group { get; set; }

		public int Order { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public bool Hidden { get; set; }

		public List<string> OnComplete { get; set; }

		public IObjective Parent { get; set; }

		public List<IObjective> Children { get; set; }

		public bool HasParent()
		{
			return Parent != null;
		}

		public bool HasChildren()
		{
			if (Children != null)
			{
				return Children.Count > 0;
			}
			return false;
		}
	}
}
