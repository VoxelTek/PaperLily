using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType]
	public class VariableEnabler : Node, IAction
	{
		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public string Variable { get; set; }

		[Export(PropertyHint.None, "")]
		public NodePath Node { get; set; } = new NodePath();


		[Export(PropertyHint.None, "")]
		public bool Invert { get; set; }

		public override void _Ready()
		{
			if (!Engine.EditorHint)
			{
				Game.Room.RegisteredRoomUpdates.Add(this);
			}
		}

		public void Execute()
		{
			if (Enabled && !Node.IsNullOrEmpty())
			{
				Node node = GetNode(Node);
				bool flag = Game.Variables.GetFlag(Variable);
				if (Invert)
				{
					flag = !flag;
				}
				if (node is IToggleable toggleable)
				{
					toggleable.Enabled = flag;
				}
			}
		}
	}
}
