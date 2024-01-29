using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "bolt")]
	public class EventTriggerItem : EventTriggerHint
	{
		[Export(PropertyHint.MultilineText, "")]
		public string Items { get; set; } = "";


		[Export(PropertyHint.MultilineText, "")]
		public string Nodes { get; set; } = "";


		public override void _EnterTree()
		{
			if (Engine.EditorHint || Game.Room.Cutscene)
			{
				return;
			}
			string[] array = Items.SplitLines();
			foreach (string item in array)
			{
				string[] array2 = Nodes.SplitLines();
				foreach (string node in array2)
				{
					Game.Events.AddItemTrigger(item, node, base.EffectiveEventName);
				}
			}
		}
	}
}
