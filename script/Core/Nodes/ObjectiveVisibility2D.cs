using System;
using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType]
	public class ObjectiveVisibility2D : Node, IAction, IToggleable
	{
		public enum ObjectiveStatus
		{
			Done,
			InProgress,
			Failed
		}

		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public string Objective { get; set; }

		[Export(PropertyHint.None, "")]
		public ObjectiveStatus Status { get; set; }

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
				bool visible = Status switch
				{
					ObjectiveStatus.Done => Game.Objectives.IsObjectiveCompleted(Objective), 
					ObjectiveStatus.InProgress => Game.Objectives.IsObjectiveInProgress(Objective), 
					ObjectiveStatus.Failed => Game.Objectives.IsObjectiveFailed(Objective), 
					_ => throw new NotImplementedException(), 
				};
				if (Invert)
				{
					visible = !visible;
				}
				if (node is CanvasItem canvasItem)
				{
					canvasItem.Visible = visible;
				}
			}
		}
	}
}
