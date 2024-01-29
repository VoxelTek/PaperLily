using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType]
	public class ActionAnimation2D : Node, IAction, IToggleable
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
					StartAnimation(node);
				}
				else
				{
					StopAnimation(node);
				}
			}
		}

		private void StartAnimation(Node node)
		{
			if (node is IAnimation2D animation)
			{
				animation.Play();
			}
			else if (node is AnimationPlayer animation2)
			{
				animation2.PlayFirstAnimation();
			}
		}

		private void StopAnimation(Node node)
		{
			if (node is IAnimation2D animation)
			{
				animation.Stop();
			}
			else if (node is AnimationPlayer animation2)
			{
				animation2.Stop();
			}
		}
	}
}
