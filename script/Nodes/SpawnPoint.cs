using Godot;
using LacieEngine.API;
using LacieEngine.Core;

namespace LacieEngine.Nodes
{
	[Tool]
	[ExportType(icon = "target")]
	public class SpawnPoint : Node2D, IInspectorCustomizer
	{
		private int _layer = 1;

		[Export(PropertyHint.None, "")]
		public Direction.Enum Direction { get; set; } = LacieEngine.Core.Direction.Enum.Down;


		[Export(PropertyHint.Range, "1,10240")]
		public int Layer
		{
			get
			{
				return _layer;
			}
			set
			{
				_layer = value;
			}
		}

		public override void _Ready()
		{
			if (!Engine.EditorHint)
			{
				Game.Room.RegisteredPoints[base.Name] = this;
			}
		}
	}
}
