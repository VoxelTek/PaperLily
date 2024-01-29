using System.Collections.Generic;

namespace LacieEngine.Objectives
{
	internal class ObjectiveDTO
	{
		public class Objective
		{
			public string Id;

			public string Name;

			public string Description;

			public bool Hidden;

			public List<string> OnComplete;

			public List<Objective> Children;
		}

		public List<Objective> Objectives;
	}
}
