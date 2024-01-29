using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "bolt")]
	public class EventTriggerOnEnter : EventTriggerHint
	{
		public override void _EnterTree()
		{
			if (!Engine.EditorHint && !Game.Room.Cutscene)
			{
				Game.Events.AddOnEnterTrigger(base.EffectiveEventName);
			}
		}
	}
}
