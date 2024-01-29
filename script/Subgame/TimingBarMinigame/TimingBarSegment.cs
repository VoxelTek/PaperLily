using Godot;
using LacieEngine.API;

namespace LacieEngine.Minigames
{
	[ExportType]
	public class TimingBarSegment : Resource
	{
		[Export(PropertyHint.None, "")]
		public int Offset { get; private set; }

		[Export(PropertyHint.None, "")]
		public bool RandomOffset { get; private set; }

		[Export(PropertyHint.None, "")]
		public int Range { get; private set; }

		[Export(PropertyHint.None, "")]
		public float Speed { get; private set; }
	}
}
