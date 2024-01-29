using System;

namespace LacieEngine.API
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ExportType : Attribute
	{
		public string icon;
	}
}
