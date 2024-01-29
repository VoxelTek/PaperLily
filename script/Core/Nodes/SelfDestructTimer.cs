using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[ExportType(icon = "stopwatch")]
	public class SelfDestructTimer : Node
	{
		[Export(PropertyHint.None, "")]
		public float WaitTime { get; set; }

		public SelfDestructTimer()
		{
		}

		public SelfDestructTimer(float time)
		{
			WaitTime = time;
		}

		public override void _Process(float delta)
		{
			WaitTime -= delta;
			if (WaitTime <= 0f)
			{
				GetParent().Delete();
			}
		}
	}
}
