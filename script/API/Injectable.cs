using System;

namespace LacieEngine.API
{
	[AttributeUsage(AttributeTargets.Class)]
	public class Injectable : Attribute
	{
		public int priority;

		public string condition;
	}
}
