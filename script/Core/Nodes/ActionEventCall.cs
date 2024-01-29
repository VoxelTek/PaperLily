using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType]
	public class ActionEventCall : Node, IAction, IToggleable
	{
		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public string Event { get; set; }

		public override void _Ready()
		{
			if (!Engine.EditorHint)
			{
				Game.Room.RegisteredRoomActions[base.Name] = this;
			}
		}

		public void Execute()
		{
			if (Enabled && !Event.IsNullOrEmpty())
			{
				((PEvent)Event).Play();
			}
		}
	}
}
