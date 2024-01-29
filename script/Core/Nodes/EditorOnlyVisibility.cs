using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType(icon = "invisible")]
	public class EditorOnlyVisibility : Node2D
	{
		public override void _EnterTree()
		{
			base.Visible = Engine.EditorHint;
		}
	}
}
