using System;

namespace LacieEngine.API
{
	[AttributeUsage(AttributeTargets.Field)]
	public class GetNode : Attribute
	{
		public string Path { get; }

		public GetNode(string path)
		{
			Path = path;
		}
	}
}
