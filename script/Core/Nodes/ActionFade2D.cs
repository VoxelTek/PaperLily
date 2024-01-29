using Godot;
using LacieEngine.Animation;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[Tool]
	[ExportType]
	public class ActionFade2D : Node, IAction, IToggleable
	{
		[Export(PropertyHint.None, "")]
		public bool Enabled { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public NodePath Node { get; set; } = new NodePath();


		[Export(PropertyHint.None, "")]
		public bool FadeIn { get; set; } = true;


		[Export(PropertyHint.None, "")]
		public float Duration { get; set; } = 0.5f;


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
			CanvasItem node = GetNode<CanvasItem>(Node);
			if (node != null)
			{
				if (FadeIn)
				{
					Game.Animations.Play(new FadeInAnimation(node, Duration));
				}
				else
				{
					Game.Animations.Play(new FadeOutAnimation(node, Duration));
				}
			}
		}
	}
}
