using Godot;
using LacieEngine.API;
using LacieEngine.Core;
using LacieEngine.Rooms;

namespace LacieEngine.Nodes
{
	[Tool]
	[ExportType(icon = "room")]
	public class RoomLighting : Node, IAction, IInspectorCustomizer
	{
		[Export(PropertyHint.None, "")]
		public bool Default { get; set; }

		[Export(PropertyHint.None, "")]
		public Lighting Lighting { get; set; }

		public bool Current { get; private set; }

		public override void _Ready()
		{
			if (Engine.EditorHint)
			{
				SetChildrenVisible(visible: false);
			}
			else if (Default)
			{
				Game.CurrentRoom.Lighting = Lighting;
				SetChildrenVisible(visible: true);
				Current = true;
			}
		}

		public void Execute()
		{
			Apply();
		}

		public void Apply()
		{
			Node[] siblings = this.GetSiblings();
			for (int i = 0; i < siblings.Length; i++)
			{
				if (siblings[i] is RoomLighting lighting)
				{
					lighting.Unset();
				}
			}
			SetChildrenVisible(visible: true);
			Game.Room.ApplyLighting(Lighting);
			Current = true;
		}

		public void Unset()
		{
			if (Current)
			{
				SetChildrenVisible(visible: false);
				Game.Room.ResetLighting();
				Current = false;
			}
		}

		private void SetChildrenVisible(bool visible)
		{
			foreach (Node child in GetChildren())
			{
				if (child is CanvasItem canvasItem)
				{
					canvasItem.Visible = visible;
				}
			}
		}
	}
}
