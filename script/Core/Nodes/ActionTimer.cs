using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType]
	public class ActionTimer : Node, IAction, IToggleable
	{
		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public NodePath Node { get; set; } = new NodePath();


		[Export(PropertyHint.None, "")]
		public bool Start { get; set; } = true;


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
				if (Start)
				{
					StartTimer(node);
				}
				else
				{
					StopTimer(node);
				}
			}
		}

		private void StartTimer(Node node)
		{
			if (node is Timer timer)
			{
				timer.Start();
			}
			else if (node is SimpleTimer timer2)
			{
				timer2.Start();
			}
		}

		private void StopTimer(Node node)
		{
			if (node is Timer timer)
			{
				timer.Stop();
			}
			else if (node is SimpleTimer timer2)
			{
				timer2.Stop();
			}
		}
	}
}
