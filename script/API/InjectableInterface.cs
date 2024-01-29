using System;

namespace LacieEngine.API
{
	[AttributeUsage(AttributeTargets.Interface)]
	public class InjectableInterface : Attribute
	{
		public bool unique;
	}
}
