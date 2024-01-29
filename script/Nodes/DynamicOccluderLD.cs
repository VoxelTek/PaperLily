using Godot;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	internal class DynamicOccluderLD : LightOccluder2D
	{
		private Position2D nCenter;

		public override void _Ready()
		{
			nCenter = GetNode("Center") as Position2D;
		}

		public override void _Process(float delta)
		{
			if (Game.Player.GetCenter().x < nCenter.GlobalPosition.x && Game.Player.GetCenter().y > nCenter.GlobalPosition.y)
			{
				base.Visible = false;
			}
			else
			{
				base.Visible = true;
			}
		}
	}
}
