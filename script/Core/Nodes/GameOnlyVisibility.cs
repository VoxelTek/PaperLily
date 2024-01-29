using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "visible")]
	public class GameOnlyVisibility : Node2D
	{
		public override void _EnterTree()
		{
			base.Visible = !Engine.EditorHint;
		}
	}
}
