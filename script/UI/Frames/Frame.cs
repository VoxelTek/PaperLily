using Godot;
using LacieEngine.Core;

namespace LacieEngine.UI
{
	public abstract class Frame : MarginContainer
	{
		public MarginContainer Container { get; protected set; }

		[Export(PropertyHint.None, "")]
		public Vector2 MinimumSize { get; set; } = Vector2.Zero;


		[Export(PropertyHint.None, "")]
		public int ContentMarginLeft { get; set; }

		[Export(PropertyHint.None, "")]
		public int ContentMarginTop { get; set; }

		[Export(PropertyHint.None, "")]
		public int ContentMarginRight { get; set; }

		[Export(PropertyHint.None, "")]
		public int ContentMarginBottom { get; set; }

		public string DecorBgTexture { get; set; }

		public LayoutPreset DecorBgAlignment { get; set; }

		public Frame()
		{
			Container = GDUtil.MakeNode<MarginContainer>("Container");
		}

		public void AddToFrame(Node child)
		{
			Container.AddChild(child);
		}

		public void ClearFrame()
		{
			foreach (Node child in Container.GetChildren())
			{
				child.Delete();
			}
		}
	}
}
