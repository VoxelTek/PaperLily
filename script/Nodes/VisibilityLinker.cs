using System.Linq;
using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	[Tool]
	[ExportType(icon = "visible")]
	public class VisibilityLinker : Node2D
	{
		[Export(PropertyHint.None, "")]
		public bool Invert { get; set; }

		[Export(PropertyHint.None, "")]
		public NodePath[] Nodes { get; set; }

		public override void _Ready()
		{
			Connect("visibility_changed", this, "UpdateVisibility");
			UpdateVisibility();
		}

		public void UpdateVisibility()
		{
			for (int i = 0; i < Nodes.Count(); i++)
			{
				if (!Nodes[i].IsNullOrEmpty() && GetNode(Nodes[i]) is CanvasItem canvasItem)
				{
					canvasItem.Visible = (Invert ? (!base.Visible) : base.Visible);
				}
			}
		}
	}
}
