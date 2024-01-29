using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[ExportType(icon = "stopwatch")]
	public class ActionCallTimer : SimpleTimer
	{
		[Export(PropertyHint.None, "")]
		public NodePath Node { get; set; } = new NodePath();


		public override void _Ready()
		{
			if (Engine.EditorHint)
			{
				return;
			}
			base.Callback = delegate
			{
				if (!Node.IsNullOrEmpty() && GetNode(Node) is IAction action)
				{
					action.Execute();
				}
			};
			base._Ready();
		}
	}
}
