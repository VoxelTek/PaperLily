using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "bolt")]
	public class EventTriggerItemRoom : EventTriggerHint
	{
		[Export(PropertyHint.MultilineText, "")]
		public string Items { get; set; } = "";


		public override void _EnterTree()
		{
			if (!Engine.EditorHint && !Game.Room.Cutscene)
			{
				string[] array = Items.SplitLines();
				foreach (string item in array)
				{
					Game.Events.AddItemRoomTrigger(item, base.EffectiveEventName);
				}
			}
		}
	}
}
