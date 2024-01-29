using Godot;
using LacieEngine.API;

namespace LacieEngine.Core
{
	[ExportType(icon = "stopwatch")]
	public class EventCallTimer : SimpleTimer
	{
		[Export(PropertyHint.None, "")]
		public string Event { get; set; }

		public override void _Ready()
		{
			base.Callback = delegate
			{
				((PEvent)Event).Play();
			};
			base._Ready();
		}
	}
}
