using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "bolt")]
	public abstract class EventTriggerHint : Node, IEventTriggerNode
	{
		[Export(PropertyHint.None, "")]
		public string Event { get; set; } = "";


		public string EffectiveEventName
		{
			get
			{
				if (!Event.IsNullOrEmpty())
				{
					return Event;
				}
				return base.Name;
			}
		}
	}
}
