using Godot;

namespace LacieEngine.Minigames
{
	public class PointAndClickSnapTarget : TextureRect
	{
		[Export(PropertyHint.None, "")]
		public bool FirstTarget { get; set; }

		[Export(PropertyHint.None, "")]
		public Vector2 SnapOffset { get; set; }

		[Export(PropertyHint.None, "")]
		public string Left { get; set; }

		[Export(PropertyHint.None, "")]
		public string Up { get; set; }

		[Export(PropertyHint.None, "")]
		public string Right { get; set; }

		[Export(PropertyHint.None, "")]
		public string Down { get; set; }
	}
}
