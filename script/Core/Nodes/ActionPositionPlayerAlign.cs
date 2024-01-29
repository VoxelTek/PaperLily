using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType]
	public class ActionPositionPlayerAlign : Node, IAction, IToggleable
	{
		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public NodePath Node { get; set; } = new NodePath();


		[Export(PropertyHint.None, "")]
		public bool AlignX { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public bool AlignY { get; set; } = true;


		public override void _Ready()
		{
			if (!Engine.EditorHint)
			{
				Game.Room.RegisteredRoomActions[base.Name] = this;
			}
		}

		public void Execute()
		{
			if (!Enabled || Node.IsNullOrEmpty())
			{
				return;
			}
			if (Game.Player == null || !Game.Player.Node.IsValid())
			{
				Log.Warn("[", "ActionPositionPlayerAlign", "::", base.Name, "] Player is not present!");
			}
			else if (GetNode(Node) is Node2D node2d)
			{
				if (AlignX)
				{
					node2d.Position = node2d.Position.ReplaceX(Game.Player.Node.Position.x);
				}
				if (AlignY)
				{
					node2d.Position = node2d.Position.ReplaceY(Game.Player.Node.Position.y);
				}
			}
		}
	}
}
