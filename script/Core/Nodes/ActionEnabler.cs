using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType]
	public class ActionEnabler : Node, IAction, IToggleable
	{
		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public NodePath Node { get; set; } = new NodePath();


		[Export(PropertyHint.None, "")]
		public bool Enable { get; set; } = true;


		public override void _Ready()
		{
			if (!Engine.EditorHint)
			{
				Game.Room.RegisteredRoomActions[base.Name] = this;
			}
		}

		public void Execute()
		{
			if (Enabled && !Node.IsNullOrEmpty())
			{
				Node node = GetNode(Node);
				if (node is IToggleable toggleable)
				{
					toggleable.Enabled = Enable;
				}
				else if (node is CollisionShape2D collision)
				{
					collision.Disabled = !Enable;
				}
				else if (node is CollisionPolygon2D polygon)
				{
					polygon.Disabled = !Enable;
				}
			}
		}
	}
}
